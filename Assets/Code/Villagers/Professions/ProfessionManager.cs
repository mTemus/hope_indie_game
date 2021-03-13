using System;
using System.Collections.Generic;
using System.Linq;
using Code.Villagers.Professions.Types;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Villagers.Professions
{
    public class ProfessionManager : MonoBehaviour
    {
        private List<BuildingTask> buildingTasks = new List<BuildingTask>();
        private List<ResourceCarryingTask> resourceCarryingTasks = new List<ResourceCarryingTask>();
        
        private List<VillagerBuilder> builders = new List<VillagerBuilder>();

        public void HireBuilder(GameObject villager)
        {
            VillagerBuilder builder = villager.AddComponent<VillagerBuilder>();
            
        }

        private Task GetTaskByProfession(ProfessionType professionType)
        {
            Task taskToGet = null;
            
            switch (professionType) {
                case ProfessionType.UNEMPLOYED:
                    break;
                
                case ProfessionType.BUILDER:
                    taskToGet = buildingTasks.FirstOrDefault(task => task.ResourcesDelivered) ?? (Task) resourceCarryingTasks.FirstOrDefault(task =>
                        task.Profession == ProfessionType.BUILDER);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(professionType), professionType, null);
            }

            return taskToGet;
        }
        
        public Task GetTask(ProfessionType profession)
        {
            Task taskToGet = null;
            
            switch (profession) {
                case ProfessionType.UNEMPLOYED:
                    break;
                
                case ProfessionType.BUILDER:
                    taskToGet = buildingTasks.FirstOrDefault(task => task.ResourcesDelivered) ?? 
                                (Task) resourceCarryingTasks.FirstOrDefault(task =>
                        task.Profession == ProfessionType.BUILDER);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(profession), profession, null);
            }

            return taskToGet;
        }
    }
}
