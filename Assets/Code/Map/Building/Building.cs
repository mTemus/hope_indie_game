using System.Collections.Generic;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Map.Building
{
    public abstract class Building : MonoBehaviour
    {
        private Vector3 entrancePivot;
        private Vector2Int buildingSize;
        private int maxOccupancy;
        private int currentOccupancy;

        private readonly List<Villager> residents = new List<Villager>();
        
        public void SetEntrancePivot(float x, float y, float z)
        {
            entrancePivot = new Vector3(x, y, z);
        }

        public void SetBuildingSize(float x, float y)
        {
            buildingSize = new Vector2Int((int) x, (int) y);
        }
        
        public void SetEntrancePivot(Vector3 newPivot)
        {
            entrancePivot = newPivot;
        }

        public void SetMaxOccupancy(int value)
        {
            maxOccupancy = value;
        }
        
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
            maxOccupancy - currentOccupancy > 0;

        public int MAXOccupancy => maxOccupancy;

        public int CurrentOccupancy => currentOccupancy;

        public Vector3 EntrancePivot => entrancePivot;

        public Vector2Int BuildingSize => buildingSize;
    }
}
