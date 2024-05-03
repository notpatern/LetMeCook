using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioQueueComponent : MonoBehaviour
    {
        Queue<EventReference> currentRecipeVoiceQueue = new Queue<EventReference>();
        EventInstance currentVoiceInstance;
        PLAYBACK_STATE currentPbState;

        void Update()
        {
            if (currentRecipeVoiceQueue.Count > 0)
            {
                currentVoiceInstance.getPlaybackState(out currentPbState);
                if (currentPbState == PLAYBACK_STATE.STOPPED)
                {
                    StartSound(currentRecipeVoiceQueue.Dequeue());
                }
            }
        }

        public void StartSound(EventReference eventReference)
        {
            PLAYBACK_STATE pbState;
            currentVoiceInstance.getPlaybackState(out pbState);

            if (pbState == PLAYBACK_STATE.PLAYING)
            {
                currentRecipeVoiceQueue.Enqueue(eventReference);
                return;
            }
            currentVoiceInstance = AudioManager.s_Instance.CreateInstance(eventReference);
            currentVoiceInstance.start();
            currentVoiceInstance.release();
        }
    }

}