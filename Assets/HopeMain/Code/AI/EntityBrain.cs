using System;
using HopeMain.Code.World.Areas;
using UnityEngine;

namespace HopeMain.Code.AI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityBrain : MonoBehaviour
    {
        public Area CurrentArea { get; set; }

        public Action<AudioClip> walkingSoundSet;
    }
}
