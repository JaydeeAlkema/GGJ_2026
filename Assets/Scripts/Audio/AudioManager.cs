using UnityEngine;
using UnityEngine.SceneManagement;

using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    [Header("General Settings")]
    [Range(0, 100)] public float MasterVolume = 50;
    [Range(0, 100)] public float AmbientVolume = 100;
    [Range(0, 100)] public float SFXVolume = 100;
    [Range(0, 100)] public float MusicVolume = 100;

    [Header("Testing")]
    [SerializeField] private EventReference test;

    #region Singleton Logic

    private void InitializeSingleton() {
        if (Instance != null) {
            Destroy(gameObject);

            Debug.LogError("Duplicate AudioManager detected, destroying new instance.");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    public void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => ClearAll();

    private void OnSceneChanged(Scene current, Scene next) => ClearAll();

    public void OnDestroy() => ClearAll();

    #endregion

    #region Mixer Logic

    Bus ambientBus, masterBus, musicBus, sfxBus;

    private void InitializeVolumeSettings() {
        AdjustMasterVolume(MasterVolume);
        AdjustAmbientVolume(AmbientVolume);
        AdjustSFXVolume(SFXVolume);
        AdjustMusicVolume(MusicVolume);
    }

    public void RefreshVolumeSettings() {
        AdjustMasterVolume(100);
        AdjustAmbientVolume(100);
        AdjustSFXVolume(100);
        AdjustMusicVolume(100);
    }

    public void InitializeBusses() {
        masterBus = RuntimeManager.GetBus("bus:/");
        ambientBus = RuntimeManager.GetBus("bus:/Ambient");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    public void AdjustMasterVolume(float masterVolume) {
        MasterVolume = masterVolume / 100;
        masterBus.setVolume(MasterVolume);
    }

    public void AdjustAmbientVolume(float ambientVolume) {
        AmbientVolume = ambientVolume / 100;
        ambientBus.setVolume(AmbientVolume);
    }

    public void AdjustSFXVolume(float sfxVolume) {
        SFXVolume = sfxVolume / 100;
        sfxBus.setVolume(SFXVolume);
    }

    public void AdjustMusicVolume(float musicVolume) {
        MusicVolume = musicVolume / 100;
        musicBus.setVolume(MusicVolume);
    }

    #endregion

    #region Playback Logic
    const string MUFFLE = "Muffle";
    /// TODO: GameState bool checks before every playback methods (don't play when game is paused or in menu)

    public void PlaySound(EventReference sfx) => RuntimeManager.PlayOneShot(sfx);

    public void PlayInstance(EventInstance instance, GameObject obj) {
        RuntimeManager.AttachInstanceToGameObject(instance, obj);
        instance.start();
        instance.release();
    }

    public EventInstance PlayInstance(EventReference sound, GameObject obj) {
        EventInstance instance = CreateInstance(sound, obj.transform.position);
        RuntimeManager.AttachInstanceToGameObject(instance, obj);
        instance.start();
        return instance;
    }

    public EventInstance PlayInstance(
    EventReference sfx,
    Vector3 eventPosition,

    string customSheet = null,
    float customSheetIntensity = 0,

    string customSheet2 = null,
    float customSheetIntensity2 = 0,

    string customSheet3 = null,
    float customSheetIntensity3 = 0,    

    string customSheet4 = null,
    float customSheetIntensity4 = 0
    ) {
        if (!sfx.IsNull) {
            EventInstance instance = CreateInstance(sfx, eventPosition);

            if (customSheet != null) instance.setParameterByName(customSheet, customSheetIntensity);
            if (customSheet2 != null) instance.setParameterByName(customSheet2, customSheetIntensity2);
            if (customSheet3 != null) instance.setParameterByName(customSheet3, customSheetIntensity3);
            if (customSheet4 != null) instance.setParameterByName(customSheet4, customSheetIntensity4);

            instance.start();
            instance.release();
            return instance;
        }

        else {
            Debug.Log("Audio source missing");
            return default;
        }
    }


    public EventInstance CreateInstance(EventReference audio, Vector3 eventPosition) {
        if (audio.IsNull) Debug.Log("Audio source missing: " + audio.Path);
        EventInstance instance = RuntimeManager.CreateInstance(audio);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(eventPosition));
        return instance;
    }

    public void ContinueInstance(params EventInstance[] instances) {
        foreach (var instance in instances) {
            instance.setPaused(false);
        }
    }

    public void PauseInstance(params EventInstance[] instances) {
        foreach (var instance in instances) {
            instance.setPaused(true);
        }
    }

    public void StopInstance(params EventInstance[] instances) {
        foreach (var instance in instances) {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void ClearInstance(params EventInstance[] instances) {
        foreach (EventInstance instance in instances) {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
    }

    public bool IsPlaying(EventInstance instance) {
        instance.getPlaybackState(out PLAYBACK_STATE state);
        return state == PLAYBACK_STATE.PLAYING;
    }

    public void ClearAll() => masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    public void MuffleAudio(bool isMuffled) => RuntimeManager.StudioSystem.setParameterByNameWithLabel(MUFFLE, isMuffled ? "muffled" : "normal");

    #endregion

    void Awake() {
        InitializeSingleton();

        InitializeBusses();
        InitializeVolumeSettings();
    }

    private void Start() {
        RuntimeManager.StudioSystem.setParameterByName("Chaos", 0.5f);
        RuntimeManager.StudioSystem.setParameterByName("Tranquil", 0.5f);
    }

    /*

    ___________________________________________________
    
    @Preset: Select SFX

    [SerializeField] FMODUnity.EventReference s_Select;

    AudioManager.Instance.PlaySound(s_Select);

    __________________________________________________
    
    @Preset: Equip Component SFX

    [SerializeField] FMODUnity.EventReference s_Equip;
    FMOD.Studio.EventInstance i_Equip;

    i_Equip = AudioManager.Instance.PlayInstance(s_Equip, transform.position); 

    // Sound changes based on component: ["Material"; "default", "diamond", "glass", "grass", "metal"]
    // Sound changes based on component: ["Emotions"; "Chaos", "Charm", "Fright", "Tranquil"]
    // Sound changes based on component: ["Complete"; "True", "False"]
    // Sound changes based on component: ["Reverb"; 0-1 float]
    ___________________________________________________

    @Preset: Control Ambient Parameters

    RuntimeManager.StudioSystem.setParameterByName("Chaos", 0-1 float);
    RuntimeManager.StudioSystem.setParameterByName("Charm", 0-1 float);
    RuntimeManager.StudioSystem.setParameterByName("Fright", 0-1 float);
    RuntimeManager.StudioSystem.setParameterByName("Tranquil", 0-1 float);

    ___________________________________________________

    @Preset: 



    */

}