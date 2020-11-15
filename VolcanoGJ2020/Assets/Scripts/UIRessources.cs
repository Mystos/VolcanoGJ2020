using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRessources : MonoBehaviour
{
    
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI mineralsText;

    private void Update()
    {
        waterText.text = "Water : " + GameManager.Instance.water;
        mineralsText.text = "Minerals : " + GameManager.Instance.minerals;
    }
}
