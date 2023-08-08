using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetaCombatSystem.Skills
{
    public class SkillsManager : MonoBehaviour
    {
        public List<Skill> Skills { get; private set; } = new();

        private void Awake()
        {
            Skills.AddRange(GetComponentsInChildren<Skill>());
        }
    }
}
