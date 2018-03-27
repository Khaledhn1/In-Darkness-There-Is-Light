using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField]
        private MouseLook m_MouseLook;
        [SerializeField]
        private FOVKick m_FovKick = new FOVKick();
        [SerializeField]
        private bool m_UseHeadBob;
        [SerializeField]
        private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField]
        private float m_StepInterval;
        [SerializeField]
        private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField]
        private bool m_IsWalking;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_RunstepLenghten;
        // This component is only enabled for "my player" (i.e. the character belonging to the local client machine).
        // Remote players figures will be moved by a NetworkCharacter, which is also responsible for sending "my player's"
        // location to other computers.

        public float speed = 10f;       // The speed at which I run
        public float jumpSpeed = 5f;    // How much power we put into our jump. Change this to jump higher.

        // Booking variables
        Vector3 direction = Vector3.zero;   // forward/back & left/right
        float verticalVelocity = 0;     // up/down

        CharacterController cc;
        Animator anim;
        private Camera m_Camera;
        private Vector3 m_OriginalCameraPosition;
        private CharacterController m_CharacterController;
        //private PauseMenu pausemenu;
        // Use this for initialization
        void Start()
        {
            cc = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            anim = GetComponent<Animator>();
            m_MouseLook.Init(transform, m_Camera.transform);
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {

            RotateView();
            // WASD forward/back & left/right movement is stored in "direction"
            direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // This ensures that we don't move faster going diagonally
            if (direction.magnitude > 1f)
            {
                direction = direction.normalized;
            }

            // Set our animation "Speed" parameter. This will move us from "idle" to "run" animations,
            // but we could also use this to blend between "walk" and "run" as well.
            anim.SetFloat("Speed", direction.magnitude);

            // If we're on the ground and the player wants to jump, set
            // verticalVelocity to a positive number.
            // If you want double-jumping, you'll want some extra code
            // here instead of just checking "cc.isGrounded".
            if (cc.isGrounded && Input.GetButton("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }
        }

        // FixedUpdate is called once per physics loop
        // Do all MOVEMENT and other physics stuff here.
        void FixedUpdate()
        {
            if (PauseMenu.isPaused == false)
            {
                Cursor.visible = false;
            }
            // "direction" is the desired movement direction, based on our player's input
            Vector3 dist = direction * speed * Time.deltaTime;

            if (cc.isGrounded && verticalVelocity < 0)
            {
                // We are currently on the ground and vertical velocity is
                // not positive (i.e. we are not starting a jump).

                // Ensure that we aren't playing the jumping animation
                anim.SetBool("Jumping", false);

                // Set our vertical velocity to *almost* zero. This ensures that:
                //   a) We don't start falling at warp speed if we fall off a cliff (by being close to zero)
                //   b) cc.isGrounded returns true every frame (by still being slightly negative, as opposed to zero)
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                // We are either not grounded, or we have a positive verticalVelocity (i.e. we ARE starting a jump)

                // To make sure we don't go into the jump animation while walking down a slope, make sure that
                // verticalVelocity is above some arbitrary threshold before triggering the animation.
                // 75% of "jumpSpeed" seems like a good safe number, but could be a standalone public variable too.
                //
                // Another option would be to do a raycast down and start the jump/fall animation whenever we were
                // more than ___ distance above the ground.
                if (Mathf.Abs(verticalVelocity) > jumpSpeed * 0.75f)
                {
                    anim.SetBool("Jumping", true);
                }

                // Apply gravity.
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            // Add our verticalVelocity to our actual movement for this frame
            dist.y = verticalVelocity * Time.deltaTime;

            // Apply the movement to our character controller (which handles collisions for us)
            cc.Move(dist);
                //m_MouseLook.UpdateCursorLock();
        }
        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }
        private void RotateView()
        {
            if (PauseMenu.isPaused == false)
            {
                // m_MouseLook.lockCursor = true;
                Cursor.visible = false;
                m_MouseLook.LookRotation(transform, m_Camera.transform);
            }else
            {
                /// m_MouseLook.lockCursor = false;
                Cursor.visible = true;
            }
        }
    }
}