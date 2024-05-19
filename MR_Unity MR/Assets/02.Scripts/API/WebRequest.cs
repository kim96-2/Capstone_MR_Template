using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest
{
    // 리퀘스트를 위한 정보
    public string URL;
    private Dictionary<string, string> _headers = new();
    private Dictionary<string, string> _querys = new();
    
    
    public delegate void ResponseCallback(string result);
    
    
    /// <summary>
    /// 헤더 데이터 추가
    /// </summary>
    /// <param name="header">헤더 키</param>
    /// <param name="value">헤더 값</param>
    public void AddHeader(string header, string value)
    {
        _headers.Add(header, value);
    }


    /// <summary>
    /// 헤더 초기화
    /// </summary>
    public void ClearHeader()
    {
        _headers.Clear();
    }
    
    /// <summary>
    /// 쿼리 데이터 추가
    /// </summary>
    /// <param name="query">쿼리 키</param>
    /// <param name="value">쿼리 값</param>
    public void AddQuery(string query, string value)
    {
        _querys.Add(query, value);
    }


    /// <summary>
    /// 쿼리 초기화
    /// </summary>
    public void ClearQuery()
    {
        _querys.Clear();
    }
    
    
    private string SetURL()
    {
        string result = URL;

        // 쿼리 없음
        if (_querys.Count <= 0)
        {
            return result;
        }

        // 쿼리 추가
        result += "?";
        foreach (var query in _querys)
        {
            result += $"{query.Key}={query.Value}&";
        }

        return result;
    }
    
    
    public IEnumerator WebRequestGet(ResponseCallback callback)
    {
        //웹 리퀘스트 설정
        using (var req = UnityWebRequest.Get(SetURL()))
        {
            foreach (var header in _headers)
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

    public IEnumerator WebRequestPost(string postData,
        ResponseCallback callback)
    {
        //웹 리퀘스트 설정
        using (var req = UnityWebRequest.PostWwwForm(SetURL(), postData))
        {
            foreach (var header in _headers)
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