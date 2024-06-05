using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WayPoint : MonoBehaviour
{

    [SerializeField] private GameObject prevPoint;
    [SerializeField] private GameObject nextPoint;
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private LineRenderer line;

    [SerializeField] float checkRange = 3f;
    
    [Space(15f)]
    [SerializeField] Transform placeUI;//배치 되는 UI

    [Space(15f)]
    [SerializeField] float minRange = 1f;
    [SerializeField] float maxRange = 100f;
    [Space(5f)]
    [SerializeField] float _UIMinSize = 1f;
    [SerializeField] float _UIMaxSize = 40f;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI description;

    public bool isDrawing = false;

    void Awake()
    {

        targetPoint = GameObject.FindWithTag("MainCamera");
        line = GetComponent<LineRenderer>();

    }

    public void Update()
    {

        Vector3 dir = transform.position - Camera.main.transform.position;
        dir.y = 0f;

        float dis = dir.magnitude;

        float value = Mathf.Clamp(dis, minRange, maxRange);
        value = (dis - minRange) / (maxRange - minRange);

        placeUI.transform.localScale = Vector3.one * Mathf.Lerp(_UIMinSize, _UIMaxSize, value);

        // Debug.Log(Vector3.Distance(this.transform.position, targetPoint.transform.position));
        
        DrawWay();

        transform.position = new Vector3(transform.position.x, targetPoint.transform.position.y, transform.position.z);

        // 현재 WayPoint로 Player가 접근 시 다음 WayPoint까지 Line 생성
        if (Vector3.Distance(this.transform.position, targetPoint.transform.position) < checkRange && isDrawing)
        {

            Debug.Log("aa");

            line.enabled = false;

            if (!nextPoint)
            {

                StartCoroutine("Goal");
                
                return;

            }

            // 다음 경로 설정

            WayPointManager.Instance.nowPoint(nextPoint);

            nextPoint.SetActive(true);
            nextPoint.GetComponent<WayPoint>().setPrevPoint(null);

            GameObject.FindWithTag("WayPointState").GetComponent<WayPointState>().setMessage(nextPoint.GetComponentInChildren<TextMeshProUGUI>().text);

            // nextPoint.GetComponent<WayPoint>().isDrawing = true;

            gameObject.SetActive(false);

        }


    }

    public void setPrevPoint(GameObject _prevPoint) { prevPoint = _prevPoint; }
    public void setNextPoint(GameObject _nextPoint) { nextPoint = _nextPoint; }

    public void Init(Double2Position pos, string message)
    {

        line = GetComponent<LineRenderer>();
        GetComponent<GeoTransform>().Init(pos.x, pos.y);
        isDrawing = true;
        description.text = message;

    }

    /// <summary>
    /// 다음 WayPoint까지 Line 생성하는 함수
    /// <summary>
    public void DrawWay()
    {

        // Line 크기 조정
        //line.SetWidth(0.2f, 0.2f);

        // 현재 WayPoint까지 Line 생성
        
        if(WayPointManager.Instance.wayType() == WayType.ONESTEP)
        {

            line.SetPosition(0, new Vector3(targetPoint.transform.position.x, targetPoint.transform.position.y - 1f, targetPoint.transform.position.z));
            line.SetPosition(1, new Vector3(this.transform.position.x, targetPoint.transform.position.y - 1f, this.transform.position.z));

        }
        else if(WayPointManager.Instance.wayType() == WayType.ALL)
        {

            if (!prevPoint) line.SetPosition(0, new Vector3(targetPoint.transform.position.x, targetPoint.transform.position.y - 1f, targetPoint.transform.position.z));
            else line.SetPosition(0, new Vector3(prevPoint.transform.position.x, targetPoint.transform.position.y - 1f, prevPoint.transform.position.z));

            line.SetPosition(1, new Vector3(this.transform.position.x, targetPoint.transform.position.y - 1f, this.transform.position.z));

        }

    }

    IEnumerator Goal() {

        isDrawing = false;

        GameObject.FindWithTag("WayPointState").GetComponent<WayPointState>().setMessage("경로 탐색 완료");

        yield return new WaitForSeconds(1f);

        GameObject.FindWithTag("WayPointState").GetComponent<WayPointState>().setMessage("");

        AppStateManager.Instance.ChangeState(AppStateType.MENU);

        WayPointManager.Instance.clearPoint();

        yield return null;
    
    }
    
}
