using System;
using System.Collections.Generic;
using UnityEngine;
using RestAPI.GoogleObject;


public class GoogleAPI : Singleton<GoogleAPI>
{
    // 검색 요청 타입
    [Serializable]
    public enum ReqType
    {
        
        Nearby,
        Place,
        Image,
    }
    
    // API url들
    private readonly Dictionary<ReqType, string> _urls = new();
    // API 요청 인스턴스
    public readonly WebRequest Req = new();
    

    private new void Awake()
    {
        base.Awake();
        // URL 설정
        _urls[ReqType.Nearby] = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";
        _urls[ReqType.Place] = "https://maps.googleapis.com/maps/api/place/details/json";
        _urls[ReqType.Image] = "https://maps.googleapis.com/maps/api/place/photo";
        
        // 인증용 헤더 설정
        if (APIKey.Google == null)
        {
            throw new Exception("Google API KEY not found");
        }
        Req.AddQuery("key", APIKey.Google);
    }
    
    
    /// <summary>
    /// 주변 검색
    /// </summary>
    /// <param name="latitude"> 위도 Y</param>
    /// <param name="longitude">경도 X</param>
    /// <param name="radius">반경 : 미터</param>
    /// <param name="callback">콜백 함수 : string</param>
    public void SearchNearby(double latitude, double longitude, int radius, WebRequest.ResponseCallback callback)
    {
        Req.URL = _urls[ReqType.Nearby];
        
        // 헤더 설정
        Req.AddQuery("location", $"{latitude}%2C{longitude}");
        Req.AddQuery("radius", $"{radius}");
        StartCoroutine(Req.WebRequestGet(callback));
    }
    

    /// <summary>
    /// 카테고리 검색
    /// </summary>
    /// <param name="placeId">상세 정보 검색할 장소의 ID</param>
    /// <param name="callback">요청 후 실행할 콜백 함수 : string</param>
    public void PlaceDetail(string placeId, WebRequest.ResponseCallback callback)
    {
        Req.URL = _urls[ReqType.Place];
        
        // 쿼리 설정
        Req.AddQuery("place_id", placeId);
        StartCoroutine(Req.WebRequestGet(callback));
    }


    /// <summary>
    /// 장소 사진 호출
    /// </summary>
    /// <param name="reference">사진 레퍼런스 코드</param>
    /// <param name="height">사진 최대 높이 : pixel</param>
    /// <param name="width">사진 최대 폭 : pixel</param>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Texture2D</param>
    public void PlacePhoto(string reference, int height, int width, WebRequest.ResponseImageCallback callback)
    {
        Req.URL = _urls[ReqType.Image];
        
        // 헤더 설정
        Req.AddQuery("photo_reference", reference);
        Req.AddQuery("maxheight", height.ToString());
        Req.AddQuery("maxwidth", width.ToString());

        StartCoroutine(Req.WebRequestImageGet(callback));
    }


    /// <summary>
    /// 주변 정보 파싱
    /// </summary>
    /// <param name="data">변환할 string</param>
    /// <returns>주변 정보 요청 결과 : NearbyResponse</returns>
    public NearbyResponse ParseNearbyRes(string data)
    {
        NearbyResponse obj = JsonUtility.FromJson<NearbyResponse>(data);
        if (obj == null)
        {
            Debug.LogWarning($"ERROR: JSON Parsing to NearbyResponse Failed \n{data}");
        }

        return obj;
    }
    
    
    /// <summary>
    /// 장소 세부 정보 파싱
    /// </summary>
    /// <param name="data">변환할 string </param>
    /// <returns>장소 세부 정보 요청 결과 : PlacesDetailsResponse</returns>
    public PlacesDetailsResponse ParseDetailRes(string data)
    {
        PlacesDetailsResponse obj = JsonUtility.FromJson<PlacesDetailsResponse>(data);
        if (obj == null)
        {
            Debug.LogWarning($"ERROR: JSON Parsing to PlaceDetails Failed \n{data}");
        }

        return obj;
    }


    /// <summary>
    /// 디버그 출력용 함수
    /// </summary>
    public void DebugFunc(string res)
    {
        Debug.Log(res);
    }
}
