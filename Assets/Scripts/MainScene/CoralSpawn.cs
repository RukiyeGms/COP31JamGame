using System.Collections.Generic;
using UnityEngine;

public class CoralSpawn : MonoBehaviour
{
    [SerializeField] List<string> CoralAnimationNames;

    [SerializeField] float maxSpawnY = 5;
    [SerializeField] float maxSpawnX = 9;
    [SerializeField] float minSpawnY = -5;
    [SerializeField] float minSpawnX = -9;

    Vector2 spawnPosition;

    [SerializeField] int coralAmount = 15;

    [SerializeField] GameObject coralPrefab;
    Animator animator;

    private void Awake()
    {
        animator = coralPrefab.GetComponent<Animator>();
    }

    public void SpawnCorals()
    {
        for (int i = 0; i < coralAmount; i++)
        {
            
            spawnPosition = GetRandomSpawnPos();
            GameObject coral = Instantiate(coralPrefab, spawnPosition, Quaternion.identity);
            animator = coral.GetComponent<Animator>();
            GetRandomCoralAnimation();

        }
    }

    void GetRandomCoralAnimation()
    {
        if (CoralAnimationNames == null) { return; }
        else
        {
            int index = Random.Range(0, CoralAnimationNames.Count);
            animator.SetBool(CoralAnimationNames[index] , true);
            
        }
    }

    Vector2 GetRandomSpawnPos()
    {
        float spawnx = Random.Range(minSpawnX, maxSpawnX);
        float spawny = Random.Range(minSpawnY, maxSpawnY);

        return new Vector2(spawnx, spawny);
    }
}
