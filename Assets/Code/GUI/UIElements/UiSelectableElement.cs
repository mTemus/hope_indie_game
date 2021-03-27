using UnityEngine;
using UnityEngine.Events;

namespace Code.GUI.UIElements
{
    public abstract class UiSelectableElement : MonoBehaviour
    {
        [SerializeField] protected UnityEvent attachedEvent;
        
        public abstract void OnElementSelected();
        public abstract void OnElementDeselected();
        public abstract void InvokeSelectedElement();
    }
}
