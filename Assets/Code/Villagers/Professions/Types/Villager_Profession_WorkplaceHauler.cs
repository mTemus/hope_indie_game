namespace Code.Villagers.Professions.Types
{
    public class Villager_Profession_WorkplaceHauler : Profession
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
