using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore;
using UnityEngine.UI;
using RestAPI.KakaoObject;

// UI에 띄울 정보 관리
public class InfoManager : Singleton<InfoManager>
{

    [SerializeField] private string recentSearch;

    private Double2Position geoPos;

    // Start is called before the first frame update
    void Start()
    {
        // 테스트용 위치-
        /*
        KakaoAPI.Instance.AddQuery("category_group_code", "CS2");
        KakaoAPI.Instance.AddQuery("x", TestGPS.Instance.x);
        KakaoAPI.Instance.AddQuery("y", TestGPS.Instance.y);
        KakaoAPI.Instance.AddQuery("radius", TestGPS.Instance.radious);

        KakaoAPI.Instance.SearchByCategory(getResult<Place>);
        */

       

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clickCategory()
    {

        GameObject clickObject = EventSystem.current.currentSelectedGameObject.GetComponent<Transform>().GetChild(0).gameObject;

        recentSearch = clickObject.GetComponent<TextMeshProUGUI>().text;

        geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(Camera.main.transform);

        // 쿼리 초기화
        KakaoAPI.Instance.Req.ClearQuery();

        InformationUI.Instance.setPage(1f);
        InformationUI.Instance.changePlace(true);

        //Debug.Log(geoPos.x + " " + geoPos.y);

        //geoPos.y가 경도 geoPos.x 가 위도 인것을 꼭 확인하기(나중에 코드에서 확인가능하게 변경할 것)
        KakaoAPI.Instance.Req.AddQuery("x", geoPos.lan.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.lat.ToString());


        KakaoAPI.Instance.Req.AddQuery("radius", TestGPS.Instance.radious);
        KakaoAPI.Instance.Req.AddQuery("page", "1");

        KakaoAPI.Instance.SearchByKeyword(recentSearch, getResult);

    }

    public void changePage(string pageNum)
    {

        KakaoAPI.Instance.Req.ClearQuery();
        KakaoAPI.Instance.Req.AddQuery("x", geoPos.lan.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.lat.ToString());
        KakaoAPI.Instance.Req.AddQuery("radius", TestGPS.Instance.radious);
        KakaoAPI.Instance.Req.AddQuery("page", pageNum);

        KakaoAPI.Instance.SearchByKeyword(recentSearch, getResult);

    }

    void getResult(string result)
    {

        // 주변 정보를 가져옴
        Response<Place> response = JsonUtility.FromJson<Response<Place>>(result);

        //주변 정보를 토대로 미니맵에 배치
        //탐색 시 실제 플레이어 위치로 할 수 있지만 조금 더 정확한 위치를 위해 Request 받았을 때의 플레이어 위치를 기준으로 미니맵 배치
        MiniMapManager.Instance.SetSearchMap((float)geoPos.lat, (float)geoPos.lan, response.documents);

        //이건 위와 다르게 플레이어 위치를 기준으로 미니맵 배치하는 함수
        //MiniMapManager.Instance.SetSearchMap(response.documents);

        //기존에 사용하던 정보들 초기화
        InformationUI.Instance.ClearInfo();

        //받아온 정보들 추가
        foreach (Place place in response.documents) InformationUI.Instance.AddInfo(place);

        // 정보 개수에 맞게 스크롤 조정
        InformationUI.Instance.setContentTransformSize((response.documents.Count * 100f) + 50f);

    }

}
