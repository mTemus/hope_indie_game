using UnityEngine;

namespace Code.System.DeveloperTools.Console
{
    public abstract class ConsoleCommandData : ScriptableObject
    {
        [SerializeField] private string prefix;
        [SerializeField] private string command;
        [SerializeField] private string valueDescription;

        public abstract bool Process(string[] args);

        public string Prefix => prefix;

        public string Command => command;

        public string ValueDescription => valueDescription;
    }
}
