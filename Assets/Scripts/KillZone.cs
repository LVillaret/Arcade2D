using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null) return;

        player.currentHealth = 0;
    }
}
