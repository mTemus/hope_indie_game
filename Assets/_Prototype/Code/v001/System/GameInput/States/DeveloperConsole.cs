using UnityEngine;

namespace _Prototype.Code.v001.System.GameInput.States
{
    public class DeveloperConsole : IInputState
    {
        public void OnStateSet()
        {
            
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Console)) 
                DeveloperTools.Console.DeveloperConsole.I.ToggleConsole();
            
            if (!DeveloperTools.Console.DeveloperConsole.I.IsConsoleActive()) return;

            if (Input.GetKeyDown(inputManager.Accept)) 
                DeveloperTools.Console.DeveloperConsole.I.GetCommand();
        }

        public void OnStateChange()
        {
            
        }
    }
}
