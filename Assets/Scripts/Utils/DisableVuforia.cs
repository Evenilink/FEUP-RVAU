using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableVuforia : MonoBehaviour {
    [SerializeField] bool disableVuforia = false;
    [SerializeField] GameObject levelRoot;
    [SerializeField] float mouseSensitivity = 10;
    private void Awake() {
        if (disableVuforia) {
            GetComponent<CustomTrackableEventHandler>().enabled = false;
            transform.Find("ChildTargets").gameObject.SetActive(false);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (disableVuforia) {
            float v = mouseSensitivity * Input.GetAxis("Mouse X");
            levelRoot.transform.Rotate(0, v, 0);

        }
	}
}
