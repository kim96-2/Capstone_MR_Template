using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointState : BasicAppState
{
    // Start is called before the first frame update
    public override void EndState()
    {
        GetComponent<WayPointManager>().clearPoint();

        base.EndState();
    }
}
