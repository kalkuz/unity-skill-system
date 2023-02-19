using System;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [Serializable]
  public sealed class BasicProjectileEnhancingParams : ProjectileEnhancingParams
  {
    [SerializeField] [Space(20f)] private int additionalPierceAmount;
    [SerializeField] private float speedMultiplierPerPierce = 1f;
    [SerializeField] private float damageMultiplierPerPierce = 1f;

    [SerializeField] [Space(20f)] private int additionalRicochetAmount;
    [SerializeField] private float speedMultiplierPerRicochet = 1f;
    [SerializeField] private float damageMultiplierPerRicochet = 1f;

    public int AdditionalPierceAmount => additionalPierceAmount;
    public float SpeedMultiplierPerPierce => speedMultiplierPerPierce;
    public float DamageMultiplierPerPierce => damageMultiplierPerPierce;

    public int AdditionalRicochetAmount => additionalRicochetAmount;
    public float SpeedMultiplierPerRicochet => speedMultiplierPerRicochet;
    public float DamageMultiplierPerRicochet => damageMultiplierPerRicochet;
  }
}