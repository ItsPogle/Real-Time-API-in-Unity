using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Singleton;

    List<int> lastLoginTime = new List<int>();
    List<int> time = new List<int>();

    public bool retrievedTime = false;
    public int timeDifference { get; private set; }

    void Awake() 
    {
        TimeManager.Singleton = this;

        string str = PlayerPrefs.GetString("lastLoginTime", "0");
        string[] array = str.Split('/');
        if(array.Length > 1)
        {
            lastLoginTime.Add(int.Parse(array[0])); // Year
            lastLoginTime.Add(int.Parse(array[1])); // Month
            lastLoginTime.Add(int.Parse(array[2])); // Day
            lastLoginTime.Add(int.Parse(array[3])); // Hour
            lastLoginTime.Add(int.Parse(array[4])); // Minute
            lastLoginTime.Add(int.Parse(array[5])); // Second
        }

        StartCoroutine(RetrieveDate());
    }

    void OnApplicationQuit() 
    {
        PlayerPrefs.SetString("lastLoginTime", string.Join("/", lastLoginTime));
    }

    public IEnumerator RetrieveDate()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.timeapi.io/api/Time/current/zone?timeZone=Australia/Sydney");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string[] str = www.downloadHandler.text.Split(',', ':');
            int.Parse(str[1]);
            time.Add(int.Parse(str[1]));
            time.Add(int.Parse(str[3]));
            time.Add(int.Parse(str[5]));
            time.Add(int.Parse(str[7]));
            time.Add(int.Parse(str[9]));
            time.Add(int.Parse(str[11]));
        }
        
        if(lastLoginTime.Count != 0)
        { timeDifference = CalculateTimeDifference(); }

        retrievedTime = true;
    }

    int CalculateTimeDifference()
    {
        List<int> curTime = time;
        int difference = 0;

        int Year = curTime[0] - lastLoginTime[0];
        int Month = curTime[1] - lastLoginTime[1];
        int Day = curTime[2] - lastLoginTime[2];
        int Hour = curTime[3] - lastLoginTime[3];
        int Minute = curTime[4] - lastLoginTime[4];
        int Second = curTime[5] - lastLoginTime[5];

        difference += (Year * 31536000);
        difference += (Month * 2628288);
        difference += (Day * 86400);
        difference += (Hour * 3600);
        difference += (Minute * 60);
        difference += (Second);

        return difference;
    }
}
