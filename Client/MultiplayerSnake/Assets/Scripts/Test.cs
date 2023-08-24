using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private int _detailsCount;

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

        }
    }
}
