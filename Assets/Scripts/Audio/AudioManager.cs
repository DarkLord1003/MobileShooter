using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //Audio Manager reference
    private static AudioManager _instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _mixer;

    [Header("Maximum Sounds")]
    [SerializeField] private int _maxSounds;

    //Tracks
    private Dictionary<string, TrackInfo> _tracks = new Dictionary<string, TrackInfo>();

    //Active Sounds
    private Dictionary<ulong, AudioPoolItem> _activePool = new Dictionary<ulong, AudioPoolItem>();

    //Audio Objects In Pool
    private List<AudioPoolItem> _pool = new List<AudioPoolItem>();

    //ID Giver
    ulong _idGiver;

    //Listener Position
    Transform _listenerPos;


    public static AudioManager Instance
    {
        get => _instance;
    }


    private void Awake()
    {
        GetReferences();
        SetTracks();
        SetItemsInPool();
       
    }


    public float GetTrackVolume(string track)
    {
        TrackInfo trackInfo;

        if(_tracks.TryGetValue(track,out trackInfo))
        {
            float volume;
            _mixer.GetFloat(track, out volume);
            return volume;
        }

        return float.MinValue;
    }
    public void SetTrackVolume(string track,float volume,float fadeTime = 0.0f)
    {
        if (!_mixer)
            return;

        TrackInfo trackInfo;

        if(_tracks.TryGetValue(track,out trackInfo))
        {
            if (trackInfo.TrackFader != null)
            {
                StopCoroutine(trackInfo.TrackFader);
            }

            if (fadeTime == 0.0f)
                _mixer.SetFloat(track, volume);
            else
            {
                trackInfo.TrackFader = SetTrackVolumeInternal(track, volume, fadeTime);
                StartCoroutine(trackInfo.TrackFader);
            }
        }
    }

    public AudioMixerGroup GetAudioGroupFromTrackName(string name)
    {
        TrackInfo trackInfo;

        if(_tracks.TryGetValue(name,out trackInfo))
        {
            return trackInfo.Group;
        }

        return null;
    }

    protected ulong ConfigurePoolObject(int poolIndex,string track,AudioClip clip,Vector3 position, float volume,
                                        float spatialBlend,float unimportance)
    {
        if (poolIndex < 0 || poolIndex > _pool.Count)
            return 0;

        AudioPoolItem poolItem = _pool[poolIndex];

        _idGiver++;
        AudioSource audioSource = poolItem.AudioSource;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatialBlend;

        audioSource.outputAudioMixerGroup = _tracks[track].Group;

        audioSource.transform.position = position;

        poolItem.Playing = true;
        poolItem.Unimportance = unimportance;
        poolItem.ID = _idGiver;
        poolItem.GameObject.SetActive(true);
        audioSource.Play();
        poolItem.Coroutine = StopSoundDelayed(_idGiver, audioSource.clip.length);
        StartCoroutine(poolItem.Coroutine);

        _activePool[_idGiver] = poolItem;

        return _idGiver;
    }

    public ulong PlayOneShotSound(string track,AudioClip clip,Vector3 position,float volume,float spetialBlend,int priotity = 128)
    {
        if (!_tracks.ContainsKey(track) || clip == null || volume.Equals(0.0f))
            return 0;

        float unimportance = (_listenerPos.position - position).sqrMagnitude / Mathf.Max(1, priotity);

        int leastImportantIndex = -1;
        float leastImportanceValue = float.MaxValue;

        for(int i = 0; i < _pool.Count; i++)
        {
            AudioPoolItem poolItem = _pool[i];

            if (!poolItem.Playing)
            {
                return ConfigurePoolObject(i, track, clip, position, volume, spetialBlend, unimportance);
            }
            else
            {
                if(poolItem.Unimportance > leastImportanceValue)
                {
                    leastImportanceValue = poolItem.Unimportance;
                    leastImportantIndex = 1;
                }
            }
        }

        if(leastImportanceValue > unimportance)
        {
            return ConfigurePoolObject(leastImportantIndex, track, clip, position, volume, spetialBlend, unimportance);
        }

        return 0;
    }

    public void StopOneShotSound(ulong id)
    {
        AudioPoolItem activeSound;

        if(_activePool.TryGetValue(id,out activeSound))
        {
            StopCoroutine(activeSound.Coroutine);

            activeSound.AudioSource.Stop();
            activeSound.AudioSource.clip = null;
            activeSound.GameObject.SetActive(false);

            _activePool.Remove(id);

            activeSound.Playing = false;
        }
    }

    private void SetTracks()
    {
        if (!_mixer)
            return;

        AudioMixerGroup[] groups = _mixer.FindMatchingGroups(string.Empty);

        foreach (AudioMixerGroup group in groups)
        {
            Debug.Log(group.name);
            TrackInfo trackInfo = new TrackInfo();
            trackInfo.Name = group.name;
            trackInfo.Group = group;
            trackInfo.TrackFader = null;
            _tracks[group.name] = trackInfo;
        }
    }


    private void SetItemsInPool()
    {
        for (int i = 0; i < _maxSounds; i++)
        {
            GameObject go = new GameObject("Pool Item");
            AudioSource audioSource = go.AddComponent<AudioSource>();
            go.transform.parent = transform;

            AudioPoolItem audioPoolItem = new AudioPoolItem();
            audioPoolItem.GameObject = go;
            audioPoolItem.AudioSource = audioSource;
            audioPoolItem.Transform = go.transform;
            audioPoolItem.Playing = false;
            go.SetActive(false);
            _pool.Add(audioPoolItem);
        }
    }

    public IEnumerator PlayOneShotSound(string track, AudioClip clip, Vector3 position, float volume, float spetialBlend,float duration,int priority = 128)
    {
        yield return new WaitForSeconds(duration);
        PlayOneShotSound(track, clip, position, volume, spetialBlend, priority);
    }
    private IEnumerator StopSoundDelayed(ulong id, float duration)
    {
        yield return new WaitForSeconds(duration);
        AudioPoolItem activeSound;

        if(_activePool.TryGetValue(id,out activeSound))
        {
            activeSound.AudioSource.Stop();
            activeSound.AudioSource.clip = null;
            activeSound.GameObject.SetActive(false);
            _activePool.Remove(id);

            activeSound.Playing = false;
        }
    }

    protected IEnumerator SetTrackVolumeInternal(string track,float volume,float fadeTime)
    {
        float startVolume;
        float timer = 0;
        _mixer.GetFloat(track, out startVolume);

        while(timer < fadeTime)
        {
            timer += Time.unscaledDeltaTime;
            _mixer.SetFloat(track, Mathf.Lerp(startVolume, volume, timer / fadeTime));

            yield return null;
        }

        _mixer.SetFloat(track, volume);
    }

    private void GetReferences()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    #region - OnEnable/Disable

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        _listenerPos = FindObjectOfType<AudioListener>().transform;
    }
}

public class TrackInfo
{
    public string Name;
    public AudioMixerGroup Group;
    public IEnumerator TrackFader;

}

public class AudioPoolItem
{
    public GameObject GameObject;
    public Transform Transform;
    public AudioSource AudioSource;
    public float Unimportance;
    public bool Playing;
    public IEnumerator Coroutine;
    public ulong ID;
}
