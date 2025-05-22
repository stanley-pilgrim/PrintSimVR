using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class PrintHeadController : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startHandPosition;
    private Transform grabTransform;
    
    [SerializeField] private float minX, maxX, minZ, maxZ; // constraints
    [SerializeField] private float movementDamping = 0.8f; // resistance
    [SerializeField] private Transform printerTransform; // keep track of printer transform

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // overriding physics
        rb.useGravity = false;
        rb.isKinematic = true;

        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
        grabInteractable.forceGravityOnDetach = false;
        grabInteractable.retainTransformParent = true;

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        startRotation = transform.rotation;
        startPosition = printerTransform.InverseTransformPoint(transform.position);
        startHandPosition = args.interactorObject.GetAttachTransform(grabInteractable).position;
        grabTransform = args.interactorObject.GetAttachTransform(grabInteractable);
    }

    void Update()
    {
        // check if someone's interacting
        var interactor = grabInteractable.interactorsSelecting.Count > 0
            ? grabInteractable.interactorsSelecting[0] : null;

        if (interactor != null)
        {
            // get hand movement delta
            Vector3 handPosition = grabTransform.position;
            Vector3 worldDelta = handPosition - startHandPosition;

            // convert global movement to printer space
            Vector3 localDelta = printerTransform.InverseTransformVector(worldDelta);

            // block y movement and adding resistance
            localDelta.y = 0f;
            localDelta *= movementDamping;

            // calculate new position in relation to printer
            Vector3 newLocalPosition = startPosition + localDelta;
            newLocalPosition.x = Mathf.Clamp(newLocalPosition.x, minX, maxX);
            newLocalPosition.z = Mathf.Clamp(newLocalPosition.z, minZ, maxZ);

            // convert back to world space
            Vector3 newWorldPosition = printerTransform.TransformPoint(newLocalPosition);

            // apply the position and rotation
            transform.SetPositionAndRotation(newWorldPosition, startRotation);
        }
    }
}
