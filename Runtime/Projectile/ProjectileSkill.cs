using UnityEngine;

namespace KalkuzSystems.Skill
{
  public abstract class ProjectileSkill : CastableSkill
  {
    [SerializeField] [Space(20f)] protected ProjectileBehaviour projectilePrefab;
    [SerializeField] protected float baseProjectileSpeed = 10f;

    [SerializeField] [Space(20f)] protected uint burstCount = 1;
    [SerializeField] protected float burstSequenceDuration;
    [SerializeField] protected float maxSpreadAngle;

    [SerializeField] [Space(20f)] protected bool becomeStuckUponDie;

    [SerializeField] protected float lifetime = 10f;
    [SerializeField] protected float maxDistance = 20f;

    public ProjectileBehaviour ProjectilePrefab => projectilePrefab;
    public float BaseProjectileSpeed => baseProjectileSpeed;
    public uint BurstCount => burstCount;
    public float BurstSequenceDuration => burstSequenceDuration;
    public float MaxSpreadAngle => maxSpreadAngle;
    public bool BecomeStuckUponDie => becomeStuckUponDie;
    public float Lifetime => lifetime;
    public float MaxDistance => maxDistance;
  }
}