
using UnityEngine;

public class WallrunPlatform : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject m_GameEvent;
    [SerializeField] GameObject m_eyeLid;
    [SerializeField] GameEventScriptableObject m_playerPos;
    private Transform m_playerTransform;
    [SerializeField] GameObject m_leftPupil;
    [SerializeField] GameObject m_rightPupil;
    private Quaternion leftRotation;
    private Quaternion rightRotation;

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
        m_eyeLid.SetActive(!(bool)args);
    }

    private void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 position = m_playerTransform.position;
        
        Vector3 leftPupilDirection = position - m_leftPupil.transform.position;
        Vector3 rightPupilDirection = position - m_rightPupil.transform.position;

        Quaternion targetLeftRotation = Quaternion.LookRotation(leftPupilDirection);
        Quaternion targetRightRotation = Quaternion.LookRotation(rightPupilDirection);

        m_leftPupil.transform.rotation = Quaternion.RotateTowards(leftRotation, targetLeftRotation, 16.2256f);
        m_rightPupil.transform.rotation = Quaternion.RotateTowards(rightRotation, targetRightRotation, 16.2256f);
    }
}
