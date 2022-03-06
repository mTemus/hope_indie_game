namespace _Prototype.Code.GUI.UIElements.SelectableElement
{
    /// <summary>
    /// 
    /// </summary>
    public class UiAcceptancePanel : UiSelectablePanel
    {
        private void Awake()
        {
            currentElement = elementsToSelect[0];
            currentElement.OnElementSelected();
        }
    }
}
