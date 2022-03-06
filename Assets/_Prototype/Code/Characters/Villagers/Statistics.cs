using System;
using UnityEngine;

namespace _Prototype.Code.Characters.Villagers
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Statistics
    {
        [SerializeField] private int strength;
        [SerializeField] private int dexterity;
        [SerializeField] private int intelligence;

        public Statistics(int strength, int dexterity, int intelligence)
        {
            this.strength = strength;
            this.dexterity = dexterity;
            this.intelligence = intelligence;
        }

        public int Strength => strength;

        public int Dexterity => dexterity;

        public int Intelligence => intelligence;
    }
}
