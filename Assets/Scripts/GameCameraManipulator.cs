using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraManipulator : MonoBehaviour {

    public CameraMotionControls cameraMotionControls;
    public KeyCode toggleAutoRotateKey = KeyCode.R;
    public KeyCode toggleDragRotation = KeyCode.D;

    private float dragSensitivity;

    private void Awake() {
        dragSensitivity = cameraMotionControls.panSensitivity;
    }

    // Update is called once per frame
    private void Update () {
		if (Input.GetKeyDown(toggleAutoRotateKey)) {
            cameraMotionControls.autoPan = !cameraMotionControls.autoPan;
        }
        if (Input.GetKeyDown(toggleDragRotation)) {
            if (cameraMotionControls.panSensitivity == dragSensitivity) {
                cameraMotionControls.panSensitivity = 0;
                return;
            }
            else {
                cameraMotionControls.panSensitivity = dragSensitivity;
            }
        }
    }

}
