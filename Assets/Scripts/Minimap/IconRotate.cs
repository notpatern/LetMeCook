using UnityEngine;

public class IconRotate : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject cameraPosition;
    Transform cameraTransform;

    void Start() {
        cameraPosition.BindEventAction(LoadCameraTransform);
    }

    void LoadCameraTransform(object obj) {
        cameraTransform = obj as Transform;
    }

    void Update()
    {
        if (cameraTransform != null) {
            this.transform.rotation = cameraTransform.rotation;
        }
        this.transform.eulerAngles += new Vector3(90, 0, 0);
    }
}
