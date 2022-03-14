using UnityEngine;

namespace _Prototype.Code.v001.DeveloperTools.Console.Command
{
    public abstract class Data : ScriptableObject
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
