using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player, respawnPoint;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = respawnPoint.transform.position;
        }
    }
}
