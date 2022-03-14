namespace _Prototype.Code.v001.System.GameInput
{
    public interface IInputState
    {
        public void OnStateSet();
        public void HandleState(InputManager inputManager);
        public void OnStateChange();
    }
}