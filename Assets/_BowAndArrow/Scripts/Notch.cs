using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PullMeasurer))]
public class Notch : XRSocketInteractor
{
    // Settings

    // Necessary stuff
    public PullMeasurer PullMeasurer { get; private set; } = null;
    public bool IsReady { get; private set; } = false;

    // Need to cast to custom for Force Deselect

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Arrow is released once the puller is released

        // Move the point where the arrow is attached
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void ReleaseArrow(SelectExitEventArgs args)
    {
        // Only release if the target is an arrow using custom deselect
    }

    public void MoveAttach(Vector3 pullPosition, float pullAmount)
    {
        // Move attach when bow is pulled, this updates the renderer as well
    }

    public void SetReady(BaseInteractionEventArgs args)
    {
        // Set the notch ready if bow is selected
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // We check for the hover here too, since it factors in the recycle time of the socket
        // We also check that notch is ready, which is set once the bow is picked up
        return base.CanSelect(interactable) && CanHover(interactable) && IsArrow(interactable);
    }

    private bool IsArrow(XRBaseInteractable interactable)
    {
        // Simple arrow check, can be tag or interaction layer as well
        return interactable is Arrow;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        // Use instantaneous so it follows smoothly
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    // This enables the socket to grab the arrow immediately
    public override bool requireSelectExclusive => false;
}
