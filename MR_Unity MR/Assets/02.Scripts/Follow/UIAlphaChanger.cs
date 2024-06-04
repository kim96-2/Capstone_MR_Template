using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIAlphaChanger : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] InputActionReference onOffReference;

    [SerializeField] float _UILocalSize = 0.5f;

    bool on;

    // Start is called before the first frame update
    void Start()
    {
        on = false;

        TurnOnOff();

        onOffReference.action.started += OnOffButtonClick;

        transform.localScale = transform.localScale * _UILocalSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        onOffReference.action.started -= OnOffButtonClick;
    }

    void OnOffButtonClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Debug.Log("Button Click");

        on = !on;

        TurnOnOff();
    }

    void TurnOnOff()
    {
        if (on)
        {
            //켜주기
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

        }
        else
        {
            
            //꺼주기
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

        }
    }
}
