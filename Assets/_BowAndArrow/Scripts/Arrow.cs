using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : XRGrabInteractable
{
    [Header("Settings")]
    public float speed = 2000.0f;

    [Header("Hit")]
    public Transform tip = null;
    public LayerMask layerMask = ~Physics.IgnoreRaycastLayer;

    private new Collider collider = null;
    private new Rigidbody rigidbody = null;

    private Vector3 lastPosition = Vector3.zero;
    private bool launched = false;

    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        // Do this first, so we get the right physics values
        if (args.interactor is XRDirectInteractor)
            Clear();

        // Make sure to do this
        base.OnSelectEntering(args);
    }

    private void Clear()
    {
        SetLaunch(false);
        TogglePhysics(true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Make sure to do this
        base.OnSelectExited(args);

        // If it's a notch, launch the arrow
        if (args.interactor is Notch notch)
            Launch(notch);
    }

    private void Launch(Notch notch)
    {
        // Double-check incase the bow is dropped with arrow socketed
        if (notch.IsReady)
        {
            SetLaunch(true);
            UpdateLastPosition();
            ApplyForce(notch.PullMeasurer);
        }
    }

    private void SetLaunch(bool value)
    {
        collider.isTrigger = value;
        launched = value;
    }

    private void UpdateLastPosition()
    {
        // Always use the tip's position
        lastPosition = tip.position;
    }

    private void ApplyForce(PullMeasurer pullMeasurer)
    {
        // Apply force to the arrow
        float power = pullMeasurer.PullAmount;
        Vector3 force = transform.forward * (power * speed);
        rigidbody.AddForce(force);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (launched)
        {
            // Check for collision as often as possible
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (CheckForCollision())
                    launched = false;

                UpdateLastPosition();
            }

            // Only set the direction with each physics update
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
                SetDirection();
        }
    }

    private void SetDirection()
    {
        // Look in the direction the arrow is moving
        if (rigidbody.velocity.z > 0.5f)
            transform.forward = rigidbody.velocity;
    }

    private bool CheckForCollision()
    {
        // Check if there was a hit
        if (Physics.Linecast(lastPosition, tip.position, out RaycastHit hit, layerMask))
        {
            TogglePhysics(false);
            ChildArrow(hit);
            CheckForHittable(hit);
        }

        return hit.collider != null;
    }

    private void TogglePhysics(bool value)
    {
        // Disable physics for childing and grabbing
        rigidbody.isKinematic = !value;
        rigidbody.useGravity = value;
    }

    private void ChildArrow(RaycastHit hit)
    {
        // Child to hit object
        Transform newParent = hit.collider.transform;
        transform.SetParent(newParent);
    }

    private void CheckForHittable(RaycastHit hit)
    {
        // Check if the hit object has a component that uses the hittable interface
        GameObject hitObject = hit.transform.gameObject;
        IArrowHittable hittable = hitObject ? hitObject.GetComponent<IArrowHittable>() : null;

        // If we find a valid component, call whatever functionality it has
        if (hittable != null)
            hittable.Hit(this);
    }
}
