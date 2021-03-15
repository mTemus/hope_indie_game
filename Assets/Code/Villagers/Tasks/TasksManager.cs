using System;
using System.Collections.Generic;
using System.Linq;
using Code.Map.Building;
using Code.Map.Building.Buildings.Components;
using Code.Resources;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public class TasksManager : MonoBehaviour
    {
        private readonly List<BuildingTask> buildingTasks = new List<BuildingTask>();
        private readonly List<ResourceCarryingTask> resourceCarryingTasks = new List<ResourceCarryingTask>();
    
    
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
                    taskToGet = buildingTasks.FirstOrDefault(task => task.ResourcesDelivered);

                    if (taskToGet != null) 
                        buildingTasks.Remove((BuildingTask) taskToGet);

                    else {
                        taskToGet = resourceCarryingTasks.FirstOrDefault(task =>
                            task.Profession == ProfessionType.BUILDER);

                        if (taskToGet != null) 
                            resourceCarryingTasks.Remove((ResourceCarryingTask) taskToGet);
                    }
                
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(profession), profession, null);
            }
        
            return taskToGet;
        }

        public void CreateBuildingTask(Construction construction)
        {
            BuildingTask bt = new BuildingTask(0, construction.transform.position + construction.PositionOffset, construction);
            construction.SetBuildingTask(bt);
            buildingTasks.Add(bt);
        }

        public void CreateResourceCarryingTask(Vector3 taskPosition, ProfessionType workerType, Warehouse storage, Resource resource, Func<Resource, Resource> onResourceDelivered)
        {
            ResourceCarryingTask rct = new ResourceCarryingTask(0, workerType, taskPosition, storage, resource, onResourceDelivered);
            resourceCarryingTasks.Add(rct);
        }

    
    }
}
