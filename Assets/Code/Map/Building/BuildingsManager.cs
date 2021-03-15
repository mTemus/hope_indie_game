using Code.Map.Building.Buildings.Components;
using Code.System;
using UnityEngine;

namespace Code.Map.Building
{
    public class BuildingsManager : MonoBehaviour
    {
        public Warehouse GetClosestWarehouse() =>
            Managers.Instance.Areas.GetVillageArea().GetWarehouse();
        
        
        
    }
}
