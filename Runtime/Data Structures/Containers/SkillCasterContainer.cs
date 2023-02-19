using System;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [Serializable]
  public struct SkillCasterContainer
  {
    public CastableSkill skill;
    public Transform skillCastSourceTransform;
    public SkillCasterAutomationType automationType;
  }
}