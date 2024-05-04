using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

using UnityEngine.XR;

public class GeoTesting : MonoBehaviour
{
    [SerializeField] GameObject InitPivotObj;

    [SerializeField] InputActionReference initInputReference;
    [SerializeField] InputActionReference createInputReference;
    [SerializeField] TMP_Text text;
    //float dTrigger = 0f;

    [Space(10f)]
    [SerializeField] GameObject tempObj;
    GameObject posCheckObj = null;

    [Space(10f)]
    [SerializeField] double pivotLon;
    [SerializeField] double pivotLat;
    [SerializeField] double pivotGeoDir = 0d;

    [Space(10f)]
    //[SerializeField] double testLon1;
    //[SerializeField] double testLat1;
    [SerializeField] List<Double2Position> testGeoPositions = new List<Double2Position>();

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {

        index = 0;
        //Double2Position tmPos = GeoTransformManager.Instance.TransformGeoToTM(testLon, testLat);

        //Debug.Log("X : " + tmPos.x + "\nY : "+ tmPos.y);
        posCheckObj = null;

        //Input.gyro.enabled = true;

        initInputReference.action.started += InitPivot;

        createInputReference.action.started += CreateObj;

        GeoTransformManager.Instance.InitPivot(pivotLat, pivotLon, pivotGeoDir, InitPivotObj.transform);

        Debug.Log(testGeoPositions[0].y + "\n" + testGeoPositions[0].x);

        Double2Position pos = GeoTransformManager.Instance.TransformGeoToTM(testGeoPositions[0].y, testGeoPositions[0].x);

        Debug.Log(pos.x + "\n" + pos.y);

        pos = GeoTransformManager.Instance.TransformTMToGeo(pos.x, pos.y);

        Debug.Log(pos.x + "\n" + pos.y);

        pos = GeoTransformManager.Instance.TransformGeoToTM(pos.x, pos.y);

        Debug.Log(pos.x + "\n" + pos.y);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(triggerInputReference.action.ReadValue<float>() > 0 && dTrigger == 0)
        {
            //기준점 임시 세팅(바라보는 방향 북쪽으로 고정)
            GeoTransformManager.Instance.InitPivot(pivotLon,pivotLat,0,Camera.main.transform);

            GeoTransform obj = Instantiate(tempObj).AddComponent<GeoTransform>();
            obj.Init(testLon1, testLat1);

            string debug = "";
            debug += "Geo Rot : " + GeoTransformManager.Instance.PivotGeoRotation.ToString("F2") + "\n";
            debug += "Unity Rot : " + GeoTransformManager.Instance.PivotUnityRotation.ToString("F2") + "\n";
            debug += "Dir : " + (obj.Position_TM.x - GeoTransformManager.Instance.PivotTMPosition.x).ToString("F2") + " " + (obj.Position_TM.y - GeoTransformManager.Instance.PivotTMPosition.y).ToString("F2");

            text.text = debug;
        }

        dTrigger = triggerInputReference.action.ReadValue<float>();
        */

        Quaternion rot;
        

        if(TryGetCenterEyeFeature(out rot))
            text.text = rot.eulerAngles.ToString();
    }

    private void OnDisable()
    {
        initInputReference.action.started -= InitPivot;

        createInputReference.action.started -= CreateObj;
    }

    bool TryGetCenterEyeFeature(out Quaternion rotation)
    {
        UnityEngine.XR.InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        if (device.isValid)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.centerEyeRotation, out rotation))
                return true;
        }
        // This is the fail case, where there was no center eye was available.
        rotation = Quaternion.identity;
        return false;
    }

    public void InitPivot(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        GeoTransformManager.Instance.InitPivot(pivotLat, pivotLon, pivotGeoDir, InitPivotObj.transform);

        string debug = "";
        debug += "Geo Rot : " + GeoTransformManager.Instance.PivotGeoRotation.ToString("F2") + "\n";
        debug += "Unity Rot : " + GeoTransformManager.Instance.PivotUnityRotation.ToString("F2") + "\n";
        debug += "Geo Piv : " + GeoTransformManager.Instance.PivotGeoPosition.x.ToString("F4") + " " + GeoTransformManager.Instance.PivotGeoPosition.y.ToString("F4") + "\n";
        debug += "Unity Piv : " + GeoTransformManager.Instance.PivotUnityPosition.x.ToString("F4") + " " + GeoTransformManager.Instance.PivotUnityPosition.y.ToString("F4") + "\n";

        text.text = debug;
    }

    public void CreateObj(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (posCheckObj == null)
        {
            posCheckObj = Instantiate(tempObj);
            posCheckObj.AddComponent<GeoTransform>();
        }
        GeoTransform geoTransform = posCheckObj.GetComponent<GeoTransform>();

        geoTransform.Init(testGeoPositions[index].y, testGeoPositions[index].x);

        index = (index + 1) % testGeoPositions.Count;

        string debug = "";
        //debug += "Unity Pos : " + geoTransform.Position_TM.x.ToString("F4") + " " + geoTransform.Position_TM.y.ToString("F4") + "\n";
        debug += "Unity Dir : " + (geoTransform.Position_Unity.x - GeoTransformManager.Instance.PivotUnityPosition.x).ToString("F2") + " " + (geoTransform.Position_Unity.y - GeoTransformManager.Instance.PivotUnityPosition.y).ToString("F2") + "\n";
        debug += "TM Dir : " + (geoTransform.Position_TM.x - GeoTransformManager.Instance.PivotTMPosition.x).ToString("F2") + " " + (geoTransform.Position_TM.y - GeoTransformManager.Instance.PivotTMPosition.y).ToString("F2") + "\n";

        text.text = debug;
    }
}
