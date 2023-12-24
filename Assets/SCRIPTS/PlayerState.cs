using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }
    
    // Player Health
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    
    // Player Calories
    public float currentCalories;
    public float maxCalories;
    
    float distanceTraveled = 0f;
    Vector3 lastPosition;
    
    public GameObject playerBody;
    
    // Player Hydration
    public float currentHydrationPercent = 100f;
    public float maxHydrationPercent = 100f;

    public bool isHydrationActive;

    private void Awake()
    {
        if (Instance!=null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(5);
        }
    }

    void Update()
    {
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;
        if (distanceTraveled >= 5)
        {
            distanceTraveled = 0;
            currentCalories -= 1;
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10f;
        }
    }
    
    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }
    
    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }
    
    public void setHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }
}
