using ithappy.Animals_FREE;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private CreatureMover mover;

    void Awake()
    {
        mover = GetComponent<CreatureMover>();
    }

    void Update()
    {
        // Forward movement only (W / Up)
        float forward = Input.GetAxis("Vertical"); // 0–1
        float turn = Input.GetAxis("Horizontal"); // -1–1

        // CreatureMover expects a Vector2 axis
        Vector2 axis = new Vector2(turn, forward);

        Vector3 target = transform.position + transform.forward * 2f;

        // Run when holding Shift
        bool isRun = Input.GetKey(KeyCode.LeftShift);

        mover.SetInput(
            axis,
            target,
            isRun,
            false // jump not used here
        );
    }
}
