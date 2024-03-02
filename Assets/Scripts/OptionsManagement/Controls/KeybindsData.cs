using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Keybind", menuName = "LetMeCook/Options/KeybindData")]
public class KeybindsData : ScriptableObject
{
    public string DisplayedKeyName;
    public InputActionReference inputActionReference;
    public int bindingIndex;
}
