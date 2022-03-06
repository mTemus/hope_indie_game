using System;
using _Prototype.Code.Characters.Villagers.Professions;
using _Prototype.Code.GUI.UIElements.SelectableElement;
using _Prototype.Code.System;
using _Prototype.Code.World.Buildings.Workplaces;
using UnityEngine;

namespace _Prototype.Code.GUI.Villager.Selecting
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfessionLabelItem : UiSelectableElement
    {
        [SerializeField] private Data data;

        private Workplace[] _workplaces = Array.Empty<Workplace>();
        
        public override void OnElementSelected()
        {
            if (_workplaces.Length <= 0) return; 
            Managers.I.Cameras.FocusCameraOn(_workplaces[0].transform);
        }

        public override void OnElementDeselected()
        {
            
        }

        public override void InvokeSelectedElement()
        {
            attachedEvent.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="normalHeight"></param>
        public void ResetLabel(float normalHeight)
        {
            RectTransform r = GetComponent<RectTransform>();
            r.sizeDelta = new Vector2(r.sizeDelta.x, normalHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadWorkplaces()
        {
            _workplaces = Managers.I.Buildings.GetAllFreeWorkplacesForProfession(data);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void ClearWorkplaces()
        {
            Array.Clear(_workplaces, 0, _workplaces.Length);
            _workplaces = null;
        }

        public Workplace[] Workplaces => _workplaces;

        public Data Data => data;
    }
}
