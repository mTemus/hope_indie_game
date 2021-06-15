using System;
using System.Collections.Generic;
using Code.Map.Building.Workplaces;
using Code.System.Assets;
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
            villager.Profession = villager.gameObject.AddComponent<Villager_Profession_Unemployed>();
            unemployed.Add(villager);
        }
        
        private void HireBuilder(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<Villager_Profession_Builder>();
            builders.Add(villager);
        }

        private void HireLumberjack(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<Villager_Profession_Lumberjack>();
            lumberjacks.Add(villager);
        }

        private void HireLocalHauler(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<Villager_Profession_WorkplaceHauler>();
            localHaulers.Add(villager);
        }

        private void HireGlobalHauler(Villager villager)
        {
            villager.Profession = villager.gameObject.AddComponent<Villager_Profession_GlobalHauler>();
            globalHaulers.Add(villager);
        }

        private void AddBehaviourTreeAIComponents(ProfessionAIType aiType, Villager villager)
        {
            BehaviourTree bt = AssetsStorage.I.GetBehaviourTreeForAIType(aiType);
            BehaviourTreeOwner bto = villager.GetComponent<BehaviourTreeOwner>();
            Blackboard blackboard = villager.GetComponent<Blackboard>();
            Dictionary<string, Variable> variablesToCopy = AssetsStorage.I.GetBlackboardForAIType(aiType).GetRoot().variables;
            
            bto.firstActivation = GraphOwner.FirstActivation.OnEnable;
            bto.enableAction = GraphOwner.EnableAction.DoNothing;
            bto.disableAction = GraphOwner.DisableAction.DisableBehaviour;
            bto.updateMode = Graph.UpdateMode.Manual;
            
            foreach (string variableName in variablesToCopy.Keys) 
                variablesToCopy[variableName].Duplicate(blackboard);

            blackboard.SetVariableValue("myWorkplace", villager.Profession.Workplace);
            blackboard.SetVariableValue("workplacePos", villager.Profession.Workplace.PivotedPosition);
            
            bto.blackboard = blackboard;
            bto.graph = bt;
         
            blackboard.InitializePropertiesBinding(bto.blackboard.propertiesBindTarget, false);
        }

        private void  AddProfessionComponent(Villager villager, ProfessionType type)
        {
            switch (type) {
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
                    throw new Exception("PROFESSION MANAGER --- CAN'T HIRE VILLAGER FOR PROFESSION: " + type);
            }
        }
        
        public void FireVillagerFromOldProfession(Villager villager)
        {
            RemoveVillagerFromProfessionStructure(villager);
            villager.Brain.Work.AbandonAllTasks();
            villager.Brain.ClearBehaviourAIComponents();
            villager.Profession.Workplace.FireWorker(villager);
            
            DestroyImmediate(villager.Profession);
            villager.UI.ProfessionName.text = "No Profession Exception";
        }
        
        public void SetVillagerProfession(Villager villager, Villager_ProfessionData villagerProfessionData, Workplace workplace)
        {
            AddProfessionComponent(villager, villagerProfessionData.Type);
            villager.Profession.Data = villagerProfessionData;
            workplace.HireWorker(villager);
            
            AddBehaviourTreeAIComponents(villagerProfessionData.AIType, villager);
            
            villager.Profession.Initialize();
            villager.UI.ProfessionName.text = villagerProfessionData.Type + " of " + villager.Profession.Workplace.name;

            villager.Brain.Behaviour.BehaviourTree.StartBehaviour();
        }
    }
}
