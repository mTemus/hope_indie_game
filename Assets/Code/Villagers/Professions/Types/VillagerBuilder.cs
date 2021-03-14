using System;

namespace Code.Villagers.Professions.Types
{
    public class VillagerBuilder : Profession
    {
        private void Update()
        {
            ProfessionAI?.Evaluate();
        }
    }
}
