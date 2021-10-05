using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.GUI.Villager;
using UnityEngine;

namespace HopeMain.Code.Characters.Villagers.Entity
{
    public class Villager : MonoBehaviour
    {
        [Header("Villager properties")]
        [SerializeField] private int healthPoints;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Statistics statistics;

        [Header("Villager components")] 
        [SerializeField] private Brain brain;
        [SerializeField] private Profession.Profession profession;
        [SerializeField] private VillagerUI ui;

        public Brain Brain => brain;
        public Statistics Statistics => statistics;

        public Profession.Profession Profession
        {
            get => profession;
            set => profession = value;
        }

        public VillagerUI UI => ui;
    }
}
