using UnityEngine;

[CreateAssetMenu(fileName = "NewTrash" , menuName = "Trash")]
public class TrashData : ScriptableObject
{
    public string id;
    public string trashName;
    public Sprite sprite;

    [TextArea]
    public string info;
}
