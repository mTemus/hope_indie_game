using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;

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
            BTO = GetComponent<BehaviourTreeOwner>();
            blackboard = GetComponent<Blackboard>();
            blackboard.SetVariableValue("myWorkplace", Workplace);
            blackboard.SetVariableValue("workplacePos", Workplace.PivotedPosition);
            
            blackboard.InitializePropertiesBinding(BTO.blackboard.propertiesBindTarget, false);
            BTO.StartBehaviour();
        }
    }
}
