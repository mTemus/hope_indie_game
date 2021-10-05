using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.Characters.Villagers.Profession;
using HopeMain.Code.GUI.Villager;
using UnityEngine;

namespace HopeMain.Code.Characters.Villagers.Entity
{
    public class Villager : MonoBehaviour
    {
        [Header("Villager properties")]
        [SerializeField] private int healthPoints;
        [SerializeField] private float speed = 5f;
        [SerializeField] private VillagersStatistics statistics;

        [Header("Villager components")] 
        [SerializeField] private Villager_Brain brain;
        [SerializeField] private Villager_Profession profession;
        [SerializeField] private VillagerUi ui;

        public Villager_Brain Brain => brain;
        public VillagersStatistics Statistics => statistics;

        public Villager_Profession Profession
        {
            get => profession;
            set => profession = value;
        }

        public VillagerUi UI => ui;
    }
}
