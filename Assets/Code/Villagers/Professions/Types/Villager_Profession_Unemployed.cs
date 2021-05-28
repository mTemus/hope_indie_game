namespace Code.Villagers.Professions.Types
{
    public class Villager_Profession_Unemployed : Villager_Profession
    {
        private void Update()
        {
            // ProfessionAI.Evaluate();
        }

        public override void Initialize()
        {
            InitializeUnemployedAI();
        }
    }
}
