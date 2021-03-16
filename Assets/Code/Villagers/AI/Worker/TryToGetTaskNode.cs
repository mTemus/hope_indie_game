using System.Collections.Generic;
using Code.System;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;

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
                Task newTask = Managers.Instance.Tasks.GetTask(profession.Type);

                if (newTask == null) continue;
                newTask.OnTaskTaken(profession.GetComponent<Villager>(), profession.OnTaskCompleted);
                newTasks.Add(newTask);
            }

            if (newTasks.Count > 0) {
                foreach (Task t in newTasks) 
                    profession.AddTask(t);

                state = NodeState.SUCCESS;
            }
            else 
                state = NodeState.FAILURE;
            
            Debug.LogWarning("TryToGetTaskNode --- " + state);
            return state;
        }
    }
}
