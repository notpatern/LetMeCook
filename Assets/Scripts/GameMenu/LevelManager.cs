using Audio;
using UI;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] DefaultLevelData m_DefaultLevelData;
        protected UIManager m_UiManager;
        [SerializeField] AudioSoundData m_AudioSoundData;
        [SerializeField] protected LevelData m_LevelData;
        [SerializeField, Tooltip("can be null")] Transform m_EndconditionParentUI;

        protected MusicManager m_MusicManager;

        protected virtual void Awake()
        {
            if(!LevelLoader.s_instance)
            {
                Instantiate(m_DefaultLevelData.LevelLoader);
            }

            GraphicsOption.GraphicsOptionManagement.LoadGraphicsOptionManagement();
            ControlOptions.ControlOptionsManagement.LoadControlOptionsManagement();
            TimeOption.TimeOptionManagement.LoadTimeOptionsManagement();
            AudioManager.InitAudioManager(m_AudioSoundData);

            m_MusicManager = new MusicManager();
            m_MusicManager.InitializeMusic(m_LevelData.levelMusicData, this);

            m_UiManager = new UIManager();
            m_UiManager.LoadUI(m_LevelData.levelUIData, m_LevelData.dialogLevelData, m_EndconditionParentUI);
        }

        void OnDestroy()
        {
            AudioManager.s_Instance.CleanUp();
        }
    }
}