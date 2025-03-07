using UnityEditor;
using UnityEngine;

namespace ItemLaunch
{
    public class ItemLauncher : MonoBehaviour
    {
        [SerializeField] bool debugMode = true;

        [Header("Curve Properties")]
        public Transform spawnPoint;
        [SerializeField] Transform endPoint;
        [SerializeField, Range(0, 50)] float height;
        [SerializeField, Range(0, 1)] float t1 = 0.5f;
        [SerializeField, Range(0, 1)] float t2 = 0.3f;

        [HideInInspector] public bool state = false;
        [HideInInspector] public float curveFadeTime;
        float timer = 0;
        float lerpDelta = 0;
        bool isTransitionning = false;
        int lerpDirection = 1;

        public LineRenderer lineRenderer;
        [SerializeField] float width;

        [Header("Throw Property")]
        [Range(0.01f, 2f)] public float throwSpeed;


        public Vector3 StartPoint => spawnPoint.position;
        public Vector3 EndPoint => endPoint.position;
        public Vector3 StartTangent => Vector3.Lerp(StartPoint, EndPoint, t1) + Vector3.up * height;
        public Vector3 EndTangent => Vector3.Lerp(EndPoint, StartPoint, t2) + Vector3.up * height;

        private void Start()
        {
            lineRenderer.widthMultiplier = 0;
        }

        public void ChangeState(bool state)
        {
            this.state = state;
            isTransitionning = true;
        }

        private void Update()
        {
            if (isTransitionning)
            {
                timer += Time.deltaTime;
                lerpDelta += Time.deltaTime / curveFadeTime * lerpDirection;

                UpdateMovement();

                if (timer >= curveFadeTime)
                {
                    isTransitionning = false;
                    timer = 0f;
                    lerpDelta = 0f;
                }
            }
        }

        void UpdateMovement()
        {
            float multiplier = 0;
            if (state)
            {
                multiplier = lerpDelta;
            }
            else
            {
                multiplier = 1 - lerpDelta;
            }

            lineRenderer.widthMultiplier = width * multiplier;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!debugMode)
                return;
        
            //Handles.DrawLine(StartPoint, EndPoint, 3f);
            Handles.DrawBezier(
                StartPoint,
                EndPoint,
                StartTangent,
                EndTangent,
                Color.red, null, 10f);
        }
        #endif
        public void ThrowItem(LaunchableItem launchedItem) { launchedItem.Init(this); }
    }
}
