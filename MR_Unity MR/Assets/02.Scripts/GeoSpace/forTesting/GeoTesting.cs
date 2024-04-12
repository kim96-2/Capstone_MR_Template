using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GeoTesting : MonoBehaviour
{
    [SerializeField] InputActionReference triggerInputReference;

    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;

    [SerializeField] TMP_Text text;

    [Space(10f)]
    [SerializeField] double testLon;
    [SerializeField] double testLat;

    // Start is called before the first frame update
    void Start()
    {
        Double2Position tmPos = GeoTransformManager.Instance.TransformGeoToTM(testLon, testLat);

        Debug.Log("X : " + tmPos.x + "\nY : "+ tmPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerInputReference.action.ReadValue<float>() > 0)
        {
            float dis = (leftController.transform.position - rightController.transform.position).magnitude;
            text.text = "Distance : " + dis.ToString("F2");
        }
    }
}
