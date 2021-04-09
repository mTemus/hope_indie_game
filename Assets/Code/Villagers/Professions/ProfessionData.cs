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
        [SerializeField] private Workplace workplace;

        public void SetGoldPerDay(int amount) =>
            goldPerDay = amount;

        public Type WorkplaceType => workplace.GetType();
        
        public BuildingType WorkplaceBuildingType => workplace.Data.BuildingType;
        
        public ProfessionType ProfessionType => professionType;

        public VillagersStatistics RequiredStats => requiredStats;

        public int GoldPerDay => goldPerDay;
    }
}
