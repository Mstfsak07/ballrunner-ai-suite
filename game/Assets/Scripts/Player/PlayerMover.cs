using UnityEngine;
using System;

namespace BallRunner.Player
{
    public sealed class PlayerMover : MonoBehaviour
    {
        [Header("Forward")]
        [SerializeField] private float forwardSpeed = 8f;

        [Header("Horizontal")]
        [SerializeField] private float horizontalSensitivity = 0.0125f;
        [SerializeField] private float horizontalLerp = 16f;
        [SerializeField] private float minX = -3.2f;
        [SerializeField] private float maxX = 3.2f;

        private bool isDragging;
        private bool firstInputRaised;
        private float dragStartScreenX;
        private float dragStartWorldX;
        private float targetX;
        public event Action OnFirstInputDetected;

        private void Awake()
        {
            targetX = transform.position.x;
        }

        private void Update()
        {
            TickForward();
            TickHorizontalInput();
            ApplyHorizontal();
        }

        private void TickForward()
        {
            transform.position += Vector3.forward * (forwardSpeed * Time.deltaTime);
        }

        private void TickHorizontalInput()
        {
            if (TryGetPointerDown(out var downX))
            {
                RaiseFirstInputIfNeeded();
                isDragging = true;
                dragStartScreenX = downX;
                dragStartWorldX = targetX;
            }

            if (isDragging && TryGetPointerHeld(out var heldX))
            {
                var delta = (heldX - dragStartScreenX) * horizontalSensitivity;
                targetX = Mathf.Clamp(dragStartWorldX + delta, minX, maxX);
            }

            if (TryGetPointerUp())
            {
                isDragging = false;
            }
        }

        private void ApplyHorizontal()
        {
            var position = transform.position;
            position.x = Mathf.Lerp(position.x, targetX, horizontalLerp * Time.deltaTime);
            transform.position = position;
        }

        private static bool TryGetPointerDown(out float x)
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    x = touch.position.x;
                    return true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                x = Input.mousePosition.x;
                return true;
            }

            x = 0f;
            return false;
        }

        private static bool TryGetPointerHeld(out float x)
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    x = touch.position.x;
                    return true;
                }
            }

            if (Input.GetMouseButton(0))
            {
                x = Input.mousePosition.x;
                return true;
            }

            x = 0f;
            return false;
        }

        private static bool TryGetPointerUp()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    return true;
                }
            }

            return Input.GetMouseButtonUp(0);
        }

        private void RaiseFirstInputIfNeeded()
        {
            if (firstInputRaised)
            {
                return;
            }

            firstInputRaised = true;
            OnFirstInputDetected?.Invoke();
        }
    }
}
