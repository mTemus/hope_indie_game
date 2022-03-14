using System;
using _Prototype.Code.v001.System;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;

namespace _Prototype.Code.v001.DeveloperTools.Console.Command
{
    [CreateAssetMenu(fileName = "Add Resources Command", menuName = "Game Data/System/Console Commands/AddResources Command", order = 0)]

    public class AddResources : Data
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
            
            Managers.I.Resources.StoreResource(resourceType, resourceAmount);
            return true;
        }   
    }
}
