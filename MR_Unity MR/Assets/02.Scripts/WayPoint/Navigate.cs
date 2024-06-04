using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate : Singleton<Navigate>
{

    [SerializeField] Vector3 foward;

    private void Start()
    {
        foward = transform.forward;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 target = WayPointManager.Instance.nowPoint().transform.position;

        Vector3 dir = target - transform.position;

        dir.y = 0f; foward.y = 0f;

        Debug.Log(foward + "  " + dir);

        Quaternion rotate = Quaternion.FromToRotation(foward, dir);

        Vector3 eulerAngles = rotate.eulerAngles;

        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 0.8f, Camera.main.transform.position.z);
        transform.rotation = rotate;
    }

    public void OnOff(bool togle)
    {
        if (togle) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }

}
