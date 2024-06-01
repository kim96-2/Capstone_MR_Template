using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    [SerializeField] private GameObject nextPoint; private WayPoint nextPointWay;
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private LineRenderer line;

    bool isDrawing = false;

    // Start is called before the first frame update
    void Start()
    {

        targetPoint = GameObject.FindWithTag("MainCamera");
        nextPointWay = GetComponent<WayPoint>();

    }

    public void Update()
    {

        if (!nextPoint) return;

        // 현재 WayPoint로 Player가 접근 시 다음 WayPoint까지 Line 생성
        if (Vector3.Distance(nextPoint.transform.position, targetPoint.transform.position) < 1f && isDrawing)
        {

            line.enabled = false;
            nextPoint.GetComponent<WayPoint>().DrawWay();

        }


    }

    public void setNextPoint(GameObject _nextPoint) { nextPoint = _nextPoint; }

    public void Init()
    {

        line = GetComponent<LineRenderer>();

    }

    /// <summary>
    /// 다음 WayPoint까지 Line 생성하는 함수
    /// <summary>
    public void DrawWay()
    {

        isDrawing = true;

        if (!nextPoint) return;

        // Line 크기 조정
        line.SetWidth(0.2f, 0.2f);

        // 다음 WayPoint까지 Line 생성
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, nextPoint.transform.position);

    }
    
}
