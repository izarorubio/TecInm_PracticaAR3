using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class ArrowPathManager : MonoBehaviour
{
    public GameObject arrowPrefab;

   //Posiciones relativas al patrón de imagen"
    public List<Vector3> localPositions = new List<Vector3>();

    private List<GameObject> instantiatedArrows = new List<GameObject>();
    private int currentArrowIndex = 0;

    private Transform imageAnchor;

    public void InitializePath(Transform imageTransform)
    {
        Debug.Log("[ArrowPathManager] Inicializando camino de flechas...");
        imageAnchor = imageTransform;

        if (arrowPrefab == null)
        {
            Debug.LogError("[ArrowPathManager] El prefab de flecha no está asignado.");
            return;
        }

        if (localPositions.Count == 0)
        {
            Debug.LogWarning("[ArrowPathManager] La lista de posiciones está vacía. No se instanciarán flechas.");
            return;
        }

        // Limpia si se vuelve a detectar la imagen
        foreach (var arrow in instantiatedArrows)
        {
            Destroy(arrow);
        }
        instantiatedArrows.Clear();

        Debug.Log($"[ArrowPathManager] Generando {localPositions.Count} flechas...");

        for (int i = 0; i < localPositions.Count; i++)
        {
            Vector3 worldPos = imageTransform.TransformPoint(localPositions[i]);
            GameObject arrow = Instantiate(arrowPrefab, worldPos, Quaternion.LookRotation(imageTransform.forward));
            arrow.name = $"Arrow_{i}";
            arrow.SetActive(false);

            var trigger = arrow.AddComponent<ArrowTrigger>();
            trigger.Initialize(this, i);

            instantiatedArrows.Add(arrow);
            Debug.Log($"[ArrowPathManager] Flecha #{i} instanciada en posición mundial: {worldPos}");
        }

        if (instantiatedArrows.Count > 0)
        {
            instantiatedArrows[0].SetActive(true);
            Debug.Log("[ArrowPathManager] Flecha inicial activada.");
        }
    }

    public void ActivateNextArrow(int index)
    {
        if (index + 1 < instantiatedArrows.Count)
        {
            instantiatedArrows[index + 1].SetActive(true);
            Debug.Log($"[ArrowPathManager] Activada flecha #{index + 1}");
        }
        else
        {
            Debug.Log("[ArrowPathManager] Última flecha alcanzada.");
        }
    }
}

