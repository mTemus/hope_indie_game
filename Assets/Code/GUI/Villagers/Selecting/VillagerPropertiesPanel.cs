using Code.GUI.UIElements;
using Code.System;
using Code.Utilities;
using Code.Villagers.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.Villagers
{
    public class VillagerPropertiesPanel : MonoBehaviour
    {
        [Header("Villager Data")]
        [SerializeField] private Image portraitImage;
        [SerializeField] private TextMeshProUGUI villagerNameText;
        [SerializeField] private TextMeshProUGUI villagerProfessionText;
        
        [Header("Villager Status")] 
        [SerializeField] private Transform status;

        [SerializeField] private TextMeshProUGUI strengthValue;
        [SerializeField] private TextMeshProUGUI dexterity;
        [SerializeField] private TextMeshProUGUI intelligenceValue;
        
        [Header("Villager Skills")] 
        [SerializeField] private Transform skills;

        [Header("Script elements")] 
        [SerializeField] private UiSelectingPointer pointer;
        [SerializeField] private UiSelectableElement[] elementsToSelect;
   
        private int selectionIdx;
        private UiSelectableElement currentElement;
        
        private void Awake()
        {
            currentElement = elementsToSelect[selectionIdx];
            currentElement.OnElementSelected();
            pointer.SetPointerOnUiElement(currentElement.transform);
            
            gameObject.SetActive(false);
        }

        public void MovePointer(int value)
        {
            selectionIdx = GlobalUtilities.IncrementIdx(selectionIdx, value, elementsToSelect.Length);
            currentElement.OnElementDeselected();
            currentElement = elementsToSelect[selectionIdx];
            pointer.SetPointerOnUiElement(currentElement.transform);
            currentElement.OnElementSelected();
        }

        public void UseSelectedElement()
        {
            currentElement.InvokeSelectedElement();
        }

        public void OpenPropertiesPanel(Villager villager)
        {
            villagerNameText.text = villager.name;
            villagerProfessionText.text = villager.Profession.Type.ToString();
            strengthValue.text = villager.Statistics.Strength.ToString();
            dexterity.text = villager.Statistics.Dexterity.ToString();
            intelligenceValue.text = villager.Statistics.Intelligence.ToString();
        }
    
        public void FocusCameraOnVillagerWorkplace()
        {
            Managers.Instance.Cameras.FocusCameraOn(Managers.Instance.VillagerSelection.SelectedVillager.Profession.Workplace.transform);
        }

        public void FocusCameraOnVillagerHouse()
        {
            
        }
        
    }
}
