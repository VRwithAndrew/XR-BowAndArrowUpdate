using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class StringRenderer : MonoBehaviour
{
    [Header("Settings")]
    public Gradient pullColor = null;

    [Header("References")]
    public PullMeasurer pullMeaserer = null;

    [Header("Render Positions")]
    public Transform start = null;
    public Transform middle = null;
    public Transform end = null;

    private LineRenderer lineRenderer = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // While in editor, make sure the line renderer follows bow
        if (Application.isEditor && !Application.isPlaying)
            UpdatePositions();
    }

    private void OnEnable()
    {
        // Update before render gives better results
        Application.onBeforeRender += UpdatePositions;

        // When being pulled, update the color
        pullMeaserer.Pulled.AddListener(UpdateColor);
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdatePositions;  
        pullMeaserer.Pulled.RemoveListener(UpdateColor);
    }

    private void UpdatePositions()
    {
        // Set positions of line renderer, middle position is the notch attach transform
        Vector3[] positions = new Vector3[] { start.position, middle.position, end.position };
        lineRenderer.SetPositions(positions);
    }

    private void UpdateColor(Vector3 pullPosition, float pullAmount)
    {
        // Using the gradient, show pull value via the string color
        Color color = pullColor.Evaluate(pullAmount);
        lineRenderer.material.color = color;
    }
}
