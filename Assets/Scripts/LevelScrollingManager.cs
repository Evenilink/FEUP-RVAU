﻿using System.Collections;
using UnityEngine;

public class LevelScrollingManager : MonoBehaviour {

    [SerializeField] private Transform rootTransform;
    [SerializeField] private float interpolationTime = 1f;
    private static LevelScrollingManager instance;

    private void Start() {
        // Singleton.
        if (instance != null && instance != this)
            Destroy(this);
        else instance = this;
    }

    public void ScrollToHeight(float height) {
        StopAllCoroutines();
        StartCoroutine(ScrollToHeightCoroutine(height));
    }

    private IEnumerator ScrollToHeightCoroutine(float height) {
        float startTime = Time.time;
        float fracComplete = 0f;
        Vector3 finalPosition = new Vector3(rootTransform.position.x, rootTransform.position.y - height, rootTransform.position.z);
        while (fracComplete < 1) {
            fracComplete = (Time.time - startTime) / interpolationTime;
            rootTransform.position = Vector3.Slerp(rootTransform.position, finalPosition, fracComplete);
            yield return null;
        }
    }

    public static LevelScrollingManager Instance() {
        return instance;
    }
}