namespace Code.GUI.UIElements.SelectableElement
{
    public class SelectableTextUiElement : UiSelectableElement
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
