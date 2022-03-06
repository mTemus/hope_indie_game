using _Prototype.Code.Characters.Villagers.Professions;
using TMPro;
using UnityEngine;

namespace _Prototype.Code.GUI.Villager.Selecting
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

        private Vector2 _labelSize;
        
        private void Awake()
        {
            _labelSize = GetComponent<RectTransform>().rect.size;
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
            parent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, _labelSize.y);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        public void LoadProfessionData(Data professionData, Characters.Villagers.Entity.Villager villager)
        {
            SetProfessionText(strengthValueProfession, professionData.RequiredStatistics.Strength);
            SetProfessionText(dexterityValueProfession, professionData.RequiredStatistics.Dexterity);
            SetProfessionText(intelligenceValueProfession, professionData.RequiredStatistics.Intelligence);
            
            SetVillagerText(strengthValueVillager, villager.Statistics.Strength, professionData.RequiredStatistics.Strength);
            SetVillagerText(dexterityValueVillager, villager.Statistics.Dexterity, professionData.RequiredStatistics.Dexterity);
            SetVillagerText(intelligenceValueVillager, villager.Statistics.Intelligence, professionData.RequiredStatistics.Intelligence);

            goldValue.text = professionData.GoldPerDay.ToString();
        }

        public void ShowNotAvailableWorkplacesPanel(bool condition)
        {
            noAvailableWorkplacePanel.SetActive(condition);
        }
        
    }
}
