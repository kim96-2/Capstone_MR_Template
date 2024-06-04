using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using RestAPI.KakaoObject;
using Unity.Collections;

public class Contents : MonoBehaviour
{

    [ReadOnly,SerializeField] protected Place info;
    [SerializeField] protected TextMeshProUGUI place_nameText;
    [SerializeField] protected TextMeshProUGUI category_nameText;
    [SerializeField] protected TextMeshProUGUI phoneText;
    [SerializeField] protected TextMeshProUGUI address_nameText;
    [SerializeField] protected TextMeshProUGUI road_address_nameText;

    public Place getPlace() { return info; }
    public void setPlace(Place place) { info = place; } 

    public void changeContents(Place place)
    {

        setPlace(place);

        /*
        info.place_name = place_nameText.text = place.place_name;
        info.category_name = category_nameText.text = place.category_name;
        info.phone = phoneText.text = place.phone;
        info.address_name = address_nameText.text = place.address_name;
        info.road_address_name = road_address_nameText.text = place.road_address_name;
        */

        place_nameText.text = place.place_name;
        category_nameText.text = place.category_name;
        phoneText.text = place.phone;
        address_nameText.text = place.address_name;
        road_address_nameText.text = place.road_address_name;
    }

    public void clickThis() { InformationUI.Instance.MoreInfo(info); }        

    //임시
    //public void clickforPath() { MiniMapManager.Instance.SetPathMap(info); }
}
