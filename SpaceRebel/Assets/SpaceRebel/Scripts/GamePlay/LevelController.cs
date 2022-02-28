using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Serializable classes
[System.Serializable]
public class EnemyWaves 
{
    [Tooltip("time for wave generation from the moment the game started")]
    public float timeToStart;

    [Tooltip("Enemy wave's prefab")]
    public GameObject wave;
}

#endregion

public class LevelController : MonoBehaviour {

    #region PUBLIC FEILDS
    [SerializeField] UIManager uIManager;
    //Serializable classes implements
    public EnemyWaves[] enemyWaves; 

    public GameObject powerUp;
    public float timeForNewPowerup;
    public GameObject[] planets;
    public float timeBetweenPlanets;
    public float planetsSpeed;

    #endregion

    #region PRIVATE FILEDS

    int delayForEnemy = 6;

    int levels=1;

    int maxPowerUPs = 2;

    int powerCount = 0;

    List<GameObject> planetsList = new List<GameObject>();

    Camera mainCamera;

    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
        //for each element in 'enemyWaves' array creating coroutine which generates the wave

        InitTheWave(delayForEnemy);
        
        StartCoroutine(PlanetsCreation());
    }

    private void Shuffle(EnemyWaves[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            EnemyWaves value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    void InitTheWave(float delay)
    {
        for (int i = 0; i < enemyWaves.Length; i++)
        {
            enemyWaves[i].timeToStart = i * delay;

            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave,i));
        }

        if (powerCount < maxPowerUPs)
        {
            powerCount++;
            StartCoroutine(PowerupBonusCreation());
        }
    }

    //Create a new wave after a delay
    IEnumerator CreateEnemyWave(float delay, GameObject Wave,int index) 
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);

        if (Player.instance != null)
        {
            Instantiate(Wave);

            if(index == enemyWaves.Length-1)
            {
                levels++;

                uIManager.LevelUp();
              
                yield return new WaitForSeconds(2.5f);

                Shuffle(enemyWaves);

                delayForEnemy = delayForEnemy > 1 ? delayForEnemy : 4;

                InitTheWave(--delayForEnemy);

            }

        }
    }

    //endless coroutine generating 'levelUp' bonuses. 
    IEnumerator PowerupBonusCreation() 
    {

        yield return new WaitForSeconds(timeForNewPowerup);
        Instantiate(
            powerUp,
            //Set the position for the new bonus: for X-axis - random position between the borders of 'Player's' movement; for Y-axis - right above the upper screen border 
            new Vector2(
                Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX), 
                mainCamera.ViewportToWorldPoint(Vector2.up).y + powerUp.GetComponent<Renderer>().bounds.size.y / 2), 
            Quaternion.identity
            );
    }

    IEnumerator PlanetsCreation()
    {
        //Create a new list copying the arrey
        for (int i = 0; i < planets.Length; i++)
        {
            planetsList.Add(planets[i]);
        }
        yield return new WaitForSeconds(10);
        while (true)
        {
            ////choose random object from the list, generate and delete it
            int randomIndex = Random.Range(0, planetsList.Count);
            GameObject newPlanet = Instantiate(planetsList[randomIndex]);
            planetsList.RemoveAt(randomIndex);
            //if the list decreased to zero, reinstall it
            if (planetsList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    planetsList.Add(planets[i]);
                }
            }
            newPlanet.GetComponent<DirectMoving>().speed = planetsSpeed;

            yield return new WaitForSeconds(timeBetweenPlanets);
        }
    }
}
