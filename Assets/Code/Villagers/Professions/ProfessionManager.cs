using System;
using System.Collections.Generic;
using System.Linq;
using Code.Villagers.Professions.Types;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Villagers.Professions
{
    public class ProfessionManager : MonoBehaviour
    {
        private List<VillagerBuilder> builders = new List<VillagerBuilder>();
       
        public void HireBuilder(GameObject villager)
        {
            VillagerBuilder builder = villager.AddComponent<VillagerBuilder>();
            
        }
    }
}
