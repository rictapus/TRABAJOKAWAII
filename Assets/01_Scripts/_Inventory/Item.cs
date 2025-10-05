using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}
