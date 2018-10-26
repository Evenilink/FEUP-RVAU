using UnityEngine;

public class CloudSystem : MonoBehaviour {

    [Header("System Settings")]
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Mesh[] cloudMeshes;
    [SerializeField] private int numClouds = 10;

    [Header("Position Spawn Info")]
    [SerializeField] private float minSpawnX;
    [SerializeField] private float maxSpawnX;
    [SerializeField] private float minSpawnY;
    [SerializeField] private float maxSpawnY;
    [SerializeField] private float minSpawnZ;
    [SerializeField] private float maxSpawnZ;

    private GameObject[] clones;

	private void Awake () {
        Debug.Log(new Vector3((Mathf.Abs(maxSpawnX) + Mathf.Abs(minSpawnX)) / 2, (Mathf.Abs(maxSpawnY) + Mathf.Abs(minSpawnY)) / 2, (Mathf.Abs(maxSpawnZ) + Mathf.Abs(minSpawnZ)) / 2));
        clones = new GameObject[numClouds];
		for (int i = 0; i < numClouds; i++) {
            Vector3 spawnPosition = new Vector3(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY), Random.Range(minSpawnZ, maxSpawnZ));
            clones[i] = Instantiate(cloudPrefab, spawnPosition, transform.rotation, transform);
            clones[i].GetComponent<MeshFilter>().mesh = cloudMeshes[Random.Range(0, cloudMeshes.Length - 1)];
        }
	}

    private void Update() {
        for (int i = 0; i < numClouds; i++) {
            if (clones[i].transform.position.z >= maxSpawnZ)
                RandomizeCloud(clones[i]);
        }
    }

    private void RandomizeCloud(GameObject cloud) {
        cloud.transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY), minSpawnZ);
        cloud.GetComponent<MeshFilter>().mesh = cloudMeshes[Random.Range(0, cloudMeshes.Length - 1)];
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.grey;
        Vector3 center = new Vector3((maxSpawnX + minSpawnX) / 2, (maxSpawnY + minSpawnY) / 2, (maxSpawnZ + minSpawnZ) / 2);
        Vector3 size = new Vector3(Mathf.Abs(maxSpawnX) + Mathf.Abs(minSpawnX), Mathf.Abs(maxSpawnY) + Mathf.Abs(minSpawnY), Mathf.Abs(maxSpawnZ) + Mathf.Abs(minSpawnZ));
        Gizmos.DrawWireCube(center, size);
    }
}
