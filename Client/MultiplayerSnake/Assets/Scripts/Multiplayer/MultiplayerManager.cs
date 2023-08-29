using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Unity.VisualScripting;
using System;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    #region Server
    private const string GameRoomName = "state_handler";

    private ColyseusRoom<State> _room;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClient();
        Connection();
    }

    private async void Connection()
    {
        _room = await client.JoinOrCreate<State>(GameRoomName);
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == false) return;
        _room.OnStateChange -= OnChange;

        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;

        _room.State.apples.ForEach(CreateApple);
        _room.State.apples.OnAdd += (key, apple) => CreateApple(apple);
        _room.State.apples.OnRemove += RemoveApple;
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void LeaveRoom()
    {
        _room?.Leave();
    }

    public void SendMesssageToServer(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }
    public void SendMesssageToServer(string key, string data)
    {
        _room.Send(key, data);
    }

    #endregion

    #region Player
    [SerializeField] private PlayerAim _playerAim;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;
    private void CreatePlayer(Player player)
    {
        Vector3 position = new Vector3(player.x, 0, player.z);
        Quaternion quaternion = Quaternion.identity;

        Snake snake = Instantiate(_snakePrefab, position, quaternion);
        snake.Init(player.d, true);

        PlayerAim aim = Instantiate(_playerAim, position, quaternion);
        aim.Init(snake._head, snake.Speed);

        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(_room.SessionId, aim, player, snake);
    }
    #endregion

    #region Enemy
    Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();    
    private void CreateEnemy(string key, Player player)
    {
        Vector3 position = new Vector3(player.x, 0, player.z);
        Snake snake = Instantiate(_snakePrefab);
        snake.Init(player.d);

       EnemyController enemy = snake.AddComponent<EnemyController>();
        enemy.Init(key, player, snake);

        _enemies.Add(key, enemy);

    }
    private void RemoveEnemy(string key, Player player)
    {
        if (_enemies.ContainsKey(key) == false)
        {
            Debug.LogError("No Enemy for remove");
            return;
        }
        EnemyController enemy = _enemies[key];
        _enemies.Remove(key);
        enemy.Destroy();
    }
    #endregion
    #region Apple
    [SerializeField] private Apple _applePrefab;
    private Dictionary<Vector2Float, Apple> _apples = new Dictionary<Vector2Float, Apple>();
    private void CreateApple(Vector2Float vector2Float)
    {
        Vector3 position = new Vector3(vector2Float.x, 0, vector2Float.z);
        Apple apple = Instantiate(_applePrefab, position, Quaternion.identity);
        apple.Init(vector2Float);
        _apples.Add(vector2Float, apple);
    }
    private void RemoveApple(int key, Vector2Float vector2Float)
    {
        if (_apples.ContainsKey(vector2Float) == false) return;
        Apple apple = _apples[vector2Float];
        _apples.Remove(vector2Float);
        apple.Destroy();
    }
    #endregion

}
