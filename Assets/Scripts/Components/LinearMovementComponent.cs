using UnityEngine;

public class LinearMovementComponent : MonoBehaviour {

    [SerializeField] private float aliveTime = 0f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Axis axisToMove;
    private enum Axis {
        FORWARD,
        BACK,
        RIGHT,
        LEFT,
        UP,
        DOWN
    }
    private Vector3 selectedAxis;

    private void Awake() {
        if (aliveTime > 0)
            Destroy(gameObject, aliveTime);
        switch (axisToMove) {
            case Axis.FORWARD:
                selectedAxis = transform.forward;
                break;
            case Axis.BACK:
                selectedAxis = -transform.forward;
                break;
            case Axis.RIGHT:
                selectedAxis = transform.right;
                break;
            case Axis.LEFT:
                selectedAxis = -transform.right;
                break;
            case Axis.UP:
                selectedAxis = transform.up;
                break;
            case Axis.DOWN:
                selectedAxis = -transform.up;
                break;
        }
    }
	
	private void Update () {
        transform.position += selectedAxis * moveSpeed * Time.deltaTime;
	}
}
