using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] TMP_Text intereactionText;

    public void UpdateInteractionText(string data)
    {
        intereactionText.text = data;
    }
}
