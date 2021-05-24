namespace Code.Villagers.Professions.Types
{
    public class VillagerGlobalHauler : Profession
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
