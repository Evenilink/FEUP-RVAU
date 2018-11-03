using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    
    [SerializeField] int levelsForEachSide = 1;
    [SerializeField] GameObject startLevel;
    [SerializeField] GameObject underLevel;
    [SerializeField] GameObject[] levels;

    List<GameObject> spawnedLevels;
    Random random;
    private float nextDistance;

    private static WorldGenerator instance;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            CreateNewLevel();
        }
    }

    public static WorldGenerator Instance() {
        return instance;
    }

    public void GenerateStart() {
        DestroyLoadedLevels();

        spawnedLevels = new List<GameObject>();
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));

        float distance = underLevel.GetComponent<MeshProperties>().GetMeshHeight();
        for (int i = 0; i < levelsForEachSide; i++) {
            float height = (i + 1) * distance * -1;
            spawnedLevels.Add(Instantiate(underLevel, new Vector3(0, height, 0), Quaternion.identity, transform));
        }

        spawnedLevels.Add(Instantiate(startLevel, transform));
        nextDistance = startLevel.GetComponent<MeshProperties>().GetMeshHeight();

        for (int i = 0; i < levelsForEachSide; i++) {
            CreateNewLevel();
        }
    }

    private void DestroyLoadedLevels() {
        if (transform.childCount > 0) {
            for (int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void CreateNewLevel() {
        GameObject level = Instantiate(getRandomLevel(), transform);
        level.transform.localPosition = new Vector3(0, nextDistance, 0);
        spawnedLevels.Add(level);
        nextDistance += level.GetComponent<MeshProperties>().GetMeshHeight(); ;
        
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
