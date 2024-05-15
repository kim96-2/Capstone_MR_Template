using System;
using UnityEngine;

public class KakaoAPI : APIManager
{
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
}
