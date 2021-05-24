using System;
using System.Collections.Generic;
using Code.Map.Building.Workplaces;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Professions.Types;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
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
            villager.Profession = villager.gameObject.AddComponent<VillagerUnemployed>();
            unemployed.Add(villager);
        }
        
        private void HireBuilder(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<VillagerBuilder>();
            builders.Add(villager);
        }

        private void HireLumberjack(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<VillagerLumberjack>();
            lumberjacks.Add(villager);
        }

        private void HireLocalHauler(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<VillagerWorkplaceHauler>();
            localHaulers.Add(villager);
        }

        private void HireGlobalHauler(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<VillagerGlobalHauler>();
            globalHaulers.Add(villager);
        }

        private void AddBehaviourTreeAIComponents(AIType aiType, GameObject destinationObject)
        {
            //TODO: unemployed AI not implemented yet
            if (aiType == AIType.villager_unemployed) {
                return;
            }
            
            BehaviourTree bt = AssetsStorage.I.GetBehaviourTreeForAIType(aiType);
            BehaviourTreeOwner bto = destinationObject.AddComponent<BehaviourTreeOwner>();
            bto.firstActivation = GraphOwner.FirstActivation.OnEnable;
            bto.enableAction = GraphOwner.EnableAction.DoNothing;
            bto.disableAction = GraphOwner.DisableAction.DisableBehaviour;
            bto.updateMode = Graph.UpdateMode.Manual;
         
            Blackboard blackboard = destinationObject.AddComponent<Blackboard>();
            Dictionary<string, Variable> variablesToCopy = AssetsStorage.I.GetBlackboardForAIType(aiType).GetRoot().variables;

            foreach (string variableName in variablesToCopy.Keys) 
                variablesToCopy[variableName].Duplicate(blackboard);

            bto.blackboard = blackboard;
            bto.graph = bt;
         
            blackboard.InitializePropertiesBinding(bto.blackboard.propertiesBindTarget, false);
        }
        
        public void FireVillagerFromOldProfession(Villager villager)
        {
            RemoveVillagerFromProfessionStructure(villager);
            villager.Profession.AbandonAllTasks();
            villager.Profession.Workplace.FireWorker(villager);
            villager.Profession.BTO.StopBehaviour();
            DestroyImmediate(villager.Profession);
            
            //TODO: clear BTO instead of destroying and adding -> add one clear bto to villager prefab, but clear whole blackboard
            
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
            
            villager.Profession.Data = professionData;
            workplace.HireWorker(villager);
            AddBehaviourTreeAIComponents(
                villager.Profession.Data.Type == ProfessionType.Unemployed
                    ? AIType.villager_unemployed
                    : AIType.villager_worker, villager.gameObject);
            
            villager.Profession.enabled = false;
            villager.Profession.Initialize();
            villager.UI.ProfessionName.text = professionData.Type + " of " + villager.Profession.Workplace.name;

            if (villager.Profession.Data.Type != ProfessionType.Unemployed) 
                villager.Profession.BTO.StartBehaviour();
        }
    }
}
