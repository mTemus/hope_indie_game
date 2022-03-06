namespace _Prototype.Code.GUI.UIElements.SelectableElement
{
    /// <summary>
    /// 
    /// </summary>
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
