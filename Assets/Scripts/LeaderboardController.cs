using TMPro;
using UnityEngine;
using SimpleJSON;


public class LeaderboardController : MonoBehaviour
{
    [SerializeField] GameObject[] otherPlayersEntries;
    [SerializeField] GameObject playerEntry, allEntries, alertAuth, leaderboardObject;
    [SerializeField] YandexSDK yandexSDK;

    Transform scoreTextObj, nameTextObj;
    string jsonData;
    string pronounText;
    string unknownUserText;
    void Start()
    {
        if (Language.isRusLang)
        {
            pronounText = "ВЫ";
            unknownUserText = "Неизвестный пользователь";
        }
        else
        {
            pronounText = "YOU";
            unknownUserText = "Unknown user";
        }
        yandexSDK = FindObjectOfType<YandexSDK>();
        UpdateLeaderBoard();
    }
    //По кнопке открытия лидерборда
    public void UpdateLeaderBoard()
    {
        yandexSDK.GetLeaderboardEntries();       
    }

    //По кнопке авторизации
    public void MakeAuth()
    {
        YandexSDK.OpenAuthorization();
    }
    //Вызывается в jslib
    public void OpenAuthAlert()
    {
        allEntries.SetActive(false);
        alertAuth.SetActive(true);
    }
    //Вызывается в jslib
    public void OpenEntries()
    {
        alertAuth.SetActive(false);
        allEntries.SetActive(true);
    }
    //В jslib после нажатия на кнопку авторизации
    public void CloseAuthWindow()
    {
        leaderboardObject.SetActive(false);
    }

    public void FillLeaderboardData(string jsonData)
    {
        yandexSDK.CheckAuthorization();

        var json = JSON.Parse(jsonData);
        var userRank = json["userRank"].ToString();
        //Если userScore = 0, То выводить -
        if (userRank == "0")
            userRank = "-";
        var count = (int)json["entries"].Count;
        
        for (int i = 0; i < count; i++)
        {
            var score = json["entries"][i]["score"].ToString();
            var name = json["entries"][i]["player"]["publicName"];
            string strName = name.ToString();
            if (string.IsNullOrEmpty(strName))
                strName = unknownUserText;
            strName = strName.Trim(new char[] { '\"', '\'' });

            for (int index = 0; index < strName.Length; index++)
            {
                if (strName[index] == ' ')
                {
                    strName = strName.Substring(0, index + 2) + ".";
                    break;
                }
            }

           

            nameTextObj = otherPlayersEntries[i].transform.Find("EntryBackground/Name");
            scoreTextObj = otherPlayersEntries[i].transform.Find("EntryBackground/Score");

            nameTextObj.GetComponent<TextMeshProUGUI>().text = strName;
            scoreTextObj.GetComponent<TextMeshProUGUI>().text = score;
        }
        
            
        playerEntry.transform.Find("EntryBackground/Name").GetComponent<TextMeshProUGUI>().text = pronounText;
        playerEntry.transform.Find("EntryBackground/Score").
            GetComponent<TextMeshProUGUI>().text = Progress.Instance.playerInfo.rounds.ToString();
        playerEntry.transform.Find("EntryBackground/PlaceText").GetComponent<TextMeshProUGUI>().text = userRank;

    }

}
