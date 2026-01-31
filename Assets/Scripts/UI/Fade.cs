using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private float playOnStartDelay = 1f;
    [SerializeField] private float playOnStartFadeSpeed = 1f;
    [SerializeField] private string fadeInTrigger = "FadeIn";
    [SerializeField] private string fadeOutTrigger = "FadeOut";

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("Fade: Animator is not assigned and was not found on this GameObject.", this);
            animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Fade: Animator is missing; debug fade sequence will not run.", this);
            return;
        }
        if (playOnStart)
        {
            StartCoroutine(DefaultFadeSequence(playOnStartDelay));
        }
    }

    private System.Collections.IEnumerator DefaultFadeSequence(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        FadeIn(playOnStartFadeSpeed);
    }

    public void FadeIn(float speed = 1f)
    {
        if (animator == null)
        {
            Debug.LogError("Fade: Animator is missing; FadeIn ignored.", this);
            return;
        }

        animator.speed = speed;
        animator.ResetTrigger(fadeOutTrigger);
        animator.SetTrigger(fadeInTrigger);
    }

    public void FadeOut(float speed = 1f)
    {
        if (animator == null)
        {
            Debug.LogError("Fade: Animator is missing; FadeOut ignored.", this);
            return;
        }

        animator.speed = speed;
        animator.ResetTrigger(fadeInTrigger);
        animator.SetTrigger(fadeOutTrigger);
    }
}
