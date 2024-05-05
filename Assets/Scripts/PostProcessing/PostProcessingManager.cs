using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PostProcessing
{
    public class PostProcessingManager : MonoBehaviour
    {
        [SerializeField] GameEventScriptableObject m_PostProcessorReferenceEvent;
        [SerializeField] Volume m_GlobalVolume;
        [SerializeField, Range(0f, 1f)] float m_ChromaticAberrationOffset = 0.5f;
        [SerializeField] float m_ChromaticAberrationSpeed = 1f;
        ChromaticAberration m_ChromaticAberration;
        float m_DefaultChromaticAberration;

        void Awake()
        {
            if (m_GlobalVolume.profile.TryGet(out m_ChromaticAberration))
            {
                m_DefaultChromaticAberration = m_ChromaticAberration.intensity.value;
            }
            else
            {
                Debug.LogError("No Chromatic Aberration on Global Volume " + m_GlobalVolume.profile.name);
            }
        }

        void Start()
        {
            m_PostProcessorReferenceEvent.TriggerEvent(this);
        }

        public void ChangeChromaticAberration(float value, float duration)
        {
            StartCoroutine(ChangeChromaticAbberationInTimeCoroutine(value, duration));
        }

        IEnumerator ChangeChromaticAbberationInTimeCoroutine(float value, float duration)
        {
            float startTime = Time.time;
            float addedValue = (value - m_ChromaticAberration.intensity.value) / duration;

            float lerpT = 0f;
            int lerpDirection = 1;

            m_ChromaticAberration.active = true;

            while (Time.time - startTime < duration)
            {
                lerpT += (Time.deltaTime / m_ChromaticAberrationSpeed) * lerpDirection;
                m_ChromaticAberration.intensity.value = Mathf.Lerp(m_DefaultChromaticAberration, addedValue, lerpT);

                if (lerpDirection == 1 && Time.time - startTime > duration * m_ChromaticAberrationOffset)
                {
                    lerpDirection = -1;
                }
                
                yield return null;
            }

            m_ChromaticAberration.intensity.value = m_DefaultChromaticAberration;
            m_ChromaticAberration.active = false;
        }
    }
}