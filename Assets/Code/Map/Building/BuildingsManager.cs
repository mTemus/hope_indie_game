using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Map.Building
{
    public enum BuildingType {
        Resources,
        Village,
        Industry,
    }
    
    public class BuildingsManager : MonoBehaviour
    {
        private readonly List<KeyValuePair<Type, Building>> buildings = new List<KeyValuePair<Type, Building>>();

        private readonly List<Building> resourcesBuildings = new List<Building>();
        private readonly List<Building> villageBuildings = new List<Building>();
        private readonly List<Building> industryBuildings = new List<Building>();

        private List<Building> GetBuildingsByType(BuildingType buildingType)
        {
            return buildingType switch {
                BuildingType.Resources => resourcesBuildings,
                BuildingType.Village => villageBuildings,
                BuildingType.Industry => industryBuildings,
                _ => null
            };
        }
        
        public void AddBuilding(BuildingType buildingType, Building building)
        {
            switch (buildingType) {
                case BuildingType.Resources:
                    resourcesBuildings.Add(building);
                    break;
                
                case BuildingType.Village:
                    villageBuildings.Add(building);
                    break;
                
                case BuildingType.Industry:
                    industryBuildings.Add(building);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, building.GetType().ToString());
            }
        }

        public Building[] GetAllBuildingOfClass(BuildingType type, Type classType)
        {
            return GetBuildingsByType(type)
                .Where(building => building.GetType() == classType)
                .ToArray();
        }

        public Building[] GetAllFreeWorkplacesOfClass(BuildingType buildingType, Type classType)
        {
            return GetBuildingsByType(buildingType)
                .Where(building => building.GetType() == classType)
                .Where(building => building.CanBeOccupied())
                .ToArray();
        }
    }
}
