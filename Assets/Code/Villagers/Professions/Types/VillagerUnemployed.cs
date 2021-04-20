namespace Code.Villagers.Professions.Types
{
    public class VillagerUnemployed : Profession
    {
        private void Update()
        {
            // ProfessionAI.Evaluate();
        }

        public override void Initialize()
        {
            type = ProfessionType.Unemployed;
            
            InitializeUnemployedAI();
        }
    }
}
