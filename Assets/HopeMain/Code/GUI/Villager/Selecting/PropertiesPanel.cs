using HopeMain.Code.GUI.UIElements.SelectableElement;
using HopeMain.Code.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HopeMain.Code.GUI.Villager.Selecting
{
    public class PropertiesPanel : UiSelectablePanel
    {
        [Header("Villager Data")]
        [SerializeField] private Image portraitImage;
        [SerializeField] private TextMeshProUGUI villagerNameText;
        [SerializeField] private TextMeshProUGUI villagerProfessionText;
        
        [Header("Villager Status")] 
        [SerializeField] private Transform status;
        
        [Header("Villager Statistics")] 
        [SerializeField] private TextMeshProUGUI strengthValue;
        [SerializeField] private TextMeshProUGUI dexterity;
        [SerializeField] private TextMeshProUGUI intelligenceValue;
        
        [Header("Villager Skills")] 
        [SerializeField] private Transform skills;
        
        private void Awake()
        {
            currentElement = elementsToSelect[selectionIdx];
            currentElement.OnElementSelected();
            pointer.SetPointerOnUiElement(currentElement.transform);
            
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="villager"></param>
        public void OpenPropertiesPanel(Characters.Villagers.Entity.Villager villager)
        {
            villagerNameText.text = villager.name;
            villagerProfessionText.text = villager.Profession.Data.Type.ToString();
            strengthValue.text = villager.Statistics.Strength.ToString();
            dexterity.text = villager.Statistics.Dexterity.ToString();
            intelligenceValue.text = villager.Statistics.Intelligence.ToString();
        }
    
        /// <summary>
        /// 
        /// </summary>
        public void FocusCameraOnVillagerWorkplace()
        {
            Managers.I.Cameras.FocusCameraOn(Managers.I.Selection.SelectedVillager.Profession.Workplace.transform);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FocusCameraOnVillagerHouse()
        {
            
        }
        
    }
}
