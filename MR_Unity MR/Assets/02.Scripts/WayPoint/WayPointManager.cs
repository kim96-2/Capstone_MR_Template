using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using RestAPI.DirectionObject;
using RestAPI.KakaoObject;

public class WayPointManager : Singleton<WayPointManager>
{

    [SerializeField] GameObject wayPoint;
    [SerializeField] private List<GameObject> points = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }
    public void testWayPoint()
    {

        /*
        SetPoints(new Vector3(0f, 0f, 5f));
        SetPoints(new Vector3(5f, 0f, 5f));
        SetPoints(new Vector3(5f, 0f, 10f));
        SetPoints(new Vector3(5f, 0f, 15f));
        SetPoints(new Vector3(8f, 0f, 12f));
        */

        Double2Position origin = GeoTransformManager.Instance.TransformUnitySpaceToGeo(Camera.main.transform);
        Double2Position dest = new Double2Position(InformationUI.Instance.lastMoreInfoPlace.x, InformationUI.Instance.lastMoreInfoPlace.y);

        DirectionAPI.Instance.DirectionTo(origin, dest, getResult);

        points[0].GetComponent<WayPoint>().DrawWay();
        points[0].GetComponent<WayPoint>().isDrawing = true;

    }

    void getResult(string result)
    {

        Response response = JsonUtility.FromJson<Response>(result);

        foreach (Feature p in response.features) { SetPoints(new Double2Position(p.geometry.coordinates[0][1], p.geometry.coordinates[0][0])); }

    }

    /// <summary>
    /// 해당 위치에 WayPoint 생성하는 함수
    /// <summary>
    /// <param name="pos">Unity 내에서 화면에 띄울 Vector3 좌표</param>
    public void SetPoints(Double2Position pos)
    {

        GameObject point = Instantiate(wayPoint);

        points.Add(point);
        point.GetComponent<WayPoint>().Init(pos);

        // Unity 상에서 확인하기 편하게 적어놓은거임
        point.name = points.Count.ToString();

        if (points.Count != 1)
        {
            points[points.Count - 2].GetComponent<WayPoint>().setNextPoint(point);
            points[points.Count - 1].SetActive(false);
        }

    }

    // 길찾기 초기화
    public void clearPoint() {

        foreach (GameObject aa in points) Destroy(aa);
        
        points.Clear(); 
    
    }
}
