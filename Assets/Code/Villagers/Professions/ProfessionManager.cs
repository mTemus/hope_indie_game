using System;
using System.Collections.Generic;
using Code.Map.Building;
using Code.Villagers.Entity;
using Code.Villagers.Professions.Types;
using UnityEngine;

namespace Code.Villagers.Professions
{
    public class ProfessionManager : MonoBehaviour
    {
        private readonly List<Villager> unemployed = new List<Villager>();
        private readonly List<Villager> builders = new List<Villager>();
        private readonly List<Villager> lumberjacks = new List<Villager>();

        private void FireVillagerFromOldProfession(Villager villager)
        {
            switch (villager.Profession.Type) {
                case ProfessionType.Unemployed:
                    unemployed.Remove(villager);
                    break;
                
                case ProfessionType.Builder:
                    builders.Remove(villager);
                    break;
                
                case ProfessionType.Lumberjack:
                    lumberjacks.Remove(villager);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            DestroyImmediate(villager.GetComponent(typeof(Profession)));
        }
        
        private void MakeVillagerUnemployed(Villager villager)
        {
            villager.UpdateProfession(villager.gameObject.AddComponent<VillagerUnemployed>(), ProfessionType.Unemployed);
            unemployed.Add(villager);
        }
        
        private void HireBuilder(Villager villager)
        {
            villager.UpdateProfession(villager.gameObject.AddComponent<VillagerBuilder>(), ProfessionType.Builder);
            builders.Add(villager);
        }

        private void HireLumberjack(Villager villager)
        {
            villager.UpdateProfession(villager.gameObject.AddComponent<VillagerLumberjack>(), ProfessionType.Lumberjack);
            lumberjacks.Add(villager);
        }
        
        public void SetVillagerProfession(Villager villager, ProfessionType professionType, Workplace workplace)
        {
            FireVillagerFromOldProfession(villager);

            switch (professionType) {
                case ProfessionType.Unemployed:
                    MakeVillagerUnemployed(villager);
                    break;
                
                case ProfessionType.Builder:
                    HireBuilder(villager);
                    break;
                
                case ProfessionType.Lumberjack:
                    HireLumberjack(villager);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(professionType), professionType, null);
            }

            villager.Profession.enabled = false;
            villager.UI.ProfessionName.text = professionType.ToString();
            villager.Profession.SetWorkplace(workplace);
            villager.Profession.InitializeWorkerAI();
            
            if (professionType != ProfessionType.Unemployed) 
                workplace.HireWorker(villager.Profession);
        }
    }
}
