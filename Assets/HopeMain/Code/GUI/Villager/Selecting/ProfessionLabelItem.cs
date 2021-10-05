using System;
using HopeMain.Code.Characters.Villagers.Profession;
using HopeMain.Code.GUI.UIElements.SelectableElement;
using HopeMain.Code.System;
using HopeMain.Code.World.Buildings.Workplace;
using UnityEngine;

namespace HopeMain.Code.GUI.Villager.Selecting
{
    public class ProfessionLabelItem : UiSelectableElement
    {
        [SerializeField] private Data data;

        private WorkplaceBase[] workplaces = Array.Empty<WorkplaceBase>();
        
        public override void OnElementSelected()
        {
            if (workplaces.Length <= 0) return; 
            Managers.I.Cameras.FocusCameraOn(workplaces[0].transform);
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
            workplaces = Managers.I.Buildings.GetAllFreeWorkplacesForProfession(data);
        }
        
        public void ClearWorkplaces()
        {
            Array.Clear(workplaces, 0, workplaces.Length);
            workplaces = null;
        }

        public WorkplaceBase[] Workplaces => workplaces;

        public Data Data => data;
    }
}
