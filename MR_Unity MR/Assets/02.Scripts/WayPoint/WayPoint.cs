using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    [SerializeField] private GameObject nextPoint;
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private LineRenderer line;

    public bool isDrawing = false;

    void Awake()
    {

        targetPoint = GameObject.FindWithTag("MainCamera");
        line = GetComponent<LineRenderer>();

    }

    public void Update()
    {

        DrawWay();

        // 현재 WayPoint로 Player가 접근 시 다음 WayPoint까지 Line 생성
        if (Vector3.Distance(this.transform.position, targetPoint.transform.position) < 1f && isDrawing)
        {

            line.enabled = false;

            if (!nextPoint)
            {

                StartCoroutine("timer");
                
                return;

            }

            nextPoint.SetActive(true);
            nextPoint.GetComponent<WayPoint>().isDrawing = true;
            gameObject.SetActive(false);

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

        // Line 크기 조정
        line.SetWidth(0.2f, 0.2f);

        // 현재 WayPoint까지 Line 생성
        line.SetPosition(0, new Vector3(targetPoint.transform.position.x, this.transform.position.y, targetPoint.transform.position.z));
        line.SetPosition(1, this.transform.position);

    }

    IEnumerator timer() {

        isDrawing = false;

        GameObject.FindWithTag("WayPointState").GetComponent<WayPointState>().setMessage("경로 탐색 완료");

        yield return new WaitForSeconds(1f);

        GameObject.FindWithTag("WayPointState").GetComponent<WayPointState>().setMessage("");

        AppStateManager.Instance.ChangeState(AppStateType.MENU);

        WayPointManager.Instance.clearPoint();

        yield return null;
    
    }
    
}
