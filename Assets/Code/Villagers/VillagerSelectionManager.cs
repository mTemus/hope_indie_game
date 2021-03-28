using System.Collections.Generic;
using Code.System;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Villagers
{
    public class VillagerSelectionManager : MonoBehaviour
    {
        private readonly List<Villager> villagersToSelect = new List<Villager>();

        private Villager selectedVillager;
    
        public void AddVillagerToSelect(Villager villager)
        {
            villagersToSelect.Add(villager);
        }

        public void RemoveVillagerToSelect(Villager villager)
        {
            if (villagersToSelect.Contains(villager)) {
                villagersToSelect.Remove(villager);
            }
        }

        public void SelectVillager()
        {
            if (villagersToSelect.Count <= 0) return;
            Vector3 playerPos = Managers.Instance.Player.GetPlayerPosition();
            float closestDistance = Vector3.Distance(villagersToSelect[0].transform.position, playerPos);
        
            foreach (Villager villager in villagersToSelect) {
                float distanceToPlayer = Vector3.Distance(villager.transform.position, playerPos);

                if (distanceToPlayer > closestDistance) continue;
                closestDistance = distanceToPlayer;
                selectedVillager = villager;
            }
        
            Managers.Instance.GUI.VillagerPropertiesPanel.OpenPropertiesPanel(selectedVillager);
        }

        public void DeselectVillager()
        {
            selectedVillager = null;
        }

        public Villager SelectedVillager => selectedVillager;
    }
}
