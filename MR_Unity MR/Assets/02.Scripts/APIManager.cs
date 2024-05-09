using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public string auth;
    public string kakaoUrl;
    public List<string> querys;

    private void Awake()
    {
        string headerName = "Authorization";
        string headerValue = $"KakaoAK {auth}";
        
        StartCoroutine(WebRequestGet(headerName, headerValue, kakaoUrl));
    }

    private IEnumerator WebRequestGet(string headerName, string headerValue, string url)
    {
        // 쿼리 설정
        for (int i = 0; i < querys.Count; i++)
        {
            url += (i == 0) ? "?" : "&";
            url += querys[i];
        }
        
        //웹 리퀘스트 설정
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