using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtonSwitch : MonoBehaviour
{
    [SerializeField] GameObject sideMenuObject;
    void Start()
    {
        sideMenuObject.SetActive(false);
    }
    //�� ������ ��������
    public void SwitchMenu()
    {
        sideMenuObject.SetActive(!sideMenuObject.activeSelf);      
    }
   

}
