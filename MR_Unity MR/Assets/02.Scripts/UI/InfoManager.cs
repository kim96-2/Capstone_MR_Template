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

        geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(Camera.main.transform);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clickCategory()
    {

        GameObject clickObject = EventSystem.current.currentSelectedGameObject.GetComponent<Transform>().GetChild(0).gameObject;

        recentSearch = clickObject.GetComponent<TextMeshProUGUI>().text;

        // 쿼리 초기화
        KakaoAPI.Instance.Req.ClearQuery();

        InformationUI.Instance.setPage(1f);
        InformationUI.Instance.changePlace(true);

        KakaoAPI.Instance.Req.AddQuery("x", geoPos.x.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.y.ToString());
        KakaoAPI.Instance.Req.AddQuery("radius", TestGPS.Instance.radious);
        KakaoAPI.Instance.Req.AddQuery("page", "1");

        KakaoAPI.Instance.SearchByKeyword(recentSearch, getResult);

    }

    public void changePage(string pageNum)
    {

        KakaoAPI.Instance.Req.ClearQuery();
        KakaoAPI.Instance.Req.AddQuery("x", geoPos.x.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.y.ToString());
        KakaoAPI.Instance.Req.AddQuery("radius", TestGPS.Instance.radious);
        KakaoAPI.Instance.Req.AddQuery("page", pageNum);

        KakaoAPI.Instance.SearchByKeyword(recentSearch, getResult);

    }

    void getResult(string result)
    {

        // 주변 정보를 가져옴
        Response<Place> response = JsonUtility.FromJson<Response<Place>>(result);

        InformationUI.Instance.ClearInfo();

        foreach (Place place in response.documents) InformationUI.Instance.AddInfo(place);

    }

}
