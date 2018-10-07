using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(TurnManager))]
public class Managers : MonoBehaviour {

    public static InputManager InputManager { get; private set; }
    public static TurnManager TurnManager { get; private set; }
    public static RoundManager RoundManager { get; private set; }

    private List<IGameManager> _startSequence;

    public static Referee Referee { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InputManager = GetComponent<InputManager>();
        TurnManager = GetComponent<TurnManager>();
        Referee = GetComponent<Referee>();
        RoundManager = GetComponent<RoundManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(InputManager);
        _startSequence.Add(TurnManager);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }
        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
                Debug.Log("Progress: " + numReady + "/" + numModules);
            yield return null;
        }

        Debug.Log("All managers started up");
    }

}
