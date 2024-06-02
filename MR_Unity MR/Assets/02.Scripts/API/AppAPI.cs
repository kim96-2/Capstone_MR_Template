using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppAPI : Singleton<AppAPI>
{
    private readonly string _url = "http://ozeco.duckdns.org:3000/api/data";
    // IEnumerator PostData(string jsonData)
    // {
    //     UnityWebRequest www = UnityWebRequest.Post("http://ozeco.duckdns.org:3000/api/data", jsonData);
    //     www.SetRequestHeader("Content-Type", "application/json");
    //     byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
    //     www.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //     www.downloadHandler = new DownloadHandlerBuffer();
    //
    //     yield return www.SendWebRequest();
    //
    //     if (www.result != UnityWebRequest.Result.Success)
    //     {
    //         Debug.Log(www.error);
    //     }
    //     else
    //     {
    //         Debug.Log("Form upload complete!");
    //     }
    // }
    //
    // void Start()
    // {
    //     string jsonData = "{\"key\":\"value\"}";
    //     StartCoroutine(PostData(jsonData));
    // }
}