using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Meni : MonoBehaviour
{
   public void Igraj()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //ko klikneš igraj naloži igro
   }

   public void Izhod()
   {
       Application.Quit(); //ko klikneš izhod zapre igro
   }


    public void NaMeni()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); //gre na meni
    }

    public void Nadaljuj()
    {
        PavzaUI.SetActive(false); //zapre okno
        Time.timeScale = 1f; //nastavi cas nazaj na normalno
        IgraNaPavzi = false;
    }


    void Pavza()
    {
        PavzaUI.SetActive(true); //prikaze okno
        Time.timeScale = 0f; // ustavi cas/igro
        IgraNaPavzi = true;
    }


    // ESC - pavza -----------------------------------------------------------------------------------------------------------------------------------------------------------
    public static bool IgraNaPavzi = false; // privzeto je pavza na false
    public GameObject PavzaUI; // spremenljivka s katero dostopamo do objekta v Unityu




    void Update() 
    {
        
        if (Input.GetKeyDown(KeyCode.Escape)) //ko klikneš escape preveri ce je igra na pavzi
        {
            if(IgraNaPavzi)
            {
                Nadaljuj(); // se izvede Nadaljuj

            }else
            {
                Pavza(); // se izvede Pavza
            }
        }



        // NASTAVITVE - Shranjevanje
        glasba.volume = glasnost;

        PlayerPrefs.SetFloat("glasnost", glasnost); //shrani glasnost

        if (slider.value > 0) // ce slider vec od 0
        {
            mute.isOn = false; //mute prekrizan 
        }else
        {
            mute.isOn = true; //ce ne mute ni prekrizan
        }

    }

    // NASTAVITVE ----------------------------------------------------------------------------------------------------------------------------------------------------

    // glasba v ozadju - slider

    public AudioSource glasba;
    private float glasnost = 1f;
    public Slider slider;

    public void NastaviGlasnost(float glas)
    {
        glasnost = glas; //v igrci spreminjas glas ki gre potem v glasnost
    }




    //mute gumb
    void Start() 
    {
        glasba.Play(); //zažene glasbo
        glasnost = PlayerPrefs.GetFloat("glasnost"); //dobi shranjene podatke
        glasba.volume = glasnost; //nastavi glasnost
        slider.value = glasnost; // nastavi slider


        //RESOLUCIJE
        resolucije = Screen.resolutions;

        oknoResolucij.ClearOptions();

        List<string> vseResolucije = new List<string>();

        int trenutnaResolucja = 0;
        for (int i = 0; i < resolucije.Length; i++)
        {
            string opcija = resolucije[i].width + " x " + resolucije[i].height;
            vseResolucije.Add(opcija);

            if(resolucije[i].width == Screen.currentResolution.width &&
               resolucije[i].height == Screen.currentResolution.height)
            {
                trenutnaResolucja = i;
            }
        }

        oknoResolucij.AddOptions(vseResolucije);
        oknoResolucij.value = trenutnaResolucja;
        oknoResolucij.RefreshShownValue();
 
    }

    public Toggle mute;
    public void VklopiGlas(bool mute)
    {
        if(mute)
        {
            glasba.volume = 0; //ce zvocnik prekrizan glasba 0
            slider.value = 0f; // in slider na 0
        }else
        {
            glasba.volume = 1; //ce ni glasba na 1
            slider.value = 0.5f; // in slider da na 0.5
        }
    }


    //Kakovost
    public void NastaviKakovost(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }




    //Resolucija
    public void CelZaslon (bool jeCelZaslon)
    {
        Screen.fullScreen = jeCelZaslon;
    }

    
    public TMPro.TMP_Dropdown oknoResolucij; //Dropdown oknoResolucij;
    Resolution[] resolucije;

    public void NastaviResolucijo(int index)
    {
        Resolution resolucija = resolucije[index];
        Screen.SetResolution(resolucija.width, resolucija.height, Screen.fullScreen);
    }









}
