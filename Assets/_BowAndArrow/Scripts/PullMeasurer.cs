using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PullMeasurer : XRBaseInteractable
{
    // Hidden in Inspector, would need to be serializable, and added to custom editor
    public class PullEvent : UnityEvent<Vector3, float> { }
    public PullEvent Pulled = new PullEvent();

    public Transform start = null;
    public Transform end = null;

    private float pullAmount = 0.0f;
    public float PullAmount => pullAmount;

    private XRBaseInteractor pullingInteractor = null;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Set interactor for measurement
        pullingInteractor = args.interactor;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Clear interactor, and reset pull amount for animation
        pullingInteractor = null;

        // Reset everything
        SetPullValues(start.position, 0.0f);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected)
        {
            // Update pull values while the measurer is grabbed
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
                CheckForPull();
        }
    }

    private void CheckForPull()
    {
        // Use the interactor's position to calculate amount
        Vector3 interactorPosition = pullingInteractor.transform.position;

        // Figure out the new pull value, and it's position in space
        float newPullAmount = CalculatePull(interactorPosition);
        Vector3 newPullPosition = CalculatePosition(newPullAmount);

        // Check if we need to send out event
        SetPullValues(newPullPosition, newPullAmount);
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        // Direction, and length
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;

        // Figure out out the pull direction
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        // What's the actual distance?
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        pullValue = Mathf.Clamp(pullValue, 0.0f, 1.0f);

        return pullValue;
    }

    private Vector3 CalculatePosition(float amount)
    {
        // Find the actual position of the hand
        return Vector3.Lerp(start.position, end.position, amount);
    }

    private void SetPullValues(Vector3 newPullPosition, float newPullAmount)
    {
        // If it's a new value
        if (newPullAmount != pullAmount)
        {
            pullAmount = newPullAmount;
            Pulled?.Invoke(newPullPosition, newPullAmount);
        }
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        // Only let direct interactors pull the string
        return base.IsSelectableBy(interactor) && IsDirectInteractor(interactor);
    }

    private bool IsDirectInteractor(XRBaseInteractor interactor)
    {
        return interactor is XRDirectInteractor;
    }

    private void OnDrawGizmos()
    {
        // Draw line from start to end point
        if (start && end)
            Gizmos.DrawLine(start.position, end.position);
    }
}
