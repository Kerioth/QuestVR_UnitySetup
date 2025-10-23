using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HitMoleGame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<RoundLoop> gameLoops;
    [Header("References")]
    [SerializeField] private List<MoleHole> allMoleHoles;
    private List<MoleHole> downMolesHoles;

    [Header("Events")]
    public UnityEvent OnStart;
    [Space]
    public UnityEvent OnGameStart;
    public UnityEvent OnGameEnd;
    [Space]
    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;
    [Space]
    public UnityEvent OnMolesUp;
    public UnityEvent OnMolesDown;
    public UnityEvent OnAnyMoleHit;
    public UnityEvent OnAllUpMolesHit;
    
    private bool isGame;
    private int molesUp;
    private int molesHit;
    
    private void Start()
    {
        downMolesHoles = new List<MoleHole>(allMoleHoles);
        OnStart?.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public async void StartGame()
    {
        OnGameStart.Invoke();
        isGame =  true;
        foreach (var loop in gameLoops)
        {
            await Awaitable.WaitForSecondsAsync(loop.startWait);
            OnRoundStart?.Invoke();
            foreach (var cycle in loop.cycles)
            {
                if(!isGame) return;
                UpMoles(cycle.molesCount, loop.molesWait);
                molesUp = cycle.molesCount;
                molesHit = 0;
                await Awaitable.WaitForSecondsAsync(cycle.delay);
            }
            OnRoundEnd?.Invoke();
        }
        OnGameEnd.Invoke();
        isGame =  false;
    }

    private void OnDisable()
    {
        isGame = false;
    }

    private async Awaitable RoundsLoop()
    {
        await Awaitable.WaitForSecondsAsync(2f);
        for (int i = 0; i < 20; i++)
        {
            if(!isGame) return;
            UpMoles(3);
            await Awaitable.WaitForSecondsAsync(3f);
        }
    }
    

    private async void UpMoles(int molesCount, float waitTime = 1f)
    {
        OnMolesUp?.Invoke();
        for (int i = 0; i < molesCount; i++)
        {
            RandomUp(waitTime);
        }
        await Awaitable.WaitForSecondsAsync(waitTime);
        OnMolesDown?.Invoke();
    }

    public void RandomUp(float waitTime = 1f)
    {
        if(downMolesHoles.Count == 0) return;
        
        int index = UnityEngine.Random.Range(0, downMolesHoles.Count);
        UpMole(index, waitTime);
    }

    private void UpMole(int mole, float waitTime = 1f)
    {
        downMolesHoles[mole].MoleUp(waitTime);
        downMolesHoles[mole].OnHit.AddListener(MoleGetHit);
        downMolesHoles[mole].OnDown.AddListener(AddMoleBack);
        downMolesHoles.RemoveAt(mole);
    }

    private void MoleGetHit(MoleHole mole)
    {
        molesHit++;
        OnAnyMoleHit?.Invoke();
        CheckMolesHit();
    }

    private void CheckMolesHit()
    {
        if(molesHit == molesUp)
            OnAllUpMolesHit?.Invoke();
    }

    private void AddMoleBack(MoleHole mole)
    {
        downMolesHoles.Add(mole);
        mole.RemoveListeners();
    }
}


