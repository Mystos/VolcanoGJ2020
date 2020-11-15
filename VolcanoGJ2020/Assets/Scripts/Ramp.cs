using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    public Transform downTransform;
    public Transform topTransform;

    public Vector3 GetPrimaryPoint(Vector3 point)
    {
        float h1 = Mathf.Abs(point.y - downTransform.position.y);
        float h2 = Mathf.Abs(point.y - topTransform.position.y);
        if (h1 < h2)
            return downTransform.position;
        else
            return topTransform.position;
    }

    public Vector3 GetSecondaryPoint(Vector3 point)
    {
        float h1 = Mathf.Abs(point.y - downTransform.position.y);
        float h2 = Mathf.Abs(point.y - topTransform.position.y);
        if (h1 > h2)
            return downTransform.position;
        else
            return topTransform.position;
    }

}
