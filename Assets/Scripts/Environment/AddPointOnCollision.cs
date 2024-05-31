using Manager;
using UnityEngine;

public class AddPointOnCollision : MonoBehaviour
{

    [SerializeField] LayerMask _removePointOnColLayer;
    [SerializeField] GameManager _gameManager;
    [SerializeField] int _addedScoreOnCol = -10;

    private void OnCollisionEnter(Collision collision)
    {
        if (_removePointOnColLayer == (_removePointOnColLayer | (1 << collision.gameObject.layer)))
        {
            _gameManager.AddScore(_addedScoreOnCol, collision.transform.position);
        }
    }
}
