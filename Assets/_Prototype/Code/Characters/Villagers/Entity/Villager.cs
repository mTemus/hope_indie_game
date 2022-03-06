using _Prototype.Code.AI.Villagers.Brain;
using _Prototype.Code.Characters.Villagers.Professions;
using _Prototype.Code.GUI.Villager;
using UnityEngine;

namespace _Prototype.Code.Characters.Villagers.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Villager : MonoBehaviour
    {
        [Header("Villager properties")]
        [SerializeField] private int healthPoints;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Statistics statistics;

        [Header("Villager components")] 
        [SerializeField] private Brain brain;
        [SerializeField] private Profession profession;
        [SerializeField] private VillagerUI ui;

        public Brain Brain => brain;
        public Statistics Statistics => statistics;

        public Profession Profession
        {
            get => profession;
            set => profession = value;
        }

        public VillagerUI UI => ui;
    }
}
