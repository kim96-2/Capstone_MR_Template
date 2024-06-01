using RestAPI.DirectionObject;
using System;
using System.Collections.Generic;
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
        Req.AddHeader("Accept", "application/json");
        Req.AddQuery("version", "1");
    }


    /// <summary>
    /// 경로 검색
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="dest"></param>
    /// <param name="callback"></param>
    public void DirectionTo(Double2Position origin, Double2Position dest, WebRequest.ResponseCallback callback)
    {
        Req.URL = _url;
        
        // PostData 설정
        Request dataObj = new();
        dataObj.startX = origin.x;
        dataObj.startY = origin.y;
        dataObj.endX = dest.x;
        dataObj.endY = dest.y;

        string data = JsonUtility.ToJson(dataObj);

        StartCoroutine(Req.WebRequestPost(data, callback));
    }
    
    
    // <summary>
    /// 디버그 출력용 함수
    /// </summary>
    public void DebugFunc(string res)
    {
        Debug.Log(res);
    }
}
