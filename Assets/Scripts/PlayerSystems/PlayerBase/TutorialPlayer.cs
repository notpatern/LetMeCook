using UnityEngine;

namespace PlayerSystems.PlayerBase
{
    public class TutorialPlayer : Player
    {
        [Header("Tutorial defined activations")]
        [SerializeField] GameObject m_RightHandGo;

        protected void Start()
        {
            m_RightHandGo.SetActive(false);
        }

        public void SetActiveRightHand(bool active)
        {
            m_RightHandGo.SetActive(active);
        }
    }

}