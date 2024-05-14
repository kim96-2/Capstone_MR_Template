using System.Collections;
using System.Collections.Generic;
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
        Headers.Add("Authorization", $"KakaoAK {auth}");
        
        
    }

    
    /// <summary>
    /// 키워드 검색 기능
    /// </summary>
    /// <remarks>쿼리 설정 후 실행 필요</remarks>
    /// <returns>키워드 검색 결과값 반환</returns>
    public List<Place> SearchByKeyword()
    {
        URLType = ReqType.Keyword;

        Response<Place> res = GetRequest<Place>();

        return res.documents;
    }

    public List<Place> SearchByCategory()
    {
        
    }

    public List<Address> SearchAddress()
    {
        
    }
}
