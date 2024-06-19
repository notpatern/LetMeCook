using System;
using FoodSystem.FoodMachinery;
using UnityEngine;

public class TutorialStoragePlatformSetInputActive : StoragePlatform
{
    [Header("Tutorial properties")]
    [SerializeField] GameEventScriptableObject m_PlayerSetActiveInput;
    [SerializeField] KeybindsData m_KeybindsData;
    [SerializeField] bool m_State;

    public override GameObject StartInteraction()
    {
        Tuple<KeybindsData, bool> action = new (m_KeybindsData, m_State);
        m_PlayerSetActiveInput.TriggerEvent(action);
        
        return base.StartInteraction();
    }
}