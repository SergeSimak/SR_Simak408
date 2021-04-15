using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game
{
    public class RoadChunk : MonoBehaviour
    {
        public static Action OnRightPositionSet;

        [SerializeField] private float _currentYRotation;
        [SerializeField] private float _animateScaleTime;
        [SerializeField] private bool _isRightChank;

        private bool _inPosition;

        private void Start()
        {
            CheckRightPosition();
        }

        private void OnMouseDown()
        {
            if (_inPosition)
                return;

            transform.rotation *= Quaternion.Euler(Vector3.up * 90);
            CheckRightPosition();
        }

        private void CheckRightPosition()
        {
            if (_isRightChank && transform.rotation.eulerAngles.y == _currentYRotation)
            {
                _inPosition = true;
                var startScale = transform.localScale;

                OnRightPositionSet?.Invoke();

                Sequence seq = DOTween.Sequence();
                seq.Append(transform.DOScale(startScale * 1.25f, _animateScaleTime / 2f));
                seq.Append(transform.DOScale(startScale, _animateScaleTime / 2f));
            }
        }
    }

}