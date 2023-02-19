using System;
using KalkuzSystems.Pooling;
using UnityEngine;

namespace KalkuzSystems.Skill
{
  [RequireComponent(typeof(ProjectileBehaviour))]
  public sealed class ProjectilePoolObject : PoolObject
  {
    private ProjectileBehaviour projectileBehaviour;
    
    public ProjectileBehaviour ProjectileBehaviour => projectileBehaviour;

    private void Awake()
    {
      projectileBehaviour = GetComponent<ProjectileBehaviour>();
    }
  }
}