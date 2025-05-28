using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public BossAI bossAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossAI.StartBossFight();
            gameObject.SetActive(false); // Desactiva trigger para que no se repita
        }
    }
}
