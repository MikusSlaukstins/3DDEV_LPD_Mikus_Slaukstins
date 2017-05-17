using UnityEngine;
using System.Collections;


public class Falling : MonoBehaviour
{
    PlayerHealth playerHealth2;
    bool playerInRange2;
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth2 = player.GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange2 = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange2 = false;
        }
    }

    void Update()
    {
        if (playerInRange2)
        {
            playerHealth2.TakeDamage(100);
        }
    }
}
