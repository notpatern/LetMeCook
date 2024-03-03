using System;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Interaction
{
    [Serializable]
    public class PlayerInteraction
    {
        IInteractable m_CurrentInteraction;

        [SerializeField] Transform m_Origin;

        [SerializeField] float m_InteractionMaxDistance;
        [SerializeField] LayerMask m_LayerMask;

        bool m_IsInteractionStopped;

        UnityEvent<GameObject, HandSystem.HandsType> m_OnActiveInteract = new UnityEvent<GameObject, HandSystem.HandsType>();
        UnityEvent<bool, string> m_OnStartInteraction = new UnityEvent<bool, string>();
        UnityEvent<bool> m_OnEndInteraction = new UnityEvent<bool>();
        
        public void BindPerformInteraction(UnityAction<GameObject, HandSystem.HandsType> action)
        {
            m_OnActiveInteract.AddListener(action);
        }

        public void BindOnInteractionUI(UnityAction<bool, string> startAction, UnityAction<bool> endAction)
        {
            m_OnStartInteraction.AddListener(startAction);
            m_OnEndInteraction.AddListener(endAction);
        }

        public void Update(float dt)
        {
            UpdateInteraction();
        }

        public void UpdateInteraction()
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Origin.position, m_Origin.forward, out hit, m_InteractionMaxDistance, m_LayerMask))
            {
                OnStartInteraction(hit.transform.GetComponent<IInteractable>());
            }
            else if(m_IsInteractionStopped)
            {
                OnResetInteraction();
            }
        }

        void OnStartInteraction(IInteractable interaction)
        {
            if (m_CurrentInteraction != interaction)
            {
                m_CurrentInteraction = interaction;

                m_OnStartInteraction.Invoke(true, m_CurrentInteraction.GetContext());
                m_IsInteractionStopped = true;
            }
        }

        void OnResetInteraction()
        {
            m_OnEndInteraction.Invoke(false);
            m_IsInteractionStopped = false;
            m_CurrentInteraction = null;
        }

        public void ActiveInteraction(HandSystem.HandsType handType)
        {
            if (m_CurrentInteraction != null)
            {
                m_OnActiveInteract.Invoke(m_CurrentInteraction.StartInteraction(), handType);

            }
            else
            {
                m_OnActiveInteract.Invoke(null, handType);
            }
        }
    }
}