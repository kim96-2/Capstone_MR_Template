using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class KakaoAPI : Singleton<KakaoAPI>
{
    // API url들
    protected Dictionary<ReqType, string> URLs = new();
    
    // 리퀘스트를 위한 정보
    protected ReqType URLType;
    private List<string> _querys = new();
    protected Dictionary<string, string> Headers = new();

    // 요청 반환 타입
    public delegate void ResponseCallback<T>(string result);

    private new void Awake()
    {
        base.Awake();
        // URL 설정
        URLs[ReqType.Address] = "https://dapi.kakao.com/v2/local/search/address.json";
        URLs[ReqType.Category] = "https://dapi.kakao.com/v2/local/search/category.json";
        URLs[ReqType.Keyword] = "https://dapi.kakao.com/v2/local/search/keyword.json";
        
        // 인증용 헤더 설정
        if (APIKey.Kakao == null)
        {
            throw new Exception("KAKAO API KEY not found");
        }
        Headers.Add("Authorization", $"KakaoAK {APIKey.Kakao}");
    }

    
    /// <summary>
    /// 키워드 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void SearchByKeyword(ResponseCallback<Place> callback)
    {
        URLType = ReqType.Keyword;
        StartCoroutine(WebRequestGet(callback));
    }
    

    /// <summary>
    /// 카테고리 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void SearchByCategory(ResponseCallback<Place> callback)
    {
        URLType = ReqType.Category;
        StartCoroutine(WebRequestGet(callback));
    }

    
    /// <summary>
    /// 주소 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Address</param>
    public void SearchAddress(ResponseCallback<Address> callback)
    {
        URLType = ReqType.Address;
        StartCoroutine(WebRequestGet(callback));
    }

    
    /// 결과 출력용 테스트 콜백함수
    private void DebugResult<T>(string data)
    {
        Response<T> result = JsonUtility.FromJson<Response<T>>(data);
        Debug.Log(result.documents);
        Debug.Log(result.meta);
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
    
    
    // GET 요청 처리 코루틴
    private IEnumerator WebRequestGet<T>(ResponseCallback<T> callback)
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
}
