using HopeMain.Code.Utilities;
using UnityEngine;

namespace HopeMain.Code.GUI.UIElements.SelectableElement
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UiSelectablePanel : MonoBehaviour
    {
        [SerializeField] protected UISelectingPointer pointer;
        [SerializeField] protected UiSelectableElement[] elementsToSelect;
   
        protected int selectionIdx;
        protected UiSelectableElement currentElement;
        
        /// <summary>
        /// 
        /// </summary>
        public void UseSelectedElement()
        {
            currentElement.InvokeSelectedElement();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void MovePointer(int value)
        {
            selectionIdx = GlobalUtilities.IncrementIdx(selectionIdx, value, elementsToSelect.Length);
            currentElement.OnElementDeselected();
            currentElement = elementsToSelect[selectionIdx];
            pointer.SetPointerOnUiElement(currentElement.transform);
            currentElement.OnElementSelected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void MovePointerWithParent(int value)
        {
            selectionIdx = GlobalUtilities.IncrementIdx(selectionIdx, value, elementsToSelect.Length);
            currentElement.OnElementDeselected();
            currentElement = elementsToSelect[selectionIdx];
            pointer.SetPointerOnUiElementWithParent(currentElement.transform);
            currentElement.OnElementSelected();
        }

        protected void MovePointerWithParent()
        {
            pointer.SetPointerOnUiElementWithParent(currentElement.transform);
            currentElement.OnElementSelected();
        }
        
        protected void GetNextElement(int value)
        {
            selectionIdx = GlobalUtilities.IncrementIdx(selectionIdx, value, elementsToSelect.Length);
            currentElement.OnElementDeselected();
            currentElement = elementsToSelect[selectionIdx];
        }
    }
}
