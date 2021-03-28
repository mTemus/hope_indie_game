using System;
using Code.GUI.UIElements.SelectableElement;
using Code.Map.Building;
using Code.System;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.GUI.Villagers.Selecting
{
    public class ProfessionLabelItem : UiSelectableElement
    {
        [SerializeField] private ProfessionData professionData;

        private Building[] workplaces = new Building[0];
        
        public override void OnElementSelected()
        {
            if (workplaces.Length <= 0) return; 
            Managers.Instance.Cameras.FocusCameraOn(workplaces[0].transform);
        }

        public override void OnElementDeselected()
        {
            
        }

        public override void InvokeSelectedElement()
        {
            attachedEvent.Invoke();
        }

        public void ResetSize(int normalHeight)
        {
            Rect r = GetComponent<RectTransform>().rect;
            r.size = new Vector2(r.width, normalHeight);
        }

        public void LoadWorkplaces()
        {
            workplaces = Managers.Instance.Buildings.GetAllFreeWorkplacesOfType(professionData.WorkplaceType);
        }
        
        public void ClearWorkplaces()
        {
            Array.Clear(workplaces, 0, workplaces.Length);
            workplaces = null;
        }

        public Building[] Workplaces => workplaces;

        public ProfessionData ProfessionData => professionData;
    }
}
