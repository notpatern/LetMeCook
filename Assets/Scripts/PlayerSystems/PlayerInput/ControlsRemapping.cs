using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
    public static class ControlsRemapping
    {
        public static Action<InputAction> SuccessfulRebinding;

        public static Dictionary<string, string> OverridesDictionary = new Dictionary<string, string>();

        public static void LoadMap()
        {
            if (File.Exists(Application.persistentDataPath + "/controlsOverrides.qt"))
            {
                LoadControlOverrides();
            }
        }

        public static void RemapKeyboardAction(InputAction actionToRebind, int targetBinding)
        {
            actionToRebind.Disable();
            var rebindOperation = actionToRebind.PerformInteractiveRebinding(targetBinding)
                .WithControlsHavingToMatchPath("<Keyboard>")
                .WithBindingGroup("Keyboard")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(operation => SuccessfulRebinding?.Invoke(null))
                .OnComplete(operation => {
                    operation.Dispose();
                    AddOverrideToDictionary(actionToRebind.id, actionToRebind.bindings[targetBinding].effectivePath, targetBinding);
                    SaveControlOverrides();
                    SuccessfulRebinding?.Invoke(actionToRebind);
                })
                .Start();
            
            
            actionToRebind.Enable();
        }

        public static void RemapGamepadAction(InputAction actionToRebind, int targetBinding)
        {
            actionToRebind.Disable();
            var rebindOperation = actionToRebind.PerformInteractiveRebinding(targetBinding)
                .WithControlsHavingToMatchPath("<Gamepad>")
                .WithBindingGroup("Gamepad")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(operation => SuccessfulRebinding?.Invoke(null))
                .OnComplete(operation => {
                    operation.Dispose();
                    AddOverrideToDictionary(actionToRebind.id, actionToRebind.bindings[targetBinding].effectivePath, targetBinding);
                    SaveControlOverrides();
                    SuccessfulRebinding?.Invoke(actionToRebind);
                })
                .Start();

            actionToRebind.Enable();
        }

        private static void AddOverrideToDictionary(Guid actionId, string path, int bindingIndex)
        {
            string key = string.Format("{0} : {1}", actionId.ToString(), bindingIndex);

            if (OverridesDictionary.ContainsKey(key))
            {
                OverridesDictionary[key] = path;
            }
            else
            {
                OverridesDictionary.Add(key, path);
            }
        }

        private static void SaveControlOverrides()
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/controlsOverrides.qt", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, OverridesDictionary);
            file.Close();
        }

        private static void LoadControlOverrides()
        {
            if (!File.Exists(Application.persistentDataPath + "/controlsOverrides.qt"))
            {
                return;
            }

            FileStream file = new FileStream(Application.persistentDataPath + "/controlsOverrides.qt", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            OverridesDictionary = bf.Deserialize(file) as Dictionary<string, string>;
            file.Close();

            foreach (var item in OverridesDictionary)
            {
                string[] split = item.Key.Split(new string[] { " : " }, StringSplitOptions.None);
                Guid id = Guid.Parse(split[0]);
                int index = int.Parse(split[1]);
                InputManager.s_PlayerInput.asset.FindAction(id).ApplyBindingOverride(index, item.Value);
            }
        }
    }
}