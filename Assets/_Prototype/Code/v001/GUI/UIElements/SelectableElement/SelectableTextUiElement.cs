namespace _Prototype.Code.v001.GUI.UIElements.SelectableElement
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
