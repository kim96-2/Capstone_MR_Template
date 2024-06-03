using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 Canvas 컴포넌트에 넣어주는 함수
public class CanvasCameraAttacher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Canvas>().worldCamera = Camera.main;
    }


}
