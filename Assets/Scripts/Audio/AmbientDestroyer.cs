using UnityEngine;
using System.Collections;
using NaughtyAttributes;

using FMODUnity;
using FMOD;

public class AmbientDestroyer : MonoBehaviour
{
    [BoxGroup("Audio")]
    [SerializeField] EventReference a_Ambient;
    [SerializeField] EventReference a_Fire;

    FMOD.Studio.EventInstance i_Fire;
    FMOD.Studio.EventInstance i_Ambient;

    void Start() {
        i_Fire = AudioManager.Instance.PlayInstance(a_Fire, gameObject);
        StartCoroutine(DestroyCoroutine(30f));
    }

    private IEnumerator DestroyCoroutine(float delay) {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.ClearInstance(i_Fire);

        i_Ambient = AudioManager.Instance.PlayInstance(a_Ambient, gameObject);

        AmbientModule module = GetComponent<AmbientModule>();
        module.ResetModule();

        RuntimeManager.StudioSystem.setParameterByName("Chaotic", 0f);
        RuntimeManager.StudioSystem.setParameterByName("Charming", 0f);
        RuntimeManager.StudioSystem.setParameterByName("Frightening", 0f);
        RuntimeManager.StudioSystem.setParameterByName("Tranquil", 0f);
    }
}