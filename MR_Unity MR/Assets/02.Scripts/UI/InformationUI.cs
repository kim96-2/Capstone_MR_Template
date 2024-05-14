using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// 주변 정보를 띄우는 UI
public class InformationUI : MonoBehaviour
{

    [SerializeField] private GameObject content;
    [SerializeField] private GameObject placeInfo;
    [SerializeField] private Scrollbar scroll;

    private bool scrollDown;

    [SerializeField] private List<GameObject> places;

    void Start()
    {
        scrollDown = true;
        AddInfo();
    }

    void Update(){
        
    }

    // 스크롤 맨 아래로 내릴 시 크기 늘려줌
    public void EndDrag()
    {
        if (scroll.value < 0 && scrollDown) {

            scrollDown = false;
            
            StartCoroutine("ContentSizeUp");

        }
    }

    public void AddInfo()
    {
        for (int i = 0; i <= 5; i++) Instantiate(placeInfo, content.transform);
    }

    // 한 번에 여러번 안늘어나게 조절
    IEnumerator ContentSizeUp()
    {

        RectTransform transform = content.GetComponent<RectTransform>();

        transform.sizeDelta = new Vector2(0, transform.sizeDelta.y + 500f);
        AddInfo();

        yield return new WaitForSeconds(0.5f);

        scrollDown = true;

        yield return null;
    }

}
