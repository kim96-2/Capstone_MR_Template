using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public string kakaoUrl;
    public string auth;

    private void Awake()
    {
        string headerName = "Authorization";
        string headerValue = $"KakaoAK {auth}";
        
        StartCoroutine(WebRequestGet(headerName, headerValue, kakaoUrl));
    }

    private IEnumerator WebRequestGet(string headerName, string headerValue, string url)
    {
        using (var req = UnityWebRequest.Get(url))
        {
            req.SetRequestHeader( headerName, headerValue);

            yield return req.SendWebRequest();

            if (req.error == null)
                Debug.Log(req.downloadHandler.text);
            else
                Debug.LogWarning("ERROR");
        }
    }
}