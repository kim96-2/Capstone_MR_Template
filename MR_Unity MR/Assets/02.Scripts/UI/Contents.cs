using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

public class Contents : MonoBehaviour
{

    [SerializeField] private Place info;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI phoneText;
    [SerializeField] private TextMeshProUGUI addressText;
    [SerializeField] private TextMeshProUGUI road_addressText;

    void Start()
    {
        // KakaoAPI.Instance.SearchByCategory(getResult<Place>);
    }

    void Update()
    {
       
    }

    public Place getPlace() { return info; }
    public void setPlace(Place place) { info = place; } 

    public void changeContents(Place place)
    {
        info.place_name = nameText.text = place.place_name;
        info.phone = phoneText.text = place.phone;
        info.address_name = addressText.text = place.address_name;
        info.road_address_name = road_addressText.text = place.road_address_name;
    }

    public void clickThis() { InformationUI.Instance.MoreInfo(info); }

}
