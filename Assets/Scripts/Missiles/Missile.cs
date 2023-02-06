using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    protected GameObject player;
    protected Rigidbody rb;
    [SerializeField] protected float speed = 15;
    [SerializeField] protected GameObject explosionEffect;
    [SerializeField] protected float rotationSpeed = 10;
    [SerializeField] protected float variations = 0;
    [SerializeField] protected float recoveryRate = 1;
    [HideInInspector] public MissileSpawning missileSpawning; 
}
