using UnityEngine;
using System.Collections;

public class CameraMotionControls : MonoBehaviour {

    [Header("Automatic Panning")]
    [Tooltip("Toggles whether the camera will automatically rotate around it's target")]
    public bool autoPan = false;
    [Tooltip("The axis on which the camera rotates. Use a value of 1 to rotate in one direction and -1 to rotate in another direction.")]
    public Vector3 rotationDirection = new Vector3(0f, 1f, 0f);
    [Tooltip("The speed at which the camera will auto-pan.")]
    public float autoPanRotationSpeed = 10f;
    [Header("Manual Panning")]
    [Tooltip("The smoothness coming to a stop of the camera afer the uses pans the camera and releases. Lower values result in significantly smoother results. This means the camera will take longer to stop rotating")]
    public float rotationSmoothing = 2f;
    [Tooltip("The object the camera will focus on.")]
    public Transform target;
    [Tooltip("How sensative the camera-panning is when the user pans -- the speed of panning.")]
    public float panSensitivity = 180f;
    [Tooltip("The min and max distance along the Y-axis the camera is allowed to move when panning.")]
    public Vector2 panLimit = new Vector2(5, 80);
    [Tooltip("The position along the Z-axis the camera game object is.")]
    public float distance = 5;
    [Header("Zooming")]
    [Tooltip("Determines whether the camera will zoom by moving on the Z-axis or by adjusting the camera's field of view.")]
    public ZoomType zoomType = ZoomType.FieldOfView;
    [Tooltip("The minimum and maximum range the camera can zoon in.")]
    public Vector2 cameraZoomRange;
    public float zoomSoothness = 0.1f;
    [Tooltip("How sensative the camera zooming is -- the speed of the zooming.")]
    public float zoomSensitivity;

    private Camera cam;
    private bool mouseClickjudge;
    private float cameraZDistance;
    new private Transform transform;
    private Vector2 startAngle;
    private float xVelocity = 0f;
    private float yVelocity = 0f;
    private float xRotationAxis = 0.0f;
    private float yRoptationAxis = 0.0f;
    private float zoomVelocity = 0f;

    public enum PanAxis {
        xAxis,
        yAxis,
        zAxis
    }

    public enum ZoomType {
        ZAxisPosition,
        FieldOfView
    }

    private void Awake () {
        cam = GetComponent<Camera>();
        transform = GetComponent<Transform>();
    }

    private void Start () {
        cameraZDistance = cam.fieldOfView;

        Vector3 angles = transform.eulerAngles;

        startAngle.x = angles.x;
        startAngle.y = angles.y;
    }

    private void Update () {
        if (autoPan) {
            xVelocity +=  panSensitivity * Time.deltaTime;
        }
        PanAround();
        Zoom();
    }

    private void LateUpdate () {
        if (target) {
            if (Input.GetMouseButton(0)) {
                xVelocity += Input.GetAxis("Mouse X") * panSensitivity;
                yVelocity -= Input.GetAxis("Mouse Y") * panSensitivity;
            }

            startAngle.x = xVelocity;
            startAngle.y = yVelocity;

            xRotationAxis += xVelocity;
            yRoptationAxis += yVelocity;

            yRoptationAxis = ClampAngle(yRoptationAxis, panLimit.x, panLimit.y);

            Quaternion rotation = Quaternion.Euler(yRoptationAxis, xRotationAxis * autoPanRotationSpeed, 0);
            Vector3 position = rotation * new Vector3(0f, 0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;

            xVelocity = Mathf.Lerp(xVelocity, 0, Time.deltaTime * rotationSmoothing);
            yVelocity = Mathf.Lerp(yVelocity, 0, Time.deltaTime * rotationSmoothing);
        }
    }

    private void PanAround () {
        #if UNITY_ANDROID || UNITY_IOS || UNITY_WSA
            Touch touch = Input.GetTouch(0);
                
            if (touch.phase == TouchPhase.Moved) {
                transform.RotateAround(target.position, target.up, Input.GetAxis("Mouse X") * rotationSpeed);
            }
        #endif
        
        #if UNITY_EDITOR || UNITY_STANDALONE
            if (autoPan) {
                transform.RotateAround(target.position, rotationDirection, autoPanRotationSpeed * Time.smoothDeltaTime);
            } else {
                transform.LookAt(target);

                if (Input.GetMouseButton(0)) {
                    transform.LookAt(target);
                    transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * panSensitivity);
                }
            }
        #endif
    }

    private void Zoom () {
        #if UNITY_ANDROID || UNITY_IOS || UNITY_WSA
            if (Input.touchCount == 2) {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                    
                cam.fieldOfView = cameraZDistance;
                cameraZDistance += deltaMagnitudeDiff * zoomnSensitivity;
                cameraZDistance = Mathf.Clamp(cameraZDistance, cameraZoomRange.x, cameraZoomRange.y);
            }
        #endif

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            cameraZDistance = Mathf.SmoothDamp(cameraZDistance, cameraZDistance -= zoomSensitivity, ref zoomVelocity, Time.deltaTime * zoomSoothness);

            if (cameraZDistance <= cameraZoomRange.x) {
                cameraZDistance = cameraZoomRange.x;
            }
        }
        else {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                cameraZDistance = Mathf.SmoothDamp(cameraZDistance, cameraZDistance += zoomSensitivity, ref zoomVelocity, Time.deltaTime * zoomSoothness);

                if (cameraZDistance >= cameraZoomRange.y) {
                    cameraZDistance = cameraZoomRange.y;
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0) {
            cam.fieldOfView = cameraZDistance;
        }
    }

    public void AdjustPanSensitivity (float _sensitivity) {
        panSensitivity = _sensitivity * 2;
    }

    public void AdjustZoomSensitivity (float _sensitivity) {
        zoomSensitivity = _sensitivity * 4;
    }

    private static float ClampAngle (float angle, float min, float max) {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}
