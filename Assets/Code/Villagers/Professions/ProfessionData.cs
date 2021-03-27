using System;
using Code.Map.Building;
using UnityEngine;


namespace Code.Villagers.Professions
{
    [CreateAssetMenu(fileName = "Profession Data", menuName = "Game Data/Villagers/Profession Data", order = 0)]
    public class ProfessionData : ScriptableObject
    {
        [SerializeField] private ProfessionType professionType;
        [SerializeField] private Transform workspace;
        [SerializeField] private VillagersStatistics requiredStats;
        [SerializeField] private int goldPerDay;

        public void SetGoldPerDay(int amount) =>
            goldPerDay = amount;

        public Type WorkplaceType => workspace.GetComponent<Building>().GetType();
        
        public ProfessionType ProfessionType => professionType;

        public VillagersStatistics RequiredStats => requiredStats;

        public int GoldPerDay => goldPerDay;
    }
}
