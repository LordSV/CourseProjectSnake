using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private int _detailsCount;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;
    private Controller _controller;
    private Snake _snake;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(_snake != null)
            {
                _snake.Destroy();
            }
            if(_controller != null)
            {
                Destroy(_controller.gameObject);
            }
            _snake = Instantiate(_snakePrefab);
            _snake.Init(_detailsCount);
            _controller = Instantiate(_controllerPrefab);
            _controller.Init(_snake);
        }
    }
}
