﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    #region PUBLIC FIELDS

    [Tooltip("Health points in integer and UI")]
    [SerializeField] int health;
 
    [Tooltip("VFX Prefab Refernces")]
    [SerializeField] GameObject destructionFX;
    [SerializeField] GameObject hitEffect;
    [SerializeField] UIManager uIManager;

    public static Player instance;

    #endregion

    #region PRIVATE

    float maxHealth;

    Vector3 initialPosition;

    int score;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        if (instance == null) 
            instance = this;

        initialPosition = transform.position;

        maxHealth = health;

        GetComponent<AudioSource>().mute = GameController.GetInstance().isAudioOff;
    }

    #endregion

    #region PUBLIC METHODS

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)   
    {
        uIManager.SetPlayerHealth(damage, maxHealth);
        
        health -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure

        if (health <= 0)
        {
            Destruction();
        }
        else
        {
            GameObject hit = PoolingController.instance.GetPoolingObject(hitEffect);
            if (hit != null)
            {
                hit.SetActive(true);
                SetPostionAndRotationForTarget(hit.transform, transform.position, Quaternion.identity);
                hit.transform.SetParent(transform);
            }
            transform.position = initialPosition;
        }
    }

    // score calculation
    public void SetScore(int scoreValue)
    {
        score += scoreValue;

        uIManager.SetScore(score);
    }

    #endregion

    #region PRIVATE METHODS

    private void SetPostionAndRotationForTarget(Transform target, Vector3 pos,Quaternion quaternion)
    {
        target.position = pos;
        target.rotation = quaternion;
    }

    /// <summary>
    /// When player exaust all its health.
    /// </summary>
    void Destruction()
    {
        string msg = "Yay!!! It was a nice attempt !!!\n " +
                               "Try One more shot to beat your previous high score.";

        GameController.GetInstance().PlayAudio(AUDIOTYPE.EXPLOSION);

        int previousScore = PlayerPrefs.GetInt(GameController.highestScoreKey);

        if (previousScore < score)
        {
            PlayerPrefs.SetInt(GameController.highestScoreKey, score);

            msg = "Yay!!!You beat your previous score!\n " +
                        "Now try for more high score.";
        }

        uIManager.GameOver(msg);

        Time.timeScale = 0;

        GameObject destruction = PoolingController.instance.GetPoolingObject(destructionFX); //generating destruction visual effect and destroying the 'Player' object
        if (destruction != null)
        {
            destruction.SetActive(true);
            SetPostionAndRotationForTarget(destruction.transform, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
    private void OnApplicationQuit()
    {
        int previousScore = PlayerPrefs.GetInt(GameController.highestScoreKey);
        if (previousScore < score)
        {
            PlayerPrefs.SetInt(GameController.highestScoreKey, score);
        }
    }
    #endregion
}
















