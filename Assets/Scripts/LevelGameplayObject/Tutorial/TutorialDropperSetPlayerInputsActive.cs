using System;
using FoodSystem.FoodMachinery;
using UnityEngine;

public class TutorialDropperSetPlayerInputsActive : FoodDropper
{
    [Header("Tutorial properties")]
    [SerializeField] GameEventScriptableObject m_PlayerSetActiveInput;
    [SerializeField] KeybindsData m_KeybindsDataLeft;
    [SerializeField] KeybindsData m_KeybindsDataRight;
    [SerializeField] bool m_State;

    public override GameObject StartInteraction()
    {
        Tuple<KeybindsData, bool> actionL = new (m_KeybindsDataLeft, m_State);
        Tuple<KeybindsData, bool> actionR = new (m_KeybindsDataRight, m_State);
        m_PlayerSetActiveInput.TriggerEvent(actionL);
        m_PlayerSetActiveInput.TriggerEvent(actionR);
        
        return base.StartInteraction();
    }
}
