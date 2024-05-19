using System;
using System.Collections.Generic;
using UnityEngine;
using RestAPI.KakaoObject;


public class KakaoAPI : Singleton<KakaoAPI>
{
    [Serializable]
    public enum ReqType
    {
        // 검색 요청 타입
        Address,
        Keyword,
        Category,
    }
    
    // API url들
    private Dictionary<ReqType, string> urls = new();
    private readonly WebRequest _req = new();
    

    private new void Awake()
    {
        base.Awake();
        // URL 설정
        urls[ReqType.Address] = "https://dapi.kakao.com/v2/local/search/address.json";
        urls[ReqType.Category] = "https://dapi.kakao.com/v2/local/search/category.json";
        urls[ReqType.Keyword] = "https://dapi.kakao.com/v2/local/search/keyword.json";
        
        // 인증용 헤더 설정
        if (APIKey.Kakao == null)
        {
            throw new Exception("KAKAO API KEY not found");
        }
        _req.AddHeader("Authorization", $"KakaoAK {APIKey.Kakao}");
    }
    
    
    /// <summary>
    /// 키워드 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void SearchByKeyword(WebRequest.ResponseCallback callback)
    {
        _req.URL = urls[ReqType.Keyword];
        StartCoroutine(_req.WebRequestGet(callback));
    }
    

    /// <summary>
    /// 카테고리 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void SearchByCategory(WebRequest.ResponseCallback callback)
    {
        _req.URL = urls[ReqType.Category];
        StartCoroutine(_req.WebRequestGet(callback));
    }

    
    /// <summary>
    /// 주소 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Address</param>
    public void SearchAddress(WebRequest.ResponseCallback callback)
    {
        _req.URL = urls[ReqType.Address];
        StartCoroutine(_req.WebRequestGet(callback));
    }

    /// <summary>
    /// Place 파싱
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Response<Place> ParsePlace(string data)
    {
        Response<Place> obj = JsonUtility.FromJson<Response<Place>>(data);
        if (obj == null)
        {
            Debug.LogWarning($"ERROR: JSON Parsing to Place Failed \n{data}");
        }

        return obj;
    }
    
    
    /// <summary>
    /// Address 파싱
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Response<Address> ParseAddress(string data)
    {
        Response<Address> obj = JsonUtility.FromJson<Response<Address>>(data);
        if (obj == null)
        {
            Debug.LogWarning($"ERROR: JSON Parsing to Address Failed \n{data}");
        }

        return obj;
    }
    
    
    /// 결과 출력용 테스트 콜백함수
    private void DebugResult(string data)
    {
        Debug.Log(data);
    }

}
