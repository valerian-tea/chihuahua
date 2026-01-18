using UnityEngine;

namespace ithappy.Animals_FREE
{
    [RequireComponent(typeof(CreatureMover))]
    public class MovePlayerInput : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField]
        private string m_HorizontalAxis = "Horizontal";

        [SerializeField]
        private string m_VerticalAxis = "Vertical";

        [SerializeField]
        private string m_JumpButton = "Jump";

        [SerializeField]
        private KeyCode m_RunKey = KeyCode.LeftShift;

        [Header("Camera")]
        [SerializeField]
        private PlayerCamera m_Camera;

        [Header("First-Person / Cinemachine fallback")]
        [Tooltip("Optional: set to the Camera.transform used in first-person setups (or leave empty to use Camera.main)")]
        [SerializeField]
        private Transform m_CameraTransform;

        [SerializeField]
        private string m_MouseX = "Mouse X";

        [SerializeField]
        private string m_MouseY = "Mouse Y";

        [SerializeField]
        private string m_MouseScroll = "Mouse ScrollWheel";

            [Header("Turning")]
            [SerializeField, Tooltip("Degrees per second to rotate when horizontal input is held")]
            private float m_TurnSpeed = 180f;

        private CreatureMover m_Mover;

        private Vector2 m_Axis;
        private bool m_IsRun;
        private bool m_IsJump;

        private Vector3 m_Target;
        private Vector2 m_MouseDelta;
        private float m_Scroll;

        private void Awake()
        {
            m_Mover = GetComponent<CreatureMover>();
        }

        private void Update()
        {
            GatherInput();
            SetInput();
        }

        public void GatherInput()
        {
            // Read raw inputs first
            float rawH = Input.GetAxis(m_HorizontalAxis);
            float rawV = Input.GetAxis(m_VerticalAxis);

            // Disallow backward motion: clamp vertical to >= 0
            rawV = Mathf.Max(0f, rawV);

            // If player is pressing left/right but not forward, treat that as "turn while moving forward"
            // (so holding left/right alone will cause continuous forward+turn motion)
            if (rawV <= 0.001f && Mathf.Abs(rawH) > 0.001f)
            {
                rawV = 1f; // full forward when only turning
            }

            // Optional smoothing could be applied here by lerping m_Axis toward target
            m_Axis = new Vector2(rawH, rawV);
            m_IsRun = Input.GetKey(m_RunKey);
            m_IsJump = Input.GetButton(m_JumpButton);

            // Determine movement target point used by the mover.
            // For third-person we use the PlayerCamera.Target. For first-person/Cinemachine setups
            // the project may not have a PlayerCamera; fall back to a camera transform (or Camera.main).
            if (m_Camera != null)
            {
                m_Target = m_Camera.Target;
            }
            else if (m_CameraTransform != null)
            {
                // use a point in front of the camera as the target
                m_Target = m_CameraTransform.position + m_CameraTransform.forward * 1f;
            }
            else if (Camera.main != null)
            {
                m_Target = Camera.main.transform.position + Camera.main.transform.forward * 1f;
            }
            else
            {
                m_Target = Vector3.zero;
            }
            m_MouseDelta = new Vector2(Input.GetAxis(m_MouseX), Input.GetAxis(m_MouseY));
            m_Scroll = Input.GetAxis(m_MouseScroll);
        }

        public void BindMover(CreatureMover mover)
        {
            m_Mover = mover;
        }

        public void SetInput()
        {
            // Continuous turning: rotate the player while horizontal input is held
            if (Mathf.Abs(m_Axis.x) > 0.001f)
            {
                float yaw = m_Axis.x * m_TurnSpeed * Time.deltaTime;
                transform.Rotate(0f, yaw, 0f);
            }

            if (m_Mover != null)
            {
                m_Mover.SetInput(in m_Axis, in m_Target, in m_IsRun, m_IsJump);
            }

            if (m_Camera != null)
            {
                m_Camera.SetInput(in m_MouseDelta, m_Scroll);
            }
        }
    }
}
