using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Linq;
using TMPro;

public class SeviyeYapisi : MonoBehaviour
{

    private int buildIndeksi;

    //HARF KAYDIRMA SİSTEMİ------------

    TextMeshProUGUI metinKutusu;
    string kelime = "";
    private List<Button> butonlar = new List<Button>();

    private LineRenderer cizgi;
    public Material materyal;
    private List<LineRenderer> ekrandakiCizgiler = new List<LineRenderer>();
    private Vector3 mousePos;
    //---------------------------------
    private void Start()
    {
        PlayerPrefs.SetString("kelime", kelime);
        PlayerPrefs.SetInt("basarisizDeneme", 0);
        buildIndeksi = SceneManager.GetActiveScene().buildIndex;
        metinKutusu = GameObject.Find("YazilanKelime").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        
        metinKutusu.text = kelime;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
                if (hit)
                {
                    kelime += hit.collider.GetComponentInChildren<Text>().text;
                    butonlar.Add(hit.transform.gameObject.GetComponent<Button>());

                    hit.transform.gameObject.GetComponent<Image>().color = Color.red;
                    metinKutusu.text = kelime;

                    //--------------------------------
                    if (cizgi == null)
                    {
                        CizgiOlustur();
                    }

                    CizgiCizBaslangic(hit.collider.bounds.center);
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
                if (hit)
                {
                    bool birDahaGecme = false;
                    if (kelime != "")
                    {
                        foreach (var bakilacakButon in butonlar)
                        {
                            if(bakilacakButon.GetComponent<RectTransform>() == (hit.collider.GetComponent<RectTransform>()))
                            {
                                birDahaGecme = true;
                            }
                        }

                        if (!birDahaGecme)
                        {
                            if (!kelime.Last().ToString().Equals(hit.collider.GetComponentInChildren<Text>().text))
                            {

                                kelime += hit.collider.GetComponentInChildren<Text>().text;
                                metinKutusu.text = kelime;

                                CizgiCizBitis(hit.collider.bounds.center);

                                if (cizgi == null)
                                {
                                    CizgiOlustur();
                                }
                                CizgiCizBaslangic(hit.collider.bounds.center);
                            }
                            hit.transform.gameObject.GetComponent<Image>().color = Color.red;
                            butonlar.Add(hit.transform.gameObject.GetComponent<Button>());
                        }
                    }
                }

                metinKutusu.text = kelime;
                CizgiCizBasili(Input.mousePosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                foreach (var buton in butonlar)
                {
                    buton.GetComponent<Image>().color = Color.white;
                }
                PlayerPrefs.SetString("kelime", kelime);
                kelime = "";
                butonlar = new List<Button>();

                foreach (var cizgiObjesi in ekrandakiCizgiler)
                {
                    if (cizgiObjesi != null)
                    {
                        Destroy(cizgiObjesi);
                    }
                }
                ekrandakiCizgiler = new List<LineRenderer>();
                PlayerPrefs.SetInt("basarisizDeneme", PlayerPrefs.GetInt("basarisizDeneme")+1);
            }
        }
        PlayerPrefs.Save();
    }
    public void AnaMenuButonu()
    {
        SceneManager.LoadScene("SeviyeMenusu");
    }
    public void SonrakiSeviye()
    {
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 6; j++)
            {
                Debug.Log("Seviye" + i + "" + j + "Skor: " + PlayerPrefs.GetInt("Seviye" + i + "" + j + "Skor"));
            }
        }

        Debug.Log("SaveIndex: " + PlayerPrefs.GetInt("SaveIndex"));
        Debug.Log("BuildIndex: " + buildIndeksi);

        if (buildIndeksi > PlayerPrefs.GetInt("SaveIndex"))
        {
            PlayerPrefs.SetInt("SaveIndex", buildIndeksi);
        }
        if (buildIndeksi == 19)
        {
            SceneManager.LoadScene("AnaMenu");
        }
        else
        {
            SceneManager.LoadScene(buildIndeksi + 1);
        }
    }


    //HARF KAYDIRMA SİSTEMİ--------------------
    void CizgiOlustur()
    {
        cizgi = new GameObject("Line" + ekrandakiCizgiler.Count).AddComponent<LineRenderer>();
        cizgi.material = materyal;
        cizgi.positionCount = 2;
        cizgi.startWidth = 0.15f;
        cizgi.endWidth = 0.15f;
        cizgi.useWorldSpace = false;
        cizgi.numCapVertices = 50;
    }
    void CizgiCizBaslangic(Vector3 konum)
    {
        mousePos = Camera.main.ScreenToWorldPoint(konum);
        mousePos.z = 0;
        cizgi.SetPosition(0, mousePos);
        cizgi.SetPosition(1, mousePos);
    }
    void CizgiCizBasili(Vector3 konum)
    {
        if (cizgi != null)
        {
            mousePos = Camera.main.ScreenToWorldPoint(konum);
            mousePos.z = 0;
            cizgi.SetPosition(1, mousePos);
            ekrandakiCizgiler.Add(cizgi);

        }
    }
    void CizgiCizBitis(Vector3 konum)
    {
        if (cizgi != null)
        {
            mousePos = Camera.main.ScreenToWorldPoint(konum);
            mousePos.z = 0;
            cizgi.SetPosition(1, mousePos);
            ekrandakiCizgiler.Add(cizgi);
            cizgi = null;
        }
    }
}
