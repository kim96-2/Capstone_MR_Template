using System;
using UnityEngine;
using UnityEngine.Events;

public class GPS : Singleton<GPS>
{
    public bool locationEnabled { get; private set; } = false;
    public float updateDelay;
    private double lastUpdateTime = 0;
    public float latitude { get; private set; }
    public float longitude { get; private set; }
    public float altitude { get; private set; }
    public float accuracy { get; private set; } = -1;

    public UnityEvent OnGPSUpdate;

    void Start()
    {
        // 현재 디바이스 상 위치 서비스 활성화 여부 확인
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled on the device.");
            locationEnabled = false;
            return;
        }

        Input.location.Start(5f, updateDelay);
        locationEnabled = true;
    }

    void Update()
    {
        if (!Input.location.isEnabledByUser)
        {
            return;
        }
        // Check if we have a new location update
        if (Input.location.lastData.timestamp != lastUpdateTime && Input.location.status == LocationServiceStatus.Running)
        {
            OnLocationUpdate();
            OnGPSUpdate?.Invoke();//업데이트 시 함수들 실행
        }
        // 위치 서비스 다시 시도
        else if (Input.location.status == LocationServiceStatus.Failed)
        {
            // Request location updates
            Input.location.Start(5f, updateDelay);
        }
    }


    /// <summary>
    /// 위치 정보 갱신
    /// </summary>
    public void OnLocationUpdate()
    {
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        altitude = Input.location.lastData.altitude;
        accuracy = Input.location.lastData.horizontalAccuracy;

        lastUpdateTime = Input.location.lastData.timestamp;
    }
}