using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class InitState : BasicAppState
{
    [Space(15f)]
    [SerializeField] TMP_Text describeText;

    [Space(15f)]
    [SerializeField] InputActionReference initInputReference;
    [SerializeField] double pivotLat;
    [SerializeField] double pivotLon;
    [SerializeField] double pivotGeoDir = 0f;
    [SerializeField] Transform pivotObj;

    public override void StartState()
    {
        initInputReference.action.started += InitPivot;

        describeText.text = "Click Right A button\nwhile looking north";

        base.StartState();
    }

    public override void EndState()
    {
        initInputReference.action.started -= InitPivot;

        base.EndState();
    }

    public void InitPivot(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (GPS.Instance.locationEnabled)
        {
            GeoTransformManager.Instance.InitPivot(GPS.Instance.latitude, GPS.Instance.longitude, pivotGeoDir, pivotObj);
        }
        else
        {
            GeoTransformManager.Instance.InitPivot(pivotLat, pivotLon, pivotGeoDir, pivotObj);
        }

        string text = "Initialize finished\n";

        text += "Pivot Lat : " + GeoTransformManager.Instance.PivotGeoPosition.x.ToString("F5") + "\n";
        text += "Pivot lon : " + GeoTransformManager.Instance.PivotGeoPosition.y.ToString("F5") + "\n";
        text += "Accuracy : " + GPS.Instance.accuracy;

        describeText.text = text;

    }
}
