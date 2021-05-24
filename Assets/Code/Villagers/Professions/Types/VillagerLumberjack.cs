namespace Code.Villagers.Professions.Types
{
    public class VillagerLumberjack : Profession
    {
        private void Update()
        {
            BTO.Tick();
        }

        public override void Initialize()
        {
            InitializeWorkerAI();
            
            // blackboard.InitializePropertiesBinding(BTO.blackboard.propertiesBindTarget, false);
            // BTO.StartBehaviour();
        }
    }
}
