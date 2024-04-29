using System;
using UnityEngine;
using UnityEngine.Events;
using Player.HandSystem;
using PlayerSystems.HandsSystem;
using FoodSystem.FoodType;
using Unity.VisualScripting;

namespace Player.Interaction
{
    [Serializable]
    public class PlayerInteraction
    {
        HandsManager m_HandManager;
        IInteractable m_CurrentInteraction;

        [SerializeField] Transform m_Origin;

        [SerializeField] float m_InteractionMaxDistance;
        public LayerMask m_LayerMask;

        bool m_IsInteractionStopped;

        UnityEvent<GameObject, HandsType> m_OnActiveInteract = new UnityEvent<GameObject, HandsType>();
        UnityEvent<bool, string> m_OnStartInteraction = new UnityEvent<bool, string>();
        UnityEvent<bool> m_OnEndInteraction = new UnityEvent<bool>();

        RaycastHit hit;

        public void InitPlayerInteraction(HandsManager handsManager)
        {
            m_HandManager = handsManager;
        }

        public void BindPerformInteraction(UnityAction<GameObject, HandsType> action)
        {
            m_OnActiveInteract.AddListener(action);
        }

        public void BindOnInteractionUI(UnityAction<bool, string> startAction, UnityAction<bool> endAction)
        {
            m_OnStartInteraction.AddListener(startAction);
            m_OnEndInteraction.AddListener(endAction);
        }

        public void Update()
        {
            UpdateInteraction();
        }

        public void UpdateInteraction()
        {
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

        public void ActiveInteraction(HandsType handType)
        {
            if (m_CurrentInteraction != null)
            {
                if (hit.collider.gameObject != null)
                {
                    GameObject food = hit.collider.gameObject;

                    if (m_HandManager.IsFoodInHand(HandsType.RIGHT, food))
                    {
                        m_OnActiveInteract.Invoke(null, handType);
                        return;
                    }

                    if (m_HandManager.IsFoodInHand(HandsType.LEFT, food))
                    {
                        m_OnActiveInteract.Invoke(null, handType);
                        return;
                    }
                }
                
                if (!m_HandManager.IsFoodHandle(handType))
                {
                    m_OnActiveInteract.Invoke(m_CurrentInteraction.StartInteraction(), handType);
                    return;
                }
            }
            m_OnActiveInteract.Invoke(null, handType);
        }
    }
}