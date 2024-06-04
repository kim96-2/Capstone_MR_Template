using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : BasicAppState
{
    //[Space(15f)]
    //[SerializeField] Button searchButton;


    [Space(15f)]
    [SerializeField]
    Michsky.MUIP.ButtonManager searchButton;


    public override void StartState()
    {
        if (GeoTransformManager.Instance.IsInited)
        {
            if (MiniMapManager.Instance)
                MiniMapManager.Instance.SetDefaultMap();

            searchButton.isInteractable = true;
        }
        else
        {
            searchButton.isInteractable = false;
        }

        base.StartState();
    }
}
