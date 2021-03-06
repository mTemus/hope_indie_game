using System.Linq;
using _Prototype.Code.v001.World.Buildings.Systems;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace _Prototype.Code.v001.GUI.Player.BuildingSelecting
{
    /// <summary>
    /// 
    /// </summary>
    public class RequiredResourcesPanel : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private UiRequiredResource[] resources;

        [Header("GUI Parts")] 
        [SerializeField] private RectTransform panelRectTransform;
        [SerializeField] private VerticalLayoutGroup layoutGroup;

        private void Awake()
        {
            enabled = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            foreach (UiRequiredResource resource in resources
                .Where(resource => resource.gameObject.activeSelf)) 
            {
                resource.UpdateRequired();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnPanelOpen()
        {
            Resource[] buildingResources = BuildingSystem.CurrentBuildingData.RequiredResources;

            int resourcesCnt = 0;

            foreach (UiRequiredResource resource in resources) {
                foreach (Resource buildingResource in buildingResources) {
                    if (resource.Type != buildingResource.Type) continue;
                    resource.gameObject.SetActive(true);
                    resource.SetAmount(buildingResource.amount);
                    resourcesCnt++;
                }
            }

            float elementHeight = resources[0].GetComponent<RectTransform>().rect.height;
            float newPanelHeight = elementHeight * resourcesCnt + layoutGroup.spacing * (resourcesCnt - 1) + layoutGroup.padding.top + layoutGroup.padding.bottom;
            panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, newPanelHeight);
            enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnPanelClose()
        {
            foreach (UiRequiredResource resource in resources) {
                resource.gameObject.SetActive(false);
            }

            enabled = false;
            gameObject.SetActive(false);
        }
    }
}
