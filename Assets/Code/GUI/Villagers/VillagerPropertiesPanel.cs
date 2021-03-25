using Code.GUI.UIElements;
using Code.GUI.UIElements.SelectableElement;
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
        [SerializeField] private SelectableBuildingReference home;
        [SerializeField] private SelectableBuildingReference workplace;


        [Header("Villager Status")] 
        [SerializeField] private Transform status;
        
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
            villagerProfessionText.text = villager.Profession.Type.ToString().ToLower();
            workplace.SetBuilding(villager.Profession.Workplace);
        }
        
        
    }
}
