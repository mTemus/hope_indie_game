using System;
using System.Linq;
using _Prototype.Code.Characters.Villagers.Professions;
using _Prototype.Code.System;
using _Prototype.Code.System.Assets;
using _Prototype.Code.System.Initialization;
using _Prototype.Code.World.Buildings.Workplaces;
using UnityEngine;

namespace _Prototype.Code.DeveloperTools.Console.Command
{
    [CreateAssetMenu(fileName = "Spawn Villager Command", menuName = "Game Data/System/Console Commands/SpawnVillager Command", order = 1)]

    public class SpawnVillager : Data
    {
        public override bool Process(string[] args)
        {
            string professionTypeRawString = args[0].First().ToString().ToUpper() +
                                          args[0].Substring(1);
            
            if (!Enum.TryParse(professionTypeRawString, out ProfessionType professionType)) {
                DeveloperConsole.I.ReturnWrongCommand("Wrong command profession type value!");
                return false;
            }

            Vector3 villagerPosition = new Vector3(Managers.I.Player.GetPlayerPosition().x, 0, 0);
            Workplace workplace =
                Managers.I.Buildings.GetClosestFreeWorkplaceForProfession(
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
