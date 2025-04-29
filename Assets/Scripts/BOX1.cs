using UnityEngine;

public class BOX1 : MonoBehaviour
{
    public KamlaAI kamlaAI;
    public AudioSource destructionSound; // Reference to the AudioSource component

    // Event to notify when an item is destroyed
    public event System.Action ItemDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            item.Drop();
            Destroy(item.gameObject);  // Destroy the item

            // Play destruction sound if AudioSource is assigned
            if (destructionSound != null && destructionSound.clip != null)
            {
                destructionSound.Play();
            }

            kamlaAI.OnItemPlaced();  // Notify KamlaAI that an item has been placed

            // Raise the ItemDestroyed event
            ItemDestroyed?.Invoke();
        }
    }
}
