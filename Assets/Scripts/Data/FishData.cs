using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Legendary
}

[CreateAssetMenu(fileName = "NewFish" , menuName = "Game/Fish")]
public class FishData : ScriptableObject
{
    public string id;
    public string fishName;
    public Rarity rarity;
    public Sprite sprite;

    [TextArea]
    public string info;


}
