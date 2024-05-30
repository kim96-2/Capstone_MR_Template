using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] float scale = 2;

    [Header("Debug Setting")]
    [SerializeField] int maxImageLoadCount = 1;//플레이 시 최대 이미지 로드 수 지정

    public WebRequest Req = new();

    Transform player;

    protected override void Awake()
    {
        base.Awake();

        mapMaterial = mapImage.material;

        player = Camera.main.transform;//플레이어를 카메라 위치로 잡음

        //SetDefaultMap(37.5518018f, 127.0736345f);
    }

    /// <summary>
    /// 미니맵 지도 이미지 적용 함수
    /// </summary>
    /// <param name="texture">지도 이미지 텍스쳐</param>
    public void SetMapImage(Texture texture)
    {
        Debug.Log("실제 이미지 생성 함수 실행");
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

        Debug.Log("이미지 생성");

        //초기 쿼리 세팅
        SetDefaultQuery();

        //특정 경위도 위치에 좌표 찍음 + 중심 좌표 지정
        Req.AddQuery("center", $"{lat},{lon}");
        Req.AddQuery("markers", $"color:blue%7C{lat},{lon}");


        StartCoroutine(Req.WebRequestImageGet((Texture2D result) =>
        {
            SetMapImage(result);
        }));
    }

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
