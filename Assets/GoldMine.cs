using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldMine : MonoBehaviour
{
    int currentTimer;
    int availableGold;

    [SerializeField] Gold playersGold;
    [SerializeField] TextMeshProUGUI btnText;
    [SerializeField] Button collectGoldBtn;
    [SerializeField] int goldPerSecond = 2;

    void Start()
    {
        availableGold = PlayerPrefs.GetInt("goldmine");

        CalculateGoldDifference();
        StartCoroutine(Timer());

        collectGoldBtn.onClick.AddListener( delegate { CollectGold(); } );
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("goldmine", availableGold);
    }

    void CalculateGoldDifference()
    {
        availableGold += TimeManager.Singleton.timeDifference * goldPerSecond;
        UpdateGoldText();
    }

    void CollectGold()
    {
        UpdateGoldText();
        playersGold.AddGold(availableGold);
        availableGold = 0;
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        btnText.text = "Collect " + availableGold + " Gold";
    }

    IEnumerator Timer() 
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            availableGold += goldPerSecond;
            UpdateGoldText();
            yield return new WaitForSeconds(1);
        }
    }
}
