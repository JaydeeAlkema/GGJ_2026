using NaughtyAttributes;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    [BoxGroup("Reference")]
    [SerializeField] FMODUnity.EventReference s_Ambient;

    [BoxGroup("Parameters")]
    [SerializeField] string name_a;

    [BoxGroup("Parameters")]
    [SerializeField] string name_b;

    [BoxGroup("Parameters")]
    [SerializeField] string string_a;

    [BoxGroup("Parameters")]
    [SerializeField] string string_b;

    [BoxGroup("Parameters")]
    [SerializeField] string float_a;

    [BoxGroup("Parameters")]
    [SerializeField] string float_b;

    private FMOD.Studio.EventInstance i_Ambient;

    private void Start() {
        if (name_a != null) i_Ambient = AudioManager.Instance.PlayInstance(s_Ambient, transform.position, name_a, float_a, name_b, float_b);
    }
}
