using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
    // API를 위한 키
    public string auth;
    protected Dictionary<ReqType, string> URLs = new();
    
    // 리퀘스트를 위한 정보
    protected ReqType URLType;
    [SerializeField] protected List<string> querys = new();
    protected Dictionary<string, string> Headers = new();
    private string _result;


    /// <summary>
    /// 쿼리 데이터 추가
    /// </summary>
    /// <param name="queryHeader">쿼리 키</param>
    /// <param name="queryValue">쿼리 값</param>
    public void AddQuery(string queryHeader, string queryValue)
    {
        querys.Add($"{queryHeader}={queryValue}");
    }


    /// <summary>
    /// 쿼리 초기화
    /// </summary>
    public void ClearQuery()
    {
        querys.Clear();
    }
    
    
    /// <summary>
    /// Get 리퀘스트 요청
    /// </summary>
    /// <typeparam name="T">리스폰스 타입</typeparam>
    /// <returns>리스폰스 반환</returns>
    protected Response<T> GetRequest<T>()
    {
        Response<T> res = new();
        
        StartCoroutine(WebRequestGet<T>());
        
        res = JsonUtility.FromJson<Response<T>>(_result);
        return res;
    }
    
    
    /// <summary>
    /// Post 리퀘스트 요청
    /// </summary>
    /// <typeparam name="T">리스폰스 타입</typeparam>
    /// <returns>리스폰스 반환</returns>
    protected Response<T> PostRequest<T>()
    {
        Response<T> res = new();
        
        StartCoroutine(WebRequestPost<T>(res));

        return res;
    }

    
    // GET 요청 처리 코루틴
    private IEnumerator WebRequestGet<T>()
    {
        // 요청에 해당하는 url
        if (!URLs.TryGetValue(URLType, out string url))
        {
            yield break;
        }
        
        // 쿼리 설정
        url += "?";
        foreach (var query in querys)
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
            {
                _result = req.downloadHandler.text;
            }
            else
                Debug.LogWarning("ERROR: API Request Failed");
        }
    }
    
    
    // TODO:POST 요청 처리 코루틴
    private IEnumerator WebRequestPost<T>(Response<T> response)
    {
        // 요청에 해당하는 url
        if (!URLs.TryGetValue(URLType, out string url))
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
}