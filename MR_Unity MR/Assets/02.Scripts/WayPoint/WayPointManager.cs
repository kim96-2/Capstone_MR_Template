using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class WayPointManager : MonoBehaviour
{

    [SerializeField] GameObject wayPoint;
    [SerializeField] private List<GameObject> points = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // 테스트용 좌표
        SetPoints(new Vector3(0f, 0f, 0f));
        SetPoints(new Vector3(0f, 0f, 5f));
        SetPoints(new Vector3(5f, 0f, 5f));
        SetPoints(new Vector3(5f, 0f, 10f));
        SetPoints(new Vector3(5f, 0f, 15f));
        SetPoints(new Vector3(8f, 0f, 12f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 해당 위치에 WayPoint 생성하는 함수
    /// <summary>
    /// <param name="pos">Unity 내에서 화면에 띄울 Vector3 좌표</param>
    public void SetPoints(Vector3 pos)
    {

        GameObject point = Instantiate(wayPoint, pos, Quaternion.identity);

        points.Add(point);

        // Unity 상에서 확인하기 편하게 적어놓은거임
        point.name = points.Count.ToString();

        // 첫 번째 WayPoint 제외하고 다음 WayPoint 설정 및 처음 WayPoint 길 연결
        if (points.Count != 1) points[points.Count - 2].GetComponent<WayPoint>().setNextPoint(point);
        if (points.Count == 2) points[points.Count - 2].GetComponent<WayPoint>().DrawWay();

    }

    // 길찾기 초기화
    public void clearPoint() {

        foreach (GameObject aa in points) Destroy(aa);
        
        points.Clear(); 
    
    }
}
