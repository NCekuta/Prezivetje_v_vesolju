using UnityEngine;

public class GeneratorAsteroidov : MonoBehaviour
{
    public Asteroid asteriodPrefab;

    public float rndUsmerjenost = 15.0f; //15 stopinj

    public float spawnRate = 2.0f; //2 sekunde
    public int spawnKolicina = 1;
    public float spawnOddaljenost = 15.0f;


    private void Start()
    {
        InvokeRepeating(nameof(Generiraj), this.spawnRate, this.spawnRate); //ponavlja funkcijo spawn
    }

    

    private void Generiraj()
    {
        for(int i = 0; i<this.spawnKolicina; i++) //generira asteroide
        {   
            //kje se generira
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnOddaljenost;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            //kam se usmeri
            float usmerjenost = Random.Range(-this.rndUsmerjenost, this.rndUsmerjenost); //naključno od -15 do 15 stopinj
            Quaternion rotacija = Quaternion.AngleAxis(usmerjenost, Vector3.forward);

            Asteroid asteroid = Instantiate(this.asteriodPrefab, spawnPoint, rotacija); //ga generira
            asteroid.Velikost = Random.Range(asteroid.minVelikost, asteroid.maxVelikost); // mu določi naključno velikost
            asteroid.DolociPot(rotacija * -spawnDirection);
        }
    }





























}
