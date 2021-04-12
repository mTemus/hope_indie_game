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

        private Workplace[] workplaces = new Workplace[0];
        
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

        public void ResetLabel(float normalHeight)
        {
            RectTransform r = GetComponent<RectTransform>();
            r.sizeDelta = new Vector2(r.sizeDelta.x, normalHeight);
        }

        public void LoadWorkplaces()
        {
            //TODO: convert to switch for workplace haulers

            Building[] buildings = professionData.ProfessionType == ProfessionType.Unemployed
                ? Managers.Instance.Buildings.GetAllBuildingOfClass(professionData.WorkplaceBuildingType,
                    professionData.WorkplaceType)
                : Managers.Instance.Buildings.GetAllFreeWorkplacesOfClass(professionData.WorkplaceBuildingType,
                    professionData.WorkplaceType);

            workplaces = buildings.Cast<Workplace>().ToArray();
        }
        
        public void ClearWorkplaces()
        {
            Array.Clear(workplaces, 0, workplaces.Length);
            workplaces = null;
        }

        public Workplace[] Workplaces => workplaces;

        public ProfessionData ProfessionData => professionData;
    }
}
