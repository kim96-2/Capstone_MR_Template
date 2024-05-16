using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KakaoAPI.Instance.AddQuery("query", "세종대학교");
        KakaoAPI.Instance.SearchByKeyword(APITest<Place>);
    }

    public void APITest<T>(string data)
    {
        Debug.Log(data);
    }
}
