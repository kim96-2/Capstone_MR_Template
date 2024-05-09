using System;
using UnityEngine;

public class GPS : MonoBehaviour
{
    private void Start()
    {
        // 위치 서비스 활성화 여부 확인
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("위치 서비스가 활성화되어 있지 않습니다.");
            return;
        }
        
        Input.location.Start();
        
        
    }
}