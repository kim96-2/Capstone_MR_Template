using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RestAPI.KakaoObject;

/// <summary>
/// 미니맵 UI를 화면에 띄워주는 걸 관리하는 매니져
/// </summary>
public class MiniMapManager : Singleton<MiniMapManager>//싱글톤으로 제작하긴 했는데... 막 안해도 될 것 같기도 하고..?
{
    [SerializeField] Image mapImage;
    Material mapMaterial;

    [Header("MiniMap Default Setting (Google)")]
    [SerializeField] string baseURL;

    [Space(5f)]
    [SerializeField] int zoom = 18;
    [SerializeField] int miniMapHeight = 800;
    [SerializeField] int miniMapWidth = 800;
    [SerializeField] int scale = 2;

    [Space(15f)]
    [SerializeField] float mapUpdateDistance = 2f;

    [Header("Debug Setting")]
    [SerializeField] int maxImageLoadCount = 1;//플레이 시 최대 이미지 로드 횟수 지정

    public WebRequest Req = new();

    Transform player;
    Vector3 lastPlayerPos;

    Query additionalQuery = null;

    protected override void Awake()
    {
        base.Awake();

        mapMaterial = mapImage.material;

        player = Camera.main.transform;//플레이어를 카메라 위치로 잡음
        lastPlayerPos = player.position;

        //SetDefaultMap(37.5518018f, 127.0736345f);
    }

    private void Update()
    {
        UpdateMapWhilePlayerMoving();
    }

    void UpdateMapWhilePlayerMoving()
    {

        if (Vector3.Distance(lastPlayerPos, player.position) > mapUpdateDistance)
        {
            Debug.Log("업데이트");
            UpdateMap();
        }
    }

    /// <summary>
    /// 미니맵 지도 이미지 적용 함수
    /// </summary>
    /// <param name="texture">지도 이미지 텍스쳐</param>
    public void SetMapImage(Texture texture)
    {
        //Debug.Log("실제 이미지 생성 함수 실행");
        mapMaterial.SetTexture("_MapTex", texture);
    }

    #region Query Setting

    /// <summary>
    /// 초기 커리를 제작해주는 함수
    /// </summary>
    void SetDefaultQuery()
    {
        Req.ClearQuery();
        Req.URL = baseURL;
        Req.AddQuery("format", "png");
        Req.AddQuery("zoom", zoom.ToString());
        Req.AddQuery("size", $"{miniMapWidth}x{miniMapHeight}");
        Req.AddQuery("scale", scale.ToString());

        Req.AddQuery("key", APIKey.Google_StaticMap);
    }


    #endregion Query Setting


    #region Map Set

    void UpdateMap()
    {
        if (!GeoTransformManager.Instance.IsInited)
        {
            Debug.LogError("초기화 되지 않은 상태로 미니맵을 생성하려 함");
            return;
        }

        Double2Position geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(player);

        UpdateMap((float)geoPos.x, (float)geoPos.y);
    }

    /// <summary>
    /// 현 상태에 맞게 미니맵을 업데이트하는 함수
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lon"></param>
    void UpdateMap(float lat,float lon)
    {
        //초기 쿼리 세팅
        SetDefaultQuery();

        //특정 경위도 위치에 좌표 찍음 + 중심 좌표 지정
        Req.AddQuery("center", $"{lat},{lon}");
        Req.AddQuery("markers", $"color:red%7C{lat},{lon}");

        if (additionalQuery != null)//추가적인 쿼리가 있으면 업데이트
        {
            Req.AddQuery(additionalQuery.Key, additionalQuery.Value);
        }

        StartCoroutine(Req.WebRequestImageGet((Texture2D result) =>
        {
            SetMapImage(result);
        }));

        //업데이트 시 플레이어 마지막 위치 업데이트
        lastPlayerPos = player.position;
    }

    /// <summary>
    /// 플레이어를 미니맵 중심으로 하여 플레이어 위치만 찍히는 미니맵 생성 함수
    /// </summary>
    public void SetDefaultMap()
    {
        if (!GeoTransformManager.Instance.IsInited)
        {
            Debug.LogError("초기화 되지 않은 상태로 미니맵을 생성하려 함");
            return;
        }

        Double2Position geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(player);

        SetDefaultMap((float)geoPos.x, (float)geoPos.y);
    }

    /// <summary>
    /// 특정 경위도 위치만 찍히는 미니맵 생성 함수
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lon"></param>
    public void SetDefaultMap(float lat,float lon)
    {
        if (!CheckMaxLoadCount()) return;//최대 이미지 로드 횟수 확인

        additionalQuery = null;//추가적 쿼리 지우기

        //Debug.Log("이미지 생성");

        UpdateMap(lat, lon);//현재 상태 맵을 업데이트 하는 함수
    
    }

    public void SetSearchMap(List<Place> places)
    {
        if (!GeoTransformManager.Instance.IsInited)
        {
            Debug.LogError("초기화 되지 않은 상태로 미니맵을 생성하려 함");
            return;
        }

        Double2Position geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(player);

        SetSearchMap((float)geoPos.x, (float)geoPos.y, places);
    }

    public void SetSearchMap(float lat, float lon,List<Place> places)
    {
        if (!CheckMaxLoadCount()) return;//최대 이미지 로드 횟수 확인

        //Search State를 위한 쿼리 제작
        additionalQuery = new();
        additionalQuery.Key = "markers";

        additionalQuery.Value = "color:blue";//컬러 세팅

        foreach(Place place in places)
        {
            additionalQuery.Value += $"%7C{place.y},{place.x}";
        }

        UpdateMap(lat,lon);
    }

    #endregion Map Set

    #region Debug Setting


    //최대 이미지 로드 횟수 확인 코드
    bool CheckMaxLoadCount()
    {
#if UNITY_EDITOR
        if (maxImageLoadCount <= 0) return false;

        maxImageLoadCount -= 1;

        return true;

#else
        return true;
#endif
    }

#endregion Debug Setting
}
