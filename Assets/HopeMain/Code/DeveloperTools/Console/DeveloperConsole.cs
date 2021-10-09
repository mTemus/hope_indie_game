using System;
using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.DeveloperTools.Console.Command;
using TMPro;
using UnityEngine;

namespace HopeMain.Code.DeveloperTools.Console
{
    /// <summary>
    /// 
    /// </summary>
    public class DeveloperConsole : MonoBehaviour
    {
        [Header("UI Parts")] 
        [SerializeField] private GameObject console;
        [SerializeField] private TMP_InputField commandInputField;
        [SerializeField] private TextMeshProUGUI commandPlaceholderField;

        [Header("Colors")] 
        [SerializeField] private Color normalTextColor;
        [SerializeField] private Color noCommandColor;
        
        [Header("Commands")] 
        [SerializeField] private Data[] commands;

        [Header("Similar commands display")] 
        [SerializeField] private RectTransform similarCommandsBackground;
        [SerializeField] private TextMeshProUGUI similarCommandsText;
        [SerializeField] private float singleCommandHeight;
        
        private float _similarCommandsWidth;

        public static DeveloperConsole I { get; private set; }

        private void Awake()
        {
            I = this;
            commandInputField.onSelect.AddListener(ResetPlaceholderText);
            commandInputField.onValueChanged.AddListener(ShowSimilarCommands);
            
            _similarCommandsWidth = similarCommandsBackground.sizeDelta.x;
            similarCommandsBackground.sizeDelta = new Vector2(_similarCommandsWidth, 0f);
            similarCommandsText.text = string.Empty;
            console.SetActive(false);
        }

        private void ShowSimilarCommands(string currentCommand)
        {
            if (currentCommand.Equals(String.Empty) || currentCommand.Equals(" ")) {
                similarCommandsBackground.sizeDelta = new Vector2(_similarCommandsWidth, 0);
                similarCommandsText.text = string.Empty;
                return;
            }

            List<string> similarCommands = commands
                .Select(commandData => commandData.Prefix + commandData.Command + " " + commandData.ValueDescription)
                .Where(command => command.Contains(currentCommand))
                .ToList();

            if (similarCommands.Count == 0) {
                similarCommandsBackground.sizeDelta = new Vector2(_similarCommandsWidth, 0);
                similarCommandsText.text = string.Empty;
                return;
            }
            
            string allSimilarCommands = string.Join("\n", similarCommands);
            float commandsHeight = 5f + similarCommands.Count * singleCommandHeight;

            similarCommandsBackground.sizeDelta = new Vector2(_similarCommandsWidth, commandsHeight);
            similarCommandsText.text = allSimilarCommands;
        }
        
        private void ResetPlaceholderText(string s)
        {
            commandPlaceholderField.text = "Enter command: ";
            commandPlaceholderField.color = normalTextColor;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public void ToggleConsole()
        {
            if (IsConsoleActive()) {
                Time.timeScale = 1f;
                commandInputField.text = String.Empty;
                console.SetActive(false);
            }
            else {
                Time.timeScale = 0f;
                console.SetActive(true);
                commandInputField.Select();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void GetCommand()
        {
            string rawCommand = commandInputField.text;

            if (!rawCommand.StartsWith("/")) return;
            string[] command = rawCommand.Remove(0, 1).Split(' ');

            foreach (Data commandData in commands) {
                if (commandData.Command != command[0]) continue;
                if (commandData.Process(command.Skip(1).ToArray())) {
                        
                    //TODO: add command to history list
                    commandInputField.Select();
                    break;
                }

                if (commandPlaceholderField.text.Length == 0) 
                    ReturnWrongCommand("No such command!");
                
                break;
            }

            commandInputField.text = string.Empty;
            commandInputField.Select();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="answerText"></param>
        public void ReturnWrongCommand(string answerText)
        {
            commandInputField.text = string.Empty;
            commandPlaceholderField.text = answerText;
            commandPlaceholderField.color = noCommandColor;
            commandInputField.Select();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //TODO: refactor
        public bool IsConsoleActive() =>
            console.activeSelf;

    }
}
