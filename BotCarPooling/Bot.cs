using UnityEngine;
using UnityEngine.Pool;

public class Bot : MonoBehaviour
{
    private float speed;
    private const float destroyZPos = -0.3f;
    private IObjectPool<Bot> botPool;
    private const string coinName = "Coin";

    private void Awake()
    { 
        speed = FindObjectOfType<GameManager>().carSpeed;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        OnGameStateChanged(GameStateManager.Instance.CurrentGameState);
    }

    private void OnDestroy() => GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;

    public void SetPool(IObjectPool<Bot> pool) => botPool = pool;

    private void Update()
    {
        transform.position += new Vector3(0, 0, -Time.deltaTime * speed);
        if (transform.position.z < destroyZPos) //Maybe with Hit collider?
            if (CompareTag(coinName))
                foreach (Transform child in transform)
                    child.gameObject.SetActive(true);             
            botPool.Release(this); // Object Release      
    }

    //Game State check
    private void OnGameStateChanged(GameState newGameState) => enabled = newGameState == GameState.GamePlay;
}
