using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WayPointState : BasicAppState
{

    [SerializeField] TextMeshProUGUI message;

    public void setMessage(string _message) {  message.text = _message; }

    // Start is called before the first frame update
    public override void StartState()
    {
        base.StartState();

        WayPointManager.Instance.getWayPoint();
    }
    public override void EndState()
    {
        WayPointManager.Instance.clearPoint();

        base.EndState();
    }

}
