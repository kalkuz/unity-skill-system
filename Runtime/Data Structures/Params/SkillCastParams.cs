using System;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [Serializable]
  public struct SkillCastParams
  {
    public Vector3 position;
    public Quaternion rotation;
    public SkillCaster caster;
  }
}