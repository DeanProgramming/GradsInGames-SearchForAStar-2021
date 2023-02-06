using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMissile : Missile
{
    private float explodeTimer = 0;
    private float randomExplode = 0;

    private void Start()
    {
        randomExplode = Random.Range(2.5f, 4);
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().gameObject;
        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        variations = Random.Range(-3, 3);
        speed += variations;
        transform.Rotate(new Vector3(1, 0, 0), Random.Range(0, 360));
        rotationSpeed = Random.Range(4, 6);
    }

    private void Update()
    {
        if (rotationSpeed < 10 - variations)
        {
            rotationSpeed += recoveryRate * Time.deltaTime;
        }

        explodeTimer += Time.deltaTime;
        if(explodeTimer >= randomExplode)
        {
            Explode();
        }
    }

    private void FixedUpdate()
    {
        Vector3 adjustedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z); 
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(adjustedPlayerPos - transform.position), rotationSpeed * Time.fixedDeltaTime);
        Vector3 direction = speed * transform.forward.normalized * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction);
    }

    public void Explode()
    {
        explosionEffect.SetActive(true);
        Destroy(explosionEffect.gameObject, 5);
        explosionEffect.transform.SetParent(null); 
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
