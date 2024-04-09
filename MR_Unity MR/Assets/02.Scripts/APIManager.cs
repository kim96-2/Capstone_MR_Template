using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public string url;
    public string auth;

    private void Awake()
    {
        StartCoroutine(WebRequestGet());
    }

    private IEnumerator WebRequestGet()
    {
        using (var req = UnityWebRequest.Get(url))
        {
            req.SetRequestHeader("Authorization", $"KakaoAK {auth}");

            yield return req.SendWebRequest();

            if (req.error == null)
                Debug.Log(req.downloadHandler.text);
            else
                Debug.LogWarning("ERROR");
        }
    }
}