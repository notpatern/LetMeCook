using UnityEngine;

public class PizzaEtRavioli : MonoBehaviour
{
    [SerializeField] float m_LerpSpeed = 10f;
    [SerializeField] float m_TimeBetweenAnim;
    [SerializeField] private float m_RotationThreshold = 0.2f;
 
    [SerializeField] private Animator m_BoulbiAnim;

    [SerializeField] Transform m_PositionAtEnd;

    Quaternion m_NextDirection;

    void Start()
    {
        GetNextDirection();
    }

    void Update()
    {
        StartRotate();
        if(Quaternion.Angle(m_NextDirection, transform.rotation) < m_RotationThreshold)
        {
            StartAnimation();
        }
    }

    void GetNextDirection()
    {
        m_NextDirection = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    void StartRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, m_NextDirection, m_LerpSpeed * Time.deltaTime);
    }

    void StartAnimation()
    {
        m_BoulbiAnim.SetTrigger("LaunchJump");
    }

    void EndAnimation()
    {
        transform.position = m_PositionAtEnd.position;
        GetNextDirection();
    }
}
