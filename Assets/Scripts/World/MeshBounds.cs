using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBounds : MonoBehaviour {
    public Vector3 min;
    public Vector3 max;

    private void Start() {
        Debug.Log("LeTotal: " + GetMeshHeight() );
    }
    public float GetMeshHeight() {
        return (max-min).y;
    }
}
