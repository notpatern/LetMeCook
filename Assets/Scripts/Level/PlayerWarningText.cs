using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerWarningText : MonoBehaviour
{
    [SerializeField] GameObject m_WarningGo;
    [SerializeField] TMP_Text m_WarningTimer;

    void Start()
    {
        m_WarningGo.SetActive(false);
    }

    void Init()
    {
        m_WarningGo.SetActive(false);
        m_WarningTimer.text = "";
    }

    public void SetWarningPanelTextTimer(string warningTextTimer)
    {
        m_WarningTimer.text = warningTextTimer;
    }

    public void ActivePanel(bool state)
    {
        m_WarningGo.SetActive(state);
    }
}
