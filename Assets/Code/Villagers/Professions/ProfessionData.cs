using System;
using Code.Map.Building;
using UnityEngine;


namespace Code.Villagers.Professions
{
    [CreateAssetMenu(fileName = "Profession Data", menuName = "Game Data/Villagers/Profession Data", order = 0)]
    public class ProfessionData : ScriptableObject
    {
        [Header("Profession data")]
        [SerializeField] private ProfessionType professionType;
        [SerializeField] private VillagersStatistics requiredStats;
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
    }
}
