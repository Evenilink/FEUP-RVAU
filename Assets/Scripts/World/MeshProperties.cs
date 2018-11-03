using UnityEngine;

public class MeshProperties : MonoBehaviour {
    public Vector3 min;
    public Vector3 max;
    public int floorId;
    public int floorIndex = -1;

    private void Start() {
    }

    public float GetMeshHeight() {
        return (max-min).y;
    }
}
