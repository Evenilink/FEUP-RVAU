using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazeMenuBehavior : MonoBehaviour {

    [SerializeField] private float gazeTriggerTime = 2f;
    [SerializeField] private Color pressedColor;
    private Color enteredBtnStartColor;

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
}
