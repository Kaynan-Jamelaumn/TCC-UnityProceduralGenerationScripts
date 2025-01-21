using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Manages the player's status, including health, stamina, food, drink, and experience.
/// Provides access to various models and controllers for player actions and attributes.
/// </summary>
public class PlayerStatusController : MonoBehaviour
{


    [Tooltip("Manages the player's health.")]
    [SerializeField] private HealthManager hpManager;
    public HealthManager HpManager => hpManager; 
    public  void Awake()
    {
        if (hpManager == null) hpManager = GetComponent<HealthManager>();
    }
}





