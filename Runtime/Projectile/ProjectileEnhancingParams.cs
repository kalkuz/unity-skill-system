using System;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [Serializable]
  public abstract class ProjectileEnhancingParams
  {
    [SerializeField] protected float additionalProjectileSpeed;

    [SerializeField] [Space(20f)] protected uint additionalBurstCount;
    [SerializeField] protected float multipliedBurstCount;

    public float AdditionalProjectileSpeed => additionalProjectileSpeed;
    public uint AdditionalBurstCount => additionalBurstCount;
    public float MultipliedBurstCount => multipliedBurstCount;
  }
}