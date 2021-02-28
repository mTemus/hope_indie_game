using Code.Player;

namespace Code.System.PlayerInput
{
    public interface IInputState
    {
        public void OnStateSet();
        public void HandleState(InputManager inputManager);
        public void OnStateChange();
    }
}