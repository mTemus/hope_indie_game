namespace Code.Villagers.Professions.Types
{
    public class Villager_Profession_WorkplaceHauler : Villager_Profession
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
