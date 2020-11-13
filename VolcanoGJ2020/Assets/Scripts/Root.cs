using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TraceRoot(Vector3 start, Vector3 end)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    public void ProlongateRoot(Vector3 newPos)
    {
        int k = line.positionCount;
        line.positionCount = k + 1;
        line.SetPosition(k, newPos);
    }

}
