using HopeMain.Code.Characters.Villagers.Profession;
using TMPro;
using UnityEngine;

namespace HopeMain.Code.GUI.Villager.Selecting
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

        public void LoadProfessionData(Villager_ProfessionData villagerProfessionData, Characters.Villagers.Entity.Villager villager)
        {
            SetProfessionText(strengthValueProfession, villagerProfessionData.RequiredStatistics.Strength);
            SetProfessionText(dexterityValueProfession, villagerProfessionData.RequiredStatistics.Dexterity);
            SetProfessionText(intelligenceValueProfession, villagerProfessionData.RequiredStatistics.Intelligence);
            
            SetVillagerText(strengthValueVillager, villager.Statistics.Strength, villagerProfessionData.RequiredStatistics.Strength);
            SetVillagerText(dexterityValueVillager, villager.Statistics.Dexterity, villagerProfessionData.RequiredStatistics.Dexterity);
            SetVillagerText(intelligenceValueVillager, villager.Statistics.Intelligence, villagerProfessionData.RequiredStatistics.Intelligence);

            goldValue.text = villagerProfessionData.GoldPerDay.ToString();
        }

        public void ShowNotAvailableWorkplacesPanel(bool condition)
        {
            noAvailableWorkplacePanel.SetActive(condition);
        }
        
    }
}
