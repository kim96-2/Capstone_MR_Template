using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 좌표 특징
/// </summary>
public enum PlacePointState
{
    NORMAL,
    HIGHLIGHT,
    IGNORE
}

public class Contents_nonUI : Contents
{
    Transform player;

    float rangeToPlayer;
    public float RangeToPlayer { get => rangeToPlayer; }

    float angleFromPlayer;
    public float AngleFromPlayer { get => angleFromPlayer; }

    float heightAdditioner = 0f;
    public float HeightAdditioner { get => heightAdditioner; set => heightAdditioner = value; }

    [Space(15f)]
    [SerializeField] Transform showUI;

    Vector3 defaultSize;
    [Header("Size Setting")]
    [SerializeField] float minRange = 1f;
    [SerializeField] float maxRange = 300f;

    [Space(5f)]
    [SerializeField] float minSizeMultiplier = 1f;
    [SerializeField] float maxSizeMultiplier = 5f;

    [Space(5f)]
    [SerializeField] float heightMoveSpeed = 10f;

    [Space(5f)]
    [SerializeField] AnimationCurve alphaMultiplier;
    [SerializeField] CanvasGroup canvasGroup;

    Vector3 defaultHeight;
    //[Space(5f)]
    //[SerializeField] float minHeightAdditioner = 0f;
    //[SerializeField] float maxHeightAdditioner = 50f;

    //현재 좌표의 상태
    PlacePointState pointState = PlacePointState.NORMAL;
    public void ChangePointState(PlacePointState state) => pointState = state;

    void Start()
    {
        // KakaoAPI.Instance.SearchByCategory(getResult<Place>);

        player = Camera.main.transform;//플레이어 위치를 카메라 위치로 세팅

        defaultSize = showUI.localScale;
        defaultHeight = showUI.localPosition;

        transform.LookAt(transform.position + (transform.position - player.position));//간지나는 회전 구현을 위한 이상한 짓거리
    }

    void Update()
    {
        if (!showUI) return;

        Vector3 dis = player.position - transform.position;
        dis.y = 0f;

        Vector3 playerDir = player.forward;
        playerDir.y = 0;

        //플레이어가 바라보는 방향과의 각도를 계산
        angleFromPlayer = Quaternion.FromToRotation(playerDir, -dis.normalized).eulerAngles.y;
        if (angleFromPlayer > 180f) angleFromPlayer = angleFromPlayer - 360f;

        float range = (dis).magnitude;//플레이어와의 거리 계산

        rangeToPlayer = range;

        range = Mathf.Clamp(range, minRange, maxRange);//최소 거리 최대 거리로 Clamp

        //거리에 선형비례하는 크기증가값 계산
        float sizeMultiplier = Mathf.Lerp(minSizeMultiplier, maxSizeMultiplier, (range - minRange) / (maxRange - minRange));

        Vector3 size, pos;
        float  alpha;

        //각각의 상태에 따라 위치, 크기, 투명도 조정
        switch (pointState)
        {
            case PlacePointState.IGNORE:
                size = defaultSize * sizeMultiplier * 0.7f;
                pos = Vector3.Lerp(showUI.localPosition, defaultHeight, heightMoveSpeed * Time.deltaTime);
                alpha = alphaMultiplier.Evaluate(1f);
                break;

            case PlacePointState.HIGHLIGHT:
                size = defaultSize * sizeMultiplier * 1.8f;
                pos = Vector3.Lerp(showUI.localPosition, defaultHeight + Vector3.up * Mathf.Tan(heightAdditioner * Mathf.Deg2Rad) * range, heightMoveSpeed * Time.deltaTime);
                alpha = alphaMultiplier.Evaluate(0f);
                break;

            default://NORMAL
                size = defaultSize * sizeMultiplier;
                pos = Vector3.Lerp(showUI.localPosition, defaultHeight + Vector3.up * Mathf.Tan(heightAdditioner * Mathf.Deg2Rad) * range, heightMoveSpeed * Time.deltaTime);
                alpha = alphaMultiplier.Evaluate((range - minRange) / (maxRange - minRange));
                break;
        }

        //크기 조정
        showUI.localScale = size;

        //높이 조정
        showUI.localPosition = pos;

        //알파값 조정
        canvasGroup.alpha = alpha;
    }

    private void OnDestroy()
    {
        transform.localScale = defaultSize;
    }
}
