using System;
using System.IO;
using RestAPI.GoogleObject;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public Double2Position pos1;
    public Double2Position pos2;
    private string result = "";
    public string para = "";
    public bool flag = false;
    public string filePath;
    
    void Start()
    {
        DirectionAPI.Instance.DirectionTo(pos1, pos2, s =>
        {
            DirectionAPI.Instance.DebugFunc(s);
        });
    }

    private void Update()
    {
        

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