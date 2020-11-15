using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRessources : MonoBehaviour
{
    
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI mineralsText;
    public TextMeshProUGUI nbrHeartText;

    private void Update()
    {
        waterText.text = "Water : " + GameManager.Instance.water;
        mineralsText.text = "Minerals : " + GameManager.Instance.minerals;
        nbrHeartText.text = GameManager.Instance.treeLevel.ToString();
    }
}
