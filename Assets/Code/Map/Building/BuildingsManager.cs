using System;
using System.Collections.Generic;
using System.Linq;
using Code.Map.Building.Buildings.Types.Village;
using Code.Map.Building.Workplaces;
using Code.System;
using Code.Villagers.Professions;
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

        private void AddWorkplacesWithoutHaulers(List<Building> listIn, List<Workplace> listOut)
        {
            listOut.AddRange(listIn.Cast<Workplace>()
                .Where(workplace => workplace.CanHireHauler()));
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

        public Workplace[] GetAllWorkplacesOfClass(BuildingType type, Type classType)
        {
            return GetBuildingsByType(type)
                .Where(building => building.GetType() == classType)
                .Cast<Workplace>()
                .ToArray();
        }

        public Workplace[] GetAllFreeWorkplacesOfClass(BuildingType buildingType, Type classType)
        {
            return GetBuildingsByType(buildingType)
                .Cast<Workplace>()
                .Where(workplace => workplace.GetType() == classType)
                .Where(workplace => workplace.CanHireWorker)
                .ToArray();
        }

        public Workplace[] GetAllWorkplacesWithHaulersToHire()
        {
            List<Workplace> workplaces = new List<Workplace>();

            AddWorkplacesWithoutHaulers(resourcesBuildings, workplaces);
            AddWorkplacesWithoutHaulers(industryBuildings, workplaces);

            return workplaces.ToArray();
        }

        public Workplace[] GetAllFreeWorkplacesForProfession(Villager_ProfessionData villagerProfessionData)
        {
            Workplace[] workplaces;
            
            switch (villagerProfessionData.Type) {
                case ProfessionType.Unemployed:
                    workplaces = Managers.I.Buildings.GetAllWorkplacesOfClass(BuildingType.Village,
                        typeof(TownHall));
                    break;
                
                case ProfessionType.Builder:
                case ProfessionType.Lumberjack:
                case ProfessionType.GlobalHauler:
                    workplaces = Managers.I.Buildings.GetAllFreeWorkplacesOfClass(
                        villagerProfessionData.WorkplaceBuildingType, villagerProfessionData.WorkplaceType);
                    break;
                
                case ProfessionType.WorkplaceHauler:
                    workplaces = Managers.I.Buildings.GetAllWorkplacesWithHaulersToHire();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return workplaces;
        }

        public Workplace GetClosestFreeWorkplaceForProfession(Villager_ProfessionData villagerProfessionData, Vector3 position)
        {
            Workplace[] freeWorkplaces = GetAllFreeWorkplacesForProfession(villagerProfessionData);

            if (freeWorkplaces.Length == 0) 
                return null;
            
            Workplace closestWorkplace = freeWorkplaces[0];
            float bestDistance = Vector3.Distance(closestWorkplace.transform.position, position);
            
            for (int i = 1; i < freeWorkplaces.Length; i++) {
                float distance = Vector3.Distance(freeWorkplaces[i].transform.position, position);
                
                if (distance > bestDistance) continue;
                bestDistance = distance;
                closestWorkplace = freeWorkplaces[i];
            }
            return closestWorkplace;
        }

        public Building GetClosestBuildingOfClass(BuildingType buildingType, Type classType, Vector3 position)
        {
            List<Building> buildings = new List<Building>(GetAllBuildingOfClass(buildingType, classType));
            
            if (buildings.Count == 0) 
                return null;
            
            Building closestBuilding = buildings[0];
            float bestDistance = Vector3.Distance(closestBuilding.transform.position, position);

            for (int i = 1; i < buildings.Count; i++) {
                float distance = Vector3.Distance(buildings[i].transform.position, position);

                if (distance > bestDistance) continue;
                bestDistance = distance;
                closestBuilding = buildings[i];
            }
            return closestBuilding;
        }
    }
}
