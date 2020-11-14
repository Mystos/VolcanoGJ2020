using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [HideInInspector] public bool isConnected = false;
    List<Tree> connectedTree;

    public float Radius { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.5f, GameManager.Instance.groundLayer);
        Radius = GameManager.Instance.treeRadiusEarth;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.tag == GameManager.Instance.saltGroundTag)
                Radius = GameManager.Instance.treeRadiusSand;
        }
        cols = Physics.OverlapSphere(transform.position, GameManager.Instance.treeRadiusSuperMineral, GameManager.Instance.superMineralLayer);
        if (cols.Length > 0)
            Radius = GameManager.Instance.treeRadiusSuperMineral;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool InRange(Vector3 position)
    {
        return (position - transform.position).magnitude <= Radius;
    }

    private void OnDrawGizmos()
    {
        if (GameManager.Instance == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GameManager.Instance.superMineralCheckRadius);
    }
}
