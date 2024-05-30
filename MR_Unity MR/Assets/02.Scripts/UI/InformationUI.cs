using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RestAPI.KakaoObject;

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

    private bool scrollDown;
    [SerializeField] private float page;
    public float getPage() { return page; }
    public void setPage(float page) { this.page = page;}

    void Start()
    {
        scrollDown = true;
        changePlace(true);
        page = 1;
    }

    void Update(){
        
    }

    // 스크롤 맨 아래로 내릴 시 크기 늘려줌
    public void EndDrag()
    {
        if (scroll.value < 0f && scrollDown) {

            scrollDown = false;
            
            StartCoroutine("ChangePageUp");

        }
        else if (scroll.value > 1.1f && scrollDown)
        {

            scrollDown = false;

            StartCoroutine("ChangePageDown");

        }
    }

    public void AddInfo(Place place)
    {

        GameObject contents = Instantiate(contentPrefab, contentTransform);
        content.Add(contents);
        contents.GetComponent<Contents>().changeContents(place);

        GameObject placepoints = Instantiate(placePointPrefab);
        placePoint.Add(placepoints);

        Debug.Log("생성 위치 : " + place.y + " " + place.x);

        placepoints.GetComponent<GeoTransform>().Init(place.y, place.x);
        placepoints.GetComponent<Contents>().changeContents(place);


    }

    public void ClearInfo()
    {
        
        for(int i = content.Count - 1; i >= 0; i--) Destroy(content[i]);
        for(int i = placePoint.Count - 1; i >= 0; i--) Destroy(placePoint[i]);

        content.Clear();
        placePoint.Clear();

        // UI관련 초기화
        scroll.value = 1f;

    }

    public void MoreInfo(Place place)
    {

        changePlace(false);

        moreInfoPlace.GetComponent<Contents>().changeContents(place);

    }

    // 지도 목록하고 상세정보 변환 true는 검색 목록 fasle는 상세정보
    public void changePlace(bool toggle)
    {
        scrollPlace.SetActive(toggle);
        moreInfoPlace.SetActive(!toggle);
    }

    IEnumerator ChangePageUp()
    {

        page++;
        InfoManager.Instance.changePage(page.ToString());

        yield return new WaitForSeconds(0.5f);

        scrollDown = true;

        yield return null;
    }

    IEnumerator ChangePageDown()
    {

        if (page > 1f)
        {
            page--;
            InfoManager.Instance.changePage(page.ToString());
        }

        yield return new WaitForSeconds(0.5f);

        scrollDown = true;

        yield return null;
    }

}
