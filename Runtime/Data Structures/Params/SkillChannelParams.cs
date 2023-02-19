using System;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [Serializable]
  public struct SkillChannelParams
  {
    [SerializeField] private float minChannelDuration;
    [SerializeField] private float maxChannelDuration;
  }
}