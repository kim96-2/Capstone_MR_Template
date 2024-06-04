using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate : Singleton<Navigate>
{
    [SerializeField] Vector3 offset;
    Transform center;

    //[SerializeField] Vector3 foward;

    private void Start()
    {
        //foward = transform.forward;
        this.gameObject.SetActive(false);

        center = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (WayPointManager.Instance.nowPoint())
        {
            Vector3 foward = center.forward;
            foward.y = 0f;

            Debug.DrawRay(transform.position, foward, Color.red);

            Vector3 right = center.right;
            right.y = 0f;

            Debug.DrawRay(transform.position, right, Color.red);

            Vector3 calCenter = center.position + foward * offset.z + right * offset.x + Vector3.up * offset.y;

            Vector3 target = WayPointManager.Instance.nowPoint().transform.position;

            Vector3 dir = target - calCenter;

            dir.y = 0f;

            //Debug.Log(dir);

            Quaternion rotate = Quaternion.FromToRotation(Vector3.forward, dir);

            transform.position = calCenter;
            transform.rotation = rotate;

        }
    }

    public void OnOff(bool togle)
    {
        if (togle) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }

}
