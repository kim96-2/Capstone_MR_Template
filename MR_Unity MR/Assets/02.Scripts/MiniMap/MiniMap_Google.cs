using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MiniMap_Google : MonoBehaviour
{
    public Image mapRawImage;

    //37.5518018, 127.0736345
    [Header("Map Information")]
    public string strBaseURL = "";
    public string Minimap_lat;
    public string Minimap_lon;
    public int zoom = 16;
    public int Minimap_width;
    public int Minimap_height;
    public string Minimap_APIKey = "";


    private string Minimap_latLast;
    private string Minimap_lonLast;
    private int zoomLast;
    private bool updateMap = true;
    private bool latUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        mapRawImage = GetComponent<Image>();
        StartCoroutine(MapLoader());
    }

    // Update is called once per frame
    void Update()
    {
        if(updateMap &&(Minimap_latLast != Minimap_lat || Minimap_lonLast != Minimap_lon || zoomLast != zoom))
        {
            mapRawImage = GetComponent<Image>();
            StartCoroutine (MapLoader());
            updateMap = false;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            double tmp = double.Parse(Minimap_lat);
            
            tmp += 0.0001000;

            Minimap_lat = tmp.ToString();
            Debug.Log(Minimap_lat);
        }
    }

    IEnumerator MapLoader()
    {
        string str = strBaseURL + Minimap_lat + "," + Minimap_lon + "&format=png&zoom=" + zoom + "&size=" + Minimap_width + "x" + Minimap_height + "&scale=2" + "&markers=color:blue%7C" + Minimap_lat + "," + Minimap_lon + "&key=" + Minimap_APIKey;
        Debug.Log(str.ToString());

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(str);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            //mapRawImage.texture = DownloadHandlerTexture.GetContent(request);
            mapRawImage.material.SetTexture("_MainTex1", DownloadHandlerTexture.GetContent(request));

            Minimap_latLast = Minimap_lat;
            Minimap_lonLast = Minimap_lon;
            zoomLast = zoom;
            updateMap = true;
        }
    }
}
