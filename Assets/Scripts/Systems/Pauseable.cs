using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pauseable : MonoBehaviour
{
    public bool isPaused { get; private set; }
    //[SerializeField] PauseableBehavior<MonoBehaviour>[] pauseableBehaviors;
    //[SerializeField] string pauseCommand = "OnPause";
    //[SerializeField] string unPauseCommand = "OnUnPause";
    
    private MonoBehaviour[] monos;
    private Animator anim;
    private NavMeshAgent nav;
    private Rigidbody rb;


    void Awake()
    {
        List<MonoBehaviour> getMonos = new List<MonoBehaviour>(GetComponents<MonoBehaviour>());
        getMonos.Remove(this);
        monos = getMonos.ToArray();

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }


    public void SetPause(bool value)
    {
        if (isPaused == value) return;
        isPaused = value;

        if (isPaused) SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);

        for (int i = 0; i < monos.Length; i++) monos[i].enabled = !isPaused;

        if (!isPaused) SendMessage("OnUnPause", SendMessageOptions.DontRequireReceiver);

        HandleComponent(anim);
        HandleComponent(nav);
        HandleComponent(rb);

    }
    public void Pause()   => SetPause(true);
    public void UnPause() => SetPause(false);
    public void TogglePause() => SetPause(!isPaused);  



    private bool s_anim_enabled;
    private void HandleComponent(Animator anim)
    {
        if(anim == null) return;

        if (isPaused)
        {
            s_anim_enabled = anim.enabled;
            anim.enabled = false;
        }
        else
        {
            anim.enabled = s_anim_enabled;
        }

    }

    private Vector3 s_nav_destination;
    private Vector3 s_nav_velocity;
    private void HandleComponent(NavMeshAgent nav)
    {
        if(nav == null) return;

        if (isPaused)
        {
            s_nav_destination = nav.destination;
            s_nav_velocity = nav.velocity;
            nav.isStopped = true;
        }

        nav.enabled = !isPaused;

        if (!isPaused)
        {
            nav.isStopped = false;
            nav.destination = s_nav_destination;
            nav.velocity = s_nav_velocity;
        }
    }

    private Vector3 s_rb_velocity;
    private Vector3 s_rb_angularVelocity;
    private void HandleComponent(Rigidbody rb)
    {
        if(rb == null) return;

        if (isPaused)
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


    private void OnEnable() => GameplayPauseManager.RegisterPausable(this);
    private void OnDisable() => GameplayPauseManager.UnRegisterPausable(this);
    private void OnDestroy() => GameplayPauseManager.UnRegisterPausable(this);


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