using System;
using UnityEngine;

namespace Code.Villagers
{
    [Serializable]
    public class VillagersStatistics
    {
        [SerializeField] private int strength;
        [SerializeField] private int dexterity;
        [SerializeField] private int intelligence;

        public VillagersStatistics(int strength, int dexterity, int intelligence)
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
