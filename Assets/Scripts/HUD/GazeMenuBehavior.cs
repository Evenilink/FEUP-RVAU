using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazeMenuBehavior : MonoBehaviour {

    [SerializeField] private float gazeTriggerTime = 2f;
    [SerializeField] private Color pressedColor;
    [SerializeField] private GameObject gazer;
    [SerializeField] private Vector3 gazerMinScale;
    [SerializeField] private Vector3 gazerMaxScale;

    private Color enteredBtnStartColor;
    private GameObject buttonObject;
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    

    void Start() {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        buttonObject = null;
        gazer.transform.localScale = gazerMinScale;
    }

    // Update is called once per frame
    void Update() {
        //Set the Pointer Event Position to that of the mouse position
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results) {
            GameObject btnObject = result.gameObject;
            
            if (isButton(result.gameObject)) {
                if (buttonObject == null) {
                    buttonObject = result.gameObject;
                    OnPointerEnter(buttonObject);
                }

                Vector3 newScale = Vector3.Lerp(gazer.transform.localScale, gazerMaxScale, Time.deltaTime / gazeTriggerTime);
                gazer.transform.localScale = newScale;
                break;
            }
        }

        if (buttonObject != null && results.Count == 0) {
            OnPointerExit(buttonObject);
            buttonObject = null;
            Vector3 newScale = Vector3.Lerp(gazerMinScale, gazer.transform.localScale, Time.deltaTime / gazeTriggerTime);
            gazer.transform.localScale = newScale;
        }
    }

    public void OnPointerEnter(GameObject obj) {
        StartCoroutine(GazeTriggerTimeline(obj));
    }

    public void OnPointerExit(GameObject obj) {
        StopAllCoroutines();
        obj.GetComponent<Image>().color = enteredBtnStartColor;
    }

    IEnumerator GazeTriggerTimeline(GameObject obj) {
        Image image = obj.GetComponent<Image>();
        enteredBtnStartColor = image.color;
        float startTime = Time.time;
        float fracCompleted = 0f;
        while (fracCompleted < 1) {
            fracCompleted = (Time.time - startTime) / gazeTriggerTime;
            image.color = Color.Lerp(enteredBtnStartColor, pressedColor, fracCompleted);
            yield return null;
        }
        ExecuteEvents.Execute(obj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    private bool isButton(GameObject obj) {
        Button btn = obj.GetComponent<Button>();
        return btn != null;
    }
}
