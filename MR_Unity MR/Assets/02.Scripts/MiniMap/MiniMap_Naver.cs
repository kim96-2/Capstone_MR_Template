using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MiniMap_Naver : MonoBehaviour
{
    public RawImage mapRawImage;

    [Header("Map Information")]
    public string strBaseURL = "";
    public string Minimap_lat = "";
    public string Minimap_lon = "";
    public int level = 14;
    public int Minimap_width;
    public int Minimap_height;
    public int Minimap_zoom;
    public string Minimap_strAPIKey = "";
    public string Minimap_secretKey = "";


    private string Minimap_latLast;
    private string Minimap_lonLast;
    private int levelLast;
    private bool updateMap = true;

    // Start is called before the first frame update
    void Start()
    {
        mapRawImage = GetComponent<RawImage>();
        StartCoroutine(MapLoader());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MapLoader()
    {
        string str = strBaseURL + "?w=" + Minimap_width.ToString() + "&h=" + Minimap_height.ToString() + "&center=" + Minimap_lon + "," + Minimap_lat + "&level=" + level.ToString()+ "&public_transit&scale=2"+ "&markers=type:d|size:small|pos:" + Minimap_lon + "%20" + Minimap_lat;
        Debug.Log(str.ToString());

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(str);

        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", Minimap_strAPIKey);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", Minimap_secretKey);

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            mapRawImage.texture = DownloadHandlerTexture.GetContent(request);
        }
    }
}
