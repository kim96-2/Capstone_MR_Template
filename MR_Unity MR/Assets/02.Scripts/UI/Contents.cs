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
    [SerializeField] private TextMeshProUGUI place_nameText;
    [SerializeField] private TextMeshProUGUI category_nameText;
    [SerializeField] private TextMeshProUGUI phoneText;
    [SerializeField] private TextMeshProUGUI address_nameText;
    [SerializeField] private TextMeshProUGUI road_address_nameText;

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
        info.place_name = place_nameText.text = place.place_name;
        info.category_name = category_nameText.text = place.category_name;
        info.phone = phoneText.text = place.phone;
        info.address_name = address_nameText.text = place.address_name;
        info.road_address_name = road_address_nameText.text = place.road_address_name;
    }

    public void clickThis() { InformationUI.Instance.MoreInfo(info); }
 
}
