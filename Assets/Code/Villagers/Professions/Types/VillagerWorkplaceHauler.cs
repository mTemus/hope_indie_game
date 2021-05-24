namespace Code.Villagers.Professions.Types
{
    public class VillagerWorkplaceHauler : Profession
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
