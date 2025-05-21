using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MazeCameraController : MonoBehaviour
{
    [Header("References")]
    public MazeGenerator generator;

    [Header("Camera Mode")]
    public float padding = 1.2f;

    private Camera cam;

    private void Awake() => cam = GetComponent<Camera>();

    private void OnEnable()
    {
        generator.onMazeGenerated += AdjustCamera;
    }

    private void OnDisable()
    {
        generator.onMazeGenerated -= AdjustCamera;
    }

    // <summary>
    // Adjusts the camera position and orthographic size based on the maze dimensions.
    // </summary>
    public void AdjustCamera()
    {
        int w = generator.Width;
        int h = generator.Height;
        float s = generator.CellSpacing;

        float worldW = (w - 1) * s;
        float worldH = (h - 1) * s;
        Vector3 center = new Vector3(worldW, 0, worldH) * 0.5f;

        cam.orthographic = true;
        cam.orthographicSize = Mathf.Max(worldW, worldH) * 0.5f * padding;
        transform.position = center + Vector3.up * (worldW + worldH) * 0.5f;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); 
    }
}
