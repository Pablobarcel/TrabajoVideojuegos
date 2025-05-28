using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip jumpClip;
    public AudioClip dashClip;
    public AudioClip attackClip;
    public AudioClip hardAttackClip;
    public AudioClip specialAttackClip;

    
    public void PlayDash() => PlaySound(dashClip);
    public void PlayAttack() => PlaySound(attackClip);
    public void PlayHardAttack() => PlaySound(hardAttackClip);
    public void PlaySpecialAttack() => PlaySound(specialAttackClip);
    public void PlayJump()
{
    Debug.Log("▶️ SONIDO DE SALTO");
    PlaySound(jumpClip);
}


    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
