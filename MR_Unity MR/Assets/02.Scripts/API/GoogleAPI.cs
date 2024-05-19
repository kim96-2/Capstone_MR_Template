using System;
using System.Collections.Generic;
using UnityEngine;
using RestAPI.KakaoObject;


public class GoogleAPI : Singleton<GoogleAPI>
{
    [Serializable]
    public enum ReqType
    {
        // 검색 요청 타입
        Nearby,
        Place,
        Keyword,
    }
    
    // API url들
    private Dictionary<ReqType, string> urls = new();
    private readonly WebRequest _req = new();
    

    private new void Awake()
    {
        base.Awake();
        // URL 설정
        urls[ReqType.Nearby] = "https://places.googleapis.com/v1/places:searchNearby";
        urls[ReqType.Place] = "https://dapi.kakao.com/v2/local/search/category.json";
        urls[ReqType.Keyword] = "https://dapi.kakao.com/v2/local/search/keyword.json";
        
        // 인증용 헤더 설정
        if (APIKey.Google == null)
        {
            throw new Exception("Google API KEY not found");
        }
        _req.AddHeader("Content-Type", "application/json");
        _req.AddHeader("X-Goog-Api-Key", APIKey.Google);
        _req.AddHeader("X-Goog-FieldMask", "*");
    }
    
    
    /// <summary>
    /// 키워드 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void SearchNearby(WebRequest.ResponseCallback callback)
    {
        _req.URL = urls[ReqType.Keyword];
        StartCoroutine(_req.WebRequestGet(callback));
    }
    

    /// <summary>
    /// 카테고리 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void PlaceDetail(WebRequest.ResponseCallback callback)
    {
        _req.URL = urls[ReqType.Place];
        StartCoroutine(_req.WebRequestGet(callback));
    }

    
    /// <summary>
    /// 주소 검색
    /// </summary>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Address</param>
    public void SearchByKeyword(WebRequest.ResponseCallback callback)
    {
        _req.URL = urls[ReqType.Keyword];
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
