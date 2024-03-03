using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Level.LevelDesign
{
    public class MovingPlatform : MonoBehaviour
    {
        public List<MovingPlatformKey> platformKeys = new();
        
        int _index;

        void Start()
        {
            if (platformKeys.Count <= 1 || _index == platformKeys.Count)
            {
                Destroy(this);
                return;
            }
            StartCoroutine(MovePlatform());
        }

        IEnumerator MovePlatform()
        {
            yield return new WaitForSeconds(platformKeys[_index].pauseBeforeMoving);
            MoveToKey(
                platformKeys[_index].position, 
                Quaternion.Euler(platformKeys[_index].rotation), 
                platformKeys[_index].scale, 
                platformKeys[_index].travelTime);
            OnEndTravel();
        }

        public void MoveToKey(Vector3 position, Quaternion rotation, Vector3 scale, float travelDuration)
        {
            float t = 0f;
            float timer = 0f;
            while (t <= 1)
            {
                Vector3.Lerp(transform.position, position, t);
                Quaternion.Lerp(transform.rotation, rotation, t);
                Vector3.Lerp(transform.localScale, scale, t);

                timer += Time.deltaTime;
                t = timer / travelDuration;
            }
        }

        void OnEndTravel()
        {
            _index++;
            Start();
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            for (int i = 0; i < platformKeys.Count-1; i++)
            {
                Handles.DrawLine(platformKeys[i].position, platformKeys[i+1].position, 1);
            }
        }
#endif
    }
}
