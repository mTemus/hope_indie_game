using Code.System;
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
                Managers.Instance.VillagerSelection.AddVillagerToSelect(other.gameObject.GetComponent<Villager>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                Managers.Instance.VillagerSelection.RemoveVillagerToSelect(other.GetComponent<Villager>());
            }
        }

        public Villager VillagerToInteract => villagerToInteract;
    }
}
