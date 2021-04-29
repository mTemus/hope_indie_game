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
            InitializeWorkerAI();
        }
    }
}
