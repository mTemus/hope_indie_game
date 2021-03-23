using Code.Resources;
using TMPro;
using UnityEngine;

namespace Code.GUI.BuildingSelecting
{
    public class UiRequiredResource : MonoBehaviour
    {
        [SerializeField] private ResourceType type;
        [SerializeField] private TextMeshProUGUI amount;

        public ResourceType Type => type;

        public TextMeshProUGUI Amount => amount;

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
