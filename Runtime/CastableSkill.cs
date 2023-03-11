using System.Collections.Generic;
using KalkuzSystems.Stats;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  public abstract class CastableSkill : SkillBase
  {
    [SerializeField] [Space(20f)] protected float cooldown = 1f;
    [SerializeField] protected List<Damage> damages;

    [SerializeField] [Space(20f)] protected SkillCastType skillCastType;
    [SerializeField] protected SkillChargeParams chargeParams;
    [SerializeField] protected SkillChannelParams channelParams;

    public float Cooldown => cooldown;
    public List<Damage> Damages => damages;

    public abstract void Cast(SkillCastParams skillCastParams);
  }
}