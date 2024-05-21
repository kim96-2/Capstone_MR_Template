using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본적으로 사용할 수 있는 상태 클래스(물론 다른 상태를 제작해도 문제는 없을 듯)
/// </summary>
public class BasicAppState : MonoBehaviour, IAppState
{
    [SerializeField] protected AppStateType stateType;//현 상태의 타입

    [SerializeField] protected GameObject defaultUI;

    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Init()
    {
        //현 상태를 상태 메니저에 정의
        if (AppStateManager.Instance) AppStateManager.Instance.SetState(stateType, this);

        //기본 UI 오브젝트를 UI Offset에 위치하게 변경
        Transform uiOffset = GameObject.FindWithTag("AppUIOffset").transform;
        defaultUI.transform.SetParent(uiOffset);
        defaultUI.transform.localPosition = Vector3.zero;
        defaultUI.transform.localRotation = Quaternion.identity;

        //기본 UI 꺼두기
        defaultUI.SetActive(false);

    }

    #region IAppState Setting
    
    public virtual void StartState()
    {
        //기본 UI 켜주기
        defaultUI.SetActive(true);
    }

    public virtual void EndState()
    {
        //기본 UI 꺼주기
        defaultUI.SetActive(false);
    }

    #endregion IAppState Setting
}
