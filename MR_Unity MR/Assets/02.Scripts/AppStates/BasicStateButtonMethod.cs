using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스테이트 변경 또는 그 외 진행을 위해 필요한 기능 함수들
/// </summary>
public class BasicStateButtonMethod : MonoBehaviour
{
    [SerializeField] AppStateType changeStateType;

    private void Start()
    {
        if (gameObject.GetComponent<Button>())
        {
            //현재 오브젝트 버튼에 상태 변경 함수를 할당해줌
            gameObject.GetComponent<Button>().onClick.AddListener(ChangeAppState);
        }
        else if (gameObject.GetComponent<Michsky.MUIP.ButtonManager>())
        {
            gameObject.GetComponent<Michsky.MUIP.ButtonManager>().onClick.AddListener(ChangeAppState);
        }
        
    }

    public void ChangeAppState()
    {
        if (AppStateManager.Instance)
        {

            AppStateManager.Instance.ChangeState(changeStateType);

        }
    }
}
