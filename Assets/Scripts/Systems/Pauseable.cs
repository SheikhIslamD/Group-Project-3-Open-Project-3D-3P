using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pauseable : MonoBehaviour
{
    public bool paused { get; private set; }

    void Awake()
    {
        List<MonoBehaviour> getMonos = new List<MonoBehaviour>(GetComponents<MonoBehaviour>());
        getMonos.Remove(this);
        monos = getMonos.ToArray();

        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        constForce = GetComponent<ConstantForce>();
    }

    public void SetPause(bool value)
    {
        if (paused == value) return;
        paused = value;

        if (paused) SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);

        for (int i = 0; i < monos.Length; i++) monos[i].enabled = !paused;

        if (!paused) SendMessage("OnUnPause", SendMessageOptions.DontRequireReceiver);

        HandleComponent(animator);
        HandleComponent(navAgent);
        HandleComponent(constForce);
        HandleComponent(rigidBody);

    }
    public void Pause()         => SetPause(true);
    public void UnPause()       => SetPause(false);
    public void TogglePause()   => SetPause(!paused);  

    private MonoBehaviour[] monos;


    private Animator animator;
    private bool s_anim_enabled;
    private void HandleComponent(Animator anim)
    {
        if(anim == null) return;

        if (paused)
        {
            s_anim_enabled = anim.enabled;
            anim.enabled = false;
        }
        else
        {
            anim.enabled = s_anim_enabled;
        }

    }

    private NavMeshAgent navAgent;
    private Vector3 s_nav_destination;
    private Vector3 s_nav_velocity;
    private void HandleComponent(NavMeshAgent nav)
    {
        if(nav == null) return;

        if (paused)
        {
            s_nav_destination = nav.destination;
            s_nav_velocity = nav.velocity;
            nav.isStopped = true;
        }

        nav.enabled = !paused;

        if (!paused)
        {
            nav.isStopped = false;
            nav.destination = s_nav_destination;
            nav.velocity = s_nav_velocity;
        }
    }

    private Rigidbody rigidBody;
    private Vector3 s_rb_velocity;
    private Vector3 s_rb_angularVelocity;
    private void HandleComponent(Rigidbody rb)
    {
        if(rb == null) return;

        if (paused)
        {
            s_rb_velocity = rb.velocity;
            s_rb_angularVelocity = rb.angularVelocity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
        else
        {
            rb.WakeUp();
            rb.velocity = s_rb_velocity;
            rb.angularVelocity = s_rb_angularVelocity;
        }

    }

    private ConstantForce constForce;
    private void HandleComponent(ConstantForce force)
    {
        if(force == null) return;
        constForce.enabled = !paused;
    }


    private void OnEnable() => Register();

    private void OnDisable() => UnRegister();
    private void OnDestroy() => UnRegister();


    [HideInInspector] public bool registered;
    public void Register()
    {
        if (registered) return;
        if (!GameplayPauseManager.Get()) return;

        GameplayPauseManager.RegisterPausable(this);
    }
    public void UnRegister()
    {
        if (!registered) return;
        if (!GameplayPauseManager.Get()) return;

        GameplayPauseManager.UnRegisterPausable(this);
    }
}

/*

public interface PauseableBehavior<T> where T : MonoBehaviour
{
    void OnPause();
    void OnUnPause();

    public sealed void Pause() => ((T)this).GetComponent<Pauseable>().Pause();
    public sealed void UnPause() => ((T)this).GetComponent<Pauseable>().UnPause();

    public sealed bool IsPaused() => ((T)this).GetComponent<Pauseable>().isPaused;
}
 */