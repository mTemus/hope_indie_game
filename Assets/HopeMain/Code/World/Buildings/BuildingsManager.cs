using System;
using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.Characters.Villagers.Professions;
using HopeMain.Code.System;
using HopeMain.Code.World.Buildings.Type.Village;
using HopeMain.Code.World.Buildings.Workplace;
using UnityEngine;

namespace HopeMain.Code.World.Buildings
{
    /// <summary>
    /// 
    /// </summary>
    public enum BuildingType {
        Resources,
        Village,
        Industry,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class BuildingsManager : MonoBehaviour
    {
        private readonly List<Building> _resourcesBuildings = new List<Building>();
        private readonly List<Building> _villageBuildings = new List<Building>();
        private readonly List<Building> _industryBuildings = new List<Building>();

        private List<Building> GetBuildingsByType(BuildingType buildingType)
        {
            return buildingType switch {
                BuildingType.Resources => _resourcesBuildings,
                BuildingType.Village => _villageBuildings,
                BuildingType.Industry => _industryBuildings,
                _ => null
            };
        }

        private void AddWorkplacesWithoutHaulers(List<Building> listIn, List<Workplaces.Workplace> listOut)
        {
            listOut.AddRange(listIn.Cast<Workplaces.Workplace>()
                .Where(workplace => workplace.CanHireHauler));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="building"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void AddBuilding(BuildingType buildingType, Building building)
        {
            switch (buildingType) {
                case BuildingType.Resources:
                    _resourcesBuildings.Add(building);
                    break;
                
                case BuildingType.Village:
                    _villageBuildings.Add(building);
                    break;
                
                case BuildingType.Industry:
                    _industryBuildings.Add(building);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, building.GetType().ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classType"></param>
        /// <returns></returns>
        public Building[] GetAllBuildingOfClass(BuildingType type, global::System.Type classType)
        {
            return GetBuildingsByType(type)
                .Where(building => building.GetType() == classType)
                .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classType"></param>
        /// <returns></returns>
        public Workplaces.Workplace[] GetAllWorkplacesOfClass(BuildingType type, global::System.Type classType)
        {
            return GetBuildingsByType(type)
                .Where(building => building.GetType() == classType)
                .Cast<Workplaces.Workplace>()
                .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="classType"></param>
        /// <returns></returns>
        public Workplaces.Workplace[] GetAllFreeWorkplacesOfClass(BuildingType buildingType, global::System.Type classType)
        {
            return GetBuildingsByType(buildingType)
                .Cast<Workplaces.Workplace>()
                .Where(workplace => workplace.GetType() == classType)
                .Where(workplace => workplace.CanHireWorker)
                .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Workplaces.Workplace[] GetAllWorkplacesWithHaulersToHire()
        {
            List<Workplaces.Workplace> workplaces = new List<Workplaces.Workplace>();

            AddWorkplacesWithoutHaulers(_resourcesBuildings, workplaces);
            AddWorkplacesWithoutHaulers(_industryBuildings, workplaces);

            return workplaces.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="professionData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Workplaces.Workplace[] GetAllFreeWorkplacesForProfession(Characters.Villagers.Professions.Data professionData)
        {
            Workplaces.Workplace[] workplaces;
            
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="professionData"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Workplaces.Workplace GetClosestFreeWorkplaceForProfession(Characters.Villagers.Professions.Data professionData, Vector3 position)
        {
            Workplaces.Workplace[] freeWorkplaces = GetAllFreeWorkplacesForProfession(professionData);

            if (freeWorkplaces.Length == 0) 
                return null;
            
            Workplaces.Workplace closestWorkplace = freeWorkplaces[0];
            float bestDistance = Vector3.Distance(closestWorkplace.transform.position, position);
            
            for (int i = 1; i < freeWorkplaces.Length; i++) {
                float distance = Vector3.Distance(freeWorkplaces[i].transform.position, position);
                
                if (distance > bestDistance) continue;
                bestDistance = distance;
                closestWorkplace = freeWorkplaces[i];
            }
            return closestWorkplace;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="classType"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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
