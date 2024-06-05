using RestAPI.DirectionObject;
using System;
using UnityEngine;

public class DirectionAPI : Singleton<DirectionAPI>
{
    private string _url;
    // API 요청 인스턴스
    public readonly WebRequest Req = new();


    private new void Awake()
    {
        base.Awake();
        // URL 설정
        _url = "https://apis.openapi.sk.com/tmap/routes/pedestrian";
        
        // 인증용 헤더 설정
        if (APIKey.Direction == null)
        {
            throw new Exception("Google API KEY not found");
        }
        Req.AddHeader("appKey", APIKey.Direction);
        Req.AddHeader("accept", "application/json");
        Req.AddHeader("content-type", "application/json");
        Req.AddQuery("version", "1");
    }


    /// <summary>
    /// 경로 검색
    /// </summary>
    /// <param name="origin">출발지 위경도</param>
    /// <param name="dest">목적지 위경도</param>
    /// <param name="callback">요청 후 실행할 콜백 함수 : Response</param>
    public void DirectionTo(Double2Position origin, Double2Position dest, WebRequest.ResponseCallback callback)
    {
        Req.URL = _url;
        
        // PostData 설정
        Request dataObj = new();

        dataObj.startX = origin.lon ;
        dataObj.startY = origin.lat;
        dataObj.endX = dest.lon;
        dataObj.endY = dest.lat;

        string data = JsonUtility.ToJson(dataObj);

        StartCoroutine(Req.WebRequestPost(data, callback));
    }


    /// <summary>
    /// Response 파싱
    /// </summary>
    /// <param name="data"> 변환할 string</param>
    /// <returns>경로 탐색 요청 결과 : Response</returns>
    public Response ParseResponse(string data)
    {
        Response obj = JsonUtility.FromJson<Response>(data);
        if (obj == null)
        {
            Debug.LogWarning($"ERROR: JSON Parsing to Direction Response Failed \n{data}");
        }

        return obj;
    }
    
    
    // <summary>
    /// 디버그 출력용 함수
    /// </summary>
    public void DebugFunc(string res)
    {
        Debug.Log(res);
    }
}
