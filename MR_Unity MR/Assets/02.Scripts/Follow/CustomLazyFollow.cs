using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 커스텀으로 만든 레이지 팔로우
/// </summary>
public class CustomLazyFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    [SerializeField] float followSpeed = 10f;

    [Space(15f)]
    [SerializeField] float rotationSpeedThreshold = 20f;
    Vector3 lastTargetFoward_forRotSpeedCheck;

    [SerializeField] float rotationAngleThreshold = 20f;
    Vector3 lastTargetFoward_forAngleCheck;

    [SerializeField] Transform followTarget;
    Transform target;

    //타겟의 바라보는 방향
    Vector3 foward;
    Vector3 right;
    Vector3 up = Vector3.up;

    //실제 타겟이 바라볼 때 사용할 방향
    Vector3 targetFoward;
    Vector3 targetRight;
    Vector3 targetUp;

    // Start is called before the first frame update
    void Start()
    {
        if(!target) target = Camera.main.transform;

        if (followTarget == null) followTarget = target;

        UpdateLocalDirection();

        targetFoward = foward;
        targetRight = right;
        targetUp = up;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLocalDirection();

        if(CheckRotationSpeed() || CheckRotationAngle())
        {
            targetFoward = foward;
            targetRight = right;
            targetUp = up;

            lastTargetFoward_forAngleCheck = foward;
        }

        UpdatePosition();
        UpdateRotation();
    }

    #region Check Setting

    bool CheckRotationSpeed()
    {
        bool result;

        float check = Quaternion.FromToRotation(lastTargetFoward_forRotSpeedCheck, foward).eulerAngles.y;
        if (check > 180f) check = check - 360f;

        if (Mathf.Abs(check) > rotationSpeedThreshold * Time.deltaTime)
        {
            result = true;
        }
        else result = false;

        lastTargetFoward_forRotSpeedCheck = foward;

        return result;
    }

    bool CheckRotationAngle()
    {
        bool result;

        float check = Quaternion.FromToRotation(lastTargetFoward_forAngleCheck, foward).eulerAngles.y;
        if (check > 180f) check = check - 360f;

        if (Mathf.Abs(check) > rotationAngleThreshold)
        {
            result = true;
        }
        else result = false;

        return result;
    }

    #endregion Check Setting

    void UpdateLocalDirection()
    {
        foward = target.forward;
        foward.y = 0f;//y 값 방향 지워줌
        foward.Normalize();

        //Debug.Log(foward);

        right = target.right;
        right.y = 0f;
        right.Normalize();

        up = Vector3.up;//위 방향은 그냥 위 방향

    }

    void UpdatePosition()
    {
        Vector3 pos = followTarget.position;

        pos += targetFoward * offset.z;
        pos += targetRight * offset.x;
        pos += targetUp * offset.y;

        transform.position = Vector3.Lerp(transform.position, pos, followSpeed * Time.deltaTime);
    }

    void UpdateRotation()
    {
        //Vector3 thisFoward = target.forward;
        //thisFoward.y = 0f;
        //thisFoward.Normalize();

        Vector3 dir = transform.position - target.position;
        dir.y = 0f;
        dir.Normalize();

        Quaternion rotation = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation,rotation, followSpeed * Time.deltaTime);
    }
}
