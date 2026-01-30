using UnityEngine;
using UnityEngine.SceneManagement;

using FMODUnity;
using FMOD.Studio;
using Unity.VisualScripting;

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

}