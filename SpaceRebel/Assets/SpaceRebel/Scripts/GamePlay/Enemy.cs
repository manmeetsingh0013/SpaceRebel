using UnityEngine;
using System.Collections;
/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>

public class Enemy : MonoBehaviour {

    #region PUBLIC FIELDS
    [Tooltip("Health points in integer")]
    [SerializeField] int health;

    [Tooltip("Enemy's projectile prefab")]
    [SerializeField] GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    [SerializeField] GameObject destructionVFX;
    [SerializeField] GameObject hitEffect;

    private int shotChance; //probability of 'Enemy's' shooting during tha path

    private float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path

    private int remainingHealth;

    private PoolingController poolingController;
    #endregion
    private void Awake()
    {
        poolingController = PoolingController.instance;
    }
    #region PRIVATE METHODS
    private void OnEnable()
    {
        remainingHealth = health;
        StartCoroutine(ActivateShooting(Random.Range(shotTimeMin, shotTimeMax)));
    }

    IEnumerator ActivateShooting(float randomDelay)
    {
        yield return new WaitForSeconds(randomDelay);
        while(gameObject.activeInHierarchy)
        {
            ActivateShooting();
            yield return new WaitForSeconds(Random.Range(shotTimeMin, shotTimeMax));
        }
        
    }
    //coroutine making a shot
    void ActivateShooting() 
    {
        if (Random.value < (float)shotChance / 100)                             //if random value less than shot probability, making a shot
        {
            GameObject bullet = poolingController.GetPoolingObject(Projectile);
            if (bullet != null)
            {
                bullet.SetActive(true);
                SetPostionAndRotationForTarget(bullet.transform, transform.position, Quaternion.identity);
            }
        }
    }

    private void SetPostionAndRotationForTarget(Transform target, Vector3 pos, Quaternion quaternion)
    {
        target.position = pos;
        target.rotation = quaternion;
    }

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Projectile.GetComponent<Projectile>() != null)
                Player.instance.GetDamage(Projectile.GetComponent<Projectile>().GetDamage());
            else
                Player.instance.GetDamage(1);
        }
    }

    //method of destroying the 'Enemy'
    void Destruction()
    {
        GameController.GetInstance().PlayAudio(AUDIOTYPE.EXPLOSION);
        GameObject destruction = poolingController.GetPoolingObject(destructionVFX);
        if (destruction != null)
        {
            destruction.SetActive(true);
            SetPostionAndRotationForTarget(destruction.transform, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
    #endregion

    #region PUBLIC METHODS
    public void SetData(int _shotChance,float _shotTimeMin,float _shotTimeMax)
    {
        shotChance = _shotChance;
        shotTimeMin = _shotTimeMin;
        shotTimeMax = _shotTimeMax;
    }
    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage) 
    {
        remainingHealth -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure
        if (remainingHealth <= 0)
            Destruction();
        else
        {
            GameObject hit = poolingController.GetPoolingObject(hitEffect);
            if (hit != null)
            {
                hit.SetActive(true);
                SetPostionAndRotationForTarget(hit.transform, transform.position, Quaternion.identity);
                hit.transform.SetParent(transform);
            }
        }
    }
    #endregion
}
