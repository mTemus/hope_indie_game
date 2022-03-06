using UnityEngine;
using UnityEngine.Events;

namespace _Prototype.Code.GUI.UIElements.SelectableElement
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UiSelectableElement : MonoBehaviour
    {
        [SerializeField] protected UnityEvent attachedEvent;
        
        /// <summary>
        /// 
        /// </summary>
        public abstract void OnElementSelected();
        
        /// <summary>
        /// 
        /// </summary>
        public abstract void OnElementDeselected();
        
        /// <summary>
        /// 
        /// </summary>
        public abstract void InvokeSelectedElement();
    }
}
