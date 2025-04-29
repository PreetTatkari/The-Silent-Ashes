using UnityEngine;

public class Box : MonoBehaviour
{
    public KamlaAI kamlaAI;
    public Transform itemPlacementPosition;  // Position where the item should be placed inside the box

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            item.Drop();
            PlaceItemInBox(item);
            kamlaAI.OnItemPlaced();  // Notify KamlaAI that an item has been placed
        }
    }

    private void PlaceItemInBox(Item item)
    {
        item.transform.position = itemPlacementPosition.position;
        item.transform.SetParent(transform);  // Parent the item to the box for better organization
        // Optionally, you can disable the item's script or interactions if needed
        item.GetComponent<Collider>().enabled = false;  // Disable the collider to prevent further interactions
    }
}
