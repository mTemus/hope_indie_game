using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player components")] 
        [SerializeField] private PlayerMovement movement;
        
        private Villager villagerToInteract;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                villagerToInteract = other.gameObject.GetComponent<Villager>();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                villagerToInteract = null;
            }
        }

        public Villager VillagerToInteract => villagerToInteract;
    }
}
