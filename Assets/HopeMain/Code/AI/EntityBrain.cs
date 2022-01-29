using System;
using HopeMain.Code.World.Areas;
using UnityEngine;

namespace HopeMain.Code.AI
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
