using System;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [Serializable]
  public struct SkillChargeParams
  {
    [SerializeField] private float minChargeDuration;
    [SerializeField] private float maxChargeDuration;
  }
}