namespace HopeMain.Code.System.GameInput
{
    public interface IInputState
    {
        public void OnStateSet();
        public void HandleState(InputManager inputManager);
        public void OnStateChange();
    }
}