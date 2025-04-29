using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private GameObject heldItem;
    private Transform itemHolder;

    void Start()
    {
        itemHolder = transform; // Assuming the item holder is the parent object
    }

    public void HoldItem(GameObject item)
    {
        heldItem = item;

        // Disable item's collider while held
        Collider itemCollider = heldItem.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        // Set item's parent to this item holder
        heldItem.transform.SetParent(itemHolder);
        heldItem.transform.localPosition = Vector3.zero; // Center the item in the holder
        heldItem.transform.localRotation = Quaternion.identity; // Reset rotation
        Rigidbody itemRigidbody = heldItem.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            itemRigidbody.isKinematic = true; // Disable physics while held
        }
    }

    public void ReleaseItem()
    {
        if (heldItem == null)
        {
            return;
        }

        // Enable item's collider when released
        Collider itemCollider = heldItem.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }

        Rigidbody itemRigidbody = heldItem.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            itemRigidbody.isKinematic = false; // Enable physics when released
        }

        // Unparent the item
        heldItem.transform.SetParent(null);

        heldItem = null;
    }
}
