using System;
using System.IO;
using RestAPI.GoogleObject;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public string lat;
    public string lng;
    private string result = "";
    public string detail = "";
    public bool flag = false;
    public string filePath;
    
    void Start()
    {
        GoogleAPI.Instance.SearchNearby(Double.Parse(lat), Double.Parse(lng), 500, (s => result = s));
    }

    private void Update()
    {
        if (result == "")
        {
            Debug.Log("Wait...");
        }
        if (result != "" && !flag)
        {
            flag = true;
            NearbyResponse res = GoogleAPI.Instance.ParseNearbyRes(result);
            
            GoogleAPI.Instance.PlaceDetail(res.results[4].place_id, s => detail = s);
        }

        if (detail != "")
        {
            Debug.Log(detail);
        }

    }
    
    // 문자열을 지정된 경로의 .txt 파일로 저장하는 함수
    public void SaveStringToFile(string content)
    {
        try
        {
            // 파일에 문자열을 쓰기
            File.WriteAllText(Application.dataPath + filePath, content);
            Debug.Log("File saved successfully at: " + filePath);
        }
        catch (Exception e)
        {
            // 오류 발생 시 메시지 출력
            Debug.LogError("Error saving file: " + e.Message);
        }
    }
}