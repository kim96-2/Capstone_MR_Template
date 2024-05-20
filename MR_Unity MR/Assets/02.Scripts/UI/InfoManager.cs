using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore;
using UnityEngine.UI;

// UI에 띄울 정보 관리
public class InfoManager : Singleton<InfoManager>
{

    [SerializeField] private string recentSearch;

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

        // 쿼리 초기화
        KakaoAPI.Instance.ClearQuery();

        InformationUI.Instance.setPage(1f);
        InformationUI.Instance.changePlace(true);

        KakaoAPI.Instance.AddQuery("query", recentSearch);
        KakaoAPI.Instance.AddQuery("x", TestGPS.Instance.x);
        KakaoAPI.Instance.AddQuery("y", TestGPS.Instance.y);
        KakaoAPI.Instance.AddQuery("radius", TestGPS.Instance.radious);
        KakaoAPI.Instance.AddQuery("page", "1");

        KakaoAPI.Instance.SearchByKeyword(getResult<Place>);

    }

    public void changePage(string pageNum)
    {

        Debug.Log(pageNum);

        KakaoAPI.Instance.ClearQuery();
        KakaoAPI.Instance.AddQuery("query", recentSearch);
        KakaoAPI.Instance.AddQuery("x", TestGPS.Instance.x);
        KakaoAPI.Instance.AddQuery("y", TestGPS.Instance.y);
        KakaoAPI.Instance.AddQuery("radius", TestGPS.Instance.radious);
        KakaoAPI.Instance.AddQuery("page", pageNum);

        KakaoAPI.Instance.SearchByKeyword(getResult<Place>);

    }

    void getResult<T>(string result)
    {

        // 주변 정보를 가져옴
        Response<Place> response = JsonUtility.FromJson<Response<Place>>(result);

        InformationUI.Instance.ClearInfo();

        foreach (Place place in response.documents) InformationUI.Instance.AddInfo(place);

    }

}
