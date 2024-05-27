using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public enum AppStateType
{
    MENU,
    INIT,
    SEARCH,
    WAYPOINT
}

public class AppStateManager : Singleton<AppStateManager>
{
    //모든 타입에 대한 상태 변수
    Dictionary<AppStateType, IAppState> allStates;

    //현재 상태 타입
    [ReadOnly, SerializeField] AppStateType currentStateType = AppStateType.MENU;

    public AppStateType CurrentStateType { get => currentStateType; }

    public IAppState CurrentState { get => allStates[currentStateType]; }

    protected override void Awake()
    {
        base.Awake();

        allStates = new Dictionary<AppStateType, IAppState>();
    }

    private void Start()
    {
        //실행 시 기본 현재 상태를 시작
        StartState(currentStateType);
    }

    /// <summary>
    /// 각 타입에 대한 상태를 정의하는 함수. 원하는 상태 제작 후 이 함수로 호출 바람
    /// </summary>
    /// <param name="stateType">원하는 상태 타입</param>
    /// <param name="state">제작한 상태</param>
    /// <returns></returns>
    public bool SetState(AppStateType stateType, IAppState state)
    {
        if (allStates.ContainsKey(stateType))
        {
            Debug.LogError("이미 위 타입에 대한 상태가 정의되어 있음 : " + stateType);

            return false;
        }

        allStates[stateType] = state;

        return true;
    }

    /// <summary>
    /// 상태를 변경시켜주는 함수
    /// </summary>
    /// <param name="changeStateType">위 타입의 상태로 변경됨</param>
    public void ChangeState(AppStateType changeStateType)
    {
        //현재 상태 종료
        EndState(currentStateType);

        //현재 상태 변경
        currentStateType = changeStateType;

        //변경된 현재 상태 시작
        StartState(currentStateType);

    }

    void StartState(AppStateType stateType)
    {
        if (!allStates.ContainsKey(stateType))
        {
            Debug.Log("시작하려는 상태가 메니져에 정의가 안되어 있음.");

            return;
        }

        allStates[stateType].StartState();
    }

    void EndState(AppStateType stateType)
    {
        if (!allStates.ContainsKey(stateType))
        {
            Debug.Log("종료하려는 상태가 메니져에 정의가 안되어 있음.");

            return;
        }

        allStates[stateType].EndState();
    }
}
