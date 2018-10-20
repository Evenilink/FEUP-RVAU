using UnityEngine;

public class Goomba : MonoBehaviour {

    [SerializeField] private float movSpeed;
    [SerializeField] private bool startFacingRight = false;
    [SerializeField] private LayerMask levelMask;

    private void Start() {
        if (!startFacingRight)
            transform.rotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
    }

    void Update () {
        transform.position += transform.right * movSpeed * Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.right, 0.09f, levelMask))
            Flip();
	}

    private void Flip() {
        transform.rotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
    }
}
