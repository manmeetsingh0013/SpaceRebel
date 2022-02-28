using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>

public class Enemy : MonoBehaviour {

    #region PUBLIC FIELDS
    [Tooltip("Health points in integer")]
    public int health;

    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;

    [HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path

    [HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path

    #endregion

    private void Start()
    {
        Invoke("ActivateShooting", Random.Range(shotTimeMin, shotTimeMax));
    }

    //coroutine making a shot
    void ActivateShooting() 
    {
        if (Random.value < (float)shotChance / 100)                             //if random value less than shot probability, making a shot
        {
            GameObject bullet = PoolingController.instance.GetPoolingObject(Projectile);
            if (bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = gameObject.transform.position;
                bullet.transform.rotation = Quaternion.identity;
            }
        }
    }

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage) 
    {
        health -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure
        if (health <= 0)
            Destruction();
        else
        {
            GameObject hit = PoolingController.instance.GetPoolingObject(hitEffect);
            if (hit != null)
            {
                hit.SetActive(true);
                hit.transform.position = transform.position;
                hit.transform.rotation = Quaternion.identity;
                hit.transform.SetParent(transform);
            }
        }
    }    

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Projectile.GetComponent<Projectile>() != null)
                Player.instance.GetDamage(Projectile.GetComponent<Projectile>().damage);
            else
                Player.instance.GetDamage(1);
        }
    }

    //method of destroying the 'Enemy'
    void Destruction()                           
    {
        GameController.GetInstance().PlayAudio(AUDIOTYPE.EXPLOSION);
        GameObject destruction = PoolingController.instance.GetPoolingObject(destructionVFX);
        if (destruction != null)
        {
            destruction.SetActive(true);
            destruction.transform.position = transform.position;
            destruction.transform.rotation = Quaternion.identity;
        }
        gameObject.SetActive(false);
    }
}
