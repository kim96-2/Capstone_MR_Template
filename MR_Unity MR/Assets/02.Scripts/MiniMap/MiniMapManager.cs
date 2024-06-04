using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RestAPI.KakaoObject;
using RestAPI.DirectionObject;

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

    //임시 길목
    public class Pathpoint
    {
        public double lat { get; set; }
        public double lon { get; set; }

        public Pathpoint(double Lat, double Lon)
        {
            lat = Lat;
            lon = Lon;
        }
    }

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
            //Debug.Log("업데이트");
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

    //한 번 더 누른 위치 따로 표시
    public void SetPointSearchMap(Place place)
    {
        if (!GeoTransformManager.Instance.IsInited)
        {
            Debug.LogError("초기화 되지 않은 상태로 미니맵을 생성하려 함");
            return;
        }

        Double2Position geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(player);

        SetPointSearchMap((float)geoPos.x, (float)geoPos.y, place);
    }


    public void SetPointSearchMap(float lat, float lon, Place place)
    {
        if (!CheckMaxLoadCount()) return;//최대 이미지 로드 횟수 확인

        //Search State를 위한 쿼리 제작
        additionalQuery = new();
        additionalQuery.Key = "markers";

        additionalQuery.Value = "color:green";//컬러 세팅

        additionalQuery.Value += $"%7C{place.y},{place.x}";
        
        UpdateMap(lat,lon);
    }

    //경로 표시
    public void SetPathMap(Response response)
    {
        if (!GeoTransformManager.Instance.IsInited)
        {
            Debug.LogError("초기화 되지 않은 상태로 미니맵을 생성하려 함");
            return;
        }

        Double2Position geoPos = GeoTransformManager.Instance.TransformUnitySpaceToGeo(player);

        List<Pathpoint> pathpoints = new List<Pathpoint>();

        foreach (Feature p in response.features)
        {
            if (p.geometry.type == "Point")
            {

                double lon = double.Parse(p.geometry.coordinates[0]);
                double lat = double.Parse(p.geometry.coordinates[1]);

                pathpoints.Add(new Pathpoint(lat, lon));

            }
        }

        SetPathMap((float)geoPos.x, (float)geoPos.y, InformationUI.Instance.lastMoreInfoPlace.y, InformationUI.Instance.lastMoreInfoPlace.x, pathpoints);
    }

    public void SetPathMap(float lat, float lon, double dest_lat, double dest_lon, List<Pathpoint> pathpoints)
    {
        if (!CheckMaxLoadCount()) return;//최대 이미지 로드 횟수 확인

        //Search State를 위한 쿼리 제작
        additionalQuery = new();
        additionalQuery.Key = "markers";

        additionalQuery.Value = "color:green";//컬러 세팅

        additionalQuery.Value += $"%7C{dest_lat},{dest_lon}";

        additionalQuery.Value += "&path=color:0x0000ff|weight:5";

        additionalQuery.Value += $"|{lat},{lon}";


        Debug.Log(additionalQuery.Value + "이제 중간 위치 추가");

        foreach (Pathpoint pathpoint in pathpoints)
        {
            additionalQuery.Value += $"|{pathpoint.lat},{pathpoint.lon}";
        }

        additionalQuery.Value += $"|{dest_lat},{dest_lon}";
    
        Debug.Log(additionalQuery.Value);

        UpdateMap(lat, lon);

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
