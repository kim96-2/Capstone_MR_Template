using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Contents : MonoBehaviour
{

    [SerializeField] private PlaceInfo info;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI addressText;

    void Start()
    {
        nameText.text = info.name;
        addressText.text = info.address;
    }

    void Update()
    {
        
    }
}
