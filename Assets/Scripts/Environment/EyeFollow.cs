
using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject _playerTransform;
    [SerializeField] float lerpSpeed;
    Transform playerTransform;

    private void Awake()
    {
        _playerTransform.BindEventAction(LoadPlayerTransform);
    }

    private void LoadPlayerTransform(object obj)
    {
        playerTransform = obj as Transform;
    }

    void Update()
    {
        Vector3 direction = playerTransform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), lerpSpeed * Time.deltaTime);
    }

   
}
