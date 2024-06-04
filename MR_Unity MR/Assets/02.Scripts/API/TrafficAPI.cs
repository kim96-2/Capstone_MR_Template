using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficAPI : Singleton<TrafficAPI>
{
    // API URL
    private readonly string _url =
        $"http://swopenAPI.seoul.go.kr/api/subway/{APIKey.Subway}/json/realtimeStationArrival/0/10/";
    
    // API 요청 인스턴스
    public readonly WebRequest Req = new();


    private new void Awake()
    {
        base.Awake();
    }


    /// <summary>
    /// 지하쳘 역명으로 실시간 교통정보
    /// </summary>
    /// <param name="stnName"></param>
    /// <param name="callback"></param>
    public void StationTraffic(string stnName, WebRequest.ResponseCallback callback)
    {
        Req.URL = _url + stnName;

        StartCoroutine(Req.WebRequestGet(callback));
    }
}
