using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Audio Collection",menuName ="Create Audio Collection")]
public class AudioCollection : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private string _audioGroup;
    [SerializeField] [Range(0.0f, 1.0f)] private float _volume;
    [SerializeField] [Range(0.0f, 1.0f)] private float _spatialBlend;
    [SerializeField] [Range(0, 256)] private int _priority;
    [SerializeField] private List<ClipBank> _clipBanks = new List<ClipBank>();

    //Properties
    public string AudioGroup => _audioGroup;
    public float Volume => _volume;
    public float SpatialBlend => _spatialBlend;
    public int Priority => _priority;
    public int BankCount => _clipBanks.Count;

    public AudioClip this[int index]
    {
        get
        {
            if (index > _clipBanks.Count || _clipBanks.Count == 0 || _clipBanks == null)
                return null;

            if (_clipBanks[index].Clips.Count == 0)
                return null;

            List<AudioClip> clips = _clipBanks[index].Clips;
            AudioClip audioClip = clips[UnityEngine.Random.Range(0, clips.Count)];

            return audioClip;
        }
    }

}

[Serializable]
public class ClipBank
{
    public List<AudioClip> Clips = new List<AudioClip>();
}
