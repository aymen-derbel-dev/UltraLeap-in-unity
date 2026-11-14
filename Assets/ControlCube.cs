using UnityEngine;

public class ControlCube : MonoBehaviour
{
    public ControlLeap controlLeap;

    void Update()
    {
        if (controlLeap == null)
        {
            return;
        }

        if (CompareTag("cube1"))
        {
            if (controlLeap.rightHandTracked)
            {
                transform.position = controlLeap.positionR;
            }
        }
        else if (CompareTag("cube2"))
        {
            if (controlLeap.leftHandTracked)
            {
                transform.position = controlLeap.positionL;
            }
        }
    }
}
