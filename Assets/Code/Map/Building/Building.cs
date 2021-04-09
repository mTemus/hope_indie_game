using System.Collections.Generic;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Map.Building
{
    public abstract class Building : MonoBehaviour
    {
        [SerializeField] private BuildingStorageModule storage;
        
        private BuildingData data;
        private int currentOccupancy;

        private readonly List<Villager> residents = new List<Villager>();
        
        public void Occupy(Villager resident)
        {
            residents.Add(resident);
            currentOccupancy++;
        }

        public void Vacate(Villager resident)
        {
            residents.Remove(resident);
            currentOccupancy--;
        }
        
        public bool CanBeOccupied() =>
            data.MAXOccupancy - currentOccupancy > 0;
        
        public BuildingData Data
        {
            get => data;
            set => data = value;
        }

        public BuildingStorageModule Storage => storage;
    }
}
