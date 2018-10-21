using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    [SerializeField] float distanceToSpawn = 3.5f;
    [SerializeField] int levelsForEachSide = 1;
    [SerializeField] GameObject startLevel;
    [SerializeField] GameObject underLevel;
    [SerializeField] GameObject[] levels;

    List<GameObject> spawnedLevels;
    Random random;
    private float nextDistance;
    
	void Start () {
        spawnedLevels = new List<GameObject>();
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));

        for (int i = 0; i < levelsForEachSide; i++) {
            float height = (i + 1) * distanceToSpawn * -1;
            spawnedLevels.Add(Instantiate(underLevel, new Vector3(0, height, 0), Quaternion.identity, transform));
        }

        spawnedLevels.Add(Instantiate(startLevel, transform));

        nextDistance = distanceToSpawn;
        for (int i = 0; i < levelsForEachSide; i++) {
            CreateNewLevel();
            //float height = (i + 1) * distanceToSpawn;
            //spawnedLevels.Add(Instantiate(getRandomLevel(), new Vector3(0, height, 0), Quaternion.identity, transform));
        }

	}

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            CreateNewLevel();
        }
    }

    public void CreateNewLevel() {
        GameObject level = Instantiate(getRandomLevel(), transform);
        level.transform.localPosition = new Vector3(0, nextDistance, 0);
        spawnedLevels.Add(level);
        nextDistance += distanceToSpawn;

        if (spawnedLevels.Count > levelsForEachSide * 2 + 1) {
            GameObject bottomLevel = spawnedLevels[0];
            spawnedLevels.RemoveAt(0);
            Destroy(bottomLevel);
        }
    }

    private GameObject getRandomLevel() {
        int rndIndex = Random.Range(0, levels.Length);
        return levels[rndIndex];
    }
}
