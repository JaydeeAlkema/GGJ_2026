using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

public class AmbientModule : MonoBehaviour
{
    [BoxGroup("Parameter: Chaotic")]
    [Range(0, 1f)][SerializeField] float chaotic;
    const string CHAOTIC = "Chaotic";

    [BoxGroup("Parameter: Charming")]
    [Range(0,1f)][SerializeField] float charming;
    const string CHARMING = "Charming";

    [BoxGroup("Parameter: Frightening")]
    [Range(0, 1f)][SerializeField] float frightening;
    const string FRIGHTENING = "Frightening";

    [BoxGroup("Parameter: Tranquil")]
    [Range(0, 1f)][SerializeField] float tranquil;
    const string TRANQUIL = "Tranquil";

    [BoxGroup("Parameter: Reverb")]
    [Range(-1, 0f)][SerializeField] float reverb;
    const string REVERB = "Reverb";

    private void Start() {
        RuntimeManager.StudioSystem.setParameterByName(CHAOTIC, chaotic);
        RuntimeManager.StudioSystem.setParameterByName(CHARMING, charming);
        RuntimeManager.StudioSystem.setParameterByName(FRIGHTENING, frightening);
        RuntimeManager.StudioSystem.setParameterByName(TRANQUIL, tranquil);
        RuntimeManager.StudioSystem.setParameterByName(REVERB, reverb);
    }

    public void ResetModule() {
        RuntimeManager.StudioSystem.setParameterByName(CHAOTIC, 0f);
        RuntimeManager.StudioSystem.setParameterByName(CHARMING, 0f);
        RuntimeManager.StudioSystem.setParameterByName(FRIGHTENING, 0f);
        RuntimeManager.StudioSystem.setParameterByName(TRANQUIL, 0f);
        RuntimeManager.StudioSystem.setParameterByName(REVERB, 0f);
    }
}
