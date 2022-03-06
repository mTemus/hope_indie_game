using System;
using System.Linq;
using _Prototype.Code.GUI.UIElements;
using _Prototype.Code.System;
using _Prototype.Code.World.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Prototype.Code.GUI.Player.BuildingSelecting
{
    /// <summary>
    /// 
    /// </summary>
    public class BuildingSelectingMenu : MonoBehaviour
    {
        [Header("Scroll Areas")]
        [SerializeField] private UIScrollArea buildingTypesArea;
        [SerializeField] private UIScrollArea buildingObjectsArea;

        [Header("Building Types")] 
        [SerializeField] private Transform buildingTypesContent;

        [Header("Building Objects")] 
        [SerializeField] private UiBuildingElement[] resourcesBuildingObjects;
        [SerializeField] private UiBuildingElement[] villageBuildingObjects;
        [SerializeField] private UiBuildingElement[] industryBuildingObjects;


        [Header("Building Properties")] 
        [SerializeField] private Image miniature;
        [SerializeField] private TextMeshProUGUI buildingName;
        [SerializeField] private TextMeshProUGUI buildingDescription;
        [SerializeField] private UiRequiredResource[] requiredResources;
        
        private int _buildingTypesIdx;
        private int _buildingObjectsIdx;

        private UiBuildingElement _currentBuilding;

        private UiBuildingElement[] _buildingObjectsArray;

        private void Awake()
        {
            _buildingObjectsArray = resourcesBuildingObjects;
            gameObject.SetActive(false);
        }

        private void UpdateBuildingProperties()
        {
            ResetResources();
            
            miniature.sprite = _currentBuilding.Sprite;
            buildingName.text = _currentBuilding.Data.BuildingName;
            buildingDescription.text = _currentBuilding.Description;

            foreach (Resource resourceData in _currentBuilding.Data.RequiredResources) {
                UiRequiredResource uiResource = requiredResources.FirstOrDefault(r => r.Type == resourceData.Type);

                if (uiResource != null) {
                    uiResource.gameObject.SetActive(true);
                    uiResource.SetAmount(resourceData.amount);
                }
                else {
                    throw new Exception("No ui resource for type: " + resourceData.Type);
                }
            }
        }

        private void UpdateBuildingObjectsArray()
        {
            switch (_buildingTypesIdx) {
                case 0:
                    _buildingObjectsArray = resourcesBuildingObjects;
                    break;
                
                case 1:
                    _buildingObjectsArray = villageBuildingObjects;
                    break;
                
                case 2:
                    _buildingObjectsArray = industryBuildingObjects;
                    break;
            }
        }

        private void ResetResources()
        {
            foreach (UiRequiredResource resource in requiredResources) 
                resource.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnMenuOpen()
        {
            _currentBuilding = _buildingObjectsArray[_buildingObjectsIdx];
            Systems.I.Building.SetBuilding(_currentBuilding.Data);
            UpdateBuildingProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnMenuClose()
        {
            _buildingObjectsIdx = 0;
            _buildingTypesIdx = 0;
            
            ResetResources();
            UpdateBuildingObjectsArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ChangeBuildingType(int value)
        {
            _buildingTypesIdx += value;
            _buildingTypesIdx = Mathf.Clamp(_buildingTypesIdx, 0, buildingTypesContent.childCount - 1);
            
            buildingTypesArea.ChangeValue(value);
            buildingObjectsArea.ChangeContent(_buildingTypesIdx);
            UpdateBuildingObjectsArray();
            
            _buildingObjectsIdx = 0;
            _currentBuilding = _buildingObjectsArray[_buildingObjectsIdx];
            Systems.I.Building.SetBuilding(_currentBuilding.Data);
            UpdateBuildingProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ChangeBuilding(int value)
        {
            _buildingObjectsIdx += value;
            _buildingObjectsIdx = Mathf.Clamp(_buildingObjectsIdx, 0, _buildingObjectsArray.Length - 1);

            buildingObjectsArea.ChangeValue(-value);
            _currentBuilding = _buildingObjectsArray[_buildingObjectsIdx];
            Systems.I.Building.SetBuilding(_currentBuilding.Data);
            UpdateBuildingProperties();
        }
    }
}
