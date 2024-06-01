using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RestAPI.KakaoObject;
using TMPro;
using Unity.Collections;

// 주변 정보를 띄우는 UI
public class InformationUI : Singleton<InformationUI>
{

    [SerializeField] private GameObject scrollPlace;
    [SerializeField] private GameObject moreInfoPlace;

    [SerializeField] private RectTransform contentTransform;

    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private GameObject placePointPrefab;
    [SerializeField] private Scrollbar scroll;

    [SerializeField] private List<GameObject> content = new();
    [SerializeField] private List<GameObject> placePoint = new();

    [Space(15f)]
    private bool scrollDown;
    [SerializeField] private float page;
    public float getPage() { return page; }
    public void setPage(float page) { this.page = page;}

    [ReadOnly,SerializeField] private string recentSearch;

    [SerializeField] int radius = 200;

    private Double2Position geoPos;

    //가장 최근에 검색한 세부정보
    //길찾기 상태로 들어갈 때 위 값을 참조하여 길찾기를 진행하면 된다
    public Place lastMoreInfoPlace;

    public void setContentTransformSize(float _size) { contentTransform.sizeDelta = new Vector2(0f, _size); }

    void Start()
    {
        scrollDown = true;
        changePlace(true);
        page = 1;
    }

    #region Info Setting

    // 정보 추가
    public void AddInfo(Place place)
    {

        // Search UI 추가
        GameObject contents = Instantiate(contentPrefab, contentTransform);
        content.Add(contents);
        contents.GetComponent<Contents>().changeContents(place);

        // 화면 상의 UI 추가
        GameObject placepoints = Instantiate(placePointPrefab);
        placePoint.Add(placepoints);

        //Debug.Log("생성 위치 : " + place.y + " " + place.x);

        placepoints.GetComponent<GeoTransform>().Init(place.y, place.x);
        placepoints.GetComponent<Contents>().changeContents(place);

    }

    // 정보들 초기화
    public void ClearInfo()
    {
        
        for(int i = content.Count - 1; i >= 0; i--) Destroy(content[i]);
        for(int i = placePoint.Count - 1; i >= 0; i--) Destroy(placePoint[i]);

        content.Clear();
        placePoint.Clear();

        // UI관련 초기화
        scroll.value = 1f;

    }

    // 세부 정보 표시하는 화면으로 변경 
    public void MoreInfo(Place place)
    {
        lastMoreInfoPlace = place;//가장 최근에 검색한 길찾기 용 세부정부 내용 업데이트

        changePlace(false);

        moreInfoPlace.GetComponent<Contents>().changeContents(place);

    }

    #endregion Info Setting

    // 스크롤 끝으로 갈 시 페이지 변경
    public void EndDrag()
    {
        if (scroll.value < 0f && scrollDown)
        {

            scrollDown = false;

            StartCoroutine("ChangePageUp");

        }
        else if (scroll.value > 1.1f && scrollDown)
        {

            scrollDown = false;

            StartCoroutine("ChangePageDown");

        }
    }

    

    // 지도 목록하고 상세정보 변환 true는 검색 목록 fasle는 상세정보
    public void changePlace(bool toggle)
    {
        scrollPlace.SetActive(toggle);
        moreInfoPlace.SetActive(!toggle);
    }

    public void clickCategory()
    {

        GameObject clickObject = EventSystem.current.currentSelectedGameObject.GetComponent<Transform>().GetChild(0).gameObject;

        recentSearch = clickObject.GetComponent<TextMeshProUGUI>().text;

        geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(Camera.main.transform);

        // 쿼리 초기화
        KakaoAPI.Instance.Req.ClearQuery();

        setPage(1f);
        changePlace(true);

        //Debug.Log(geoPos.x + " " + geoPos.y);

        //geoPos.y가 경도 geoPos.x 가 위도 인것을 꼭 확인하기(나중에 코드에서 확인가능하게 변경할 것) => 수정 완료
        KakaoAPI.Instance.Req.AddQuery("x", geoPos.lan.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.lat.ToString());


        KakaoAPI.Instance.Req.AddQuery("radius", radius.ToString());
        KakaoAPI.Instance.Req.AddQuery("page", "1");

        KakaoAPI.Instance.SearchByKeyword(recentSearch, getResult);

    }

    public void changePage(string pageNum)
    {

        KakaoAPI.Instance.Req.ClearQuery();
        KakaoAPI.Instance.Req.AddQuery("x", geoPos.lan.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.lat.ToString());
        KakaoAPI.Instance.Req.AddQuery("radius", radius.ToString());
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
        ClearInfo();

        //받아온 정보들 추가
        foreach (Place place in response.documents) AddInfo(place);

        // 정보 개수에 맞게 스크롤 조정
        setContentTransformSize((response.documents.Count * 100f) + 50f);

    }

    #region Page Change Setting

    // 스크롤 맨 위로 올릴 때
    IEnumerator ChangePageUp()
    {

        page++;
        changePage(page.ToString());
        //InfoManager.Instance.changePage(page.ToString());

        yield return new WaitForSeconds(0.5f);

        scrollDown = true;

        yield return null;
    }

    // 스크롤 맨 아래로 내릴 때
    IEnumerator ChangePageDown()
    {

        if (page > 1f)
        {
            page--;
            changePage(page.ToString());
            //InfoManager.Instance.changePage(page.ToString());
        }

        yield return new WaitForSeconds(0.5f);

        scrollDown = true;

        yield return null;
    }

    #endregion Page Change Setting

}
