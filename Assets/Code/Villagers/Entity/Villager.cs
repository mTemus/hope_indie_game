using Code.Villagers.Professions;
using UnityEngine;

namespace Code.Villagers.Entity
{
    public class Villager : MonoBehaviour
    {
        [Header("Villager properties")]
        [SerializeField] private int healthPoints;
        [SerializeField] private float speed = 5f;
        [SerializeField] private VillagersStatistics statistics;

        [Header("Villager components")]
        [SerializeField] private Profession profession;
        [SerializeField] private VillagerUi ui;
        
        public void MoveTo(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position , position, speed * Time.deltaTime);
        }
        
        public void MoveTo(Vector3 position, float villagerSpeed)
        {
            transform.position = Vector3.MoveTowards(transform.position , position, villagerSpeed * Time.deltaTime);
        }

        public void SetStatistics(VillagersStatistics newStatistics)
        {
            statistics = newStatistics;
        }
        
        public bool IsOnPosition(Vector3 position) =>
            transform.position == position;

        public VillagersStatistics Statistics => statistics;

        public Profession Profession => profession;

        public VillagerUi UI => ui;
    }
}
