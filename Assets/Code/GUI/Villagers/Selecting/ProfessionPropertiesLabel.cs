using Code.Villagers.Entity;
using Code.Villagers.Professions;
using TMPro;
using UnityEngine;

namespace Code.GUI.Villagers.Selecting
{
    public class ProfessionPropertiesLabel : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color goodColor;
        [SerializeField] private Color neutralColor;
        [SerializeField] private Color badColor;
        [SerializeField] private Color normalColor;

        [Header("Text values")]
        [SerializeField] private TextMeshProUGUI strengthValueProfession;
        [SerializeField] private TextMeshProUGUI strengthValueVillager;
        [SerializeField] private TextMeshProUGUI dexterityValueProfession;
        [SerializeField] private TextMeshProUGUI dexterityValueVillager;
        [SerializeField] private TextMeshProUGUI intelligenceValueProfession;
        [SerializeField] private TextMeshProUGUI intelligenceValueVillager;
        [SerializeField] private TextMeshProUGUI goldValue;

        [Header("Workplace values")] 
        [SerializeField] private GameObject noAvailableWorkplacePanel;

        private Vector2 labelSize;
        
        private void Awake()
        {
            labelSize = GetComponent<RectTransform>().rect.size;
        }

        private Color CompareStats(int profession, int villager)
        {
            int difference = profession - villager;
            
            if (difference >= 5) 
                return badColor;
            if (difference > 1) 
                return neutralColor;
            
            return goodColor;
        }

        private void SetProfessionText(TextMeshProUGUI text, int professionValue)
        {
            text.text = professionValue.ToString();
        }

        private void SetVillagerText(TextMeshProUGUI text, int villagerValue, int professionValue)
        {
            text.text = "(" + villagerValue + ")";
            text.color = CompareStats(professionValue, villagerValue);
        }

        public void AttachPanelToProfession(Transform parent)
        {
            transform.SetParent(parent);
            parent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, labelSize.y);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        public void LoadProfessionData(ProfessionData professionData, Villager villager)
        {
            SetProfessionText(strengthValueProfession, professionData.RequiredStats.Strength);
            SetProfessionText(dexterityValueProfession, professionData.RequiredStats.Dexterity);
            SetProfessionText(intelligenceValueProfession, professionData.RequiredStats.Intelligence);
            
            SetVillagerText(strengthValueVillager, villager.Statistics.Strength, professionData.RequiredStats.Strength);
            SetVillagerText(dexterityValueVillager, villager.Statistics.Dexterity, professionData.RequiredStats.Dexterity);
            SetVillagerText(intelligenceValueVillager, villager.Statistics.Intelligence, professionData.RequiredStats.Intelligence);

            goldValue.text = professionData.GoldPerDay.ToString();
        }

        public void ShowNotAvailableWorkplacesPanel(bool condition)
        {
            noAvailableWorkplacePanel.SetActive(condition);
        }
        
    }
}
