namespace HopeMain.Code.GUI.UIElements.SelectableElement
{
    public class UiAcceptancePanel : UiSelectablePanel
    {
        private void Awake()
        {
            currentElement = elementsToSelect[0];
            currentElement.OnElementSelected();
        }
    }
}
