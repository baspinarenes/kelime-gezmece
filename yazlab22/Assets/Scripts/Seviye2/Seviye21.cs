using AillieoUtils.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;
using TMPro;

public class Seviye21 : MonoBehaviour
{
    public GameObject seviyeTamamlandiUI;
    public TextMeshProUGUI seviyeTamamlandiSkorUI;
    public TextMeshProUGUI seviyeTamamlandiBestSkorUI;

    //BULMACA AYARLARI----------------------------------------------------------------------
    private int buildIndeksi;
    private float timer = 0F;

    string[][] kullanilacakHarflerDizisi = new string[3][];
    string[] kelimelerDizisi = new string[3];
    string[][] kelimelerinYerleri = new string[3][];
    int rastgeleBulmacaIndeksi;
    //--------------------------------------------------------------------------------------

    List<GameObject> kaydirmaButonlariList = new List<GameObject>();
    void Start()
    {

        buildIndeksi = SceneManager.GetActiveScene().buildIndex;
        BulmacaTanimlari();
        rastgeleBulmacaIndeksi = UnityEngine.Random.Range(0, kullanilacakHarflerDizisi.Length);

        GameObject ParentGameObject = GameObject.FindGameObjectWithTag("panel");
        RadialLayoutGroup RadialLayout = ParentGameObject.GetComponent<RadialLayoutGroup>();
        RadialLayout.AngleDelta = 360f / kullanilacakHarflerDizisi[rastgeleBulmacaIndeksi].Length;
        RadialLayout.AngleStart = 90;

        for (int i = 0; i < kullanilacakHarflerDizisi[rastgeleBulmacaIndeksi].Length; i++)
        {
            GameObject button = new GameObject();
            button.name = kullanilacakHarflerDizisi[rastgeleBulmacaIndeksi][i].ToString();
            button.AddComponent<RectTransform>();
            button.AddComponent<Button>();
            button.AddComponent<Image>();
            button.AddComponent<CircleCollider2D>();
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Objeler/beyazArkaplan");
            button.GetComponent<CircleCollider2D>().radius = 90;

            GameObject text = new GameObject();
            text.AddComponent<Text>();
            text.transform.SetParent(button.gameObject.transform);

            button.GetComponentInChildren<Text>().text = kullanilacakHarflerDizisi[rastgeleBulmacaIndeksi][i].ToString();
            button.GetComponentInChildren<Text>().color = Color.black;
            button.GetComponentInChildren<Text>().fontSize = 60;
            button.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            button.GetComponentInChildren<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

            kaydirmaButonlariList.Add(button);
        }

        foreach (var button2 in kaydirmaButonlariList)
        {
            button2.transform.SetParent(ParentGameObject.gameObject.transform);
        }

        HarfleriKaristir();
    }

    void Update()
    {
        this.timer += Time.deltaTime;
        PlayerPrefs.SetInt("sayac", (int)Math.Ceiling(timer));
        for (int i = 0; i < kelimelerinYerleri[rastgeleBulmacaIndeksi].Length; i++)
        {
            GameObject kutu = GameObject.Find(kelimelerinYerleri[rastgeleBulmacaIndeksi][i]);
            kutu.GetComponent<Image>().enabled = true;
            if (PlayerPrefs.GetString("kelime").Equals(kelimelerDizisi[rastgeleBulmacaIndeksi]))
            {
                kutu.GetComponentInChildren<Text>().text = kelimelerDizisi[rastgeleBulmacaIndeksi].ToCharArray()[i].ToString();
            }

            bool oyunBitti = true;

            for (int k = 0; k < kelimelerinYerleri[rastgeleBulmacaIndeksi].Length; k++)
            {

                GameObject kutuGecici = GameObject.Find(kelimelerinYerleri[rastgeleBulmacaIndeksi][k]);

                if (kutuGecici.GetComponent<Image>().enabled == true && kutuGecici.GetComponentInChildren<Text>().text == "")
                    oyunBitti = false;
            }

            if (oyunBitti)
            {
                if (seviyeTamamlandiUI.activeInHierarchy == false)
                {
                    int simdikiPuan;
                    if (PlayerPrefs.GetInt("sayac") <= 4)
                        simdikiPuan = 100;
                    else
                    {
                        simdikiPuan = 100 - ((PlayerPrefs.GetInt("basarisizDeneme") - 1) * 3) - ((PlayerPrefs.GetInt("sayac") - 4) * 1);
                        if (simdikiPuan < 0)
                            simdikiPuan = 0;
                    }

                    int oncekiPuan = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Skor");

                    if (simdikiPuan > oncekiPuan)
                    {
                        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Skor", simdikiPuan);
                        seviyeTamamlandiBestSkorUI.text = "EN İYİ SKOR: " + PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Skor") + "*";
                        seviyeTamamlandiSkorUI.text = "SKORUN: " + simdikiPuan;
                    }
                    else
                    {
                        seviyeTamamlandiBestSkorUI.text = "EN İYİ SKOR: " + PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Skor");
                        seviyeTamamlandiSkorUI.text = "SKORUN: " + simdikiPuan;
                    }
                    seviyeTamamlandiUI.SetActive(true);
                }
                
            }
        }
        PlayerPrefs.Save();
    }

    public void HarfleriKaristir()
    {
        foreach (var button2 in kaydirmaButonlariList)
        {
            button2.transform.SetParent(null);
        }

        GameObject ParentPanel = GameObject.FindGameObjectWithTag("panel");

       
        int n = kaydirmaButonlariList.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject deger = kaydirmaButonlariList[k];
            kaydirmaButonlariList[k] = kaydirmaButonlariList[n];
            kaydirmaButonlariList[n] = deger;
        }

        foreach (var button2 in kaydirmaButonlariList)
        {
            button2.transform.SetParent(ParentPanel.gameObject.transform);
        }
    }



    void BulmacaTanimlari()
    {
        kullanilacakHarflerDizisi[0] = new string[] { "Z", "A", "R", "F" };
        kullanilacakHarflerDizisi[1] = new string[] { "D", "A", "M", "A" };
        kullanilacakHarflerDizisi[2] = new string[] { "O", "V", "A", "L" };

        kelimelerDizisi[0] = "ZARF";
        kelimelerDizisi[1] = "DAMA";
        kelimelerDizisi[2] = "OVAL";

        kelimelerinYerleri[0] = new string[] { "2-3", "3-3", "4-3", "5-3" };
        kelimelerinYerleri[1] = new string[] { "2-3", "3-3", "4-3", "5-3" };
        kelimelerinYerleri[2] = new string[] { "2-3", "3-3", "4-3", "5-3" };
    }
}
