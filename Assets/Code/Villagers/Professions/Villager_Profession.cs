using System.Collections.Generic;
using Code.Map.Building.Workplaces;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Tasks;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using Task = Code.Villagers.Tasks.Task;

namespace Code.Villagers.Professions
{
    public enum ProfessionType
    {
        Unemployed,
        Builder,
        Lumberjack,
        WorkplaceHauler,
        GlobalHauler,
    }
    
    public abstract class Villager_Profession : MonoBehaviour
    {
        public Workplace Workplace { get; set; }
        public Resource CarriedResource { get; set; }
        public Villager_ProfessionData Data { get; set; }
        public bool IsCarryingResource => CarriedResource != null && CarriedResource.amount > 0;
        
        public abstract void Initialize();
        
    }
}