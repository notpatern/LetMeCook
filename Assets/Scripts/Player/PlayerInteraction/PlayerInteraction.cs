using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerInteraction
{
    IInteractable m_CurrentInteraction;

    [SerializeField] Transform m_Origin;

    [SerializeField] float m_InteractionMaxDistance;
    [SerializeField] LayerMask m_LayerMask;

    public UnityEvent<GameObject, int> m_OnInteract;

    public void BindPerformInteraction(UnityAction<GameObject, int> action)
    { 
        m_OnInteract.AddListener(action);
    }

    public void Update(float dt)
    {
        TestInteraction();
    }

    public void TestInteraction()
    {
        RaycastHit hit;
        if(Physics.Raycast(m_Origin.position, m_Origin.forward, out hit, m_InteractionMaxDistance, m_LayerMask))
        {
            m_CurrentInteraction = hit.transform.GetComponent<IInteractable>();

            if(m_CurrentInteraction)
            {
                m_OnInteract.Invoke(m_CurrentInteraction.gameObject, 0);
            }
        }
    }
}