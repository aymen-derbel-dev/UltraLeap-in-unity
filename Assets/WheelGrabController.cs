using UnityEngine;

public class WheelGrabController : MonoBehaviour
{
    [Header("UltraLeap Tracked Hands")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Wheels")]
    public Transform[] blueWheels;
    public Transform[] redWheels;

    [Header("Matching Color Piquets")]
    public Transform[] bluePiquets;
    public Transform[] redPiquets;

    [Header("Grab Settings")]
    public float yTolerance = 0.05f;

    [Header("Placement Settings")]
    public float placementRadius = 0.25f;
    public float topOffset = 0.05f;

    private Transform grabbedBlueWheel;
    private Transform grabbedRedWheel;

    private Vector3 blueOffset;
    private Vector3 redOffset;

    void Update()
    {
        HandleHand(
            leftHand, blueWheels, bluePiquets,
            ref grabbedBlueWheel, ref blueOffset);

        HandleHand(
            rightHand, redWheels, redPiquets,
            ref grabbedRedWheel, ref redOffset);
    }

    void HandleHand(
        Transform hand,
        Transform[] wheels,
        Transform[] piquets,
        ref Transform grabbedWheel,
        ref Vector3 offset)
    {
        if (hand == null)
            return;

        if (grabbedWheel == null)
        {
            TryGrabWheel(hand, wheels, ref grabbedWheel, ref offset);
            return;
        }

        // Move the wheel with the hand.
        grabbedWheel.position = hand.position + offset;

        // Release it when it reaches a matching-color piquet.
        if (TryPlaceOnPiquet(grabbedWheel, piquets))
        {
            grabbedWheel = null;
        }
    }

    void TryGrabWheel(
        Transform hand,
        Transform[] wheels,
        ref Transform grabbedWheel,
        ref Vector3 offset)
    {
        if (wheels == null)
            return;

        foreach (Transform wheel in wheels)
        {
            if (wheel == null)
                continue;

            float yDifference =
                Mathf.Abs(hand.position.y - wheel.position.y);

            if (yDifference <= yTolerance)
            {
                grabbedWheel = wheel;
                offset = wheel.position - hand.position;

                Debug.Log("Grabbed wheel: " + wheel.name);
                return;
            }
        }
    }

    bool TryPlaceOnPiquet(
        Transform wheel,
        Transform[] piquets)
    {
        if (piquets == null)
            return false;

        foreach (Transform piquet in piquets)
        {
            if (piquet == null)
                continue;

            // Check horizontal distance, ignoring height.
            Vector2 wheelXZ = new Vector2(
                wheel.position.x, wheel.position.z);

            Vector2 piquetXZ = new Vector2(
                piquet.position.x, piquet.position.z);

            float horizontalDistance =
                Vector2.Distance(wheelXZ, piquetXZ);

            // Check that the wheel has reached the top.
            float topY = piquet.position.y + topOffset;

            bool reachedTop =
                wheel.position.y >= topY &&
                wheel.position.y <= topY + placementRadius;

            if (horizontalDistance <= placementRadius && reachedTop)
            {
                // Place the wheel on top of the piquet.
                wheel.position = new Vector3(
                    piquet.position.x,
                    topY,
                    piquet.position.z);

                Debug.Log(
                    wheel.name + " placed on " + piquet.name);

                return true;
            }
        }

        return false;
    }
}
