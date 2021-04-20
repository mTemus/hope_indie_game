namespace Code.Villagers.Professions.Types
{
    public class VillagerLumberjack : Profession
    {
        private void Update()
        {
            professionAI.Evaluate();
        }

        public override void Initialize()
        {
            type = ProfessionType.Lumberjack;
            
            InitializeWorkerAI();
        }
    }
}
