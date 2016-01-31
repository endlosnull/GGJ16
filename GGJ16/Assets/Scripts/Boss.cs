using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Pooling;

[System.Serializable]
public class ScoreUpdateEvent : UnityEvent<string, string> {}
[System.Serializable]
public class ChangeScreenEvent : UnityEvent<string> {}
[System.Serializable]
public class UpdateTimeEvent : UnityEvent<float> {}

public class Boss : Singleton<Boss>
{
    public ScoreUpdateEvent ScoreUpdate = new ScoreUpdateEvent();
    public List<User> users = new List<User>();
    public Field field;
    public int[] Scores = new int[] { 0, 0 };

    [System.Serializable]
    public enum State
    {
        SettingUp,
        PreGame,
        Loadout,
        InGame
    }

    public State state = State.SettingUp;
    public bool IsSettingUp { get { return state == State.SettingUp; } }
    public bool IsInGame { get { return state == State.InGame; } }

    public Canvas startGameCanvas;
    public UnityEngine.UI.Button startGameButton;
    public UpdateTimeEvent UpdateTime = new UpdateTimeEvent();
    public ChangeScreenEvent ChangeScreen = new ChangeScreenEvent();

    private float time;

    void Awake()
    {
        ChangeScreen.Invoke("SelectTeam");
    }

    public void Update()
    {
        startGameButton.enabled = (IsSettingUp && users.Count > 0);
        time += Time.deltaTime;
        UpdateTime.Invoke(time);
    }

    public void AddKeyboardPlayer()
    {
        string prefix = "Key";
        User existingUser = users.Find(x => x.inputPrefix == prefix);
        if (existingUser == null)
        {
            GameObject go = new GameObject();
            go.name = "user" + users.Count;
            User user = go.AddComponent<User>();
            user.inputPrefix = prefix;
            users.Add(user);
        }
    }

    public void AddJoystickPlayer(int index)
    {
        string prefix = "Joy" + index;
        User existingUser = users.Find(x => x.inputPrefix == prefix);
        if (existingUser == null)
        {
            GameObject go = new GameObject();
            go.name = "user" + users.Count;
            User user = go.AddComponent<User>();
            user.inputPrefix = prefix;
            users.Add(user);
        }
    }

    public void AddRemote(int index)
    {
        string prefix = "Rmt" + index;
        User existingUser = users.Find(x => x.inputPrefix == prefix);
        if (existingUser == null)
        {
            GameObject go = new GameObject();
            go.name = "user" + users.Count;
            User user = go.AddComponent<User>();
            user.inputPrefix = prefix;
            users.Add(user);
        }
    }

    public void ChangeStateInt(int nextStateIdx)
    {
        ChangeState((State)nextStateIdx);
    }

    void ChangeState(State nextState)
    {
        if (state == nextState)
        {
            return;
        }
        state = nextState;
        switch (state)
        {
            case State.SettingUp:
                ChangeScreen.Invoke("SelectTeam");
                StartSetup();
                break;
            case State.InGame:
                ChangeScreen.Invoke("Play");
                StartGame();
                break;
            default:
                break;
        }
    }

    public void StartSetup()
    {
        users.Clear();
    }


    void StartGame()
    {
        time = 0f;
        for (int i = 0; i < users.Count; ++i)
        {
            GameObject go = GameObjectFactory.Instance.Spawn("p-Actor", null, Vector3.zero, Quaternion.identity);
            go.name = "hero" + i;
            Actor actor = go.GetComponent<Actor>();
            actor.controller = go.AddComponent<ActorController>();
            GameObject bodyObject = GameObjectFactory.Instance.Spawn("p-ActorBodyOne", null, Vector3.zero, Quaternion.identity);
            bodyObject.transform.parent = actor.transform;
            actor.body = bodyObject.AddComponent<Body>();
            GameObject attachObject = GameObjectFactory.Instance.Spawn("p-AttachHeaddressBird", null, Vector3.zero, Quaternion.identity);
            attachObject.transform.parent = bodyObject.transform;
            attachObject.transform.localPosition = Vector3.up * 0.65f;
            actor.body.attachments.Add(attachObject);
            users[i].controlledActor = actor;
            go.AddComponent<ActionSequencer>();
            go.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        }

        GameObject fieldObject = GameObjectFactory.Instance.Spawn("p-Field", null, Vector3.zero, Quaternion.identity);
        field = fieldObject.GetComponent<Field>();
        field.BeginRound();

    }

    public void FixedUpdate()
    {
        if (field == null)
            return;

        Ball ball = field.ball;
        for (int i = 0; i < users.Count; ++i)
        {
            Actor actor = users[i].controlledActor;
            actor.BallHandling(ball);
        }
    }
}