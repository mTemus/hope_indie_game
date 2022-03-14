namespace _Prototype.Code.v001.GUI.UIElements.SelectableElement
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
