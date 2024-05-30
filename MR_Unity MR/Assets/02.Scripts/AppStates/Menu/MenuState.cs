using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : BasicAppState
{
    [Space(15f)]
    [SerializeField] Button searchButton;

    public override void StartState()
    {
        if (GeoTransformManager.Instance.IsInited)
        {
            searchButton.interactable = true;
        }
        else
        {
            searchButton.interactable = false;
        }

        base.StartState();
    }
}
