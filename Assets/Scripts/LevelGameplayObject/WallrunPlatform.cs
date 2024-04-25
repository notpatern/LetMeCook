
using UnityEngine;

public class WallrunPlatform : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject m_GameEvent;
    [SerializeField] GameObject m_eyeLid;
    [SerializeField] GameEventScriptableObject m_playerPos;
    private Transform m_playerTransform;
    [SerializeField] GameObject m_leftPupil;
    [SerializeField] GameObject m_rightPupil;
    [SerializeField] GameObject m_eyeBall;
    private Quaternion leftRotation;
    private Quaternion rightRotation;

    bool isEyeActive;

    void Awake()
    {
        m_playerPos.BindEventAction((object playerTransform) =>
        {
            m_playerTransform = (Transform)playerTransform;
        });
        m_eyeLid.SetActive(true);
        
        leftRotation = m_leftPupil.transform.rotation;
        rightRotation = m_rightPupil.transform.rotation;
    }

    void OnEnable()
    {
        m_GameEvent.BindEventAction(ActiveLight);
    }

    void OnDisable()
    {
        m_GameEvent.UnbindEventAction(ActiveLight);
    }

    void ActiveLight(object args)
    {
        isEyeActive = (bool)args;
        m_eyeLid.SetActive(!(bool)args);
    }

    private void Update()
    {
        if (isEyeActive)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        Vector3 position = m_playerTransform.position;
        
        Vector3 eyeBallDirection = (position - m_eyeBall.transform.position).normalized;

        Quaternion eyeBallRotation = Quaternion.LookRotation(eyeBallDirection);

        m_leftPupil.transform.rotation = Quaternion.RotateTowards(leftRotation, eyeBallRotation, 16.2256f);
        m_rightPupil.transform.rotation = Quaternion.RotateTowards(rightRotation, eyeBallRotation, 16.2256f);
    }
}
