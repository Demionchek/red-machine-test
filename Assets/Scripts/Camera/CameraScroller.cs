using Camera;
using Player;
using Player.ActionHandlers;
using UnityEngine;
using Utils.Singleton;

namespace Assets.Scripts.Camera
{
    public class CameraScroller : DontDestroyMonoBehaviour
    {
        [Header("Camera scroll settings")]
        public float speedMultiplier = 20f;
        public float dragSmooth = 0.25f;

        [Space(10)]
        [Header("Camera lock frame")]
        public float maxY = 5f;
        public float maxX = 5f;
        public float minY = -5f;
        public float minX = -5f;

        private Vector3 _startPosition;
        private ClickHandler _clickHandler;
        private bool _isScrolling;
        private Vector3 _velocity;

        protected override void Awake() {
            base.Awake();

            _clickHandler = ClickHandler.Instance;
            _clickHandler.SetDragEventHandlers(OnDragStart, OnDragEnd);
        }

        private void Update()
        {
            if (!_isScrolling)
                return;

            CameraScrolling();
        }

        private void CameraScrolling()
        {
            Vector3 coursorPos = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 scrollDir = _startPosition - coursorPos;
            Vector3 targetPos = transform.position + scrollDir * speedMultiplier;
            targetPos = ClampPosition(targetPos);

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, dragSmooth);
            _startPosition = coursorPos;
        }

        private Vector3 ClampPosition(Vector3 targetPos)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
            targetPos.z = transform.position.z;
            return targetPos;
        }

        public void OnDragStart(Vector3 startPos)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;

            _startPosition = startPos;
            _isScrolling = true;
        }

        public void OnDragEnd(Vector3 endPos)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;

            _isScrolling = false;
        }


    }
}
