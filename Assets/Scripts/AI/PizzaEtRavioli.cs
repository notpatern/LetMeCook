using UnityEngine;

public class PizzaEtRavioli : MonoBehaviour
{
    [SerializeField] float m_Speed = 10f;
    [SerializeField] Vector2 m_Jump = new Vector2(5f, 7f);

    Vector3 m_NextDirection;

    void Start()
    {
        GetNextDirection();
    }

    void Update()
    {
        
    }

    void GetNextDirection()
    {
        m_NextDirection = new Vector3(0, Random.Range(0, 360), 0);
        float jumpForce = Random.Range(m_Jump.x, m_Jump.y);
    }
}
