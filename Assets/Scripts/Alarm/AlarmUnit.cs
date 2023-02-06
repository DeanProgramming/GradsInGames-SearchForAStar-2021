using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmUnit : MonoBehaviour
{
    private bool detected = false;
    private float cooldownTimer = 0;
    private MissileSpawning missileSpawning;
    [SerializeField] private GameObject triggeredEffects;
    [SerializeField] private MeshRenderer outerSphere;
    [SerializeField] private MeshRenderer secondOuterSphere;
    [SerializeField] private Vector3 dest;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioClip alarmSound;
    private AudioSource audioSource;

    void Start()
    {
        rb.GetComponent<Rigidbody>();
        missileSpawning = FindObjectOfType<MissileSpawning>();
        audioSource = GetComponent<AudioSource>();
        FindNewPoint();
    }


    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            DetectionCheck();
        }
    }

    private void DetectionCheck()
    {
        if (detected)
        {
            cooldownTimer = 5;
            detected = false;
            missileSpawning.SpawnNewBasicMissile();
            missileSpawning.SpawnNewBasicMissile();
            audioSource.PlayOneShot(alarmSound, .3f);
            triggeredEffects.SetActive(true);
            outerSphere.enabled = false;
            secondOuterSphere.enabled = false;
        }
        else
        {
            triggeredEffects.SetActive(false);
            outerSphere.enabled = true;
            secondOuterSphere.enabled = true;
            //Fly towards random point

            transform.LookAt(dest);
            Vector3 direction = 15 * transform.forward.normalized * Time.deltaTime;
            rb.MovePosition(transform.position + direction);

            if (Vector3.Distance(transform.position, dest) < 5)
            {
                FindNewPoint();
            }
        }
    }

    private void FindNewPoint()
    {
        List<Vector3> spawnPoints = missileSpawning.spawnPoints;
        int i = Random.Range(0, spawnPoints.Count / 2);
        dest = new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), transform.position.y, Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && cooldownTimer <= 0)
        {
            detected = true;
        }
    }
}
