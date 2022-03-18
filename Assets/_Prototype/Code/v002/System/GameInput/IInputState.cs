namespace _Prototype.Code.v002.System.GameInput
{
    /// <summary>
    /// Interface for implementing object that will be handling states of user input
    /// </summary>
    public interface IInputState
    {
        /// <summary>
        /// Event that should be called while state is set, similar to Unity's 'Start' method
        /// </summary>
        public void OnStateSet();
        
        /// <summary>
        /// Event that should be called in 'Update' method
        /// </summary>
        /// <param name="inputManager">Reference to current InputManager</param>
        public void HandleState(InputManager inputManager);
        
        /// <summary>
        /// Event that should be called before new state will be set
        /// </summary>
        public void OnStateChange();
    }
}