using System;
using _Prototype.Code.v001.World.Buildings;
using _Prototype.Code.v001.World.Buildings.Workplaces;
using UnityEngine;

namespace _Prototype.Code.v001.Characters.Villagers.Professions
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "Villager Profession Data", menuName = "Game Data/Villagers/Profession Data", order = 0)]
    public class Data : ScriptableObject
    {
        [Header("Profession data")]
        [SerializeField] private ProfessionType type;
        [SerializeField] private ProfessionAIType aiType;
        [SerializeField] private Statistics requiredStatistics;
        [SerializeField] private int resourceCarryingLimit;
        
        //TODO: this wont work, it need to be moved to "profession.cs"
        [SerializeField] private int goldPerDay;

        [Header("Workplace data")] 
        [SerializeField] private World.Buildings.Data workplaceBuildingData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void SetGoldPerDay(int amount) =>
            goldPerDay = amount;

        public Type WorkplaceType => workplaceBuildingData.Prefab.GetComponent<Workplace>().GetType();
        public BuildingType WorkplaceBuildingType => workplaceBuildingData.BuildingType;
        public ProfessionType Type => type;
        public ProfessionAIType AIType => aiType;
        public Statistics RequiredStatistics => requiredStatistics;
        public int GoldPerDay => goldPerDay;
        public int ResourceCarryingLimit => resourceCarryingLimit;
    }
}
