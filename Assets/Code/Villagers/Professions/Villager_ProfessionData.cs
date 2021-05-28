using System;
using Code.Map.Building;
using Code.Map.Building.Workplaces;
using UnityEngine;


namespace Code.Villagers.Professions
{
    [CreateAssetMenu(fileName = "Villager Profession Data", menuName = "Game Data/Villagers/Profession Data", order = 0)]
    public class ProfessionData : ScriptableObject
    {
        [Header("Profession data")]
        [SerializeField] private ProfessionType type;
        [SerializeField] private VillagersStatistics requiredStatistics;
        [SerializeField] private int resourceCarryingLimit;
        
        //TODO: this wont work, it need to be moved to "profession.cs"
        [SerializeField] private int goldPerDay;

        [Header("Workplace data")] 
        [SerializeField] private BuildingData workplaceBuildingData;

        public void SetGoldPerDay(int amount) =>
            goldPerDay = amount;

        public Type WorkplaceType => workplaceBuildingData.Prefab.GetComponent<Workplace>().GetType();
        public BuildingType WorkplaceBuildingType => workplaceBuildingData.BuildingType;
        public ProfessionType Type => type;
        public VillagersStatistics RequiredStatistics => requiredStatistics;
        public int GoldPerDay => goldPerDay;
        public int ResourceCarryingLimit => resourceCarryingLimit;
    }
}
