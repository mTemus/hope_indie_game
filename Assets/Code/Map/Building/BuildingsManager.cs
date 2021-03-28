using System;
using System.Collections.Generic;
using Code.Map.Building.Buildings.Components;
using Code.System;
using UnityEngine;

namespace Code.Map.Building
{
    public class BuildingsManager : MonoBehaviour
    {
        private readonly List<KeyValuePair<Type, Building>> buildings = new List<KeyValuePair<Type, Building>>();
        
        public void AddBuilding(Building b)
        {
            buildings.Add(new KeyValuePair<Type, Building>(b.GetType(), b));
        }

        public Building[] GetAllBuildingsOfType(Type buildingType)
        {
            List<Building> returnBuildings = new List<Building>();

            foreach (KeyValuePair<Type, Building> building in buildings) {
                if (building.Key == buildingType) 
                    returnBuildings.Add(building.Value);
                
            }
            return returnBuildings.ToArray();
        }

        public Building[] GetAllFreeWorkplacesOfType(Type buildingType)
        {
            List<Building> returnBuildings = new List<Building>();

            foreach (KeyValuePair<Type, Building> building in buildings) {
                if (building.Key != buildingType) continue;
                if (!building.Value.CanBeOccupied()) continue;
                returnBuildings.Add(building.Value);
            }
            
            return returnBuildings.ToArray();
        }
        
        public Warehouse GetClosestWarehouse() =>
            Managers.Instance.Areas.GetVillageArea().GetWarehouse();
        
        
        
    }
}
