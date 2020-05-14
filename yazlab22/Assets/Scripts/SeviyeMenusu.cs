using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SeviyeMenusu : MonoBehaviour
{
    public List<Button> seviyeButonlari;
    public GameObject puanTablosuUI;

    private void Update()
    {
        for (int i = 0; i < seviyeButonlari.Count; i++)
        {
            if (i <= PlayerPrefs.GetInt("SaveIndex")-1)
            {
                if (seviyeButonlari[i].interactable != true)
                {
                    seviyeButonlari[i].interactable = true;
                }
            }
            else if (i!=0)
            {
                seviyeButonlari[i].interactable = false;
            }
        }

        for (int k = 1;  k <= 3; k++)
        {
            for (int l = 1; l <= 6; l++)
            {
                GameObject geciciText = GameObject.Find("Seviye" + k + l + "Skor");
                if (geciciText != null)
                {
                    geciciText.transform.GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Seviye" + k + l + "Skor").ToString();
                }

            }
        }
    }

    public void DevamEtButonu()
    {
        if(PlayerPrefs.GetInt("SaveIndex") > 1) {
            SceneManager.LoadScene(PlayerPrefs.GetInt("SaveIndex") + 1);
        } 
    }
    public void AnaMenuButonu()
    {
        SceneManager.LoadScene("SeviyeMenusu");
    }
    public void GirisMenusu()
    {
        SceneManager.LoadScene("AnaMenu");
    }

    public void Sifirla()
    {
        PlayerPrefs.DeleteAll();
    }

    public void PuanTablosuButonu()
    {
        puanTablosuUI.SetActive(true);
    }

    public void PuanTablosuKapatButonu()
    {
        puanTablosuUI.SetActive(false);
    }

    public void SeviyeSec()
    {
        string seviyeStr = EventSystem.current.currentSelectedGameObject.name;
        string[] seviyelerStr = seviyeStr.Split('-');
        int seviye = int.Parse(seviyelerStr[0]);
        int altSeviye = int.Parse(seviyelerStr[1]);

        if (seviye == 1)
        {
            SceneManager.LoadScene(altSeviye + 1);
        }
        else
        {
            SceneManager.LoadScene((seviye - 1) * 6 + altSeviye + 1);
        }

    }
}
