using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KakaoAPI : APIManager
{
    private new void Awake()
    {
        base.Awake();
        // URL 설정
        urls[ReqType.Address] = "https://dapi.kakao.com/v2/local/search/address.json";
        urls[ReqType.Category] = "https://dapi.kakao.com/v2/local/search/category.json";
        urls[ReqType.Keyword] = "https://dapi.kakao.com/v2/local/search/keyword.json";
        
        // 인증용 헤더 설정
        headers.Add("Authorization", $"KakaoAK {auth}");

        Request<Place>();

    }
}
