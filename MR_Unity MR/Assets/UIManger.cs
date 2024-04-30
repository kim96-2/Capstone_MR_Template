using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{

    [SerializeField] private Sprite map;
    [SerializeField] private Image leftUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestAction()
    {
        leftUI.sprite = map;
    }
}
