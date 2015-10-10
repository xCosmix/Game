using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Action {
    //STATIC::::::::::::::::
    private static MonoBehaviour invoker;
    private static List<User> users = new List<User>();

    public class User : System.Object
    {
        public User (int id)
        {
            this.id = id;
        }

        public int id;
        public int position;
        public List<Action> running = new List<Action>();
        public List<Action> queue = new List<Action>();
        public Action request;

        public void Request (Action input)
        {
            request = input;
            Actualize();
        }
        private byte PrivateRequest(Action input)
        {
            if (input.monoInstance)
            {
                if (ContainsName(input.name, input.tag, running) || ContainsName(input.name, input.tag, queue)) return 2;
            }
            if (input.layer == 0) { return 0; }
            List<Action> myTag = new List<Action>();
            foreach (Action act in running)
            {
                if (act.tag == input.tag) myTag.Add(act);
            }
            if (myTag.Count == 0) { return 0; }
            byte higherLayer = HigherLayer(myTag);
            if (higherLayer <= input.layer) { return 0; } else { return 1; }
        }
        private byte HigherLayer (List<Action> acts)
        {
            byte higherLayer = 0;
            foreach (Action act in acts)
            {
                if (act.layer > higherLayer)
                    higherLayer = act.layer;
            }
            return higherLayer;
        }
        private bool ContainsName (string name, string tag, List<Action> acts)
        {
            foreach (Action act in acts)
            {
                if (act.name == name && act.tag == tag)
                    return true;
            }
            return false;
        }
        private void Accept (Action action)
        {
            if (running.Contains(action)) return;
           // Debug.Log("Accept: " + action.name);
            if (queue.Contains(action)) queue.Remove(action);
            running.Add(action);
            action.Restart();
            action.Run();
        }
        private void Deliver (Action action)
        {
            if (queue.Contains(action)) return;
            //Debug.Log("Deliver: " + action.name);
            if (running.Contains(action)) running.Remove(action);
            queue.Add(action);
            action.Queue();
            action.Run();
        }
        public void Remove (Action action)
        {
           // Debug.Log("Remove: " + action.name);
            running.Remove(action);
            Actualize();
        }
        private void Actualize ()
        {
            List<Action> forAccept = new List<Action>();
            List<Action> forDeliver = new List<Action>();
            int value_ = 0;

            if (request!=null)
            {
                value_ = PrivateRequest(request);
                if (value_ != 2) { if (value_ == 0) Accept(request); else Deliver(request); }
                request = null;
            }

            foreach (Action act in running)
            {
                value_ = PrivateRequest(act);
                if (value_ != 2) { if (value_ == 0) forAccept.Add(act); else forDeliver.Add(act); }
            }

            foreach (Action act in forAccept) Accept(act);
            foreach (Action act in forDeliver) Deliver(act);

            foreach (Action act in queue)
            {
                value_ = PrivateRequest(act);
                if (value_ != 2) { if (value_ == 0) forAccept.Add(act); else forDeliver.Add(act); }
            }

            foreach (Action act in forAccept) Accept(act);
            foreach (Action act in forDeliver) Deliver(act);

        }
    }
    public static User GetUser (int userID)
    {
        foreach (User user in users)
        {
            if (user.id != userID) continue;
            return user;
        }
        User newUser = new User(userID);
        newUser.position = users.Count;
        users.Add(newUser);
        return newUser;
    }
    public static void Invoker ()
    {
        System.Type[] comps = { typeof(MonoBehaviour) };
        invoker = new GameObject("invoker", comps).GetComponent<MonoBehaviour>();
    }
    //STATIC::::::::::::::::
    //Fields
    public byte layer;
    public string name;
    public string tag;
    public bool monoInstance, queueable;
    public User user;
    public float startTime;
    public int frames;
    //Fields
    private int currentFrames;
    private bool isFixed;
    private bool goal_, break_, queue_;
    private Coroutine run;
    public void Create (GameObject userGO, string name, string tag, byte layer)
    {
        user = GetUser(userGO.GetInstanceID());
        this.name = name;
        this.tag = tag;
        this.layer = layer;
        monoInstance = true;
        queueable = true;
        user.Request(this);
    }
    public void Create(GameObject userGO, string name, string tag, byte layer, int frames) //frames zero mean action without frame count 
    {
        user = GetUser(userGO.GetInstanceID());
        this.name = name;
        this.tag = tag;
        this.layer = layer;
        monoInstance = true;
        queueable = true;
        isFixed = true;
        this.frames = frames;
        user.Request(this);
    }
    public void Queue ()
    {
        queue_ = true;
    }
    public void Run ()
    {
        if (invoker == null) Invoker();
        if (run == null)
        {
            if (!isFixed)
                run = invoker.StartCoroutine(Run_private());
            else if (frames==0)
                run = invoker.StartCoroutine(Run_private_fixed());
            else
                run = invoker.StartCoroutine(Run_private_fixed_limited());
        }
    }
    public void Restart ()
    {
        queue_ = false;
    }
    private IEnumerator Run_private ()
    {
        OnLoad();
        while (queue_) yield return null;
        goal_ = Goal();
        break_ = Break();
        if (!goal_ && !break_)
        {
            startTime = Time.time;

            OnStart();

            while (!goal_ && !break_)
            {
                while (queue_) yield return null;
                yield return null;
                OnUpdate();
                goal_ = Goal();
                break_ = Break();
            }
        }
        End();
    }
    private IEnumerator Run_private_fixed()
    {
        OnLoad();
        while (queue_) yield return null;
        goal_ = Goal();
        break_ = Break();
        if (!goal_ && !break_)
        {
            startTime = Time.time;

            OnStart();

            while (!goal_ && !break_)
            {
                while (queue_) yield return null;
                yield return new WaitForFixedUpdate(); 
                OnUpdate();
                goal_ = Goal();
                break_ = Break();
            }
        }
        End();
    }
    private IEnumerator Run_private_fixed_limited()
    {
        OnLoad();
        while (queue_) yield return null;
        goal_ = Goal();
        break_ = Break();
        if (!goal_ && !break_)
        {
            startTime = Time.time;
            currentFrames = 0;

            OnStart();

            while (!goal_ && !break_ && !AtFrame(frames))
            {
                while (queue_) yield return null;
                yield return new WaitForFixedUpdate(); 
                OnUpdate();
                goal_ = Goal();
                break_ = Break();
                currentFrames++;
            }
        }
        End();
    }
    public void End()
    {
        if (goal_) OnGoal(); else OnBreak();
        OnEnd();
        run = null;
        user.Remove(this);
    }
    public bool AtFrame (int frame){ return currentFrames == frame;}
    public bool AfterFrame(int frame){ return currentFrames > frame;}
    public bool BeforeFrame(int frame){ return currentFrames < frame;}
    //CUSTOMIZABLE
    public virtual bool Goal ()
    {
        return true;
    }
    public virtual bool Break ()
    {
        return true;
    }
    public virtual void OnLoad ()
    {

    }
    public virtual void OnStart ()
    {

    }
    public virtual void OnUpdate ()
    {

    }
    public virtual void OnGoal ()
    {

    }
    public virtual void OnBreak ()
    {

    }
    public virtual void OnEnd()
    {

    }
    //CUSTOMIZABLE
}
