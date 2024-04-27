using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneSystem : MonoBehaviour
{
    public static int cutsceneID;

    //Config

    [SerializeField] private string[] endScenes;
    [SerializeField] private string animationName;
    [SerializeField] private float fadeTime = 1;
    [SerializeField] private Image blackout;
    [SerializeField] private Button continueButton;
    private Animator animator;
    private bool waitingForPlayer;
    private bool fadingOut;
    private float fadeTimer = 1;
    private InputActions input;


    private void Awake()
    {
        input = new();
        input.Cutscenes.Enable();
        animator = GetComponent<Animator>();
        continueButton.interactable = false;
        blackout.color = Color.black;
        if(SettingsSave.save.autoPlayCutscenes) continueButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (input.Cutscenes.Proceed.WasPerformedThisFrame()) Continue();
        if (input.Cutscenes.Skip.IsPressed()) End();

        if (fadingOut)
        {
            if (fadeTimer < 1) fadeTimer += Time.deltaTime / fadeTime;
            else SceneManager.LoadScene(endScenes[cutsceneID]);
        }
        else
        {
            if (fadeTimer > 0) fadeTimer -= Time.deltaTime / fadeTime;
            else
            {
                fadeTimer = 0;
                BeginAnimation();
            }
        }
        blackout.color = new(0, 0, 0, fadeTimer);
    }

    private void Continue()
    {
        if (!waitingForPlayer) return;
        animator.speed = 1;
        waitingForPlayer = false;
        continueButton.interactable = false;
    }

    private void BeginAnimation() => animator.Play(animationName + cutsceneID);


    //Callbacks
    public void Pause()
    {
        if (SettingsSave.save.autoPlayCutscenes) return;
        animator.speed = 0;
        waitingForPlayer = true;
        continueButton.interactable = true;
    }
    public void End() => fadingOut = true;









    /* OLD VERSION
     
    

    //[SerializeField] private float timeToSkip;
    [SerializeField] private float fadeTime;

    public Cutscene[] cutscenes;

    [SerializeField] private Button continueButton;
    [SerializeField] private Image comicImage;
    [SerializeField] private Image blackout;

    //private float skipTimer;
    private bool waitToContinue;
    private float fadeTimer = -10000000;
    private int currentImage;
    private int fadingScene = 1;
    InputActions input;

    private void Awake()
    {
        comicImage.sprite = cutscenes[cutsceneID].panels[0];
        comicImage.color = Color.clear;
        fadeTimer = -0.00000000001f;
        blackout.color = Color.black;
        input = new();
        input.Cutscenes.Enable();
    }

    private void Update()
    {
        if(fadingScene != 0)
        {
            if(fadingScene == 1)
            {
                if (blackout.color.a > 0) blackout.color -= Color.black * Time.deltaTime;
                else
                {
                    blackout.color = Color.clear;
                    fadingScene = 0;
                }
            }
            else if (fadingScene == -1)
            {
                if (blackout.color.a < 1) blackout.color += Color.black * Time.deltaTime;
                else Transfer();
            }
        }
        else
        {
            if (input.Cutscenes.Proceed.WasPerformedThisFrame()) Continue();
            if (input.Cutscenes.Skip.IsPressed()) Finish();
                /*if (input.Cutscenes.Skip.IsPressed())
                {
                    if (skipTimer < timeToSkip)
                    {
                        skipTimer += Time.deltaTime;
                    }
                    else Finish();
                }
                else if (skipTimer > 0) skipTimer = 0;* /

            if (fadeTimer > -fadeTime)
            {
                float pre = fadeTimer;
                fadeTimer -= Time.deltaTime;
                if (pre.Sign() != fadeTimer.Sign())
                {
                    fadeTimer = -0.00000000001f;
                    currentImage++;

                    if (currentImage == cutscenes[cutsceneID].panels.Length)
                    {
                        //Debug.LogFormat("Showing image {0} out of {1}", currentImage + 1, cutscenes[cutsceneID].panels.Length);
                        Finish();
                        return;
                    }

                    comicImage.sprite = cutscenes[cutsceneID].panels[currentImage];
                }

                comicImage.color = new(1, 1, 1, (fadeTimer >= 0) ? fadeTimer / fadeTime : -fadeTimer / fadeTime);
            }
            else waitToContinue = true;

            continueButton.interactable = waitToContinue;
        }
    }

    public void Continue()
    {
        if (!waitToContinue) return;
        fadeTimer = fadeTime;
        waitToContinue = false;
    }

    public void Finish() => fadingScene = -1;
    public void Transfer() => SceneManager.LoadScene(cutscenes[cutsceneID].endScene);




    [Serializable]
    public struct Cutscene
    {
        public Sprite[] panels;

        public string endScene;
    }
    */



}
