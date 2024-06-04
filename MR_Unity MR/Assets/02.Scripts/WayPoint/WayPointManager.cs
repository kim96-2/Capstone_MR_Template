using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using RestAPI.DirectionObject;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using static MiniMapManager;

public enum WayType
{

    ONESTEP,
    ALL

}

public class WayPointManager : Singleton<WayPointManager>
{

    [SerializeField] GameObject wayPoint;
    [SerializeField] private List<GameObject> points = new List<GameObject>();
    [SerializeField] private WayType _wayType;

    public WayType wayType() { return _wayType; }

    [SerializeField] GameObject _nowPoint;

    public void nowPoint(GameObject obj) { _nowPoint = obj; }
    public GameObject nowPoint() { return _nowPoint; }

    [SerializeField] Response testapi;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void getWayPoint()
    {

        if (points.Count != 0) { clearWayPoint(); }

        Double2Position origin = GeoTransformManager.Instance.TransformUnitySpaceToGeo(Camera.main.transform);
        Double2Position dest = new Double2Position(InformationUI.Instance.lastMoreInfoPlace.y, InformationUI.Instance.lastMoreInfoPlace.x);

        DirectionAPI.Instance.DirectionTo(origin, dest, getResult);

    }

    public void resetWayPoint()
    {

        clearWayPoint();
        getWayPoint();

    }

    public void clearWayPoint()
    {

        foreach (GameObject gameObject in points) { Destroy(gameObject); }
        points.Clear();

    }

    void getResult(string result)
    {

        Response response = JsonUtility.FromJson<Response>(result);

        testapi = response;

        foreach (Feature p in response.features) {
            
            if(p.geometry.type == "Point")
            {

                double lon = double.Parse(p.geometry.coordinates[0]);
                double lat = double.Parse(p.geometry.coordinates[1]);
                
                Debug.Log(lon + "  , " + lat);
                SetPoints(new Double2Position(lat, lon), p.properties.description);   
            }
        
        }
        MiniMapManager.Instance.SetPathMap(response);
    }

    /// <summary>
    /// 해당 위치에 WayPoint 생성하는 함수
    /// <summary>
    /// <param name="pos">Unity 내에서 화면에 띄울 Vector3 좌표</param>
    public void SetPoints(Double2Position pos, string message)
    {

        GameObject point = Instantiate(wayPoint);

        points.Add(point);
        point.GetComponent<WayPoint>().Init(pos, message);

        // Unity 상에서 확인하기 편하게 적어놓은거임
        point.name = points.Count.ToString();

        if (points.Count == 1) _nowPoint = point;

        if (_wayType == WayType.ONESTEP)
        {

            if (points.Count != 1)
            {
                points[points.Count - 2].GetComponent<WayPoint>().setNextPoint(point);
                points[points.Count - 1].GetComponent<WayPoint>().setPrevPoint(points[points.Count - 2]);
                points[points.Count - 1].SetActive(false);
            }

        }
        else if(_wayType == WayType.ALL)
        {

            if (points.Count != 1)
            {
                points[points.Count - 2].GetComponent<WayPoint>().setNextPoint(point);
                points[points.Count - 1].GetComponent<WayPoint>().setPrevPoint(points[points.Count - 2]);
            }

        }

    }

    // 길찾기 초기화
    public void clearPoint() {

        foreach (GameObject aa in points) Destroy(aa);
        
        points.Clear(); 
    
    }
}
