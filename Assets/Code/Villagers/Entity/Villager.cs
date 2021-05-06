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
        
        public bool MoveTo(Vector3 position)
        {
            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, speed * Time.deltaTime);
            
            transform.position = villagerPosition;

            return villagerPosition == position;
        }
        
        public bool MoveTo(Vector3 position, float villagerSpeed)
        {
            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, villagerSpeed * Time.deltaTime);
            
            transform.position = villagerPosition;

            return villagerPosition == position;
        }

        public void SetStatistics(VillagersStatistics newStatistics)
        {
            statistics = newStatistics;
        }

        public void SetProfession(Profession newProfession)
        {
            profession = newProfession;
        }
        
        public VillagersStatistics Statistics => statistics;

        public Profession Profession => profession;

        public VillagerUi UI => ui;
    }
}
