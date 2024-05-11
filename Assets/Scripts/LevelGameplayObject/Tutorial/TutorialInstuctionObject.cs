using Dialog;
using PlayerSystems.PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tutorial
{
    public class TutorialInstuctionObject : MonoBehaviour
    {
        [SerializeField] bool m_IsStartingOnStart;
        [SerializeField] GameEventScriptableObject m_OnLoadPlayerTransformEvent;
        Transform m_PlayerTransform;
        [SerializeField] GameObject m_CanvasWorldGo;
        [SerializeField] TMP_Text m_TutoText;
        [SerializeField] KeybindsData[] m_Keys;

        [SerializeField] TutorialInstuctionObject m_OptionalNextTask;

        bool m_isInTask = false;
        bool m_IsTaskFinished = false;

        void Awake()
        {
            m_OnLoadPlayerTransformEvent.BindEventAction(OnLoadPlayerTransform);

            m_CanvasWorldGo.SetActive(false);

            if (m_IsStartingOnStart)
            {
                StartTask();
            }
        }

        void Update()
        {
            if (m_isInTask)
            {
                m_CanvasWorldGo.transform.LookAt(m_PlayerTransform);
            }
        }

        void OnLoadPlayerTransform(object arg)
        {
            m_PlayerTransform = (Transform)arg;
        }

        public void StartTask()
        {
            InputAction inputAction;
            string[] loadedKeys = new string[m_Keys.Length];

            for (int i = 0; i < loadedKeys.Length; i++)
            {
                inputAction = InputManager.s_PlayerInput.asset.FindAction(m_Keys[i].inputActionReference.action.id);
                loadedKeys[i] = DialogUIManagement.c_KEYOPTIONS + inputAction.GetBindingDisplayString(m_Keys[i].bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames) + DialogUIManagement.c_KEYENDOPTIONS;
            }

            string data = string.Format(m_TutoText.text, loadedKeys);
            m_TutoText.text = data;
            m_CanvasWorldGo.SetActive(true);
            m_isInTask = true;
        }

        public void OnCompleteTask()
        {
            if (m_IsTaskFinished) return;

            m_IsTaskFinished = true;

            m_CanvasWorldGo.SetActive(false);
            m_isInTask = false;

            if (m_OptionalNextTask)
            {
                m_OptionalNextTask.StartTask();
            }
        }
    }

}