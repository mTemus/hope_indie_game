using UnityEngine;

namespace Code.Map.Building
{
    //TODO: remove this and store reference to Building.cs instead
    
    public class BuildingData
    {
        private Transform buildingObject;
        
        public Transform BuildingObject
        {
            get => buildingObject;
            set => buildingObject = value;
        }
    }
}
