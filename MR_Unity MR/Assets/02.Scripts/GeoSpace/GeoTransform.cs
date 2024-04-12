using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Double2Position
{
    public Double2Position()
    {
        x= 0; y = 0;
    }

    public Double2Position(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetPosition(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public double x , y;
}

/// <summary>
/// 지리 공간 Transform 컴포넌트
/// 유니티 Transform 처럼 사용하되 위경도 값과 
/// </summary>
public class GeoTransform : MonoBehaviour
{
    Double2Position position_Geo;//경위도 좌표계(WGS84)
    public Double2Position Position_Geo { get => position_Geo;}

    Double2Position position_TM;//TM 좌표계(EPSG:5181, 혹은 EPSG:5186)
    public Double2Position Position_TM { get => position_TM;}

    Double2Position position_Unity;//유니티 좌표계

    /// <summary>
    /// 지리 Trasform 초기화 함수 (Transform의 경도 위도 입력)
    /// </summary>
    /// <param name="lon">경도</param>
    /// <param name="lat">위도</param>
    public void Init(double lon,double lat)
    {
        position_Geo = new Double2Position(lon,lat);

        position_TM = GeoTransformManager.Instance.TransformGeoToTM(lon, lat);

        position_Unity = GeoTransformManager.Instance.TransformTMToUnitySpace(position_TM.x, position_TM.y);

        //자동으로 계산된 위치로 Transform 포지션 변경
        this.transform.position = new Vector3((float)position_Unity.x, this.transform.position.y, (float)position_Unity.y);
    }
}
