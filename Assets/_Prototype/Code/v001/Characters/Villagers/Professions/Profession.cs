using _Prototype.Code.v001.World.Buildings.Workplaces;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;

namespace _Prototype.Code.v001.Characters.Villagers.Professions
{
    /// <summary>
    /// 
    /// </summary>
    public enum ProfessionType
    {
        Unemployed,
        Builder,
        Lumberjack,
        WorkplaceHauler,
        GlobalHauler,
        StoneMiner,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public enum ProfessionAIType
    {
        VillagerWorker,
        VillagerUnemployed
    }
    
    /// <summary>
    /// 
    /// </summary>
    public abstract class Profession : MonoBehaviour
    {
        public Workplace Workplace { get; set; }
        public Resource CarriedResource { get; set; }
        public Data Data { get; set; }
        public bool IsCarryingResource => CarriedResource != null && CarriedResource.amount > 0;
        
        /// <summary>
        /// 
        /// </summary>
        public abstract void Initialize();
    }
}