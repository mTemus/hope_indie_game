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
            switch (villager.Profession.Data.Type) {
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
            villager.SetProfession(villager.gameObject.AddComponent<VillagerWorkplaceHauler>());
            localHaulers.Add(villager);
        }

        private void HireGlobalHauler(Villager villager)
        {
            //TODO: add component
            globalHaulers.Add(villager);
        }
        
        public void FireVillagerFromOldProfession(Villager villager)
        {
            RemoveVillagerFromProfessionStructure(villager);
            villager.Profession.AbandonAllTasks();
            villager.Profession.Workplace.FireWorker(villager);
            DestroyImmediate(villager.Profession);
            villager.UI.ProfessionName.text = "No Profession Exception";
        }
        
        public void SetVillagerProfession(Villager villager, ProfessionData professionData, Workplace workplace)
        {
            switch (professionData.Type) {
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
                    throw new ArgumentOutOfRangeException(nameof(professionData.Type), professionData.Type, null);
            }
            
            villager.Profession.SetProfessionData(professionData);
            villager.Profession.Initialize();
            workplace.HireWorker(villager);
            villager.Profession.enabled = false;
            
            if (professionData.Type == ProfessionType.WorkplaceHauler) 
                villager.UI.ProfessionName.text = professionData.Type + " of " + villager.Profession.Workplace.name;
            else 
                villager.UI.ProfessionName.text = professionData.Type.ToString();
        }
    }
}
