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
    
    [SerializeField] private int m_trickChance = 2000;

    Quaternion m_NextDirection;

    private bool m_isActive = false;

    private bool m_isFacingWall;
    private float m_RaycastDistance;

    private bool m_runTrick = false;
    private string m_currentAnimationName;
    
    
    void Start()
    {
        StartCoroutine(StartOffsetCoroutine());
    }

    void Update()
    {
        if (!m_isActive)
        {
            if (Random.Range(0, m_trickChance) == 1 && !m_runTrick)
            {
                m_runTrick = true;
            }
            
            if (!m_runTrick)
            {
                StartRotate();
                GoForJump();
            }
            else
            {
                StartAnimation("LaunchThrow");
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

    void StartAnimation(string animationName)
    {
        m_isActive = true;
        m_currentAnimationName = animationName;
        m_BoulbiAnim.SetTrigger(animationName);
    }

    void EndAnimation()
    {
        m_isActive = false;
        switch (m_currentAnimationName)
        {
            case "LaunchJump":
                transform.position = m_PositionAtEnd.position;
                GetNextDirection();
                break;
            case "LaunchThrow":
                m_runTrick = false;
                break;
            default:
                Debug.Log("Là c'est zinzin");
                break;
        }
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

    void GoForJump()
    {
        if(Quaternion.Angle(m_NextDirection, transform.rotation) < m_RotationThreshold)
        {
            if (!CheckForWall())
            {
                StartAnimation("LaunchJump");
            }
            else
            {
                GetNextDirection();
            }
        }
    }
}
