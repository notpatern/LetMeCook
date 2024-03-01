using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level.LevelDesign
{
    [System.Serializable]
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] List<MovingPlatformKey> platformKeys = new();
        
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
            MoveToKey(platformKeys[_index].position, platformKeys[_index].rotation, platformKeys[_index].travelTime);
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
