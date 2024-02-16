using System;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Interaction
{
    [Serializable]
    public class PlayerInteraction
    {
        [SerializeField] PlayerInteractionUI playerInteractionUI;

        IInteractable m_CurrentInteraction;

        [SerializeField] Transform m_Origin;

        [SerializeField] float m_InteractionMaxDistance;
        [SerializeField] LayerMask m_LayerMask;

        public UnityEvent<GameObject, HandSystem.HandsType> m_OnActiveInteract;

        public void BindPerformInteraction(UnityAction<GameObject, HandSystem.HandsType> action)
        {
            m_OnActiveInteract.AddListener(action);
        }

        public void Update(float dt)
        {
            StartInteraction();
        }

        public void StartInteraction()
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Origin.position, m_Origin.forward, out hit, m_InteractionMaxDistance, m_LayerMask))
            {
                m_CurrentInteraction = hit.transform.GetComponent<IInteractable>();

                if (m_CurrentInteraction)
                {
                    
                }
            }
        }

        public void ActiveInteraction(HandSystem.HandsType handType)
        {
            m_OnActiveInteract.Invoke(m_CurrentInteraction.gameObject, handType);
        }
    }
}