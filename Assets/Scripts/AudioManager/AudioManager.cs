using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	private static Dictionary<string, Sound> listSound;

	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;
	public AudioClip[] steps;
	public float stepsVolume;

	List<Sound> soundsSteps;

	public bool soundEnabled = true;

    private void Start()
	{
		Play("Background");
	}
	void Awake()
	{
		if (instance != null)
		{
			LoadSound();
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			listSound = new Dictionary<string, Sound>(sounds.Length);
			DontDestroyOnLoad(gameObject);
			if (enabled)
				LoadSound();
		}

		void LoadSound()
		{
            foreach (Sound s in sounds)
				AddSound(s);
            soundsSteps = new List<Sound>();
            foreach (AudioClip clip in steps)
			{
                Sound s = new Sound();
                s.name = clip.name;
                s.clip = clip;
				s.volume = stepsVolume;
				s.pitch = 1;
                AddSound(s);

				soundsSteps.Add(s);

            }


            void AddSound(Sound s)
            {
                if (!listSound.ContainsKey(s.name))
                {
                    s.source = instance.gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;
                    s.source.loop = s.loop;
                    s.source.volume = s.volume;
                    s.source.pitch = s.pitch;

                    s.source.outputAudioMixerGroup = mixerGroup;

                    listSound.Add(s.name, s);
                }
            }
        }
	}

	public void Play(string sound)
	{
		if (!soundEnabled)
			return;
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public static void Play(Sound s, float pitch = 1)
	{
		if (!instance.soundEnabled)
			return;

		if (s == null)
		{
			Debug.LogWarning("Sound: null!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.pitch = pitch;
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		//s.source.Play();
		s.source.PlayOneShot(s.clip);

	}

	public static void Play(string value, float pitch = 1)
	{
		if (!instance)
			return;

		if (listSound.ContainsKey(value))
		{
			Play(listSound[value], pitch);
		}
		else
		{
			Debug.LogWarning("Sound: " + value + " not found!");
		}
	}

	float minTime;

	public void PlayStep(float volume)
	{
		if (Time.time > minTime)
		{
            minTime = Time.time + 0.4f;
            Sound sound = soundsSteps[Random.Range(0, soundsSteps.Count)];
			int i = 10;
			while (sound.source.isPlaying && i-- > 0)
				sound = soundsSteps[Random.Range(0, soundsSteps.Count)];
			if (i > 0)
			{
				sound.source.volume = sound.volume * volume;
                sound.source.Play();
			}
		}
    }
}
