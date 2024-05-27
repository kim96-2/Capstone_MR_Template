using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 상태가 되기 위한 인터페이스
/// </summary>
public interface IAppState
{
    public void StartState();

    public void EndState();
}
