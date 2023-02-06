using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawning : MonoBehaviour
{
    public List<Vector3> spawnPoints;

    [Header("Basic Missile")]
    [SerializeField] private int basicMissileLimit = 5;
    [SerializeField] private GameObject basicMissile;
    [SerializeField] private BasicMissile[] basicMissileGroup;

    [Header("Fast Missile")]
    [SerializeField] private int fastMissileLimit = 3;
    [SerializeField] private GameObject fastMissilePrefab;
    [SerializeField] private FastMissile[] fastMissileGroup;

    [Header("Trail Missile")]
    [SerializeField] private int trialMissileLimit = 3;
    [SerializeField] private GameObject trailMissilePrefab;
    [SerializeField] private TrailMissile[] trailMissileGroup;

    [Header("Scatter Missile")]
    [SerializeField] private int scatterMissileLimit = 3;
    [SerializeField] private GameObject scatterMissilePrefab;
    [SerializeField] private ScatterMissile[] scatterMissileGroup;

    [Header("Alarm")]
    [SerializeField] private int alarmUnitLimit = 0;
    [SerializeField] private int alarmUnitCount = 0;
    [SerializeField] private GameObject alarmUnitPrefab;

    [Header("New Basic Missile")]
    [SerializeField] private int smallExplosiveMissileLimit = 5;
    [SerializeField] private GameObject smallExplosiveMissile;
    [SerializeField] private BasicExplosiveType[] smallExplosiveMissileGroup;

    public void StartMissileReleasing()
    {
        StartCoroutine(BasicMissileCoroutine());
        StartCoroutine(FastMissileCoroutine());
        StartCoroutine(AlarmUnitCoroutine());
        StartCoroutine(TrailMissileCoroutine());
        StartCoroutine(ScatterMissileCoroutine());
        StartCoroutine(smallExplosiveMissileCoroutine());
    }

    IEnumerator BasicMissileCoroutine()
    {
        while (Time.timeScale != 0 && basicMissileLimit != 0)
        {
            yield return new WaitForSeconds(1);
            if (basicMissileGroup.Length < basicMissileLimit)
            {
                SpawnNewBasicMissile();
                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator FastMissileCoroutine()
    {
        while (Time.timeScale != 0 && fastMissileLimit != 0)
        {
            yield return new WaitForSeconds(1.5f);
            if (fastMissileGroup.Length < fastMissileLimit)
            {
                SpawnNewFastMissile();
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    IEnumerator AlarmUnitCoroutine()
    {
        while (Time.timeScale != 0 && alarmUnitLimit != 0)
        {
            yield return new WaitForSeconds(2.5f);
            if (alarmUnitCount < alarmUnitLimit)
            { 
                SpawnNewAlarmUnit();
                yield return new WaitForSeconds(2.5f);
            }
        }
    }

    IEnumerator TrailMissileCoroutine()
    {
        while (Time.timeScale != 0 && trialMissileLimit != 0)
        {
            yield return new WaitForSeconds(1.25f);
            if (trailMissileGroup.Length < trialMissileLimit)
            { 
                SpawnNewTrailMissile();
                yield return new WaitForSeconds(1.25f);
            }
        }
    }

    IEnumerator ScatterMissileCoroutine()
    {
        while (Time.timeScale != 0 && scatterMissileLimit != 0)
        {
            yield return new WaitForSeconds(2.5f);
            if (scatterMissileGroup.Length < scatterMissileLimit)
            { 
                SpawnNewScatterMissile();
                yield return new WaitForSeconds(2.5f);
            }
        }
    }

    IEnumerator smallExplosiveMissileCoroutine()
    {
        while (Time.timeScale != 0 && smallExplosiveMissileLimit != 0)
        {
            yield return new WaitForSeconds(.2f);
            if (smallExplosiveMissileGroup.Length < smallExplosiveMissileLimit)
            {
                SpawnSmallExplosiveMissile();
                yield return new WaitForSeconds(.2f);
            }
        }
    }


    // Attempting to make it generic function
    private void SpawnMissileType<T>(string missileTypeName, GameObject missile, T[] missileGroup) where T : Missile
    {
        int i = Random.Range(0, spawnPoints.Count / 2);
        GameObject spawnedMissile = Instantiate(missile, new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), Random.Range(spawnPoints[i * 2].y, spawnPoints[(i * 2) + 1].y), Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)), Quaternion.identity);
        spawnedMissile.GetComponent<T>().missileSpawning = gameObject.GetComponent<MissileSpawning>();
        spawnedMissile.transform.SetParent(this.transform);
        missileGroup = FindObjectsOfType<T>();

        foreach (var loopMissile in missileGroup)
        {
            if (loopMissile != null)
            {
                //loopMissile.GetComponent<T>().missiles = missileGroup;
            }
        }
    }


    public void SpawnSmallExplosiveMissile()
    {
        int i = Random.Range(0, spawnPoints.Count / 2);
        GameObject spawnedMissile = Instantiate(smallExplosiveMissile, new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), Random.Range(spawnPoints[i * 2].y, spawnPoints[(i * 2) + 1].y), Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)), Quaternion.identity);
        spawnedMissile.GetComponent<BasicExplosiveType>().missileSpawning = gameObject.GetComponent<MissileSpawning>();
        spawnedMissile.transform.SetParent(this.transform);
        smallExplosiveMissileGroup = FindObjectsOfType<BasicExplosiveType>();

        foreach (var loopMissile in smallExplosiveMissileGroup)
        {
            if (loopMissile != null)
            {
                loopMissile.GetComponent<BasicExplosiveType>().missiles = smallExplosiveMissileGroup;
            }
        }
    }

    public void SpawnNewBasicMissile()
    {
        int i = Random.Range(0, spawnPoints.Count / 2);
        GameObject spawnedMissile = Instantiate(basicMissile, new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), Random.Range(spawnPoints[i * 2].y, spawnPoints[(i * 2) + 1].y), Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)), Quaternion.identity);
        spawnedMissile.GetComponent<BasicMissile>().missileSpawning = gameObject.GetComponent<MissileSpawning>();
        spawnedMissile.transform.SetParent(this.transform);
        basicMissileGroup = FindObjectsOfType<BasicMissile>();

        foreach (var loopMissile in basicMissileGroup)
        {
            if (loopMissile != null)
            {
                loopMissile.GetComponent<BasicMissile>().missiles = basicMissileGroup;
            }
        }
    }

    private void SpawnNewFastMissile()
    {
        int i = Random.Range(0, spawnPoints.Count / 2);
        GameObject spawnedMissile = Instantiate(fastMissilePrefab, new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), Random.Range(spawnPoints[i * 2].y, spawnPoints[(i * 2) + 1].y), Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)), Quaternion.identity);
        spawnedMissile.GetComponent<FastMissile>().missileSpawning = gameObject.GetComponent<MissileSpawning>();
        spawnedMissile.transform.SetParent(this.transform);
        fastMissileGroup = FindObjectsOfType<FastMissile>();

        foreach (var loopMissile in fastMissileGroup)
        {
            if (loopMissile != null)
            {
                loopMissile.GetComponent<FastMissile>().missiles = fastMissileGroup;
            }
        }
    }


    private void SpawnNewTrailMissile()
    {
        int i = Random.Range(0, spawnPoints.Count / 2);
        GameObject spawnedMissile = Instantiate(trailMissilePrefab, new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), Random.Range(spawnPoints[i * 2].y, spawnPoints[(i * 2) + 1].y), Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)), Quaternion.identity);
        spawnedMissile.GetComponent<TrailMissile>().missileSpawning = gameObject.GetComponent<MissileSpawning>();
        spawnedMissile.transform.SetParent(this.transform);
        trailMissileGroup = FindObjectsOfType<TrailMissile>();

        foreach (var loopMissile in trailMissileGroup)
        {
            if (loopMissile != null)
            {
                loopMissile.GetComponent<TrailMissile>().missiles = trailMissileGroup;
            }
        }
    }

    private void SpawnNewScatterMissile()
    {
        int i = Random.Range(0, spawnPoints.Count / 2);
        GameObject spawnedMissile = Instantiate(scatterMissilePrefab, new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), Random.Range(spawnPoints[i * 2].y, spawnPoints[(i * 2) + 1].y), Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)), Quaternion.identity);
        spawnedMissile.GetComponent<ScatterMissile>().missileSpawning = gameObject.GetComponent<MissileSpawning>();
        spawnedMissile.transform.SetParent(this.transform);
        scatterMissileGroup = FindObjectsOfType<ScatterMissile>();

        foreach (var loopMissile in scatterMissileGroup)
        {
            if (loopMissile != null)
            {
                loopMissile.GetComponent<ScatterMissile>().missiles = scatterMissileGroup;
            }
        }
    }


    private void SpawnNewAlarmUnit()
    {
        int i = Random.Range(0, spawnPoints.Count / 2);
        GameObject spawnedAlarm = Instantiate(alarmUnitPrefab, new Vector3(Random.Range(spawnPoints[i * 2].x, spawnPoints[(i * 2) + 1].x), Random.Range(spawnPoints[i * 2].y, spawnPoints[(i * 2) + 1].y), Random.Range(spawnPoints[i * 2].z, spawnPoints[(i * 2) + 1].z)), Quaternion.identity);
        spawnedAlarm.transform.SetParent(this.transform);
        alarmUnitCount++;
    }



    public IEnumerator UpdateRecordBasicMissile()
    {
        while (Time.timeScale != 0 && basicMissileLimit != 0)
        {
            yield return new WaitForSeconds(1);
            basicMissileGroup = FindObjectsOfType<BasicMissile>();
        }
    }

    public IEnumerator UpdateRecordFastMissile()
    {
        while (Time.timeScale != 0 && fastMissileLimit != 0)
        {
            yield return new WaitForSeconds(1);
            fastMissileGroup = FindObjectsOfType<FastMissile>();
        }
    }

    public IEnumerator UpdateRecordTrailMissile()
    {
        while (Time.timeScale != 0 && trialMissileLimit != 0)
        {
            yield return new WaitForSeconds(1);
            trailMissileGroup = FindObjectsOfType<TrailMissile>();
        }
    }

    public IEnumerator UpdateRecordScatterMissile()
    {
        while (Time.timeScale != 0 && scatterMissileLimit != 0)
        {
            yield return new WaitForSeconds(1);
            scatterMissileGroup = FindObjectsOfType<ScatterMissile>();
        }
    }

    public IEnumerator UpdateRecordnewBasic()
    {
        while (Time.timeScale != 0 && smallExplosiveMissileLimit != 0)
        {
            yield return new WaitForSeconds(1);
            smallExplosiveMissileGroup = FindObjectsOfType<BasicExplosiveType>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < spawnPoints.Count / 2; i++)
        {
            Gizmos.DrawLine(spawnPoints[i * 2], spawnPoints[(i * 2) + 1]);
        }
    }
}
