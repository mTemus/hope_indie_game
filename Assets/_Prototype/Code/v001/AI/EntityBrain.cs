using System;
using _Prototype.Code.v001.World.Areas;
using UnityEngine;

namespace _Prototype.Code.v001.AI
{
    /// <summary>
    /// Unified parent class that is inherited by main brains of entities, contains data that is processed by every
    /// entity that has any AI 
    /// </summary>
    public abstract class EntityBrain : MonoBehaviour
    {
        public Area CurrentArea { get; set; }

        public Action<AudioClip> walkingSoundSet;
    }
}
