using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class ArrowPathManager : MonoBehaviour
{
    public GameObject arrowPrefab;

    public List<Vector3> localPositions = new List<Vector3>(); //Posiciones relativas al patrón de imagen
    public List<Vector3> localRotations = new List<Vector3>(); //Rotaciones relativas " " " "

    private List<GameObject> instantiatedArrows = new List<GameObject>(); // Lista de flechas instanciadas
    private Transform imageAnchor; // patrón de imagen detectado

    public AudioClip arrowTriggerSound; // Sonido al tocar cada flecha

    public void InitializePath(Transform imageTransform) //llamado desde CustomImageManager al detectar la imagen
    {
        Debug.Log("[ArrowPathManager] Inicializando camino de flechas...");

        imageAnchor = imageTransform;

        // Debugs
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

        if (localRotations.Count != localPositions.Count)
        {
            Debug.LogWarning("[ArrowPathManager] La cantidad de rotaciones no coincide con la de posiciones. Usando rotación por defecto.");
        }

        // Eliminar flechas de antes (por si acaso las hay)
        foreach (var arrow in instantiatedArrows)
        {
            Destroy(arrow);
        }
        instantiatedArrows.Clear();

        // Crear una flecha por cada posición
        for (int i = 0; i < localPositions.Count; i++)
        {
            // Posición relativa convertida a posición mundial
            Vector3 worldPos = imageTransform.TransformPoint(localPositions[i]);

            // Rotación relativa convertida a rotación mundial
            Quaternion localRot = (i < localRotations.Count) ? Quaternion.Euler(localRotations[i]) : Quaternion.identity;
            Quaternion worldRot = imageTransform.rotation * localRot;

            GameObject arrow = Instantiate(arrowPrefab, worldPos, worldRot);
            arrow.name = $"Arrow_{i}";
            arrow.SetActive(false); // al inicio solo activa la 1ª flecha

            //Para el sonido al tocarlas
            var trigger = arrow.AddComponent<ArrowTrigger>();
            trigger.Initialize(this, i);
            trigger.triggerSound = arrowTriggerSound;

            instantiatedArrows.Add(arrow);
            Debug.Log($"[ArrowPathManager] Flecha #{i} → posición mundial: {worldPos}, rotación: {worldRot.eulerAngles}");
        }

        // Activar la primera flecha
        if (instantiatedArrows.Count > 0)
        {
            instantiatedArrows[0].SetActive(true);
            Debug.Log("[ArrowPathManager] Flecha inicial activada.");
        }
    }

    // Activa la siguiente flecha cuando se toca una
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

