using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour {

    public KeyCode levelSwitherKey = KeyCode.Space;
    public int sceneIndex = 0;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(levelSwitherKey)) {
            SceneManager.LoadScene(sceneIndex);
        }
	}

}
