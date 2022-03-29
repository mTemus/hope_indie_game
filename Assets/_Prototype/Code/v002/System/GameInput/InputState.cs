using System;

namespace _Prototype.Code.v002.System.GameInput
{
    public abstract class InputState
    {
        internal readonly global::GameInput gameInput;
        
        public Action<IInputState> onStateTransition;

        protected InputState(global::GameInput gameInput)
        {
            this.gameInput = gameInput;
        }
    }
}