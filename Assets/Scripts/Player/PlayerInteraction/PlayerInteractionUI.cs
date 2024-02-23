using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] TMP_Text intereactionText;

    public void SetActiveInteractionText(bool state)
    {
        intereactionText.gameObject.SetActive(state);
    }

    public void UpdateInteractionText(string data)
    {
        intereactionText.text = data;
    }
}
