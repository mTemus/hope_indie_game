using Code.Map.Resources;
using Code.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Villagers.Entity
{
    public class VillagerUi : MonoBehaviour
    {
        [Header("GUI")] 
        [SerializeField] private TextMeshProUGUI professionName;
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private Image resourceImage;
        
        public void SetResourceIcon(bool visible, ResourceType carriedResource)
        {
            //TODO: rework this, its a mess
            if (resourceImage.gameObject.activeSelf && visible) return;
            if (visible) {
                resourceImage.gameObject.SetActive(true);
                resourceImage.sprite = AssetsStorage.I.GetResourceIcon(carriedResource);
            }
            else {
                resourceImage.sprite = null;
                resourceImage.gameObject.SetActive(false);
            }
        }

        public TextMeshProUGUI ProfessionName => professionName;

        public TextMeshProUGUI StateText => stateText;
    }
}
