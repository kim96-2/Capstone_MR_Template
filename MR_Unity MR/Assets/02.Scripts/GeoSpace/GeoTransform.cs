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

    /// <summary>
    /// 위도 (경위도 좌표로 사용시에만)
    /// </summary>
    public double lat { get => x; }

    /// <summary>
    /// 경도 (경위도 좌표로 사용시에만)
    /// </summary>
    public double lon { get => y; }
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
    public Double2Position Position_Unity { get => position_Unity;}

    private void OnDestroy()
    {
        
    }

    /// <summary>
    /// 지리 Trasform 초기화 함수 (Transform의 경도 위도 입력)
    /// </summary>
    /// <param name="lat">위도</param>
    /// <param name="lon">경도</param>
    public void Init(double lat,double lon)
    {
        position_Geo = new Double2Position(lat,lon);

        position_TM = GeoTransformManager.Instance.TransformGeoToTM(lat, lon);

        position_Unity = GeoTransformManager.Instance.TransformTMToUnitySpace(position_TM.x, position_TM.y);

        //Debug.Log(position_Unity.x + "\n" + position_Unity.y);

        //자동으로 계산된 위치로 Transform 포지션 변경
        this.transform.position = new Vector3((float)position_Unity.x, this.transform.position.y, (float)position_Unity.y);
    }

    /// <summary>
    /// 실시간으로 기준점이 변경될 때 실행되는 함수
    /// </summary>
    public void UpdateInitPos()
    {
        Init(position_Geo.lat, position_Geo.lon);//자기 경위도와 새로운 경위도 기준점을 가지고 위치 업데이트
    }
}
