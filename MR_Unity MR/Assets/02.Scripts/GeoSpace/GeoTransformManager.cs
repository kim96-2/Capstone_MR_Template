using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

public class GeoTransformManager : MonoBehaviour
{
    [Serializable]
    class EllipsoidSetting//지구 타원체 세팅(예시 - WGS84)
    {
        public double longRadius;//장반경
        public double shortRadius;//단반경

        /// <summary>
        /// 편평률 -> 실제로는 1/편평률로 사용할 땐 다시 1 / floatness 해줘야됨
        /// </summary>
        [Space(5f)]
        public double floatness;
        
        //제 1 이심률
        public double E2_1 { get => (longRadius * longRadius - shortRadius * shortRadius) / (longRadius * longRadius); }

        //제 2 이심률
        public double E2_2 { get => (longRadius * longRadius - shortRadius * shortRadius) / (shortRadius * shortRadius); }

    }

    [Serializable]
    class TMSpaceSetting//TM 좌표계 세팅(예시 - TM 동부 원점,중부 원점 등)
    {
        [SerializeField] double _lon0; // 입력시 디그리값
        [SerializeField] double _lat0; // 입력시 디그리값

        public double Lon0 { get => _lon0 * Mathf.Deg2Rad; }//원점 경도(라디안값)
        public double Lat0 { get => _lat0 * Mathf.Deg2Rad; }//원점 위도(라디안값)
        [Space(5f)]
        public double X0;//X축(East) 원점 가산값
        public double Y0;//Y축(North) 원점 가산값
        [Space(5f)]
        public double K0;//원점 축적 계수
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

    public Double2Position PivotGeoPosition {  get => pivotGeoPosition;}
    public Double2Position PivotTMPosition { get => pivotTMPosition;}
    public Double2Position PivotUnityPosition {get => pivotUnityPosition;}

    [Space(5f)]
    [SerializeField] double pivotGeoRotation;//기준 방위각
    [SerializeField] double pivotUnityRotation;//기준 유니티 회전각

    public double PivotGeoRotation { get => pivotGeoRotation;}
    public double PivotUnityRotation { get => pivotUnityRotation;}

    [Header("Transform Setting")]
    [SerializeField]
    EllipsoidSetting WGS84_Setting;

    [SerializeField]
    TMSpaceSetting EPSG5186_Setting;

    void Awake()
    {
        //싱글톤 세팅
        if ( _instance != null )
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    #region Singleton Setting
    static void CreateSingleton()//싱글톤 Resources 파일에서 가져오는 함수
    {
        GeoTransformManager instance = Instantiate(Resources.Load<GeoTransformManager>("GeoTransformManager"));

        _instance = instance;
        Debug.Log("매니져 생성");
    }

    #endregion Singleton Setting

    #region Initialize Setting
    /// <summary>
    /// 기준점 초기화를 위한 함수
    /// </summary>
    /// <param name="pivot_lat">기준 위도</param>
    /// <param name="pivot_lon">기준 경도</param>
    /// <param name="pivotGeoRotation">기준 방위각</param>
    /// <param name="pivotTransform">기준 오브젝트 Transform(카메라 Transform 넣어주면 될 거임)</param>
    public void InitPivot(double pivot_lat,double pivot_lon,double pivotGeoRotation, Transform pivotTransform)
    {
        pivotGeoPosition.SetPosition(pivot_lat, pivot_lon);

        pivotTMPosition = TransformGeoToTM(pivot_lat, pivot_lon);

        pivotUnityPosition.SetPosition(pivotTransform.position.x, pivotTransform.position.y);

        this.pivotGeoRotation = pivotGeoRotation;

        Vector3 rot = pivotTransform.right;
        rot.y = 0f;
        rot.Normalize();
        //Debug.Log(rot);

        pivotUnityRotation =  Quaternion.FromToRotation(Vector3.right, rot).eulerAngles.y;
    }
    #endregion Initialize Setting


    #region Transform Setting
    /// <summary>
    /// 경위도 좌표계에서 TM 좌표계로 변환해주는 함수
    /// </summary>
    /// <param name="lon">경도</param>
    /// <param name="lat">위도</param>
    public Double2Position TransformGeoToTM(double lat, double lon)
    {
        //경위도 라이안 값으로 변경
        lon = lon * Mathf.Deg2Rad;
        lat = lat * Mathf.Deg2Rad;

        //사용하는 좌표계 세팅
        EllipsoidSetting ellipsoid = WGS84_Setting;
        TMSpaceSetting tm = EPSG5186_Setting;

        //T = tan^2(위도)
        double T = Math.Tan(lat) * Math.Tan(lat);
        //Debug.Log("T : " + T);

        //C = e^2 / (1 - e^2) * cos^2(위도)
        double C = ellipsoid.E2_1 / (1 - ellipsoid.E2_1) * Math.Cos(lat) * Math.Cos(lat);
        //Debug.Log("C : " + C);

        //A = (경도 - 기준경도) * cos(위도)
        double A = (lon - tm.Lon0) * Math.Cos(lat);
        //Debug.Log("A : " + A);

        //N = 장반경 / (1 - e^2 * sin ^2(위도))^0.5
        double insideN = Math.Round(1d - ellipsoid.E2_1 * Math.Sin(lat) * Math.Sin(lat), 8);
        double N = ellipsoid.longRadius / Math.Sqrt(insideN);
        //Debug.Log("N : " + N);

        //지오선장 계산
        double M = CalculateM(ellipsoid.E2_1, lat, ellipsoid.longRadius);
        //Debug.Log("M : " + M);

        //기준 지오선장 계산
        double M0 = CalculateM(ellipsoid.E2_1, tm.Lat0, ellipsoid.longRadius);
        //Debug.Log("M0 : " + M0);

        //N 방향
        double finalY =
            tm.Y0 +
            tm.K0 *
            (M - M0
            + N * Math.Tan(lat) *
            (Pow(A,2) / 2d
            + Pow(A,4) / 24d * (5d - T + 9d * C + 4d * Pow(C,4))
            + Pow(A,6) / 720d * (61d - 58d * T + Pow(T,2) + 600d * C - 330d * ellipsoid.E2_2)
            ));

        //E 방향
        double finalX =
            tm.X0 +
            tm.K0 * N *
            (A + Pow(A,3) / 6d * (1d - T + C)
            + Pow(A,5) / 120d * (5d - 18d * T + Pow(T,2) + 72d * C - 58d * ellipsoid.E2_2)
        );

        //Debug.Log("Y : " + finalY + "\nX : " + finalX);

        return new Double2Position(finalX,finalY);
    }

    /// <summary>
    /// M(지오선장)값을 계산하는 함수
    /// </summary>
    /// <param name="e2_1"></param>
    /// <param name="lat"></param>
    /// <param name="longRadius"></param>
    /// <returns></returns>
    double CalculateM(double e2_1,double lat,double longRadius)
    {
        //미쳐버린 계산 식
        return
            longRadius *
            ((1d - e2_1 / 4d - Pow(e2_1, 2) * 3d / 64d - Pow(e2_1, 3) * 5d / 256d) * lat
            - (e2_1 * 3d / 8d + Pow(e2_1, 2) * 3d / 32d + Pow(e2_1, 3) * 45d / 1024d) * Mathf.Sin((float)lat * 2f)
            + (Pow(e2_1, 2) * 15d / 256d + Pow(e2_1, 3) * 45d / 1024) * Mathf.Sin((float)lat * 4f)
            - Pow(e2_1, 3) * 35d / 3072d * Mathf.Sin((float)lat * 6f));
    }

    double Pow(double x,int n)
    {
        double final = 1d;
        for (int i = 0; i < n; i++) final *= x;

        return final;
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
        Vector3 dir = new Vector3((float)(x - pivotTMPosition.x), 0 , (float)(y  - pivotTMPosition.y));

        

        //위치 변화량을 TM 좌표계와 Unity 좌표계 회전각 변화량 만큼 회전(회전 시키는 방향 중요!!)
        //dir = Quaternion.Euler(0, 0, (90f - (float)pivotGeoRotation) - (float)pivotUnityRotation) * dir;
        dir = Quaternion.Euler(0, (float)pivotUnityRotation - (float)pivotGeoRotation, 0) * dir;

        Debug.Log(dir);

        //마지막  Unity 좌표계 기준점에 최종 위치 변화량을 더해주어 위치 변환 마무리
        return new Double2Position(pivotUnityPosition.x + dir.x, pivotUnityPosition.y + dir.z);
    }

    /// <summary>
    /// 유니티 좌표를 위경도 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Double2Position TransformUnitySpaceToGeo(float x, float z)
    {
        //유니티 좌표계에서 기준점으로 부터 위치 변화량 계산
        Vector3 dir = new Vector3((float)(x - pivotUnityPosition.x), 0, (float)(z - pivotUnityPosition.y));

        //위치 변화량을 회전각 변화량 만큼 회전(위 TMToUnitySpace 변환 때 사용한 회전간 변환을 역으로 사용)
        dir = Quaternion.Euler(0, - (float)pivotUnityRotation + (float)pivotGeoRotation, 0) * dir;

        //TM 좌표 계산(아직 정확하지 않아서 정확한지 확인 필요)
        double x_TM = pivotTMPosition.x + dir.x;
        double y_TM = pivotTMPosition.y + dir.y;

        return TransformGeoToTM(x_TM, y_TM);
    }

    /// <summary>
    /// 유니티 좌표를 위경도 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="target">변환을 원하는 오브젝트 위치(Transform)</param>
    /// <returns></returns>
    public Double2Position TransformUnitySpaceToGeo(Transform target)
    {
        return TransformUnitySpaceToGeo(target.position.x, target.position.z);
    }

    /// <summary>
    /// TM 좌표계를 위경도 좌표계로 변환하는 함수
    /// </summary>
    /// <param name="x">TM 좌표계에서 동쪽 변화량</param>
    /// <param name="y">TM 좌표계에서 북쪽 변화량</param>
    /// <returns></returns>
    public Double2Position TransformTMToGeo(double x,double y)
    {

        //사용하는 좌표계 세팅
        EllipsoidSetting ellipsoid = WGS84_Setting;
        TMSpaceSetting tm = EPSG5186_Setting;

        //기준 지오선장 계산
        double M0 = CalculateM(ellipsoid.E2_1, tm.Lat0, ellipsoid.longRadius);

        //지오선장 계산 (x - tm.X0) (y - tm.Y0)
        double M = M0 + (y - tm.Y0) / tm.K0;

        //Debug.Log(y - tm.Y0);

        double u1 = M / (ellipsoid.longRadius * (1 - ellipsoid.E2_1 / 4 - Pow(ellipsoid.E2_1, 2) * 3d / 64d - Pow(ellipsoid.E2_1, 3) * 5d / 256d));

        double e1 = (1d - Math.Sqrt(1 - ellipsoid.E2_1)) / (1d + Math.Sqrt(1 - ellipsoid.E2_1));

        double lat1 = u1 + (e1 * 3d / 2d - Pow(e1, 3) * 27d / 32d) * Math.Sin(2d * u1) +
            (Pow(e1, 2) * 21d / 16d - Pow(e1, 4) * 55d / 32d) * Math.Sin(4d * u1) +
            Pow(e1, 3) * 151d / 96d * Math.Sin(6d * u1) + Pow(e1, 4) * 1097d / 512d * Math.Sin(8d * u1);

        //Debug.Log(lat1);

        double R1 = ellipsoid.longRadius * (1d - ellipsoid.E2_1) / Math.Pow(1d - ellipsoid.E2_1 * Pow(Math.Sin(lat1), 2), 1.5d);

        double C1 = ellipsoid.E2_2 * Pow(Math.Cos(lat1), 2);

        double T1 = Pow(Math.Tan(lat1),2);

        double N1 = ellipsoid.longRadius / Math.Sqrt(1d - ellipsoid.E2_1 * Pow(Math.Sin(lat1), 2));

        double D = (x - tm.X0) / (N1 * tm.K0);

        double lat_Final = lat1 - N1 * Math.Tan(lat1) / R1 * 
            (D * D / 2d
            - Pow(D, 4) / 24d * (5d + 3 * T1 + 10d * C1 - 4d * Pow(C1, 2) - 9d * ellipsoid.E2_2)
            + Pow(D, 6) / 720d * (61d + 90d * T1 + 298 * C1 + 45d * Pow(T1, 2) - 252d * ellipsoid.E2_2 - 3d * Pow(C1, 2)));

        double lon_Final = tm.Lon0 + 1d / Math.Cos(lat1) *
            (D - Pow(D, 3) / 6d * (1d + 2d * T1 + C1)
            + Pow(D, 5) / 120d * (5d - 2d * C1 + 28 * T1 - 3d * Pow(C1, 2) + 8d * ellipsoid.E2_2 + 24d * Pow(T1, 2)));

        return new Double2Position(lat_Final * 180d / Math.PI, lon_Final * 180d / Math.PI);
    }

    #endregion Transform Setting
}
