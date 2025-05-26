using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{
    public ArrowPathManager pathManager;
    public int arrowIndex;

    public AudioClip triggerSound;
    private AudioSource audioSource;

    private bool triggered = false; // para evitar más de una activación


    public void Initialize(ArrowPathManager manager, int index)
    {
        pathManager = manager;
        arrowIndex = index;

        // Asegurar que hay un AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return; // ya activado una vez

        if (other.CompareTag("MainCamera"))
        {
            triggered = true; // marca como activado
            PlaySound();
            pathManager.ActivateNextArrow(arrowIndex); // activar siguiente flecha
            Destroy(gameObject, triggerSound ? triggerSound.length : 0f); // destruir después de reproducir el sonido
        }
    }

    //Reproducir sonido
    private void PlaySound()
    {
        if (triggerSound != null && audioSource != null)
        {
            audioSource.clip = triggerSound;
            audioSource.Play();
        }
    }
}
