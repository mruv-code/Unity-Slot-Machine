
using UnityEngine;

public class SlotAudioManager : MonoBehaviour
{
    [Header("Dedicated Spin Audio")]
    [SerializeField] private AudioSource spinAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Other Sound Effects")]
    [SerializeField] private AudioClip spinClip;
    [SerializeField] private AudioClip reelStopClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip buttonClickClip;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaySpinSound();
        }
    }

    public void PlaySpinSound()
    {
        if (spinAudioSource == null)
        {
            Debug.LogError("Spin Audio Source is not assigned!");
            return;
        }

        if (spinClip == null)
        {
            Debug.LogError("Spin Clip is not assigned!");
            return;
        }

        spinAudioSource.Stop();
        spinAudioSource.clip = spinClip;
        spinAudioSource.loop = false;
        spinAudioSource.volume = 1f;
        spinAudioSource.Play();

        Debug.Log("Spin audio playing: " + spinAudioSource.isPlaying);
    }

    public void StopSpinSound()
    {
        if (spinAudioSource != null)
            spinAudioSource.Stop();
    }

    public void PlayReelStop()
    {
        PlaySound(reelStopClip);
    }

    public void PlayWin()
    {
        PlaySound(winClip);
    }

    public void PlayLose()
    {
        PlaySound(loseClip);
    }

    public void PlayButtonClick()
    {
        PlaySound(buttonClickClip);
    }

    private void PlaySound(AudioClip clip)
    {
        if (sfxAudioSource != null && clip != null)
            sfxAudioSource.PlayOneShot(clip);
    }
}
