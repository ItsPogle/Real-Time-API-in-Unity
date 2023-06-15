using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gold : MonoBehaviour
{
    public int myGold { get; private set; }

    [SerializeField] TextMeshProUGUI goldText;

    void Start()
    {
        myGold = PlayerPrefs.GetInt("gold");
        goldText.text = myGold.ToString();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("gold", myGold);
    }

    public void AddGold(int gold)
    {
        myGold += gold;
        goldText.text = myGold.ToString();
    }
}
