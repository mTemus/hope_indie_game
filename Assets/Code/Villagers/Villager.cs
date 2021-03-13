using Code.Villagers.Professions;
using UnityEngine;

namespace Code.Villagers
{
    public class Villager : MonoBehaviour
    {
        [SerializeField] private int healthPoints;
        [SerializeField] private float speed = 5f;
        
        [SerializeField] private Profession profession;
        
        public void MoveTo(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position , position, speed * Time.deltaTime);
        }
        
        public void MoveTo(Vector3 position, float villagerSpeed)
        {
            transform.position = Vector3.MoveTowards(transform.position , position, villagerSpeed * Time.deltaTime);
        }

        public bool IsOnPosition(Vector3 position) =>
            transform.position == position;

    }
}
