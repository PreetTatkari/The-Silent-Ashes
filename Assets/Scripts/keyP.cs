using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private HashSet<string> collectedKeys = new HashSet<string>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key1"))
        {
            collectedKeys.Add("Key1");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Key2"))
        {
            collectedKeys.Add("Key2");
            Destroy(other.gameObject);
        }
    }

    public bool HasKey(string keyTag)
    {
        return collectedKeys.Contains(keyTag);
    }
}
