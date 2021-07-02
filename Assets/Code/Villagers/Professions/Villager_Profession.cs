using Code.Map.Building.Workplaces;
using Code.Map.Resources;
using UnityEngine;

namespace Code.Villagers.Professions
{
    public enum ProfessionType
    {
        Unemployed,
        Builder,
        Lumberjack,
        WorkplaceHauler,
        GlobalHauler,
        StoneMiner,
    }
    
    public enum ProfessionAIType
    {
        villager_worker,
        villager_unemployed
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