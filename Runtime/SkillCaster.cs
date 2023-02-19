using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  public sealed class SkillCaster : MonoBehaviour
  {
    [SerializeField] private List<SkillCasterContainer> skills;
    [SerializeField] private bool startCooldownOnEnable = true;
    [SerializeField] private bool startCooldownOnAdd = true;

    private readonly Dictionary<int, bool> skillCooldownStates = new();
    private readonly Dictionary<int, float> skillCooldownRemainingTimes = new();

    private void OnEnable()
    {
      // Initialize skill cooldown states
      for (var i = 0; i < skills.Count; i++)
      {
        var container = skills[i];
        InitializeSkillCooldown(i);

        if (startCooldownOnEnable)
        {
          StartCoroutine(Cooldown(i));
        }
        else if (container.automationType == SkillCasterAutomationType.AUTOMATIC)
        {
          CastSkill(i);
        }
      }
    }

    private void AddSkill(SkillCasterContainer container)
    {
      skills.Add(container);
      var index = skills.Count - 1;
      InitializeSkillCooldown(index);

      if (startCooldownOnAdd)
      {
        StartCoroutine(Cooldown(index));
      }
      else if (container.automationType == SkillCasterAutomationType.AUTOMATIC)
      {
        CastSkill(index);
      }
    }

    private void ReplaceSkill(int index, SkillCasterContainer container)
    {
      skills[index] = container;
      InitializeSkillCooldown(index);

      if (startCooldownOnAdd)
      {
        StartCoroutine(Cooldown(index));
      }
      else if (container.automationType == SkillCasterAutomationType.AUTOMATIC)
      {
        CastSkill(index);
      }
    }

    private void InitializeSkillCooldown(int index)
    {
      skillCooldownStates[index] = false;
      skillCooldownRemainingTimes[index] = 0f;
    }

    public void CastSkill(int skillIndex)
    {
      // Check if skill index is valid
      if (skillIndex < 0 || skillIndex >= skills.Count) return;

      // Check if skill is on cooldown
      if (skillCooldownStates.TryGetValue(skillIndex, out var isOnCooldown) && isOnCooldown)
      {
        Debug.LogWarning("Skill is on cooldown!");
        return;
      }

      var container = skills[skillIndex];
      var skill = container.skill;
      var skillCastSourceTransform = container.skillCastSourceTransform;

      // Cast skill
      skill.Cast(new SkillCastParams
      {
        position = skillCastSourceTransform.position,
        rotation = skillCastSourceTransform.rotation,
        caster = this
      });

      // Start cooldown
      StartCoroutine(Cooldown(skillIndex));
    }

    private IEnumerator Cooldown(int skillIndex)
    {
      var container = skills[skillIndex];
      var skill = container.skill;
      var cooldown = skill.Cooldown;

      skillCooldownStates[skillIndex] = true;
      skillCooldownRemainingTimes[skillIndex] = cooldown;

      var timeElapsed = 0f;
      while (timeElapsed < cooldown)
      {
        timeElapsed += Time.deltaTime;
        skillCooldownRemainingTimes[skillIndex] -= Time.deltaTime;
        yield return null;
      }

      skillCooldownStates[skillIndex] = false;

      if (container.automationType == SkillCasterAutomationType.AUTOMATIC)
      {
        CastSkill(skillIndex);
      }
    }

    public float GetRemainingCooldownTime(int skillIndex)
    {
      return skillCooldownRemainingTimes[skillIndex];
    }
  }
}