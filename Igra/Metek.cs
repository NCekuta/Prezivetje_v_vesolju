using UnityEngine;

public class Metek : MonoBehaviour
{
    public float Hitrost = 500.0f;
    public float CasTrajanja = 10.0f;
    private Rigidbody2D Telo;

    private void Awake()
    {
        Telo = GetComponent<Rigidbody2D>(); //najde v unityu in lahko kasneje urejas (linked)
    }

    public void Strel(Vector2 Smer)
    {
        Telo.AddForce(Smer * this.Hitrost); //hitrost metka

        Destroy(this.gameObject, this.CasTrajanja); //metek izgine glede na casTrajanja
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject); //metek izgine ko nekaj zadane
    }




































}
