using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IWebRequest
{
    public delegate void ResponseCallback(string result);
    
    public IEnumerator WebRequestGet(Dictionary<string, string> headers, string url, ResponseCallback callback)
    {
        //웹 리퀘스트 설정
        using (var req = UnityWebRequest.Get(url))
        {
            foreach (var header in headers)
            {
                req.SetRequestHeader(header.Key, header.Value);
            }
            
            yield return req.SendWebRequest();

            // 요청 결과 콜백
            if (req.error == null)
            {
                callback(req.downloadHandler.text);
            }
            else
                Debug.LogWarning("ERROR: API Request Failed");
        }
    }

    public IEnumerator WebRequestPost(Dictionary<string, string> headers, string url, string postData,
        ResponseCallback callback)
    {
        //웹 리퀘스트 설정
        using (var req = UnityWebRequest.PostWwwForm(url, postData))
        {
            foreach (var header in headers)
            {
                req.SetRequestHeader(header.Key, header.Value);
            }
            
            yield return req.SendWebRequest();

            // 요청 결과 콜백
            if (req.error == null)
            {
                callback(req.downloadHandler.text);
            }
            else
                Debug.LogWarning("ERROR: API Request Failed");
        }
    }
}