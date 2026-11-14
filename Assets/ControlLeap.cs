using UnityEngine;
using Leap;
using Leap.Unity;

public class ControlLeap : MonoBehaviour
{
    public LeapProvider leapProvider;

    public Vector3 positionR;
    public Vector3 positionL;

    public bool rightHandTracked;
    public bool leftHandTracked;

    void Update()
    {
        if (leapProvider == null) return;

        Frame currentFrame = leapProvider.CurrentFrame;

        rightHandTracked = false;
        leftHandTracked = false;

        for (int i = 0; i < currentFrame.Hands.Count; i++)
        {
            Hand currentHand = currentFrame.Hands[i];

            if (currentHand.IsLeft)
            {
                positionL = currentHand.PalmPosition;
                leftHandTracked = true;
            }
            else
            {
                positionR = currentHand.PalmPosition;
                rightHandTracked = true;
            }
        }
    }
}
