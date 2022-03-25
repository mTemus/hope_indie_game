using System;
using _Prototype.Code.v002.System.GameInput;

namespace _Prototype.Code.v002.Player.Tools
{
    public enum PlayerTool
    {
        Hand = 0,
        Hammer = 1,
        VillagersBook = 2,
        BuildingsBook = 3,
        CoinPocket = 4
    }

    public abstract class Tool
    {
        public event Action<IInputState> onInputChange; 

        public abstract void UseTool();
    }
}
