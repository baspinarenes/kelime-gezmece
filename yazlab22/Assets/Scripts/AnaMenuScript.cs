using UnityEngine;
using UnityEngine.SceneManagement;

public class AnaMenuScript : MonoBehaviour
{
    public bool sifirla;
    private void Start()
    {
        if (sifirla)
        {
            PlayerPrefs.DeleteAll();
        }
    }
    public void OynaButonFonksiyonu(){
        SceneManager.LoadScene("SeviyeMenusu");
    }

    public void CikisButonFonksiyonu(){
        Application.Quit();
    }
}
