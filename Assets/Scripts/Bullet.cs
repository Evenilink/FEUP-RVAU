using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] private float aliveTime = 10f;
    [SerializeField] private float moveSpeed = 1f;

    private void Awake() {
        Destroy(gameObject, aliveTime);
    }
	
	void Update () {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
	}
}
