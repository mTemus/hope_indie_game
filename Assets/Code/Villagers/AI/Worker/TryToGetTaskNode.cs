using System.Collections.Generic;
using Code.System;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;

namespace Code.Villagers.AI.Worker
{
    public class TryToGetTaskNode : Node
    {
        private readonly Profession profession;
        private int taskCnt = 3;

        public TryToGetTaskNode(Profession profession)
        {
            this.profession = profession;
        }

        public override NodeState Evaluate()
        {
            List<Task> newTasks = new List<Task>();
            
            for (int i = 0; i < taskCnt; i++) {
                Task newTask = Managers.Instance.Professions.GetTask(profession.Type);

                if (newTask != null) 
                    newTasks.Add(newTask);
            }

            if (newTasks.Count > 0) {
                foreach (Task t in newTasks) 
                    profession.AddTask(t);

                state = NodeState.SUCCESS;
            }
            else 
                state = NodeState.FAILURE;
            
            return state;
        }
    }
}