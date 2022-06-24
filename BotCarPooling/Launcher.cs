// Traffic Racer game bot car object pooling.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Launcher : MonoBehaviour
{
    private IObjectPool<Bot> botPool;
    public List<Transform> botPositionList;
    public List<Bot> botList;
    private float botInstantiateTimer = 5f;
    private const float botInstantiateTimerMinValue = 1f;

    private int botCount = 0;
    public float getReleaseTimer = 1.5f;

    private void Awake()
    {
        botPool = new ObjectPool<Bot>(
            CreateBullet,
            OnGet,
            OnRelease);

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        OnGameStateChanged(GameStateManager.Instance.CurrentGameState);
    }

    private void OnDestroy() => GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;

    private Bot CreateBullet()
    {
        if (botCount == 4) botCount = 0;
        Bot bot = Instantiate(botList[botCount]);
        botCount++;
        PositionSet(bot);
        bot.SetPool(botPool);
        return bot;
    }

    private void OnGet(Bot bot)
    {
        bot.gameObject.SetActive(true);
        PositionSet(bot);
    }

    private void OnRelease(Bot bot) => bot.gameObject.SetActive(false);

    //public void OnDestroy(Bot bot)
    //{
    //    Destroy(bot.gameObject);
    //}

    void Update()
    {
        botInstantiateTimer -= Time.deltaTime;
        if (botInstantiateTimer < 0)
        {
            botPool.Get();
            botInstantiateTimer = getReleaseTimer;
        }
    }

    public void GetReleaseTimerDecrease()
    {
        if (getReleaseTimer > botInstantiateTimerMinValue) getReleaseTimer -= 0.1f;        
    }

    private void PositionSet(Bot bot)
    {
        int rndBotPos = Random.Range(0, botPositionList.Count);
        bot.transform.position = new Vector3(botPositionList[rndBotPos].position.x,
            bot.transform.position.y,
            botPositionList[rndBotPos].position.z);
    }

    //Game State check-Gameplay or Pause
    private void OnGameStateChanged(GameState newGameState) => enabled = newGameState == GameState.GamePlay;
}
