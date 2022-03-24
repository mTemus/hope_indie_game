using System;
using _Prototype.Code.v002.System.GameInput;

namespace _Prototype.Code.v002.Player.Tools
{
    public abstract class Tool
    {
        public event Action<IInputState> onInputChange; 

        public abstract void UseTool();
    }
}
