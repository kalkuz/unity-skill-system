using System.Collections;
using KalkuzSystems.Pooling;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [CreateAssetMenu(menuName = "Kalkuz/Skills/Projectile/Basic")]
  public sealed class BasicProjectileSkill : ProjectileSkill
  {
    [SerializeField] [Space(20f)] private int basePierceAmount;
    [SerializeField] private int baseRicochetAmount;
    [SerializeField] private float ricochetRadius = 10f;

    [SerializeField] [Space(20f)] private BasicProjectileEnhancingParams projectileEnhancingParams;

    public int BasePierceAmount => basePierceAmount;
    public int BaseRicochetAmount => baseRicochetAmount;
    public float RicochetRadius => ricochetRadius;
    public BasicProjectileEnhancingParams ProjectileEnhancingParams => projectileEnhancingParams;

    public override void Cast(SkillCastParams skillCastParams)
    {
      if (burstCount == 0) return;

      skillCastParams.caster.StartCoroutine(SequentialCast(skillCastParams));
    }

    private IEnumerator SequentialCast(SkillCastParams skillCastParams)
    {
      var isSequential = burstSequenceDuration > 0;

      if (burstCount == 1)
      {
        var inst = PoolProvider.GetPool(projectilePrefab.PoolObject.ID).Request() as ProjectilePoolObject;
        if (inst)
        {
          var comp = inst.ProjectileBehaviour;
          comp.Initialize(skillCastParams.position, skillCastParams.rotation, this, "");
        }
      }
      else
      {
        var delay = new WaitForSeconds(burstSequenceDuration / burstCount);

        var deltaRotation = maxSpreadAngle / (burstCount - 1);

        for (var i = 0; i < burstCount; i++)
        {
          var angle = -maxSpreadAngle * 0.5f + i * deltaRotation;
          var rot = skillCastParams.rotation * Quaternion.Euler(0, 0, angle);

          var inst = PoolProvider.GetPool(projectilePrefab.PoolObject.ID).Request() as ProjectilePoolObject;
          if (inst)
          {
            var comp = inst.ProjectileBehaviour;
            comp.Initialize(skillCastParams.position, rot, this, "");
          }

          if (isSequential) yield return delay;
        }
      }

      if (!isSequential) yield return null;
    }

    public override string GetDescription()
    {
      return "";
    }
  }
}