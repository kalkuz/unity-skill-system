using UnityEngine;

namespace KalkuzSystems.Skill
{
  public abstract class ProjectileSkill : CastableSkill
  {
    [SerializeField] [Space(20f)] protected ProjectileBehaviour projectilePrefab;
    
    [SerializeField] [Space(20f)] protected Vector3 baseProjectileSpeed = Vector3.up;
    [SerializeField] protected float minSpeedMagnitude;
    [SerializeField] protected float maxSpeedMagnitude;

    [SerializeField] [Space(20f)] protected Vector3 baseLocalAcceleration = Vector3.zero;
    [SerializeField] protected Vector3 baseGlobalAcceleration = Vector3.zero;
    [SerializeField] protected float minNetAccelerationMagnitude;
    [SerializeField] protected float maxNetAccelerationMagnitude;

    [SerializeField] [Space(20f)] protected Vector3 baseLocalJerk = Vector3.zero;
    [SerializeField] protected Vector3 baseGlobalJerk = Vector3.zero;

    [SerializeField] [Space(20f)] protected uint burstCount = 1;
    [SerializeField] protected float burstSequenceDuration;
    [SerializeField] protected float maxSpreadAngle;

    [SerializeField] [Space(20f)] protected bool becomeStuckUponDie;

    [SerializeField] protected float lifetime = 10f;
    [SerializeField] protected float maxDistance = 20f;

    public ProjectileBehaviour ProjectilePrefab => projectilePrefab;
    public Vector3 BaseProjectileSpeed => baseProjectileSpeed;
    public float MinSpeedMagnitude => minSpeedMagnitude;
    public float MaxSpeedMagnitude => maxSpeedMagnitude;

    public Vector3 BaseLocalAcceleration => baseLocalAcceleration;
    public Vector3 BaseGlobalAcceleration => baseGlobalAcceleration;
    public float MinNetAccelerationMagnitude => minNetAccelerationMagnitude;
    public float MaxNetAccelerationMagnitude => maxNetAccelerationMagnitude;

    public Vector3 BaseLocalJerk => baseLocalJerk;
    public Vector3 BaseGlobalJerk => baseGlobalJerk;

    public uint BurstCount => burstCount;
    public float BurstSequenceDuration => burstSequenceDuration;
    public float MaxSpreadAngle => maxSpreadAngle;
    public bool BecomeStuckUponDie => becomeStuckUponDie;
    public float Lifetime => lifetime;
    public float MaxDistance => maxDistance;
  }
}