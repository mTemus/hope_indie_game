namespace HopeMain.Code.GUI.UIElements.SelectableElement
{
    public class SelectableUiImageElement : UiSelectableElement
    {
        public override void OnElementSelected()
        {
        }

        public override void OnElementDeselected()
        {
        }

        public override void InvokeSelectedElement()
        {
            attachedEvent.Invoke();
        }
    }
}
