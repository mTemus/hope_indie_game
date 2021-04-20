namespace Code.Villagers.Professions.Types
{
    public class VillagerBuilder : Profession
    {
        private void Update()
        {
            professionAI?.Evaluate();
        }

        public override void Initialize()
        {
            type = ProfessionType.Builder;
            
            InitializeWorkerAI();
        }
    }
}
