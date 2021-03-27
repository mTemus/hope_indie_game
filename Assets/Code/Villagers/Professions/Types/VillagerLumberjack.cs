namespace Code.Villagers.Professions.Types
{
    public class VillagerLumberjack : Profession
    {
        private void Update()
        {
            ProfessionAI.Evaluate();
        }
    }
}
