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

    public void ActiveWarningPanel(float warningPanelMiddleScreenDuration)
    {
        StartCoroutine(WarningScreenMiddle(warningPanelMiddleScreenDuration));
    }

    IEnumerator WarningScreenMiddle(float duration)
    {
        m_WarningGo.SetActive(true);
        yield return new WaitForSeconds (duration);
        m_WarningGo.SetActive(false);
    }
}
