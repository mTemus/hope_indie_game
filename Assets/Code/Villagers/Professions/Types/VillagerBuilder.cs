namespace Code.Villagers.Professions.Types
{
    public class VillagerBuilder : Profession
    {
        private void Update()
        {
            BTO.Tick();
        }

        public override void Initialize()
        {
            InitializeWorkerAI();
        }
    }
}
