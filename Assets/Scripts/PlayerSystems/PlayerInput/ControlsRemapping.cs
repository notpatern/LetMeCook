using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using PlayerSystems.PlayerInput;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening.Plugins.Core.PathCore;

namespace Player.Input
{

    public static class ControlsRemapping
    {
        public static Action<InputAction> SuccessfulRebinding;

        static Dictionary<string, string> OverridesDictionary = new Dictionary<string, string>();

        static string keyFormat = "{0} : {1}";

        public static void LoadMap()
        {
            LoadControlOverrides();
        }

        public static void RemapKeyboardMouseAction(InputAction actionToRebind, int targetBinding)
        {
            RemapAction(actionToRebind, targetBinding, new string[] {"Keyboard", "Gamepad", "Mouse"});
        }

        static void RemapAction(InputAction actionToRebind, int targetBinding, string[] groups)
        {
            string oldPath = actionToRebind.bindings[targetBinding].path;

            actionToRebind.Disable();
            var rebindOperation = actionToRebind.PerformInteractiveRebinding(targetBinding)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(operation => SuccessfulRebinding?.Invoke(null))
                .OnComplete(operation => {
                    operation.Dispose();
                    AddOverrideToDictionary(actionToRebind.id, actionToRebind.bindings[targetBinding].effectivePath, targetBinding);

                    CheckForDuplication(actionToRebind, targetBinding, groups, oldPath);
                    SaveControlOverrides();

                    SuccessfulRebinding?.Invoke(actionToRebind);
                })
                .Start();

            actionToRebind.Enable();
        }

        static void CheckForDuplication(InputAction actionToRebind, int targetBinding, string[] groups, string oldPath)
        {
            string key;
            foreach(KeyValuePair<string, string> actionDic in OverridesDictionary)
            {
                key = string.Format(keyFormat, actionToRebind.id, targetBinding);

                if (actionDic.Key != key)
                {
                    if (actionDic.Value == actionToRebind.bindings[targetBinding].effectivePath)
                    {
                        actionToRebind.ApplyBindingOverride(targetBinding, oldPath);

                        AddOverrideToDictionary(actionToRebind.id, actionToRebind.bindings[targetBinding].effectivePath, targetBinding);
                        return;
                    }
                }
            }
        }

        private static void AddOverrideToDictionary(Guid actionId, string path, int bindingIndex)
        {
            string key = string.Format(keyFormat, actionId.ToString(), bindingIndex);

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