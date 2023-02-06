using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMissile : Missile
{
    private float baseSpeed;
    public FastMissile[] missiles;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().gameObject;
        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        baseSpeed = speed;
    }

    private void Update()
    {
        if (rotationSpeed < 25)
        {
            rotationSpeed += recoveryRate * Time.deltaTime;
        }
        speed += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    { 
        Vector3 adjustedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        if (Vector3.Distance(adjustedPlayerPos, gameObject.transform.position) < 10 && Vector3.Dot(player.transform.position - gameObject.transform.position, gameObject.transform.forward) < 0 || Vector3.Distance(adjustedPlayerPos, gameObject.transform.position) < 2)
        {
            //print("Slow down rotation"); 
            rotationSpeed = 1;
            speed = baseSpeed;
        }


        float repelStrength = 0;
        if (Vector3.Distance(adjustedPlayerPos, gameObject.transform.position) > 5 && Vector3.Dot(player.transform.position - gameObject.transform.position, gameObject.transform.forward) > 0)
        {
            repelStrength = 1;
        }


        GameObject closestMissile = null;
        float distance = float.MaxValue;

        foreach (var missileToTest in missiles)
        {
            if (missileToTest != null)
            {
                if (Vector3.Distance(missileToTest.gameObject.transform.position, gameObject.transform.position) < 10 && missileToTest.gameObject != gameObject)
                {
                    if (Vector3.Distance(missileToTest.gameObject.transform.position, gameObject.transform.position) < distance)
                    {
                        distance = Vector3.Distance(missileToTest.gameObject.transform.position, gameObject.transform.position);
                        closestMissile = missileToTest.gameObject;
                    }
                    Debug.DrawLine(gameObject.transform.position, missileToTest.transform.position, Color.red);
                }
                else
                {
                    Debug.DrawLine(gameObject.transform.position, missileToTest.transform.position, Color.green);
                }
            }
        }

        if (closestMissile != null)
        {
            Vector3 directionToOtherMissile = closestMissile.transform.position - gameObject.transform.position;
            float rightDot = Vector3.Dot(directionToOtherMissile, gameObject.transform.right);

            if (rightDot < 0)
            {
                transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * (0 + (250 * repelStrength)), Space.World);
            }
            else if (rightDot > 0)
            {
                transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * (0 + (250 * repelStrength)), Space.World);
            }
        }



        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(adjustedPlayerPos - transform.position), rotationSpeed * Time.fixedDeltaTime);
        Vector3 direction = speed * transform.forward.normalized * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction);
    }

    public void Explode()
    {
        explosionEffect.SetActive(true);
        Destroy(explosionEffect.gameObject, 5);
        explosionEffect.transform.SetParent(null); 
        missileSpawning.StartCoroutine("UpdateRecordFastMissile");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
            Explode();
        }
    }
}