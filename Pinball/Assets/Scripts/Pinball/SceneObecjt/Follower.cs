using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform FollowObj;

    private void Update()
    {
        if(!FollowObj)
        {
            return;
        }
        transform.localPosition = FollowObj.localPosition;
    }
}
