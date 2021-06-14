using Code.System;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player components")] 
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerAnimations animations;

        public PlayerMovement Movement => movement;
        public PlayerAnimations Animations => animations;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                Managers.I.VillagerSelection.AddVillagerToSelect(other.gameObject.GetComponent<Villager>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                Managers.I.VillagerSelection.RemoveVillagerToSelect(other.GetComponent<Villager>());
            }
        }
    }
}
