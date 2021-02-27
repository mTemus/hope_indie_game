using UnityEngine;

namespace Code.Map.Building
{
    public class BuildingData
    {
        private BuildingScript buildingScript;
        private Transform buildingObject;


        public BuildingScript BuildingScript
        {
            get => buildingScript;
            set => buildingScript = value;
        }

        public Transform BuildingObject
        {
            get => buildingObject;
            set => buildingObject = value;
        }
    }
}
