using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites; //tabela asteroidov

    public float Velikost = 1.0f;
    public float minVelikost = 0.2f;
    public float maxVelikost = 1.0f;
    public float Hitrost = 6.0f;
    public float CasTrajanja = 25.0f; //kdaj zgine asteroid



    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D Telo;

    //dobi komponente v unityu katere lahko v nadaljevanju urejas   
    private void Awake()
    {
        Telo = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); 
    }


    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)]; //random objekt iz tabele

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value *360.0f); //razlicni asteroidi (zavrti v z smeri)
        this.transform.localScale = Vector3.one * this.Velikost;

        Telo.mass = this.Velikost; //vecja kot je velikost, vecja je masa asteroida
    }

    public void DolociPot(Vector2 Smer)
    {
        Telo.AddForce(Smer * this.Hitrost); //hitrors objekta

        Destroy(this.gameObject, this.CasTrajanja); //kdaj objekt izgine
    }

    //Unici/razpolovi asteroid
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Metek")  //ce se dotakne metka
        {
            if((this.Velikost * 0.5f) >= 0.3f) //ce je velikost vecja od 0.3f se razpolovi drugace ne
            {
                Razpolovi();    //2 nova asteroida
                Razpolovi();
            }
            FindObjectOfType<GameManager>().AsteroidUnicen(this);
            Destroy(this.gameObject);
        }
    }

    
    private void Razpolovi()
    {
        Vector2 Smer = this.transform.position;
        Smer += Random.insideUnitCircle * 0.5f; //kje se spawna in nova smer objekta

        Asteroid pol = Instantiate(this, Smer, this.transform.rotation);
        pol.Velikost = this.Velikost * 0.5f; //nova velikost 1/2

        pol.DolociPot(Random.insideUnitCircle.normalized * this.Hitrost); //nova pot objekta
    }



    //rotacija asteroida okoli svoje osi ---------------------------------------------------------------------------------------------------
    private float rotZ;
    public float RotacijaHitrost;

    void FixedUpdate() 
    {
        rotZ += Time.deltaTime * RotacijaHitrost;
        transform.rotation = Quaternion.Euler(0,0,rotZ); 
    }






























}
