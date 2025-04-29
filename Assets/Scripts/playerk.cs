using UnityEngine;

public class playerk : MonoBehaviour
{
    public Transform carryPosition;
    private GameObject carriedKey;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && carriedKey == null) // Left mouse button to pick up
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Key"))
                {
                    carriedKey = hit.collider.gameObject;
                    carriedKey.transform.position = carryPosition.position;
                    carriedKey.transform.parent = carryPosition;
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && carriedKey != null) // Right mouse button to drop
        {
            carriedKey.transform.parent = null;
            carriedKey = null;
        }
    }
}
