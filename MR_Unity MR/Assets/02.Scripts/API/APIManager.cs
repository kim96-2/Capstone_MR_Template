using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public enum ReqType
{
    Address,
    Keyword,
    Category,
}

public class APIManager : Singleton<APIManager>
{
    // API를 위한 키
    public string auth;
    public Dictionary<ReqType, string> urls = new();
    
    // 리퀘스트를 위한 정보
    public ReqType urlType;
    public List<string> querys = new();
    public Dictionary<string, string> headers = new();

    
    // 리퀘스트 요청 함수
    public Response<T> Request<T>()
    {
        Response<T> res = new();
        
        StartCoroutine(WebRequestGet<T>(res));

        return res;
    }

    
    // API 처리 코루틴
    private IEnumerator WebRequestGet<T>(Response<T> response)
    {
        // 요청에 해당하는 url
        if (!urls.TryGetValue(urlType, out string url))
        {
            yield break;
        }
        
        // TODO: Query Dictionary로 수정
        // 쿼리 설정
        url += "?";
        foreach (var query in querys)
        {
            url += $"{query}&";
        }
        
        //웹 리퀘스트 설정
        using (var req = UnityWebRequest.Get(url))
        {
            foreach (var header in headers)
            {
                req.SetRequestHeader(header.Key, header.Value);
            }
            
            yield return req.SendWebRequest();

            if (req.error == null)
                Debug.Log(req.downloadHandler.text);
            else
                Debug.LogWarning("ERROR");
        }
    }
}