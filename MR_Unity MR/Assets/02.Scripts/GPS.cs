using System;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public bool locationEnabled { get; private set; } = false;
    public float latitude { get; private set; }
    public float longitude { get; private set; }
    public float altitude { get; private set; }
    public float accuracy { get; private set; }

    void Start()
    {
        // 현재 디바이스 상 위치 서비스 활성화 여부 확인
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled on the device.");
            return;
        }

        // Request location updates
        Input.location.Start(5f, 10f);
    }

    void Update()
    {
        // Check if we have a new location update
        if (Input.location.status == LocationServiceStatus.Running)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            altitude = Input.location.lastData.altitude;
            accuracy = Input.location.lastData.horizontalAccuracy;
        }
        // 위치 서비스 다시 시도
        else if (Input.location.isEnabledByUser)
        {
            // Request location updates
            Input.location.Start(5f, 10f);
        }
    }
}