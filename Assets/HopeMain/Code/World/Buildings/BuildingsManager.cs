using System;
using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.Characters.Villagers.Profession;
using HopeMain.Code.System;
using HopeMain.Code.World.Buildings.Type.Village;
using HopeMain.Code.World.Buildings.Workplace;
using UnityEngine;

namespace HopeMain.Code.World.Buildings
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

        private void AddWorkplacesWithoutHaulers(List<Building> listIn, List<WorkplaceBase> listOut)
        {
            listOut.AddRange(listIn.Cast<WorkplaceBase>()
                .Where(workplace => workplace.CanHireHauler));
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

        public Building[] GetAllBuildingOfClass(BuildingType type, global::System.Type classType)
        {
            return GetBuildingsByType(type)
                .Where(building => building.GetType() == classType)
                .ToArray();
        }

        public WorkplaceBase[] GetAllWorkplacesOfClass(BuildingType type, global::System.Type classType)
        {
            return GetBuildingsByType(type)
                .Where(building => building.GetType() == classType)
                .Cast<WorkplaceBase>()
                .ToArray();
        }

        public WorkplaceBase[] GetAllFreeWorkplacesOfClass(BuildingType buildingType, global::System.Type classType)
        {
            return GetBuildingsByType(buildingType)
                .Cast<WorkplaceBase>()
                .Where(workplace => workplace.GetType() == classType)
                .Where(workplace => workplace.CanHireWorker)
                .ToArray();
        }

        public WorkplaceBase[] GetAllWorkplacesWithHaulersToHire()
        {
            List<WorkplaceBase> workplaces = new List<WorkplaceBase>();

            AddWorkplacesWithoutHaulers(resourcesBuildings, workplaces);
            AddWorkplacesWithoutHaulers(industryBuildings, workplaces);

            return workplaces.ToArray();
        }

        public WorkplaceBase[] GetAllFreeWorkplacesForProfession(Characters.Villagers.Profession.Data professionData)
        {
            WorkplaceBase[] workplaces;
            
            switch (professionData.Type) {
                case ProfessionType.Unemployed:
                    workplaces = Managers.I.Buildings.GetAllWorkplacesOfClass(BuildingType.Village,
                        typeof(TownHall));
                    break;
                
                case ProfessionType.Builder:
                case ProfessionType.Lumberjack:
                case ProfessionType.GlobalHauler:
                case ProfessionType.StoneMiner:
                    workplaces = Managers.I.Buildings.GetAllFreeWorkplacesOfClass(
                        professionData.WorkplaceBuildingType, professionData.WorkplaceType);
                    break;
                
                case ProfessionType.WorkplaceHauler:
                    workplaces = Managers.I.Buildings.GetAllWorkplacesWithHaulersToHire();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return workplaces;
        }

        public WorkplaceBase GetClosestFreeWorkplaceForProfession(Characters.Villagers.Profession.Data professionData, Vector3 position)
        {
            WorkplaceBase[] freeWorkplaces = GetAllFreeWorkplacesForProfession(professionData);

            if (freeWorkplaces.Length == 0) 
                return null;
            
            WorkplaceBase closestWorkplace = freeWorkplaces[0];
            float bestDistance = Vector3.Distance(closestWorkplace.transform.position, position);
            
            for (int i = 1; i < freeWorkplaces.Length; i++) {
                float distance = Vector3.Distance(freeWorkplaces[i].transform.position, position);
                
                if (distance > bestDistance) continue;
                bestDistance = distance;
                closestWorkplace = freeWorkplaces[i];
            }
            return closestWorkplace;
        }

        public Building GetClosestBuildingOfClass(BuildingType buildingType, global::System.Type classType, Vector3 position)
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
