namespace Code.Villagers.Professions.Types
{
    public class Villager_Profession_Unemployed : Profession
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
