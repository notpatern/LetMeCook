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

        bool m_IsInteractionStopped;

        UnityEvent<GameObject, HandSystem.HandsType> m_OnActiveInteract;

        public void Init()
        {
            m_OnActiveInteract = new UnityEvent<GameObject, HandSystem.HandsType>();
        }

        public void BindPerformInteraction(UnityAction<GameObject, HandSystem.HandsType> action)
        {
            m_OnActiveInteract.AddListener(action);
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

                playerInteractionUI.SetActiveInteractionText(true);
                playerInteractionUI.UpdateInteractionText("Press [F] to interact with " + m_CurrentInteraction.GetContext());
                m_IsInteractionStopped = true;
            }
        }

        void OnResetInteraction()
        {
            playerInteractionUI.SetActiveInteractionText(false);
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