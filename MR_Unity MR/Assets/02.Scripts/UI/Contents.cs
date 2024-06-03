using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using RestAPI.KakaoObject;

public class Contents : MonoBehaviour
{

    [SerializeField] private Place info;
    [SerializeField] private TextMeshProUGUI place_nameText;
    [SerializeField] private TextMeshProUGUI category_nameText;
    [SerializeField] private TextMeshProUGUI phoneText;
    [SerializeField] private TextMeshProUGUI address_nameText;
    [SerializeField] private TextMeshProUGUI road_address_nameText;

    Transform player;

    [Space(15f)]
    [SerializeField] Transform showUI;

    Vector3 defaultSize;
    [Header("Size Setting")]
    [SerializeField] float minRange = 1f;
    [SerializeField] float maxRange = 300f;

    [Space(5f)]
    [SerializeField] float minSizeMultiplier = 1f;
    [SerializeField] float maxSizeMultiplier = 5f;

    Vector3 defaultHeight;
    [Space(5f)]
    [SerializeField] float minHeightAdditioner = 0f;
    [SerializeField] float maxHeightAdditioner = 50f;


    void Start()
    {
        // KakaoAPI.Instance.SearchByCategory(getResult<Place>);

        player = Camera.main.transform;//플레이어 위치를 카메라 위치로 세팅

        if (!showUI) return;

        defaultSize = showUI.localScale;
        defaultHeight = showUI.localPosition;

        transform.LookAt(transform.position + (transform.position - player.position));//간지나는 회전 구현을 위한 이상한 짓거리
    }

    void Update()
    {
        if (!showUI) return;

        float range = (player.position - transform.position).magnitude;//플레이어와의 거리 계산

        range = Mathf.Clamp(range, minRange, maxRange);//최소 거리 최대 거리로 Clamp

        //Debug.Log(showUI);

        //거리에 선형비례하는 크기증가값 계산
        float sizeMultiplier = Mathf.Lerp(minSizeMultiplier, maxSizeMultiplier, (range - minRange) / (maxRange - minRange));

        showUI.localScale = defaultSize * sizeMultiplier;

        float heightAdditioner = Mathf.Lerp(minHeightAdditioner, maxHeightAdditioner, (range - minRange) / (maxRange - minRange));

        showUI.localPosition = defaultHeight + Vector3.up * heightAdditioner;
    }

    private void OnDestroy()
    {
        transform.localScale = defaultSize;
    }

    public Place getPlace() { return info; }
    public void setPlace(Place place) { info = place; } 

    public void changeContents(Place place)
    {

        info.place_name = place_nameText.text = place.place_name;
        info.category_name = category_nameText.text = place.category_name;
        info.phone = phoneText.text = place.phone;
        info.address_name = address_nameText.text = place.address_name;
        info.road_address_name = road_address_nameText.text = place.road_address_name;

        //미니맵용 좌표
        info.x = place.x;
        info.y = place.y;
    }

    public void clickThis() { InformationUI.Instance.MoreInfo(info); }        

}
