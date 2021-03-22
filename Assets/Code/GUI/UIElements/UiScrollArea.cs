using System;
using UnityEngine;

namespace Code.GUI.UIElements
{
    public abstract class UiScrollArea : MonoBehaviour
    {
        [SerializeField] protected Transform content;

        protected float elementValue;
        protected float maxValue;
        protected float minValue;
        
        public abstract void ChangeValue(int value);

        public abstract void ResetArea();
    }
}
