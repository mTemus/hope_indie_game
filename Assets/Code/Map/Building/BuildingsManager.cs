using System;
using System.Collections.Generic;
using System.Linq;
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
            return (from buildingKVP in buildings where buildingKVP.Key == buildingType select buildingKVP.Value).ToArray();
        }
        
        public Warehouse GetClosestWarehouse() =>
            Managers.Instance.Areas.GetVillageArea().GetWarehouse();
        
        
        
    }
}
