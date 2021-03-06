using _Prototype.Code.v001.World.Buildings;
using UnityEngine;

namespace _Prototype.Code.v001.System.Initialization
{
    public class InitializeBuilding : InitializeObject
    {
        public override void InitializeMe()
        {
            Building building = GetComponent<Building>();
            Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position))
                .AddBuilding(building, building.Data);
            Managers.I.Buildings.AddBuilding(building.Data.BuildingType, building);
            building.Initialize();
            DestroyImmediate(this);
        }
    }
}
