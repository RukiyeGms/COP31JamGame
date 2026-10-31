using System.Collections.Generic;
using UnityEngine;

public class CoralSpawn : MonoBehaviour
{
    [SerializeField] List<Sprite> CoralSprites;

    [SerializeField] float maxSpawnY = 5;
    [SerializeField] float maxSpawnX = 9;
    [SerializeField] float minSpawnY = -5;
    [SerializeField] float minSpawnX = -9;

    Vector2 spawnPosition;

    [SerializeField] int coralAmount = 15;

    [SerializeField] GameObject coralPrefab;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = coralPrefab.GetComponent<SpriteRenderer>();
    }

    public void SpawnCorals()
    {
        for (int i = 0; i < coralAmount; i++)
        {
            GetRandomCoralSprite();
            spawnPosition = GetRandomSpawnPos();
            Instantiate(coralPrefab, spawnPosition, Quaternion.identity);

        }
    }

    void GetRandomCoralSprite()
    {
        if (CoralSprites == null) { return; }
        else
        {
            int index = Random.Range(0, CoralSprites.Count);
            spriteRenderer.sprite = CoralSprites[index];
            
        }
    }

    Vector2 GetRandomSpawnPos()
    {
        float spawnx = Random.Range(minSpawnX, maxSpawnX);
        float spawny = Random.Range(minSpawnY, maxSpawnY);

        return new Vector2(spawnx, spawny);
    }
}
