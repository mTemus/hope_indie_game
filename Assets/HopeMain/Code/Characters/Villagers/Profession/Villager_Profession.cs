using HopeMain.Code.World.Buildings.Workplace;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.Characters.Villagers.Profession
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