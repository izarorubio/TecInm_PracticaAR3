using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{
    public ArrowPathManager pathManager;
    public int arrowIndex;

    public void Initialize(ArrowPathManager manager, int index)
    {
        pathManager = manager;
        arrowIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera")) // o algún objeto que representa al usuario
        {
            pathManager.ActivateNextArrow(arrowIndex);
            Destroy(gameObject); // o simplemente desactiva si prefieres
        }
    }
}
