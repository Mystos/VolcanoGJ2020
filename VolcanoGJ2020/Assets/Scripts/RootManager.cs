using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    public LayerMask treeLayer;
    public GameObject rootPrefab;
    public GameObject rootHandlePrefab;
    public Camera camera;

    //Root building
    bool isPlacing = false;
    Tree sourceTree;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit,100000, treeLayer))
            {
                
                GameObject treeGo = hit.transform.gameObject;
                Transform objectHit = hit.transform;

                Debug.Log("Object found : " + objectHit.gameObject);
                GameObject rootGo = Instantiate(rootPrefab, hit.point, Quaternion.identity);
                Root root = rootGo.GetComponent<Root>();

                // Do something with the object that was hit by the raycast.
            }
        }
    }
}
