using HopeMain.Code.DeveloperTools.Console;
using UnityEngine;

namespace HopeMain.Code.System.GameInput.States
{
    public class DeveloperConsoleInputState : IInputState
    {
        public void OnStateSet()
        {
            
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Console)) 
                DeveloperConsole.I.ToggleConsole();
            
            if (!DeveloperConsole.I.IsConsoleActive()) return;

            if (Input.GetKeyDown(inputManager.Accept)) 
                DeveloperConsole.I.GetCommand();
        }

        public void OnStateChange()
        {
            
        }
    }
}
