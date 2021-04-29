using Code.Map.Resources;
using Code.System;
using TMPro;
using UnityEngine;

namespace Code.GUI.BuildingSelecting
{
    public class UiRequiredResource : MonoBehaviour
    {
        [SerializeField] private ResourceType type;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private TextMeshProUGUI current;

        private int requiredAmount;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void UpdateRequired()
        {
            int currentAmount = Managers.Instance.Resources.GetResourceByType(type).amount;
            current.text = "(" + currentAmount + ")";
            current.color = currentAmount - requiredAmount < 0 ? Color.red : Color.white;
        }

        public void SetAmount(int value)
        {
            requiredAmount = value;
            amount.text = requiredAmount.ToString();
        }
        
        public ResourceType Type => type;
    }
}
