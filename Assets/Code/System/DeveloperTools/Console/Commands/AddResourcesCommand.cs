using System;
using Code.Resources;
using UnityEngine;

namespace Code.System.DeveloperTools.Console.Commands
{
    [CreateAssetMenu(fileName = "Add Resources Command", menuName = "Game Data/System/Console Commands/AddResources Command", order = 0)]

    public class AddResourcesCommand : ConsoleCommandData
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
            
            Managers.Instance.Resources.StoreResource(resourceType, resourceAmount);
            return true;
        }   
    }
}
