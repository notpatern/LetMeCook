using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSway : MonoBehaviour
{
    [Header("Shaking:")]
    [SerializeField] float shakeSpeed = 1;
    [SerializeField] float shakeAmount = 0.1f;

    [Header("Mouse:")]
    [SerializeField] float followMouseAmount = .15f;
    [SerializeField] float followMouseSpeed = 3f;

    Vector3 defaultLocalPos;
    Vector3 randomDirection;
    Vector3 mousePos;

    void Start()
    {
        defaultLocalPos = transform.localPosition;
    }

    void Update()
    {
        var shakePos = new Vector3(Mathf.PerlinNoise(Time.time * shakeSpeed, 0), Mathf.PerlinNoise(10, Time.time * shakeSpeed), 0) * shakeAmount;
        var normalizedMousePos = Mouse.current.position.value / new Vector3(Screen.width, Screen.height);
        normalizedMousePos = new Vector3(Mathf.Clamp01(normalizedMousePos.x), Mathf.Clamp01(normalizedMousePos.y), 0);
        mousePos = Vector3.Lerp(mousePos, normalizedMousePos * followMouseAmount, followMouseSpeed);
        transform.localPosition = defaultLocalPos + shakePos + mousePos;
    }
}
