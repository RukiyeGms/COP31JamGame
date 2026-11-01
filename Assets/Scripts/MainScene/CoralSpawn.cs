
using System.Collections.Generic;
using UnityEngine;

public class CoralSpawn : MonoBehaviour
{
    List<GameObject> spawnedCorals = new List<GameObject>();
    [SerializeField] List<string> CoralAnimationNames;

    [SerializeField] float maxSpawnY = 5;
    [SerializeField] float maxSpawnX = 9;
    [SerializeField] float minSpawnY = -5;
    [SerializeField] float minSpawnX = -9;

    [SerializeField] int coralAmount = 15;
    [SerializeField] GameObject coralPrefab;

    [SerializeField] LayerMask spawnObstacleMask;
    [SerializeField] float checkRadius = 0.25f;
    [SerializeField] int maxAttempts = 10;

    public void SpawnCorals()
    {
        for (int i = 0; i < coralAmount; i++)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector2 spawnPosition = GetRandomSpawnPos();

                
                if (Physics2D.OverlapCircle( spawnPosition, checkRadius, spawnObstacleMask))
                {
                    continue;
                }

                GameObject coral = Instantiate(coralPrefab, spawnPosition, Quaternion.identity);

                spawnedCorals.Add(coral);

                Animator animator = coral.GetComponent<Animator>();
                GetRandomCoralAnimation(animator);

                break;
            }
        }
    }

    void GetRandomCoralAnimation(Animator animator)
    {
        if (animator == null ||
            CoralAnimationNames == null ||
            CoralAnimationNames.Count == 0)
            return;

        int index = Random.Range(0, CoralAnimationNames.Count);
        animator.SetBool(CoralAnimationNames[index], true);
    }

    Vector2 GetRandomSpawnPos()
    {
        float x = Random.Range(minSpawnX, maxSpawnX);
        float y = Random.Range(minSpawnY, maxSpawnY);

        return new Vector2(x, y);
    }

    public void ClearCorals()
    {
        foreach (GameObject coral in spawnedCorals)
        {
            if (coral != null)
            {
                coral.SetActive(false);
                Destroy(coral);
            }
        }

        spawnedCorals.Clear();
    }

    public void RefreshCorals()
    {
        ClearCorals();
        SpawnCorals();
    }
}
