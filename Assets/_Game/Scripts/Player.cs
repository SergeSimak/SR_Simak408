using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private List<Transform> _currentPath;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private int _countPathChunks;

        private int _chunksOnPositions;

        private void Awake()
        {
            RoadChunk.OnRightPositionSet += ChunkOnPosition;
        }

        private IEnumerator MoveThroughPath(float time)
        {
            while (_currentPath.Count > 0)
            {
                var currentPos = transform.position;
                var directionToPoint = (_currentPath[0].position - currentPos).normalized;
                var rotation = Quaternion.LookRotation(directionToPoint).eulerAngles;
                rotation.x = 0;
                rotation.z = 0;

                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), _rotateSpeed * (elapsedTime / time));
                    transform.position = Vector3.Lerp(currentPos, _currentPath[0].position, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                yield return time;
                _currentPath.RemoveAt(0);
            }

            _animator.SetBool("Run", false);
        }

        public void ChunkOnPosition()
        {
            _chunksOnPositions++;

            if (_chunksOnPositions == _countPathChunks)
            {
                _animator.SetBool("Run", true);
                StartCoroutine(MoveThroughPath(1f));
            }
        }

        private void OnDestroy()
        {
            RoadChunk.OnRightPositionSet -= ChunkOnPosition;
        }
    }
}
