using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.Serialization;


[System.Serializable]
public enum ReqType
{
    // 검색 요청 타입
    Address,
    Keyword,
    Category,
}


public class APIManager : Singleton<APIManager>
{
    // API url들
    protected Dictionary<ReqType, string> URLs = new();
    
    // 리퀘스트를 위한 정보
    protected ReqType URLType;
    private List<string> _querys = new();
    protected Dictionary<string, string> Headers = new();

    // 요청 반환 타입
    public delegate void ResponseCallback<T>(string result);
    
    
    // GET 요청 처리 코루틴
    protected IEnumerator WebRequestGet<T>(ResponseCallback<T> callback)
    {
        // 요청에 해당하는 url
        if (!URLs.TryGetValue(URLType, out string url))
        {
            yield break;
        }
        
        // 쿼리 설정
        url += "?";
        foreach (var query in _querys)
        {
            url += $"{query}&";
        }
        
        //웹 리퀘스트 설정
        using (var req = UnityWebRequest.Get(url))
        {
            foreach (var header in Headers)
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
    
    
    // TODO:POST 요청 처리 코루틴
    private IEnumerator WebRequestPost<T>(string data, ResponseCallback<T> response)
    {
        // 요청에 해당하는 url
        if (!URLs.TryGetValue(URLType, out string url))
        {
            yield break;
        }
        
        // TODO: Query Dictionary로 수정
        // 쿼리 설정
        url += "?";
        foreach (var query in _querys)
        {
            url += $"{query}&";
        }
        
        //웹 리퀘스트 설정
        using (var req = UnityWebRequest.Get(url))
        {
            foreach (var header in Headers)
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
    
    
    /// <summary>
    /// 쿼리 데이터 추가
    /// </summary>
    /// <param name="queryHeader">쿼리 키</param>
    /// <param name="queryValue">쿼리 값</param>
    public void AddQuery(string queryHeader, string queryValue)
    {
        _querys.Add($"{queryHeader}={queryValue}");
    }


    /// <summary>
    /// 쿼리 초기화
    /// </summary>
    public void ClearQuery()
    {
        _querys.Clear();
    }
}