using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level.LevelDesign
{
    [System.Serializable]
    public class MovingPlatform : MonoBehaviour
    {
        [HideInInspector] public List<MovingPlatformKey> _platformKeys = new();
        
        int _index;

        void Start()
        {
            if (_platformKeys.Count <= 1 || _index == _platformKeys.Count)
            {
                Destroy(this);
                return;
            }
            StartCoroutine(MovePlatform());
        }

        IEnumerator MovePlatform()
        {
            yield return new WaitForSeconds(_platformKeys[_index].pauseBeforeMoving);
            MoveToKey(_platformKeys[_index].position, _platformKeys[_index].rotation, _platformKeys[_index].travelTime);
            OnEndTravel();
        }

        void MoveToKey(Vector3 position, Quaternion rotation, float travelDuration)
        {
            float t = 0f;
            float timer = 0f;
            while (t <= 1)
            {
                Vector3.Lerp(transform.position, position, t);
                Quaternion.Lerp(transform.rotation, rotation, t);

                timer += Time.deltaTime;
                t = timer / travelDuration;
            }
        }

        void OnEndTravel()
        {
            _index++;
            Start();
        }
    }
}
