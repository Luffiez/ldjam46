using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    private int currentHealth;
    private float decayRate = 1f;
    private float decayStrength = 0.2f; 
    private float decayMultiplier = 2f;

    private bool isBurning = false;

    void Start()
    {
        InvokeRepeating("Decay", 0, decayMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Decay()
    {
        //float amount = 
        if (isBurning)
        {

        }
    }
}
