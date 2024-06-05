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

public enum SearchType
{
    KeyWord,
    Category
}

// 주변 정보를 띄우는 UI
public class InformationUI : Singleton<InformationUI>
{

    [SerializeField] private GameObject scrollPlace;
    [SerializeField] private GameObject moreInfoPlace;

    [SerializeField] private RectTransform contentTransform;

    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private GameObject placePointPrefab;
    [SerializeField] private Scrollbar scroll;

    [SerializeField] private TextMeshProUGUI searchKeyword;

    [SerializeField] private List<Contents> contents = new();
    [SerializeField] private List<Contents_nonUI> placePoint = new();

    [SerializeField] private SearchType searchType;

    List<Contents_nonUI> placePoints_forSwap = new();

    [Space(15f)]
    private bool scrollDown;
    [SerializeField] private float page;
    public float getPage() { return page; }
    public void setPage(float page) { this.page = page;}

    [ReadOnly,SerializeField] private string recentSearch;
    [SerializeField] private string recentKeyWord;

    [SerializeField] int radius = 200;

    private Double2Position geoPos;

    //가장 최근에 검색한 세부정보
    //길찾기 상태로 들어갈 때 위 값을 참조하여 길찾기를 진행하면 된다
    private Place _lastMoreInfoPlace = null;
    public Place lastMoreInfoPlace { get => _lastMoreInfoPlace; }

    [Header("Point Swap Setting")]
    [SerializeField,Range(0f,90f)] float pointHeigntMaxAngle = 30f;
    [SerializeField, Range(0, 45f)] float eachPointHeightMaxAngel = 5f;
    [SerializeField] float areaCount = 10;

    public void setContentTransformSize(float _size) { contentTransform.sizeDelta = new Vector2(0f, _size); }

    void Start()
    {
        _lastMoreInfoPlace = null;

        scrollDown = true;
        changePlace(true);
        page = 1;
    }

    private void Update()
    {
        SwapPlacePoints();
    }

    #region Place Swap Setting

    List<Contents_nonUI> areaPoints = new();
    /// <summary>
    /// 각 특징 좌표들 높이 가중치 계산
    /// </summary>
    void SetPlacePointsHeightAdditioner()
    {
        //float hfov = Camera.main.fieldOfView * Camera.main.aspect;

        float areaAngle = 360 / areaCount;

        //구역 각도 안에 있는 좌표들 분류 후 구역마다 정렬
        for (int area = 0; area < areaCount; area++)
        {
            areaPoints.Clear();
            for(int i=0;i<placePoints_forSwap.Count;i++)
            {
                if (placePoints_forSwap[i].AngleFromPlayer > -180f + areaAngle * area &&
                    placePoints_forSwap[i].AngleFromPlayer <= -180f + areaAngle * (area + 1))
                {
                    areaPoints.Add(placePoints_forSwap[i]);
                }
            }

            SwapPointsInArea(areaPoints);
        }
    }

    void SwapPointsInArea(List<Contents_nonUI> areaPoints)
    {
        Contents_nonUI temp;

        for (int i = 0; i < areaPoints.Count; i++)
        {
            for(int j = i; j < areaPoints.Count; j++)
            {
                if (areaPoints[i].RangeToPlayer > areaPoints[j].RangeToPlayer)
                {
                    temp = areaPoints[i];
                    areaPoints[i] = areaPoints[j];
                    areaPoints[j] = temp;
                }
            }
        }

        float angle = pointHeigntMaxAngle / areaPoints.Count;
        if (angle > eachPointHeightMaxAngel) angle = eachPointHeightMaxAngel;

        //플레이어와 멀리 있을 만큼 높이 가중치를 크게 부여 (각도로 부여)
        for (int i = 0; i < areaPoints.Count; i++)
        {
            areaPoints[i].HeightAdditioner = angle * (i + 1f);
        }
    }

    void SwapPlacePoints()//실시간으로 정렬하는 함수
    {
        if (lastMoreInfoPlace != null) return;//세부 정보를 확인하고 있다면 실행 X

        Contents_nonUI temp;

        float hfov = Camera.main.fieldOfView * Camera.main.aspect;

        //화면 안에 있는 지점들을 분류
        int placeInScreen = 0;
        for (int i = 0; i < placePoints_forSwap.Count; i++)
        {
            if (Mathf.Abs(placePoints_forSwap[i].AngleFromPlayer) < hfov / 2f)
            {
                temp = placePoints_forSwap[i];
                placePoints_forSwap[i] = placePoints_forSwap[placeInScreen];
                placePoints_forSwap[placeInScreen] = temp;

                placePoints_forSwap[i].ChangePointState(PlacePointState.NORMAL);

                placeInScreen++;
            }
        }

        //화변 밖에 있는 애들 높이 가중치 0으로 적용 -> 비활성화 시켜줌
        for(int i = placeInScreen; i < placePoints_forSwap.Count; i++)
        {
            //placePoints_forSwap[i].HeightAdditioner = 0f;
            placePoints_forSwap[i].ChangePointState(PlacePointState.IGNORE);
        }

        /*
        //화면 안의 특징 지점들을 플레이어 와의 거리 순으로 정렬 시켜줌
        for (int i = 0; i < placeInScreen; i++)
        {
            for (int j = i; j < placeInScreen; j++)
            {
                if (placePoints_forSwap[i].RangeToPlayer > placePoints_forSwap[j].RangeToPlayer)
                {
                    temp = placePoints_forSwap[i];
                    placePoints_forSwap[i] = placePoints_forSwap[j];
                    placePoints_forSwap[j] = temp;
                }
            }
        }

        float angle = pointHeigntMaxAngle / placeInScreen;
        if(angle > eachPointHeightMaxAngel) angle = eachPointHeightMaxAngel;

        //플레이어와 멀리 있을 만큼 높이 가중치를 크게 부여 (각도로 부여)
        for (int i = 0; i < placeInScreen; i++)
        {
            placePoints_forSwap[i].HeightAdditioner = angle * (i + 1f);
        }
        */
    }

    #endregion Place Swap Setting

    #region Info Setting

    // 정보 추가
    public void AddInfo(Place place)
    {

        // Search UI 추가
        Contents content = Instantiate(contentPrefab, contentTransform).GetComponent<Contents>();
        contents.Add(content);
        content.changeContents(place);

        // 화면 상의 UI 추가
        Contents_nonUI placepoints = Instantiate(placePointPrefab).GetComponent<Contents_nonUI>();
        placePoint.Add(placepoints);

        placePoints_forSwap.Add(placepoints);//스왑용 리스트에도 위치 저장

        placepoints.ChangePointState(PlacePointState.NORMAL);

        //Debug.Log("생성 위치 : " + place.y + " " + place.x);

        placepoints.GetComponent<GeoTransform>().Init(place.y, place.x);
        placepoints.changeContents(place);

    }

    // 정보들 초기화
    public void ClearInfo()
    {
        

        for (int i = contents.Count - 1; i >= 0; i--) Destroy(contents[i].gameObject);
        for(int i = placePoint.Count - 1; i >= 0; i--) Destroy(placePoint[i].gameObject);

        placePoints_forSwap.Clear();

        contents.Clear();
        placePoint.Clear();

        // UI관련 초기화
        scroll.value = 1f;

    }

    // 세부 정보 표시하는 화면으로 변경 
    public void MoreInfo(Place place)
    {
        _lastMoreInfoPlace = place;//가장 최근에 검색한 길찾기 용 세부정부 내용 업데이트

        changePlace(false);

        moreInfoPlace.GetComponent<Contents>().changeContents(place);

        //자세한 정보 위치 미니맵에 띄우기
        MiniMapManager.Instance.SetPointSearchMap(place);

        //선택된 세부 정보 위치를 강조하고 그 외 위치들은 비활성화
        foreach (Contents_nonUI points in placePoint)
        {
            if(points.getPlace() == place) points.ChangePointState(PlacePointState.HIGHLIGHT);
            else points.ChangePointState(PlacePointState.IGNORE);
        }

    }

    public void CancleMoreInfo()
    {

        changePlace(true);

        foreach(Contents_nonUI points in placePoint)
        {
            points.ChangePointState(PlacePointState.NORMAL);
        }

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

    /// <summary>
    /// 지도 목록하고 상세정보 변환 true는 검색 목록 fasle는 상세정보
    /// </summary>
    /// <param name="toggle"></param>
    public void changePlace(bool toggle)
    {
        scrollPlace.SetActive(toggle);
        moreInfoPlace.SetActive(!toggle);
    }

    public void clickKeyword()
    {

        searchType = SearchType.KeyWord;

        geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(Camera.main.transform);

        // 쿼리 초기화
        KakaoAPI.Instance.Req.ClearQuery();

        setPage(1f);
        changePlace(true);

        //Debug.Log(geoPos.x + " " + geoPos.y);

        //geoPos.y가 경도 geoPos.x 가 위도 인것을 꼭 확인하기(나중에 코드에서 확인가능하게 변경할 것) => 수정 완료
        KakaoAPI.Instance.Req.AddQuery("x", geoPos.lon.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.lat.ToString());

        KakaoAPI.Instance.Req.AddQuery("page", "1");

        KakaoAPI.Instance.SearchByKeyword(searchKeyword.text, getResult);

    }

    public void clickCategory()
    {

        searchType = SearchType.Category;

        /*
        GameObject clickObject = EventSystem.current.currentSelectedGameObject.GetComponent<Transform>().GetChild(2).gameObject;

        recentSearch = clickObject.GetComponent<TextMeshProUGUI>().text;
        */

        recentSearch = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().text;

        setCategory();

        geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(Camera.main.transform);

        // 쿼리 초기화
        KakaoAPI.Instance.Req.ClearQuery();

        setPage(1f);
        changePlace(true);

        //Debug.Log(geoPos.x + " " + geoPos.y);

        //geoPos.y가 경도 geoPos.x 가 위도 인것을 꼭 확인하기(나중에 코드에서 확인가능하게 변경할 것) => 수정 완료

        /*
        KakaoAPI.Instance.Req.AddQuery("x", geoPos.lan.ToString());
        KakaoAPI.Instance.Req.AddQuery("y", geoPos.lat.ToString());
        KakaoAPI.Instance.Req.AddQuery("radius", radius.ToString());
        */

        KakaoAPI.Instance.Req.AddQuery("page", "1");

        // KakaoAPI.Instance.SearchByKeyword(recentSearch, getResult);

        KakaoAPI.Instance.SearchByCategory(recentKeyWord, geoPos.lat, geoPos.lon, radius, getResult);

    }

    public void changePage(string pageNum)
    {

        if(searchType == SearchType.KeyWord)
        {

            KakaoAPI.Instance.Req.ClearQuery();
            KakaoAPI.Instance.Req.AddQuery("x", geoPos.lon.ToString());
            KakaoAPI.Instance.Req.AddQuery("y", geoPos.lat.ToString());
            KakaoAPI.Instance.Req.AddQuery("radius", radius.ToString());
            KakaoAPI.Instance.Req.AddQuery("page", pageNum);

            KakaoAPI.Instance.SearchByKeyword(recentSearch, getResult);

        }
        else if(searchType == SearchType.Category)
        {

            setCategory();

            KakaoAPI.Instance.Req.ClearQuery();
            KakaoAPI.Instance.Req.AddQuery("page", pageNum);

            KakaoAPI.Instance.SearchByCategory(recentKeyWord, geoPos.lat, geoPos.lon, radius, getResult);

        }

    }

    public void setCategory()
    {

        if (recentSearch == "대형마트") recentKeyWord = "MT1";
        else if (recentSearch == "편의점") recentKeyWord = "CS2";
        else if (recentSearch == "어린이집") recentKeyWord = "PS3";
        else if (recentSearch == "학교") recentKeyWord = "SC4";
        else if (recentSearch == "학원") recentKeyWord = "AC5";
        else if (recentSearch == "주차장") recentKeyWord = "PK6";
        else if (recentSearch == "주유소") recentKeyWord = "OL7";
        else if (recentSearch == "지하철역") recentKeyWord = "SW8";
        else if (recentSearch == "은행") recentKeyWord = "BK9";
        else if (recentSearch == "문화시설") recentKeyWord = "CT1";
        else if (recentSearch == "중개업소") recentKeyWord = "AG2";
        else if (recentSearch == "공공가관") recentKeyWord = "PO3";
        else if (recentSearch == "관광명소") recentKeyWord = "AT4";
        else if (recentSearch == "숙박") recentKeyWord = "AD5";
        else if (recentSearch == "음식점") recentKeyWord = "FD6";
        else if (recentSearch == "카페") recentKeyWord = "CE7";
        else if (recentSearch == "병원") recentKeyWord = "HP8";
        else if (recentSearch == "약국") recentKeyWord = "PM9";

    }

    void getResult(string result)
    {

        // 주변 정보를 가져옴
        Response<Place> response = JsonUtility.FromJson<Response<Place>>(result);

        //주변 정보를 토대로 미니맵에 배치
        //탐색 시 실제 플레이어 위치로 할 수 있지만 조금 더 정확한 위치를 위해 Request 받았을 때의 플레이어 위치를 기준으로 미니맵 배치
        MiniMapManager.Instance.SetSearchMap((float)geoPos.lat, (float)geoPos.lon, response.documents);

        //이건 위와 다르게 플레이어 위치를 기준으로 미니맵 배치하는 함수
        //MiniMapManager.Instance.SetSearchMap(response.documents);

        _lastMoreInfoPlace = null;

        //기존에 사용하던 정보들 초기화
        ClearInfo();

        //받아온 정보들 추가
        foreach (Place place in response.documents) AddInfo(place);

        Invoke("SetPlacePointsHeightAdditioner", 0.1f);//가중치 초기화 함수를 약간 느리게 실행

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
