using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Utils
{
    public static T PickRandom<T>(T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }
    
    public static Vector3 FindIntercept(Vector3 origin1, float speed1, Vector3 targetPos, Vector3 targetVel)
    {
        Vector3 dirToTarget = Vector3.Normalize(targetPos - origin1);

        Vector3 targetVelOrth = Vector3.Dot(targetVel, dirToTarget) * dirToTarget;

        Vector3 targetVelTang = targetVel - targetVelOrth;
        Vector3 shotVelTang = targetVelTang;

        float shotVelSpeed = shotVelTang.magnitude;

        if (shotVelSpeed > speed1)
        {
            return Vector3.zero; // too slow to intercept target, it will never catch up, so return zero
        }
        else
        {
            float shotSpeedOrth = Mathf.Sqrt(speed1 * speed1 - shotVelSpeed * shotVelSpeed);
            Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;

            float timeToCollision = ((origin1 - targetPos).magnitude) / (shotVelOrth.magnitude - targetVelOrth.magnitude);

            Vector3 shotVel = shotVelOrth + shotVelTang;
            return origin1 + shotVel * timeToCollision;
        }
    }
}