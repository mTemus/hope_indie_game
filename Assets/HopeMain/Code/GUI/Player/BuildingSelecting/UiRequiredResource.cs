using HopeMain.Code.System;
using HopeMain.Code.World.Resources;
using TMPro;
using UnityEngine;

namespace HopeMain.Code.GUI.Player.BuildingSelecting
{
    /// <summary>
    /// 
    /// </summary>
    public class UiRequiredResource : MonoBehaviour
    {
        [SerializeField] private ResourceType type;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private TextMeshProUGUI current;

        private int _requiredAmount;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateRequired()
        {
            int currentAmount = Managers.I.Resources.GetResourceByType(type).amount;
            current.text = "(" + currentAmount + ")";
            current.color = currentAmount - _requiredAmount < 0 ? Color.red : Color.white;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetAmount(int value)
        {
            _requiredAmount = value;
            amount.text = _requiredAmount.ToString();
        }
        
        public ResourceType Type => type;
    }
}
