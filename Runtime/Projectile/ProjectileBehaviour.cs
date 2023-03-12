using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KalkuzSystems.Pooling;
using KalkuzSystems.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Skill
{
  [DisallowMultipleComponent]
  [RequireComponent(typeof(ProjectilePoolObject))]
  public sealed class ProjectileBehaviour : MonoBehaviour
  {
    [SerializeField] private ProjectilePoolObject poolObject;

    [SerializeField] [Space(20f)] private float collisionRadius;

    [SerializeField] private UnityEvent onSpawn;
    [SerializeField] private UnityEvent onHit;
    [SerializeField] private UnityEvent onDeath;
    [SerializeField] private UnityEvent onHide;

    private Vector3 currentSpeed;

    private Vector3 currentLocalAcceleration;
    private Vector3 currentGlobalAcceleration;

    private List<Damage> damages;

    private float distanceTravelled;
    private List<CharacterStats> hitCharacters;
    private Coroutine lifetimeCoroutine;

    private Transform m_transform;

    private int remainingPierce;
    private int remainingRicochet;

    private ProjectileSkill sourceSkill;
    private BasicProjectileSkill sourceSkillCastedBasic;

    private string targetTag;
    public PoolObject PoolObject => poolObject;

    private Transform Transform
    {
      get
      {
        if (!m_transform) m_transform = transform;
        return m_transform;
      }
    }

    private void Update()
    {
      if (sourceSkillCastedBasic)
      {
        if (distanceTravelled > sourceSkillCastedBasic.MaxDistance)
        {
          Die(null);
          return;
        }
      }

      var deltaLocalJerk = sourceSkill.BaseLocalJerk * Time.deltaTime;
      var deltaGlobalJerk = sourceSkill.BaseGlobalJerk * Time.deltaTime;

      currentLocalAcceleration += deltaLocalJerk;
      currentGlobalAcceleration += deltaGlobalJerk;

      var netCurrentAcceleration = Transform.rotation * currentLocalAcceleration + currentGlobalAcceleration;
      var netDeltaAcceleration = netCurrentAcceleration * Time.deltaTime;
      netDeltaAcceleration = netDeltaAcceleration.normalized * Mathf.Clamp(netDeltaAcceleration.magnitude,
        sourceSkill.MinNetAccelerationMagnitude, sourceSkill.MaxNetAccelerationMagnitude);

      currentSpeed += netDeltaAcceleration;
      var currentSpeedMagnitude = currentSpeed.magnitude;
      var currentSpeedNormalized = currentSpeed.normalized;

      currentSpeed = currentSpeedNormalized * Mathf.Clamp(currentSpeedMagnitude, sourceSkill.MinSpeedMagnitude,
        sourceSkill.MaxSpeedMagnitude);

      var deltaSpeed = currentSpeed * Time.deltaTime;
      var deltaSpeedMagnitude = deltaSpeed.magnitude;
      var deltaSpeedNormalized = deltaSpeed.normalized;

      Update2D(deltaSpeed, deltaSpeedNormalized, deltaSpeedMagnitude);

      Transform.position += deltaSpeed;
      distanceTravelled += deltaSpeedMagnitude;
    }

    private void Update2D(Vector3 deltaSpeed, Vector3 deltaSpeedNormalized, float deltaSpeedMagnitude)
    {
      var hit = Physics2D.CircleCast(Transform.position, collisionRadius, deltaSpeed, deltaSpeedMagnitude);
      var hitCollider = hit.collider;
      if (hitCollider && hitCollider.CompareTag(targetTag))
      {
        var character = hitCollider.GetComponent<CharacterStats>();
        if (character)
        {
          if (!(character.IsDied || hitCharacters.Contains(character)))
          {
            Hit(character);
          }
        }
      }
      
      Transform.rotation = Quaternion.LookRotation(Vector3.forward, deltaSpeedNormalized);
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(Transform.position, collisionRadius);
    }

    public void Initialize(Vector3 position, Quaternion rotation, ProjectileSkill sourceSkill, string targetTag)
    {
      if (!sourceSkill) throw new Exception("Source Skill parameter given to Projectile Behaviour should not be null.");

      Transform.position = position;
      Transform.rotation = rotation;

      this.sourceSkill = sourceSkill;
      sourceSkillCastedBasic = sourceSkill as BasicProjectileSkill;

      PostInitialize(targetTag);
    }

    private void PostInitialize(string targetTag)
    {
      hitCharacters ??= new List<CharacterStats>();
      hitCharacters.Clear();

      this.targetTag = targetTag;

      distanceTravelled = 0f;

      currentSpeed = Transform.rotation * sourceSkill.BaseProjectileSpeed;
      currentLocalAcceleration = sourceSkill.BaseLocalAcceleration;
      currentGlobalAcceleration = sourceSkill.BaseGlobalAcceleration;

      damages = new List<Damage>(sourceSkill.Damages);

      if (sourceSkillCastedBasic)
      {
        remainingPierce = sourceSkillCastedBasic.BasePierceAmount +
                          sourceSkillCastedBasic.ProjectileEnhancingParams.AdditionalPierceAmount;
        remainingRicochet = sourceSkillCastedBasic.BaseRicochetAmount +
                            sourceSkillCastedBasic.ProjectileEnhancingParams.AdditionalRicochetAmount;

        if (lifetimeCoroutine != null) StopCoroutine(lifetimeCoroutine);
        lifetimeCoroutine = StartCoroutine(Lifetime(sourceSkillCastedBasic.Lifetime));
      }
      else
      {
        remainingPierce = 0;
        remainingRicochet = 0;
      }

      onSpawn?.Invoke();
      enabled = true;
    }

    private void Hit(CharacterStats character)
    {
      hitCharacters.Add(character);

      var dodged = character.ShouldDodge(1f);
      if (dodged) return;

      onHit?.Invoke();

      foreach (var damage in damages) character.ApplyDamage(damage, 0f, 1f);

      if (remainingPierce > 0)
      {
        remainingPierce--;

        if (sourceSkillCastedBasic)
        {
          currentSpeed *= sourceSkillCastedBasic.ProjectileEnhancingParams.SpeedMultiplierPerPierce;

          for (var i = 0; i < damages.Count; i++)
          {
            damages[i] *= sourceSkillCastedBasic.ProjectileEnhancingParams.DamageMultiplierPerPierce;
          }
        }
      }
      else if (remainingRicochet > 0)
      {
        remainingRicochet--;

        if (sourceSkillCastedBasic)
        {
          currentSpeed *= sourceSkillCastedBasic.ProjectileEnhancingParams.SpeedMultiplierPerRicochet;

          for (var i = 0; i < damages.Count; i++)
            damages[i] *= sourceSkillCastedBasic.ProjectileEnhancingParams.DamageMultiplierPerRicochet;
        }

        FindRicochetDirection();
      }
      else
      {
        Die(character.transform);
      }
    }

    private void Die(Transform hitTarget)
    {
      if (sourceSkill.BecomeStuckUponDie)
      {
        Transform.parent = hitTarget;
      }
      else
      {
        Hide();
      }

      if (lifetimeCoroutine != null)
      {
        StopCoroutine(lifetimeCoroutine);
        lifetimeCoroutine = null;
      }

      onDeath?.Invoke();
      enabled = false;
      poolObject.ReturnToPool();
    }

    public void Hide()
    {
      onHide?.Invoke();
      enabled = false;
    }

    private void FindRicochetDirection()
    {
      if (!sourceSkillCastedBasic) return;

      var cols = Physics2D.OverlapCircleAll(Transform.position, sourceSkillCastedBasic.RicochetRadius);
      if (cols.Length <= 0) return;

      var selected = cols.Select(i => i.GetComponent<CharacterStats>()).Where(i => i);

      var closest = selected
        .Where(i => i.CompareTag(targetTag) && !hitCharacters.Contains(i))
        .OrderBy(i => Vector3.Distance(i.transform.position, Transform.position)).FirstOrDefault();

      if (closest) Transform.up = closest.transform.position - Transform.position;
    }

    private IEnumerator Lifetime(float time)
    {
      yield return new WaitForSeconds(time);
      Die(null);
    }
  }
}