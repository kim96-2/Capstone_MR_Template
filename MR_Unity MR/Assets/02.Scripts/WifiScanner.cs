using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class WifiScanData
{
    public string macAddress { get; set; }
    public int signalStrength { get; set; }  // dBm
    public double signalToNoiseRatio { get; set; }  // SNR (dB)
    public int channel { get; set; }  // 주파수 (MHz)
    public long age { get; set; }  // 타임스탬프 (시스템 부팅 후의 시간)
}

public class WifiScanner : MonoBehaviour
{
    private AndroidJavaObject _wifiManager;
    public List<WifiScanData> scanDataList { get; private set; } = new List<WifiScanData>();

    void Start()
    {
        // Android의 Context 가져오기
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        // WifiManager 객체 생성
        _wifiManager = context.Call<AndroidJavaObject>("getSystemService", "wifi");

        // Wi-Fi 스캔 시작
        StartWifiScan();
    }

    void StartWifiScan()
    {
        // Wi-Fi 스캔 시작
        _wifiManager.Call<bool>("startScan");
        
        // 스캔 결과 가져오기
        AndroidJavaObject scanResults = _wifiManager.Call<AndroidJavaObject>("getScanResults");

        // 결과 저장
        int size = scanResults.Call<int>("size");
        for (int i = 0; i < size; i++)
        {
            AndroidJavaObject scanResult = scanResults.Call<AndroidJavaObject>("get", i);

            WifiScanData data = new WifiScanData
            {
                macAddress = scanResult.Get<string>("BSSID"),
                signalStrength = scanResult.Get<int>("level"),
                channel = FrequencyToChannel(scanResult.Get<int>("frequency")),
                age = scanResult.Get<long>("timestamp"),
                signalToNoiseRatio = CalculateSNR(scanResult.Get<int>("level"))
            };

            scanDataList.Add(data);
        }

        // 결과를 JSON 형식으로 변환
        string jsonData = CreateJsonData();
        Debug.Log(jsonData);
    }
    
    
    int FrequencyToChannel(int frequency)
    {
        // 주파수를 채널로 변환하는 간단한 예시 (실제 채널 변환 로직은 복잡)
        // 이 예시는 2.4GHz 범위의 주파수만 고려
        if (frequency >= 2412 && frequency <= 2484)
        {
            return (frequency - 2412) / 5 + 1;
        }
        return 0;  // 잘못된 주파수
    }
    
    double CalculateSNR(int signalStrength)
    {
        // 잡음을 0dBm으로 가정하여 SNR 계산
        int noiseLevel = 0;  // 잡음 (dBm)
        return signalStrength - noiseLevel;
    }

    
    string CreateJsonData()
    {
        // JSON 형식으로 데이터 변환
        var jsonData = new
        {
            considerIp = "false",
            wifiAccessPoints = scanDataList.Select(data => new
            { 
                data.macAddress,
                data.signalStrength,
                data.signalToNoiseRatio,
                data.channel,
                data.age
            }).ToList()
        };

        // JSON 문자열로 직렬화
        return JsonConvert.SerializeObject(jsonData, Formatting.Indented);
    }
}
