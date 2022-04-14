using UnityEngine;

public class Igralec : MonoBehaviour
{   
    public Metek MetekPrefab;

    public float HitrostPospeska = 1.0f; //public - pokaze se v unityu kjer lahko spreminjas hitrost
    public float HitrostObracanja = 1.0f;
    private Rigidbody2D telo;
    private bool Pospesek;
    private bool Zavora;
    private float Obracanje;

    public ParticleSystem ogenjPospesek;
    public ParticleSystem ZavoraL;
    public ParticleSystem ZavoraD;

    public AudioSource ZvokPospeska;
    

    private void Awake() //enkrat
    {
        telo = GetComponent<Rigidbody2D>(); //najde rigidbody v Unity - fizics
    }

    // Preverjanje vnosa 
    private void Update() //vedno deluje
    {
        ZvokPospeska.volume = 0f; //vedno deluje ampak je glasnost na 0

        Pospesek = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        Zavora = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            Obracanje = 1.0f;
        }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            Obracanje = -1.0f;
        }else{
            Obracanje = 0.0f;
        }
        
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Streljanje();
        }

        //za zvok pospeska - vedno deluje
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            ZvokPospeska.volume = 1f; // ko pritisneš w,s se glasnost poveča
    }




    //Glede na vnos se izvrsi premik telesa
    private void FixedUpdate() //dolocen cas deluje
    {
        if(Pospesek){
            telo.AddForce(this.transform.up * this.HitrostPospeska);
            ogenjPospesek.Play(); // zavrti animacijo pospeksa (ogenj)  

        }else if(Zavora)
        {
            telo.AddForce(this.transform.up * -this.HitrostPospeska);
            prikaziZavoraD(); //animacija
            prikaziZavoraL(); // animacija
        }
        
        if(Obracanje == 1.0f){
            telo.AddTorque(Obracanje * this.HitrostObracanja);
            prikaziZavoraL();
        }else if(Obracanje == -1.0f)
        {
            telo.AddTorque(Obracanje * this.HitrostObracanja);
            prikaziZavoraD();
        }else
        {
            
        }

    
    //usmeritev proti miški
    
    /*Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition)- transform.position;

    mousePos.Normalize();

    float rotationZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

    transform.rotation = Quaternion.Euler(0f, 0f, rotationZ-90);
    */


    }

    //Glede na vnos se izvrsi Streljanje
    private void Streljanje()
    {
        Metek metek = Instantiate(this.MetekPrefab, this.transform.position, this.transform.rotation);
        metek.Strel(this.transform.up);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroid")) //ce se zaletis v asteroid
        {
            telo.velocity = Vector3.zero; //hitrost gre na 0
            telo.angularVelocity = 0.0f;

            this.gameObject.SetActive(false); //igralec je neaktiven

            FindObjectOfType<GameManager>().IgralecMrtev(this); //poisce metodo igralecMrtev


                  
        }
    }

    //Animacije, ki se izvedejo ko pritisneš tipko
    void ustvariOgenj()
    {
        ogenjPospesek.Play();
    }

    void prikaziZavoraL()
    {
        ZavoraL.Play();
    }

    void prikaziZavoraD()
    {
        ZavoraD.Play();
    }

    

    //MEJE EKRANA ---------------------------------------------------------------------------------------------------------------------------
    private Vector2 MejeEkrana; //meje ekrna
    private float SirinaIgralca;
    private float VisinaIgralca;

    
    void Start()
    {
        MejeEkrana = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        SirinaIgralca = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        VisinaIgralca = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }


    void LateUpdate() 
    {
        Vector3 pozicija = transform.position; //trenutna pozicija objekta
        pozicija.x = Mathf.Clamp(pozicija.x, MejeEkrana.x * -1 + SirinaIgralca, MejeEkrana.x - SirinaIgralca);
        pozicija.y = Mathf.Clamp(pozicija.y, MejeEkrana.y * -1 + VisinaIgralca, MejeEkrana.y - VisinaIgralca);
        transform.position = pozicija;

    }








































}
