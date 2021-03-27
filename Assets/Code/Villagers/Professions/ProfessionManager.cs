using System;
using System.Collections.Generic;
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
            villager.gameObject.AddComponent<VillagerUnemployed>();
            unemployed.Add(villager);
        }
        
        private void HireBuilder(Villager villager)
        {
            villager.gameObject.AddComponent<VillagerBuilder>();
            builders.Add(villager);
        }

        private void HireLumberjack(Villager villager)
        {
            villager.gameObject.AddComponent<VillagerLumberjack>();
            lumberjacks.Add(villager);
        }
        
        public void SetVillagerProfession(Villager villager, ProfessionType professionType)
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
        }
    }
}
