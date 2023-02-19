using UnityEngine;

namespace KalkuzSystems.Skill
{
  public abstract class BaseSkill : ScriptableObject
  {
    [SerializeField] protected string skillName;
    [SerializeField] protected string description;
    [SerializeField] protected Sprite icon;
    
    public string SkillName => skillName;
    public string Description => description;
    public Sprite Icon => icon;
    
    public abstract string GetDescription();
  }
}