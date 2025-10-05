using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem.Composites;

public class ItemUI : MonoBehaviour
{
    #region Variables
    [SerializeField] Image image;
    [SerializeField] Button button;
    #endregion

    public void Initialize(string inventoryItemName, Item item, Action<string> removeItemAction)
    {
        image.sprite = item.icon;
        transform.localScale = Vector3.one;
        button.onClick.AddListener(() => removeItemAction.Invoke(inventoryItemName));
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
