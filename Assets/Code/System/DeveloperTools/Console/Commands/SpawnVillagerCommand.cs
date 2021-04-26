using System;
using System.Linq;
using Code.Map.Building;
using Code.System.Initialization;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.System.DeveloperTools.Console.Commands
{
    [CreateAssetMenu(fileName = "Spawn Villager Command", menuName = "Game Data/System/Console Commands/SpawnVillager Command", order = 1)]

    public class SpawnVillagerCommand : ConsoleCommandData
    {
        public override bool Process(string[] args)
        {
            string professionTypeRawString = args[0].First().ToString().ToUpper() +
                                          args[0].Substring(1);
            
            if (!Enum.TryParse(professionTypeRawString, out ProfessionType professionType)) {
                DeveloperConsole.I.ReturnWrongCommand("Wrong command profession type value!");
                return false;
            }

            Vector3 villagerPosition = new Vector3(Managers.Instance.Player.GetPlayerPosition().x, 0, 0);
            Workplace workplace =
                Managers.Instance.Buildings.GetClosestFreeWorkplaceForProfession(
                    AssetsStorage.I.GetProfessionDataForProfessionType(professionType), villagerPosition);

            if (workplace == null) {
                DeveloperConsole.I.ReturnWrongCommand("No free workplace for that type of villager!");
                return false;
            }

            GameObject villagerGO = Instantiate(
                AssetsStorage.I.GetVillagerPrefab(professionType), 
                villagerPosition, 
                Quaternion.identity);

            InitializeVillager iv = villagerGO.GetComponent<InitializeVillager>();
            iv.Workplace = workplace;
            iv.InitializeMe();
            return true;
        }
    }
}
