using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRessources : MonoBehaviour
{
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI mineralsText;
    public TextMeshProUGUI nbrHeartText;
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        waterText.text = GameManager.Instance.water.ToString();
        mineralsText.text = GameManager.Instance.minerals.ToString();
        nbrHeartText.text = GameManager.Instance.treeLevel.ToString();
        scoreText.text = GameManager.Instance.Score.ToString();
    }
}
