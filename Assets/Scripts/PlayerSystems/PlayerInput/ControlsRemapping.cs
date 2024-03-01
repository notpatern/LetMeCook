using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using PlayerSystems.Input;
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
            if (File.Exists(Application.persistentDataPath + "/controlOverrides.qt"))
            {
                LoadControlOverrides();
            }
        }

        public static void RemapKeyboardMouseAction(InputAction actionToRebind, int targetBinding)
        {
            RemapAction(actionToRebind, targetBinding, new string[] {"Keyboard", "Gamepad", "Mouse"});
        }

        static void RemapAction(InputAction actionToRebind, int targetBinding, string[] groups)
        {
            actionToRebind.Disable();
            foreach(string group in groups )
            {
                var rebindOperation = actionToRebind.PerformInteractiveRebinding(targetBinding)
                    .WithControlsHavingToMatchPath($"<{group}>")
                    .WithBindingGroup(group)
                    .WithCancelingThrough("<Keyboard>/escape")
                    .OnCancel(operation => SuccessfulRebinding?.Invoke(null))
                    .OnComplete(operation => {
                        operation.Dispose();
                        AddOverrideToDictionary(actionToRebind.id, actionToRebind.bindings[targetBinding].effectivePath, targetBinding);
                        SaveControlOverrides();
                        SuccessfulRebinding?.Invoke(actionToRebind);
                    })
                    .Start();
            }
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
            FileStream file = new FileStream(Application.persistentDataPath + "/controlOverrides.qt", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, OverridesDictionary);
            file.Close();
        }

        private static void LoadControlOverrides()
        {
            if (!File.Exists(Application.persistentDataPath + "/controlOverrides.qt"))
            {
                return;
            }

            FileStream file = new FileStream(Application.persistentDataPath + "/controlOverrides.qt", FileMode.OpenOrCreate);
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