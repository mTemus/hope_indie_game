using System;
using _Prototype.Code.System;
using _Prototype.Code.System.Assets;
using _Prototype.Code.World.Resources;
using UnityEngine;

namespace _Prototype.Code.DeveloperTools.Console.Command
{
    [CreateAssetMenu(fileName = "Spawn Resources On Ground Command", menuName = "Game Data/System/Console Commands/SpawnResourcesOnGround Command", order = 0)]
    public class ThrowResourceOnGround : Data
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
