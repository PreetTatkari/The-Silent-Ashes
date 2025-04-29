using UnityEngine;

public class Item : MonoBehaviour
{
    private bool isHeld = false;
    private Transform player;

    private void Update()
    {
        if (isHeld)
        {
            transform.position = player.position;
        }
    }

    public void PickUp(Transform playerTransform)
    {
        isHeld = true;
        player = playerTransform;
    }

    public void Drop()
    {
        isHeld = false;
        player = null;
    }
}

