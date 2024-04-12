using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GeoTesting : MonoBehaviour
{
    [SerializeField] InputActionReference triggerInputReference;
    [SerializeField] TMP_Text text;
    float dTrigger = 0f;

    [Space(10f)]
    [SerializeField] GameObject tempObj;

    [Space(10f)]
    [SerializeField] double testLon;
    [SerializeField] double testLat;

    [Space(10f)]
    [SerializeField] double testLon1;
    [SerializeField] double testLat1;

    // Start is called before the first frame update
    void Start()
    {
        //Double2Position tmPos = GeoTransformManager.Instance.TransformGeoToTM(testLon, testLat);

        //Debug.Log("X : " + tmPos.x + "\nY : "+ tmPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerInputReference.action.ReadValue<float>() > 0 && dTrigger == 0)
        {
            //기준점 임시 세팅(바라보는 방향 북쪽으로 고정)
            GeoTransformManager.Instance.InitPivot(testLon,testLat,0,Camera.main.transform);

            GeoTransform obj = Instantiate(tempObj).AddComponent<GeoTransform>();
            obj.Init(testLon1, testLat1);

            string debug = "";
            debug += "Geo Rot : " + GeoTransformManager.Instance.PivotGeoRotation.ToString("F2") + "\n";
            debug += "Unity Rot : " + GeoTransformManager.Instance.PivotUnityRotation.ToString("F2") + "\n";
            debug += "Dir : " + (obj.Position_TM.x - GeoTransformManager.Instance.PivotTMPosition.x).ToString("F2") + " " + (obj.Position_TM.y - GeoTransformManager.Instance.PivotTMPosition.y).ToString("F2");

            text.text = debug;
        }

        dTrigger = triggerInputReference.action.ReadValue<float>();
    }
}
