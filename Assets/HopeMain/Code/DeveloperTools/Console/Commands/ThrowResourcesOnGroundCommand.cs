using System;
using HopeMain.Code.System;
using HopeMain.Code.System.Assets;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.DeveloperTools.Console.Commands
{
    [CreateAssetMenu(fileName = "Spawn Resources On Ground Command", menuName = "Game Data/System/Console Commands/SpawnResourcesOnGround Command", order = 0)]
    public class ThrowResourcesOnGroundCommand : ConsoleCommandData
    {
        public override bool Process(string[] args)
        {
            string resourceTypeString = args[0];
            string resourceAmountString = args[1];

            if (!Enum.TryParse(resourceTypeString.ToUpper(), out ResourceType resourceType)) {
                DeveloperConsole.I.ReturnWrongCommand("Wrong command resource type value!");
                return false;
            }

            if (!int.TryParse(resourceAmountString, out int resourceAmount)) {
                DeveloperConsole.I.ReturnWrongCommand("Wrong command resource amount value!");
                return false;
            }
            
            AssetsStorage.I.ThrowResourceOnTheGround(new Resource(resourceType, resourceAmount), Managers.I.Player.GetPlayerPosition().x);
            return true;
        }
    }
}
