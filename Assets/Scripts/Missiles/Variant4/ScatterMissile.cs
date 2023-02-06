using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterMissile : Missile
{
    [HideInInspector] public ScatterMissile[] missiles;
    [SerializeField] private LineRenderer trail;
    private float trailTimer;
    private Vector3[] trailPositions;
    [SerializeField] private GameObject smallMissilePrefab;

    private void Start()
    {
        for (int i = 0; i < trail.positionCount; i++)
        {
            trail.SetPosition(i, transform.position);
        }
        trailPositions = new Vector3[trail.positionCount];
        trail.enabled = true;

        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().gameObject;
        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        variations = Random.Range(-3, 3);
        speed += variations;
    }

    private void Update()
    {
        if (rotationSpeed < 10 - variations)
        {
            rotationSpeed += recoveryRate * Time.deltaTime;
        }

        trailTimer += Time.deltaTime;
        if (trailTimer > .05f)
        {
            trailTimer = 0;
            trail.GetPositions(trailPositions);
            trail.SetPosition(0, transform.position);
            for (int i = 0; i < trailPositions.Length - 1; i++)
            {
                trail.SetPosition(i + 1, trailPositions[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 adjustedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        if (Vector3.Distance(adjustedPlayerPos, gameObject.transform.position) < 10)
        {
            //print("Slow down rotation"); 
            rotationSpeed = Random.Range(4f, 6f);
        }

        float repelStrength = 0;

        if (Vector3.Distance(adjustedPlayerPos, gameObject.transform.position) > 5) //Greater than 5
        {
            repelStrength = Mathf.InverseLerp(5, 20, Vector3.Distance(adjustedPlayerPos, gameObject.transform.position));
        }


        GameObject closestMissile = null;
        float distance = float.MaxValue;

        foreach (var missileToTest in missiles)
        {
            if (missileToTest != null)
            {
                if (Vector3.Distance(missileToTest.gameObject.transform.position, gameObject.transform.position) < 5 && missileToTest.gameObject != gameObject)
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
                transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * (100 + (250 * repelStrength)), Space.World);
            }
            else if (rightDot > 0)
            {
                transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * (100 + (250 * repelStrength)), Space.World);
            }
        }


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(adjustedPlayerPos - transform.position), rotationSpeed * Time.fixedDeltaTime);
        Vector3 direction = speed * transform.forward.normalized * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction);

        if (Vector3.Distance(adjustedPlayerPos, gameObject.transform.position) < 7) //Less than 7
        {
            Debug.DrawLine(gameObject.transform.position, adjustedPlayerPos, Color.green);
            Explode();
        }
        else
        {
            Debug.DrawLine(gameObject.transform.position, adjustedPlayerPos, Color.red);
        }
    }

    public void Explode()
    {
        ReleaseSmallerMissiles();
        explosionEffect.SetActive(true);
        Destroy(explosionEffect.gameObject, 5);
        explosionEffect.transform.SetParent(null); 
        missileSpawning.StartCoroutine("UpdateRecordScatterMissile");
        Destroy(gameObject);
    }

    public void ReleaseSmallerMissiles()
    {
        Vector3 adjustedPlayerPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        print(player.transform.position.y);
        for (int i = 0; i < 6; i++)
        {
            GameObject spawnedMissile = Instantiate(smallMissilePrefab, adjustedPlayerPos, Quaternion.identity);
            spawnedMissile.transform.SetParent(missileSpawning.transform);
        }
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
