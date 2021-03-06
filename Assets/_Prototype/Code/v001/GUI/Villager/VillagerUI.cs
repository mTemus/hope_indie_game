using _Prototype.Code.v001.System.Assets;
using _Prototype.Code.v001.World.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Prototype.Code.v001.GUI.Villager
{
    /// <summary>
    /// 
    /// </summary>
    public class VillagerUI : MonoBehaviour
    {
        [Header("GUI")] 
        [SerializeField] private TextMeshProUGUI professionName;
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private Image resourceImage;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="carriedResource"></param>
        public void SetResourceIcon(ResourceType carriedResource)
        {
            if (resourceImage.gameObject.activeSelf) return;
            resourceImage.sprite = AssetsStorage.I.GetResourceIcon(carriedResource);
            resourceImage.gameObject.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearResourceIcon()
        {
            resourceImage.sprite = null;
            resourceImage.gameObject.SetActive(false);
        }

        public TextMeshProUGUI ProfessionName => professionName;

        public TextMeshProUGUI StateText => stateText;
    }
}
