using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEditor;

// Based on this tutorial: https://www.red-gate.com/simple-talk/dotnet/c-programming/calling-restful-apis-unity3d/
public class WeatherManager : MonoBehaviour {

    private const string API_KEY = "72bf022fc5e268985f4b9f9af3f7ac19";
    private const int accuracy = 500;
    private const int updateDistance = 1000;
    [SerializeField] private int maxTimeOut = 10;
    [SerializeField] private GameObject cloudSystem;

    void Start() {
        StartCoroutine(GetWeather(CheckWeatherStatus));
    }

    IEnumerator GetWeather(Action<WeatherInfo> OnSuccess) {
        float latitude, longitude;

#if UNITY_STANDALONE
        latitude = 41.15f;
        longitude = -8.61f;
#endif

#if UNITY_ANDROID
        if (!Input.location.isEnabledByUser) {
            Debug.LogError("Location service is not enabled.");
            // yield break;
        }
        else Debug.LogError("Services enabled.");

        Input.location.Start(accuracy, updateDistance);

        int currWait = maxTimeOut;
        while (Input.location.status == LocationServiceStatus.Initializing && currWait > 0) {
            yield return new WaitForSeconds(1);
            currWait--;
        }

        if (currWait < 1) {
            // Service timed out.
            Debug.LogError("Timed out.");
            yield break;
        } else if (Input.location.status == LocationServiceStatus.Failed) {
            // Connection has failed.
            Debug.LogError("Unable to determine device location.");
            yield break;
        } else {
            // Access granted and location retrieved.
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
        }
#endif

        string header = "http://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&APPID=" + API_KEY;
        using (UnityWebRequest req = UnityWebRequest.Get(header)) {
            yield return req.SendWebRequest();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            string weatherJSON = System.Text.Encoding.Default.GetString(result);
            WeatherInfo weatherInfo = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
            CheckWeatherStatus(weatherInfo);
        }
    }

    private void CheckWeatherStatus(WeatherInfo weatherInfo) {
        Debug.Log("Current weather is: " + weatherInfo.weather[0].main);
        string weather = weatherInfo.weather[0].main;
        switch (weather) {
            case "Clear":
                break;
            case "Rain":
            case "Snow":
            case "Thunderstorm":
            case "Drizzle":
            case "Atmosphere":
            case "Clouds":
                Instantiate(cloudSystem, null);
                break;
            default: break;
        }
    }
}
