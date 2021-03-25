using UnityEngine;

namespace Code.GUI.UIElements
{
    public abstract class UiSelectableElement : MonoBehaviour
    {
        public abstract void OnElementSelected();
        public abstract void OnElementDeselected();
        public abstract void InvokeSelectedElement();
    }
}
