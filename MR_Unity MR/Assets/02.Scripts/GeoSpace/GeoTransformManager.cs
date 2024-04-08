using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GeoTransformManager : MonoBehaviour
{
    [System.Serializable]
    class EllipsoidSetting//지구 타원체 세팅(예시 - WGS84)
    {
        public double longRadius;//장반경
        public double shortRadius;//단반경

        /// <summary>
        /// 편평률 -> 실제로는 1/편평률로 사용할 땐 다시 1 / floatness 해줘야됨
        /// </summary>
        [Space(5f)]
        public double floatness;

    }

    [System.Serializable]
    class TMSpaceSetting//TM 좌표계 세팅(예시 - TM 동부 원점,중부 원점 등)
    {
        public double lon0;//원점 경도
        public double lat0;//원점 위도
        [Space(5f)]
        public double x0;//X축(East) 원점 가산값
        public double y0;//Y축(North) 원점 가산값
        [Space(5f)]
        public double k0;//원점 축적 계수
    }

    //싱글톤으로 사용 할 수 있게 세팅
    private static GeoTransformManager _instance;
    public static GeoTransformManager Instance
    {
        get
        {
            if (_instance == null) CreateSingleton();

            return _instance;
        }
    }

    [Header("Pivot Setting")]
    [SerializeField] Double2Position pivotGeoPosition;//기준점 경위도 좌표
    [SerializeField] Double2Position pivotTMPosition;//기준점 TM 좌표
    [SerializeField] Double2Position pivotUnityPosition;//기준점 유니티 좌표

    [Space(5f)]
    [SerializeField] double pivotGeoRotation;//기준 방위각
    [SerializeField] double pivotUnityRotation;//기준 유니티 회전각

    [Header("Transform Setting")]
    [SerializeField]
    EllipsoidSetting WGS84_Setting;

    [SerializeField]
    TMSpaceSetting EPSG5186_Setting;

    void Awake()
    {
        //싱글톤 세팅
        if ( _instance == null ) CreateSingleton();
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    #region Singleton Setting
    static void CreateSingleton()//싱글톤 Resources 파일에서 가져오는 함수
    {
        GeoTransformManager instance = Instantiate(Resources.Load<GeoTransformManager>("GeoTransformManager"));

        _instance = instance;

        DontDestroyOnLoad(_instance.gameObject);
    }

    #endregion Singleton Setting

    #region Initialize Setting
    /// <summary>
    /// 기준점 초기화를 위한 함수
    /// </summary>
    /// <param name="pivot_lon">기준 경도</param>
    /// <param name="pivot_lat">기준 위도</param>
    /// <param name="pivotGeoRotation">기준 방위각</param>
    /// <param name="pivotTransform">기준 오브젝트 Transform(카메라 Transform 넣어주면 될 거임)</param>
    public void InitPivot(double pivot_lon,double pivot_lat,double pivotGeoRotation, Transform pivotTransform)
    {
        pivotGeoPosition.SetPosition(pivot_lon, pivot_lat);

        pivotTMPosition = TransformGeoToTM(pivot_lon, pivot_lat);

        pivotUnityPosition.SetPosition(pivotTransform.position.x, pivotTransform.position.y);

        this.pivotGeoRotation = pivotGeoRotation;

        Vector2 rot = pivotTransform.right;

        pivotUnityRotation =  Quaternion.FromToRotation(Vector2.right, rot).eulerAngles.z;
    }
    #endregion Initialize Setting

    /// <summary>
    /// 경위도 좌표계에서 TM 좌표계로 변환해주는 함수
    /// </summary>
    /// <param name="lon">경도</param>
    /// <param name="lat">위도</param>
    public Double2Position TransformGeoToTM(double lon, double lat)
    {
        return new Double2Position();
    }

    /// <summary>
    /// TM 좌표계에서 유니티 좌표계로 변환해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Double2Position TransformTMToUnitySpace(double x,double y)
    {
        if(pivotGeoPosition.y == 0)
        {
            Debug.LogError("기준점 초기화가 안되있어서 좌표계 변환이 불가능");
            return null;
        }

        //TM 좌표계에서 기준점으로 부터 위치 변화량 계산
        Vector2 dir = new Vector2((float)(x - pivotGeoPosition.x), (float)(y  - pivotGeoPosition.y));

        //위치 변화량을 TM 좌표계와 Unity 좌표계 회전각 변화량 만큼 회전(회전 시키는 방향 중요!!)
        dir = Quaternion.Euler(0, 0, (90f - (float)pivotGeoRotation) - (float)pivotUnityRotation) * dir;

        //마지막  Unity 좌표계 기준점에 최종 위치 변화량을 더해주어 위치 변환 마무리
        return new Double2Position(pivotUnityPosition.x + dir.x, pivotUnityPosition.y + dir.y);
    }
}
