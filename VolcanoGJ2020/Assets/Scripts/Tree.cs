using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [HideInInspector] public bool isConnected = false;
    List<Tree> connectedTree;
    public TreeType type;
    public float testRadius;

    public float Radius { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.5f, GameManager.Instance.groundLayer);
        Radius = GameManager.Instance.treeRadiusEarth;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.tag == GameManager.Instance.sandGroundTag)
            {
                Radius = GameManager.Instance.treeRadiusSand;
                return;
            }
        }
        cols = Physics.OverlapSphere(transform.position, GameManager.Instance.superMineralCheckRadius, GameManager.Instance.superMineralLayer);
        if (cols.Length > 0)
            Radius = GameManager.Instance.treeRadiusSuperMineral;
    }


    public bool InRange(Vector3 position)
    {
        return (position - transform.position).magnitude <= Radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, testRadius);
        if (GameManager.Instance == null)
            return;

        switch (type)
        {
            case TreeType.Cutting:
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, GameManager.Instance.superMineralCheckRadius);
                break;
            case TreeType.Shield:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, GameManager.Instance.shieldRadius);
                break;
            case TreeType.Sanitizer:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, GameManager.Instance.sanitizeRadius);
                break;
            default:
                break;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Radius);
    
    }

    public enum TreeType
    {
        Cutting = 0,
        Shield = 1,
        Sanitizer = 2
    }

}
