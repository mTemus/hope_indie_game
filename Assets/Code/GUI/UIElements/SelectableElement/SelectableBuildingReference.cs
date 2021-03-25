using Code.Map.Building;
using Code.System;
using UnityEngine;

namespace Code.GUI.UIElements.SelectableElement
{
    public class SelectableBuildingReference : UiSelectableElement
    {
        private Transform building;
        
        public override void OnElementSelected()
        {
            
        }

        public override void OnElementDeselected()
        {
            
        }

        public override void InvokeSelectedElement()
        {
            // ADD arrow above building
            // ADD camera movement to transform then focus
            
            Managers.Instance.Cameras.FocusCameraOn(building);
        }

        public void SetBuilding(Building b)
        {
            building = b.transform;
        }
    }
}
