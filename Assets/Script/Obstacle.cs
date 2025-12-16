using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ElementType obstacleElement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;

        // Check if player can pass this obstacle
        if (CanPlayerPass(player.currentElement))
        {
            Destroy(gameObject);
        }
        else
        {
            KillPlayer(player);
        }
    }

    void KillPlayer(PlayerController player)
    {
        player.KillPlayer();
        StartCoroutine(ShowGameOverAfterDelay());

    }
    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // wait for 2 seconds
        GameManager.Instance.GameOver();
    }

    bool CanPlayerPass(ElementType playerElement)
    {
        // Space element can pass all obstacles
        if (playerElement == ElementType.Space)
            return true;

        // Player element must match obstacle element
        return playerElement == obstacleElement;
    }
}
