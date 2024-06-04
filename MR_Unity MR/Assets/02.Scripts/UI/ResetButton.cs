using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (!gameObject.GetComponent<Button>()) Debug.LogError(gameObject.name);

        //현재 오브젝트 버튼에 상태 변경 함수를 할당해줌
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonAddReset);

    }

    // Update is called once per frame
    void ButtonAddReset()
    {

        if (WayPointManager.Instance) WayPointManager.Instance.resetWayPoint();

    }
}
