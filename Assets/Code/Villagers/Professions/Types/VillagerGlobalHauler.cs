namespace Code.Villagers.Professions.Types
{
    public class VillagerGlobalHauler : Profession
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
