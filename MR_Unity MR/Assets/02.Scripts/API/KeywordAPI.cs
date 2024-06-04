using RestAPI.KeywordObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordAPI : Singleton<KeywordAPI>
{
    private readonly string url = "http://ozeco.duckdns.org:3000/api/data";

    public readonly WebRequest Req = new();


    /// <summary>
    /// 키워드 데이터 불러오기
    /// </summary>
    /// <param name="callback">키워드 요청 결과 : Keyword</param>
    public void GetKeyword(WebRequest.ResponseCallback callback)
    {
        Req.URL = url;

        StartCoroutine(Req.WebRequestGet(callback));
    }


    /// <summary>
    /// 키워드 파싱
    /// </summary>
    /// <param name="jsonData">변환할 string</param>
    /// <returns>키워드 결과</returns>
    public string ParseKeyword(string jsonData)
    {
        Keyword keywordData = JsonUtility.FromJson<Keyword>(jsonData);

        return keywordData.keyword;
    }
} 
