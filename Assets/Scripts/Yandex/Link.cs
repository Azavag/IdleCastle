using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Link : MonoBehaviour
{

    public static Link Instance;
    public string currentDomain;
    string link = "";
    //��������� ������
    [DllImport("__Internal")]
    private static extern string GetDomainExtern();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(this);

#if !UNITY_EDITOR 
            GetDomainExtern();
#endif
        }
        else Destroy(gameObject);
    }

    //�� jslib
    public void SetDomain(string domainString)
    {
        currentDomain = domainString;
    }
    //�� ������
    public void GotoDeveloperPage()
    {
        link = string.Format("https://yandex.{0}/games/developer?name=DemiGames", currentDomain);
        Application.OpenURL(link);
    }
}
