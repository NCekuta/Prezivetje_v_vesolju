using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public Igralec igralec; //link to igralec
    public int zivljenje = 3;   //st. zivljenj
    public float respawnTime = 3.0f; //cas respavna

    public Color OffBarva; //barva ko nemors umret
    public Color OnBarva; //barva ko lahko umres

    private SpriteRenderer sr;

    public GameObject eksplozijaAsteroid; //2 animaciji eksploziji 
    public GameObject eksplozijaIgralec;

    public int tocke = 0;   //sešteva tocke
    public Text TockeText;
    public Text zivljenjaText;  

    
    public Image[] zivljenja; // 3 slike živlejnj
    public Sprite polnoZ; // slika življenja ko je na voljo
    public Sprite praznoZ; // slika ko ni več na voljo
    public Sprite none; //prazna slika


    public GameObject konecIgre; //konec igre zaslon

    public GameObject novNajvisjiRezultat; //nov najvisji rezultat zaslon
    public InputField ImeNajbolsega; //spremenljivka za polje za vnos imena

    public TMPro.TMP_Text TvojRezultatText; //spremenljivka za tvoj rezultat na end screenu
    public TMPro.TMP_Text NajbolsiTekst; //spremenljivka za najbolsi rezultat na end screenu
    


    void Start(){

        tocke = 0; //tocke nastavi na 0

        novNajvisjiRezultat.SetActive(false); //screen odstrani
        konecIgre.SetActive(false); // konec screen odstrani

        TockeText.text = "Točke: " + tocke; //izpise se tocke 0
        zivljenjaText.text = "Življenja: " + zivljenje; //izpise se zacetno stevilo zivljenj
        
        for (int i = 0; i < zivljenja.Length; i++) //ce je i manjsi od stevila zivljenj kot ga ima igralec
        {
            if(i<zivljenje){
                zivljenja[i].sprite = polnoZ; //se pojavi slikza polnega življenja 
            }else{
                zivljenja[i].sprite = none; //prikaze prazno sliko
            }
        }
    }



    //ce umres
    public void IgralecMrtev(Igralec igralec)
    {
        this.eksplozijaIgralec.transform.position = this.igralec.transform.position; //eksplozija na istem mestu kot igralec
        Instantiate(eksplozijaIgralec); //play eksplozija animation
        
        int NeNavoljo = 3-zivljenje; //ce nastavis manj zivljenj, celoti odšteje neuporabljena

        this.zivljenje--; //zmanjsa zivljenje
        
        // updejta slikce zivljenj
        for (int i = 0; i < zivljenja.Length-NeNavoljo; i++) //ce je i manjsi od stevila zivljenj kot ga ima igralec
        {
            if(i<zivljenje){
                zivljenja[i].sprite = polnoZ; //se pojavi slikza polnega življenja 
            }else{
                zivljenja[i].sprite = praznoZ; // ce ne se pojavi slikca praznega zivljnja
            }
        }
        
        zivljenjaText.text = "Življenja: " + zivljenje; //prikaze zivljnja na ekranu

        if(this.zivljenje <= 0) //ce je zivljenj 0 je konec igre
        {
            KonecIgre();
        
        }else{
            Invoke(nameof(Respawn), this.respawnTime); //ce ne te respawna v dolocenem casu
        }
    }

    private void Respawn()
    {
        this.igralec.transform.position = Vector3.zero; //respawna te na zacetni poziciji
        this.igralec.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        
        sr = igralec.gameObject.GetComponent<SpriteRenderer>();
        sr.color = OffBarva;    //spremeni barvo dokler si offCollision

        this.igralec.gameObject.SetActive(true);
        Invoke(nameof(TrkOn), 3.0f); //izvede metodo TrkOn po 3 sekundah
    }

    private void TrkOn()
    {
        this.igralec.gameObject.layer = LayerMask.NameToLayer("Igralec"); //nastavi collisne nazaj
        sr.color = OnBarva; //nastavi barvo nazaj
    }

    public void AsteroidUnicen(Asteroid asteroid)
    {
        this.eksplozijaAsteroid.transform.position = asteroid.transform.position; //eksplozija na istem mestu kot asteroid
        Instantiate(eksplozijaAsteroid); //play eksplozija animation

        if(asteroid.Velikost < 0.3f){
            this.tocke = tocke + 500;
        }else if(asteroid.Velikost <= 0.5){
            this.tocke = tocke + 100;
        }else if(asteroid.Velikost <= 0.8){
            this.tocke = tocke + 50;
        }else
            this.tocke = tocke + 25;
        
        TockeText.text = "Točke: " + tocke; //prikaze rezultat na ekranu
        
    }








    // ko je konec igre
    private void KonecIgre()
    {
        CancelInvoke(); // prekine vse prilklice

        if(PoisciNajvisjiRezultat(tocke)){
            novNajvisjiRezultat.SetActive(true); //se prikaze ekran za vnesitev imena in se aktivira VnesenoIme(), ko vneses ime
        }else{
            konecIgre.SetActive(true); // ce ne se prikaze end ekran
            TvojRezultatText.text = "" + tocke; //izpise trenutne tocke
            NajbolsiTekst.text = PlayerPrefs.GetInt("tocke_najbolsega") + " - " + PlayerPrefs.GetString("ime_najbolsega"); //tekst se nastavi na najbolsega
        }
    }


    
    public bool PoisciNajvisjiRezultat(int rezultat)
    {
        int NajvisjiRezultat = PlayerPrefs.GetInt("tocke_najbolsega"); //pridobi tocke ki so shranjene
        if(rezultat > NajvisjiRezultat){ //ce je zdejsen rezultat vecji od najvecjega
            return true;
        }
        return false;
    }

    public void VnesenoIme() // se aktivira ko v polje unseš ime
    {
        string ime = ImeNajbolsega.text; //v ime se shrani kar si vneseu
        Debug.Log(ime);

        PlayerPrefs.SetString("ime_najbolsega", ime); //shrani ime
        PlayerPrefs.SetInt("tocke_najbolsega", tocke); //shrani tocke

        novNajvisjiRezultat.SetActive(false); //čestitke ekran zgine
        konecIgre.SetActive(true); //end ekran se pojavi

        TvojRezultatText.text = "" + tocke; //izpise trenutne tocke
        NajbolsiTekst.text = PlayerPrefs.GetInt("tocke_najbolsega") + " - " + PlayerPrefs.GetString("ime_najbolsega"); //tekst se nastavi na najbolsega
    }



    public void IgrajZnova() // ko pritisneš gumb na end screenu
    {
        SceneManager.LoadScene("Vesolje"); // znova se nalozi scena z igro
    }





    

    

























}
