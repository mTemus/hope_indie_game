namespace Code.Villagers.Professions.Types
{
    public class VillagerWorkplaceHauler : Profession
    {
        private void Update()
        {
            professionAI?.Evaluate();
        }

        public override void Initialize()
        {
            type = ProfessionType.WorkplaceHauler;
            
            InitializeWorkerAI();
        }
    }
}
