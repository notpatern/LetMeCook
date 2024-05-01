using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PizzaEtRavioli : MonoBehaviour
{
    [SerializeField] float m_LerpSpeed = 10f;
    [SerializeField] float m_StartOffsetRange = 3;
    [SerializeField] private float m_RotationThreshold = 0.2f;
 
    [SerializeField] private Animator m_BoulbiAnim;

    [SerializeField] Transform m_PositionAtEnd;

    Quaternion m_NextDirection;

    private bool m_isActive = false;

    private bool m_isFacingWall;
    private float m_RaycastDistance;
    

    void Start()
    {
        StartCoroutine(StartOffsetCoroutine());
    }

    void Update()
    {
        if (!m_isActive)
        {
            StartRotate();
            if(Quaternion.Angle(m_NextDirection, transform.rotation) < m_RotationThreshold)
            {
                if (!CheckForWall())
                {
                    StartAnimation();
                }
                else
                {
                    GetNextDirection();
                }
            }
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
        m_isActive = true;
        m_BoulbiAnim.SetTrigger("LaunchJump");
    }

    void EndAnimation()
    {
        m_isActive = false;
        transform.position = m_PositionAtEnd.position;
        GetNextDirection();
    }

    bool CheckForWall()
    {
       return m_isFacingWall = Physics.Raycast(transform.position, transform.forward, m_RaycastDistance);
    }

    IEnumerator StartOffsetCoroutine()
    {
        m_isActive = true;
        yield return new WaitForSeconds(Random.Range(0, m_StartOffsetRange));
        m_RaycastDistance = Vector3.Distance(m_PositionAtEnd.position ,transform.position);
        GetNextDirection();
        m_isActive = false;
        
    }
}
