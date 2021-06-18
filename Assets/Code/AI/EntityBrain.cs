using System;
using Code.System.Areas;
using UnityEngine;

namespace Code.AI
{
    public abstract class EntityBrain : MonoBehaviour
    {
        public Area CurrentArea { get; set; }

        public Action<AudioClip> onWalkingSoundSet;
    }
}
