using Code.Map.Resources;
using Code.System.Assets;
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
        
        public void SetResourceIcon(ResourceType carriedResource)
        {
            if (resourceImage.gameObject.activeSelf) return;
            resourceImage.sprite = AssetsStorage.I.GetResourceIcon(carriedResource);
            resourceImage.gameObject.SetActive(true);
        }

        public void ClearResourceIcon()
        {
            resourceImage.sprite = null;
            resourceImage.gameObject.SetActive(false);
        }

        public TextMeshProUGUI ProfessionName => professionName;

        public TextMeshProUGUI StateText => stateText;
    }
}
