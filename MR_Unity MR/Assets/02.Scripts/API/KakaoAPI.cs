using System;
using System.Collections.Generic;
using UnityEngine;
using RestAPI.KakaoObject;


public class KakaoAPI : Singleton<KakaoAPI>
{
    // 검색 요청 타입
    [Serializable]
    public enum ReqType
    {
        Address,
        Keyword,
        Category,
    }
    
    // API url들
    private readonly Dictionary<ReqType, string> urls = new();
    // API 요청 인스턴스
    public readonly WebRequest Req = new();
    

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
        Req.AddHeader("Authorization", $"KakaoAK {APIKey.Kakao}");
    }


    /// <summary>
    /// 키워드 검색
    /// </summary>
    /// <param name="keyword">검색할 키워드 : string</param>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void SearchByKeyword(string keyword, WebRequest.ResponseCallback callback)
    {
        Req.URL = urls[ReqType.Keyword];
        Req.AddQuery("query", keyword);
        
        StartCoroutine(Req.WebRequestGet(callback));
    }


    /// <summary>
    /// 카테고리 검색
    /// </summary>
    /// <param name="categoryCode">카테고리 코드</param>
    /// <param name="latitude">위도 Y</param>
    /// <param name="longitude">경도 X</param>
    /// <param name="radius">반경 : 미터</param>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Place</param>
    public void SearchByCategory(string categoryCode, double latitude, double longitude, int radius, WebRequest.ResponseCallback callback)
    {
        Req.URL = urls[ReqType.Category];
        Req.AddQuery("category_group_code", categoryCode);
        Req.AddQuery("x", longitude.ToString());
        Req.AddQuery("y", latitude.ToString());
        Req.AddQuery("radius", radius.ToString());
        
        StartCoroutine(Req.WebRequestGet(callback));
    }


    /// <summary>
    /// 주소 검색
    /// </summary>
    /// <param name="query">검색할 키워드</param>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Address</param>
    public void SearchAddress(string query, WebRequest.ResponseCallback callback)
    {
        Req.URL = urls[ReqType.Address];
        Req.AddQuery("query", query);
        
        StartCoroutine(Req.WebRequestGet(callback));
    }

    /// <summary>
    /// Place 파싱
    /// </summary>
    /// <param name="data">변환할 string</param>
    /// <returns>장소 검색 요청 결과 : Response{Place}</returns>
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
    /// <param name="data">변환할 string</param>
    /// <returns>주소 검색 요청 결과 : Response{Address}</returns>
    public Response<Address> ParseAddress(string data)
    {
        Response<Address> obj = JsonUtility.FromJson<Response<Address>>(data);
        if (obj == null)
        {
            Debug.LogWarning($"ERROR: JSON Parsing to Address Failed \n{data}");
        }

        return obj;
    }
    
    
    /// <summary>
    /// 디버그 출력용 함수
    /// </summary>
    public void DebugResult(string data)
    {
        Debug.Log(data);
    }
}
