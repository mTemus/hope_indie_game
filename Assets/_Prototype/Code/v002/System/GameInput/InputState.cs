namespace _Prototype.Code.v002.System.GameInput
{
    public abstract class InputState
    {
        internal readonly global::GameInput gameInput;

        protected InputState(global::GameInput gameInput)
        {
            this.gameInput = gameInput;
        }
    }
}