using UnityEngine;

public class LaunchableItem : MonoBehaviour
{
    Rigidbody rb;
    ItemLauncher _launcher;
    bool _doingTheBezierCurve = false;
    float _t;
    
    public void Init(ItemLauncher launcher)
    {
        _launcher = launcher;
        transform.LookAt(launcher.transform);
        _doingTheBezierCurve = true;
        GetComponent<Rigidbody>().useGravity = false;
    }
    
    void Update()
    {
        if (!_doingTheBezierCurve)
            return;

        if (_t < 1)
        {
            transform.position = CalculateBezierPoint(_t, _launcher.StartPoint, _launcher.EndPoint, _launcher.StartTangent, _launcher.EndTangent);
            _t += Time.deltaTime * _launcher.throwSpeed;
        }
        else
            QuitBezierCurve();
    }

    void OnCollisionEnter(Collision other) { QuitBezierCurve(); }

    void QuitBezierCurve()
    {
        GetComponent<Rigidbody>().useGravity = true;
        _doingTheBezierCurve = false;
        Destroy(this);
    }
    
    Vector3 CalculateBezierPoint(float t, Vector3 start, Vector3 end, Vector3 startTangent, Vector3 endTangent) {
        // P(t) = start*(1-t)^3 + 3*startTangent*t*(1-t)^2 + 3*endTangent*t^2*(1-t) + end*t^3

        float u = 1-t;
        return start * (u*u*u) + startTangent * (3 * t * (u*u)) + endTangent * (3 * (t*t) * u) + end * (t*t*t);
    }
}
