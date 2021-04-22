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
        private readonly List<Villager> localHaulers = new List<Villager>();
        private readonly List<Villager> globalHaulers = new List<Villager>();


        private void RemoveVillagerFromProfessionStructure(Villager villager)
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

                case ProfessionType.WorkplaceHauler:
                    localHaulers.Remove(villager);
                    break;
                
                case ProfessionType.GlobalHauler:
                    globalHaulers.Remove(villager);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void FireVillagerFromOldProfession(Villager villager)
        {
            RemoveVillagerFromProfessionStructure(villager);
            villager.Profession.AbandonAllTasks();
            villager.Profession.Workplace.FireWorker(villager.Profession);
            DestroyImmediate(villager.Profession);
            villager.UI.ProfessionName.text = "No Profession Exception";
        }
        
        private void MakeVillagerUnemployed(Villager villager)
        {
            villager.SetProfession(villager.gameObject.AddComponent<VillagerUnemployed>());
            unemployed.Add(villager);
        }
        
        private void HireBuilder(Villager villager)
        {
            villager.SetProfession(villager.gameObject.AddComponent<VillagerBuilder>());
            builders.Add(villager);
        }

        private void HireLumberjack(Villager villager)
        {
            villager.SetProfession(villager.gameObject.AddComponent<VillagerLumberjack>());
            lumberjacks.Add(villager);
        }

        private void HireLocalHauler(Villager villager)
        {
            //TODO: add component
            localHaulers.Add(villager);
        }

        private void HireGlobalHauler(Villager villager)
        {
            //TODO: add component
            globalHaulers.Add(villager);
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

                case ProfessionType.WorkplaceHauler:
                    HireLocalHauler(villager);
                    break;
                
                case ProfessionType.GlobalHauler:
                    HireGlobalHauler(villager);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(professionType), professionType, null);
            }

            villager.Profession.Initialize();
            workplace.HireWorker(villager.Profession);
            villager.Profession.enabled = false;
            villager.UI.ProfessionName.text = professionType.ToString();

            if (professionType == ProfessionType.WorkplaceHauler) {
                villager.UI.ProfessionName.text += " of " + villager.Profession.Workplace.name;
            }
        }
    }
}
