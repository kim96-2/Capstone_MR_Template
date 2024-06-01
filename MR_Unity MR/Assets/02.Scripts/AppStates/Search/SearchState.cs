using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class SearchState : BasicAppState
{
    // Start is called before the first frame update
    public override void StartState()
    {

        if(InformationUI.Instance) InformationUI.Instance.changePlace(true);

        base.StartState();
    }

    public override void EndState()
    {
        InformationUI.Instance.ClearInfo();//스테이트 변경되면 카테고리 지우기

        base.EndState();
    }
}
