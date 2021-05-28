using System;
using Code.GUI.UIElements.SelectableElement;
using Code.Map.Building.Workplaces;
using Code.System;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.GUI.Villagers.Selecting
{
    public class ProfessionLabelItem : UiSelectableElement
    {
        [SerializeField] private Villager_ProfessionData data;

        private Workplace[] workplaces = new Workplace[0];
        
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

        public Workplace[] Workplaces => workplaces;

        public Villager_ProfessionData Data => data;
    }
}
