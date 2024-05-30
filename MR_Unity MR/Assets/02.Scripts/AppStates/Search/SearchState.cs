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

    // Update is called once per frame
    void Update()
    {
        
    }
}
