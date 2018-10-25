using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour {

    private const string API_KEY = "72bf022fc5e268985f4b9f9af3f7ac19";
    private const string cityID = "6458924";

    void Start() {
        StartCoroutine(GetWeather(CheckWeatherStatus));
    }

    IEnumerator GetWeather(Action<WeatherInfo> OnSuccess) {
        using (UnityWebRequest req = UnityWebRequest.Get("http://api.openweathermap.org/data/2.5/weather?id=" + cityID + "&APPID=" + API_KEY)) {
            yield return req.SendWebRequest();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            string weatherJSON = System.Text.Encoding.Default.GetString(result);
            Debug.Log(weatherJSON);
            WeatherInfo weatherInfo = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
            OnSuccess(weatherInfo);
        }
    }

    private void CheckWeatherStatus(WeatherInfo weatherInfo) {
        Debug.Log("Current weather is: " + weatherInfo.weather[0].main);
        string weather = weatherInfo.weather[0].main;
        switch (weather) {
            case "Thunderstorm":
                break;
            case "Drizzle":
                break;
            case "Rain":
                break;
            case "Snow":
                break;
            case "Atmosphere":
                break;
            case "Clear":
                break;
            case "Clouds":
                break;
            default: break;
        }
    }
}
