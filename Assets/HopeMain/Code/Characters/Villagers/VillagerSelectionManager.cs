using System.Collections.Generic;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.System;
using UnityEngine;

namespace HopeMain.Code.Characters.Villagers
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
            Vector3 playerPos = Managers.I.Player.GetPlayerPosition();
            float closestDistance = Vector3.Distance(villagersToSelect[0].transform.position, playerPos);
        
            foreach (Villager villager in villagersToSelect) {
                float distanceToPlayer = Vector3.Distance(villager.transform.position, playerPos);

                if (distanceToPlayer > closestDistance) continue;
                closestDistance = distanceToPlayer;
                selectedVillager = villager;
            }
        
            Managers.I.GUI.VillagerPropertiesPanel.OpenPropertiesPanel(selectedVillager);
        }
        
        public void DeselectVillager()
        {
            selectedVillager = null;
        }

        public bool AreVillagersNearby() =>
            villagersToSelect.Count > 0;
        
        public Villager SelectedVillager => selectedVillager;
    }
}
