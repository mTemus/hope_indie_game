using _Prototype.Code.GUI.UIElements;
using _Prototype.Code.GUI.UIElements.SelectableElement;
using _Prototype.Code.System;
using _Prototype.Code.System.GameInput;
using _Prototype.Code.World.Buildings.Workplaces;
using UnityEngine;
using UnityEngine.UI;

namespace _Prototype.Code.GUI.Villager.Selecting
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfessionChangingPanel : UiSelectablePanel
    {
        [Header("Properties")] 
        [SerializeField] private float normalLabelHeight;
        
        [Header("Components")]
        [SerializeField] private UiAcceptancePanel acceptancePanel;
        [SerializeField] private UIVerticalGroup professionsGroup;
        
        [Header("Additional pointers")] 
        [SerializeField] private Image currentProfessionPointer;
        [SerializeField] private ProfessionPropertiesLabel propertiesLabel;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;

        private int _workplacesIdx;
        private ProfessionLabelItem _currentProfession;

        private void Awake()
        {
            currentElement = elementsToSelect[0];
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCurrentWorkPointerPosition()
        {
            Characters.Villagers.Entity.Villager villager = Managers.I.Selection.SelectedVillager;

            foreach (UiSelectableElement element in elementsToSelect) {
                ProfessionLabelItem currItem = (ProfessionLabelItem) element;
                
                if (currItem.Data.Type != villager.Profession.Data.Type) continue;
                RectTransform pointerRect = currentProfessionPointer.GetComponent<RectTransform>();
                currentProfessionPointer.transform.SetParent(element.transform);
                
                float newY = normalLabelHeight / 2;
                pointerRect.anchoredPosition = new Vector2(newY, -newY);
                break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReloadProfessionWorkplaces()
        {
            foreach (UiSelectableElement selectableElement in elementsToSelect) 
                selectableElement.GetComponent<ProfessionLabelItem>().LoadWorkplaces();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnPanelOpen()
        {
            InputManager.VillagerProperties.SetToVillagerProfessionDisplayChildState(Managers.I.GUI.ProfessionChangingPanel);
            Characters.Villagers.Entity.Villager villager = Managers.I.Selection.SelectedVillager;
            
            //Initialize profession pointer pos
            UpdateCurrentWorkPointerPosition();
            
            //Initialize first selected item
            _currentProfession = (ProfessionLabelItem) elementsToSelect[0];
            
            //Initialize profession properties panel
            propertiesLabel.AttachPanelToProfession(_currentProfession.transform);
            professionsGroup.UpdateElementsPosition();
            
            //Initialize pointer
            pointer.SetPointerOnUiElementWithParent(_currentProfession.transform);
            
            // Initialize workplaces
            rightArrow.SetActive(true);
            ReloadProfessionWorkplaces();
            
            //Initialize profession label data
            if (AreThereAnyWorkplaces()) {
                propertiesLabel.LoadProfessionData(_currentProfession.Data, villager); 
                Managers.I.Cameras.FocusCameraOn(_currentProfession.Workplaces[0].transform);
            }
            else {
                propertiesLabel.ShowNotAvailableWorkplacesPanel(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnPanelClose()
        {
            foreach (UiSelectableElement selectableElement in elementsToSelect) {
                ProfessionLabelItem labelItem = (ProfessionLabelItem) selectableElement;
                labelItem.ResetLabel(normalLabelHeight);
                labelItem.ClearWorkplaces();
            }

            selectionIdx = 0;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetPointerOnProfession(int value)
        {
            GetNextElement(value);
            _currentProfession.ResetLabel(normalLabelHeight);
            _currentProfession = (ProfessionLabelItem) currentElement;

            if (AreThereAnyWorkplaces()) {
                propertiesLabel.ShowNotAvailableWorkplacesPanel(false);
                propertiesLabel.LoadProfessionData(_currentProfession.Data, Managers.I.Selection.SelectedVillager);
            }
            else {
                propertiesLabel.ShowNotAvailableWorkplacesPanel(true);
            }
            
            propertiesLabel.AttachPanelToProfession(_currentProfession.transform);
            professionsGroup.UpdateElementsPosition();
            MovePointerWithParent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ShowWorkplace(int value)
        {
            //TODO: switch arrows on available workplaces, but someday, maybe
            //TODO: arrows are buggy
            if (!AreThereAnyWorkplaces()) return;
            _workplacesIdx += value;
            
            if (_workplacesIdx < 0) _workplacesIdx = 0;
            else if (_workplacesIdx >= _currentProfession.Workplaces.Length) _workplacesIdx = _currentProfession.Workplaces.Length - 1;
            
            leftArrow.SetActive(_workplacesIdx > 0);
            rightArrow.SetActive(_workplacesIdx < _currentProfession.Workplaces.Length -1);
            
            Managers.I.Cameras.FocusCameraOn(CurrentWorkplace.transform);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowAcceptancePanel()
        {
            acceptancePanel.gameObject.SetActive(true);
            InputManager.VillagerProperties.SetToNewProfessionAcceptChildState(acceptancePanel);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void TakeProfession()
        {
            Characters.Villagers.Entity.Villager selectedVillager = Managers.I.Selection.SelectedVillager;

            if (selectedVillager.Profession.Data.Type == _currentProfession.Data.Type) {
                if (selectedVillager.Profession.Workplace == CurrentWorkplace) return;
                selectedVillager.Profession.Workplace.FireWorker(selectedVillager);
                CurrentWorkplace.HireWorker(selectedVillager);
                
            } else {
                Managers.I.Professions.FireVillagerFromOldProfession(selectedVillager);
                Managers.I.Professions.SetVillagerProfession(selectedVillager,
                    _currentProfession.Data, CurrentWorkplace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //TODO: refactor
        public bool AreThereAnyWorkplaces() =>
            _currentProfession.Workplaces.Length != 0;

        public Workplace CurrentWorkplace =>
            _currentProfession.Workplaces[_workplacesIdx];

        /// <summary>
        /// 
        /// </summary>
        public void CloseAcceptablePanel()
        {
            acceptancePanel.gameObject.SetActive(false);
            InputManager.VillagerProperties.SetToVillagerProfessionDisplayChildState(Managers.I.GUI.ProfessionChangingPanel);
        }
    }
}
