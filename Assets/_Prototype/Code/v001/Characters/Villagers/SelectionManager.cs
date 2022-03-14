using System.Collections.Generic;
using _Prototype.Code.v001.Characters.Villagers.Entity;
using _Prototype.Code.v001.System;
using UnityEngine;

namespace _Prototype.Code.v001.Characters.Villagers
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {
        private readonly List<Villager> _villagersToSelect = new List<Villager>();

        private Villager _selectedVillager;
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="villager"></param>
        public void AddVillagerToSelect(Villager villager)
        {
            _villagersToSelect.Add(villager);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="villager"></param>
        public void RemoveVillagerToSelect(Villager villager)
        {
            if (_villagersToSelect.Contains(villager)) {
                _villagersToSelect.Remove(villager);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SelectVillager()
        {
            Vector3 playerPos = Managers.I.Player.GetPlayerPosition();
            float closestDistance = Vector3.Distance(_villagersToSelect[0].transform.position, playerPos);
        
            foreach (Villager villager in _villagersToSelect) {
                float distanceToPlayer = Vector3.Distance(villager.transform.position, playerPos);

                if (distanceToPlayer > closestDistance) continue;
                closestDistance = distanceToPlayer;
                _selectedVillager = villager;
            }
        
            Managers.I.GUI.VillagerPropertiesPanel.OpenPropertiesPanel(_selectedVillager);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void DeselectVillager()
        {
            _selectedVillager = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool AreVillagersNearby() =>
            _villagersToSelect.Count > 0;
        
        public Villager SelectedVillager => _selectedVillager;
    }
}
