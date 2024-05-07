
using UnityEngine;
using TMPro;

public class QueueTextFollow : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject _playerTransform;
    [SerializeField] float lerpSpeed;

    [SerializeField] float minimumViewingDistance;
    [SerializeField] float maximumViewingDistance;
    [SerializeField] public TextMeshPro text;
    Transform playerTransform;

    private void Awake()
    {
        _playerTransform.BindEventAction(LoadPlayerTransform);
        text.alpha = 0;
    }

    private void LoadPlayerTransform(object obj)
    {
        playerTransform = obj as Transform;
    }

    void Update()
    {
        Vector3 direction = playerTransform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), lerpSpeed * Time.deltaTime);
        LerpTextAlpha();
    }

    private void LerpTextAlpha()
    {
        float distance = (text.transform.position - playerTransform.position).magnitude;

        float range = maximumViewingDistance - minimumViewingDistance;
        float completionPosition = distance - minimumViewingDistance;

        if (maximumViewingDistance > distance && distance > minimumViewingDistance)
        {
            text.alpha = 1 - (completionPosition / range);
        }
    }
}
