using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float Speed { get { return _speed; } }
    [SerializeField] private Tail _tailPrefab;
    [SerializeField] private Transform _head;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rotateSpeed = 90f;
    private Vector3 _targetDirection = Vector3.zero;
    private Tail _tail;

    public void Init(int detailCount)
    {
        _tail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _tail.Init(_head, _speed, detailCount);
    }
    public void SetDetailCount(int detailCount)
    {
        _tail.SetDetailCount(detailCount);
    }
    public void Destroy()
    {
        _tail.Destroy();
        Destroy(gameObject);
    }
    private void Update()
    {
        Move();
    }

    private void Rotate()
    {
        //Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
        //_head.rotation = Quaternion.RotateTowards(_head.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
        ////float diffY = _directionPoint.eulerAngles.y - _head.eulerAngles.y;

        ////if (diffY > 180f) diffY = (diffY - 180f) * -1f;
        ////else if (diffY < -180f) diffY = (diffY +180f) * -1f;

        ////float maxAngle = Time.deltaTime * _rotateSpeed;
        ////float rotateY = Mathf.Clamp(diffY, -maxAngle, maxAngle);

        ////_head.Rotate(0, rotateY, 0);
    }

    private void Move()
    {
        transform.position += _head.forward * Time.deltaTime * _speed;
    }

    public void SetRotation(Vector3 pointToLook)
    {
        _head.LookAt(pointToLook);
    }
    //public void LerpRotation(Vector3 cursorPosition)
    //{
    //    _targetDirection = cursorPosition - _head.position;
    //    //_directionPoint.LookAt(cursorPosition);
    //}

}
