using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using RestAPI.DirectionObject;
using System.Linq.Expressions;

public class WayPointManager : Singleton<WayPointManager>
{

    [SerializeField] GameObject wayPoint;
    [SerializeField] private List<GameObject> points = new List<GameObject>();

    [SerializeField] Response testapi;

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
        Double2Position dest = new Double2Position(InformationUI.Instance.lastMoreInfoPlace.y, InformationUI.Instance.lastMoreInfoPlace.x);

        DirectionAPI.Instance.DirectionTo(origin, dest, getResult);

        /*
        points[0].GetComponent<WayPoint>().DrawWay();
        points[0].GetComponent<WayPoint>().isDrawing = true;
        */

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

                SetPoints(new Double2Position(lat, lon));

            }
        
        }

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
