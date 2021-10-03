using System.Linq;
using Code.Map.Building.Systems;
using Code.Map.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.BuildingSelecting
{
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
