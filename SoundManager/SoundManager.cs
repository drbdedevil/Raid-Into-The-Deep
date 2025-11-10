using Godot;
using System;
using System.Collections.Generic;

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }
    private AudioStreamPlayer player;
    private float SoundK = 1f;
    public List<KeyValuePair<string, AudioStreamPlayer>> audioStreamPool = new();
    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
            return;
        }
        Instance = this;
    }

    public void PlaySoundOnce(string Path, float Volum = 1f)
    {
        AudioStreamPlayer player = new AudioStreamPlayer();
        AddChild(player);

        player.Stream = GD.Load<AudioStream>(Path);
        player.VolumeDb = LinearToDb(Volum * SoundK);

        audioStreamPool.Add(new KeyValuePair<string, AudioStreamPlayer>(Path, player));

        player.Play();
        player.Finished += () => OnSoundFinished(Path, player);
    }
    public void PlaySoundLoop(string Path, float Volum = 1f)
    {
        AudioStreamPlayer player = new AudioStreamPlayer();
        AddChild(player);

        player.Stream = GD.Load<AudioStream>(Path);
        player.VolumeDb = LinearToDb(Volum * SoundK);

        audioStreamPool.Add(new KeyValuePair<string, AudioStreamPlayer>(Path, player));

        player.Play();
    }
    public void StopSoundLoop(string Path)
    {
        int index = audioStreamPool.FindIndex(pair => pair.Key == Path);
        if (index != -1)
        {
            audioStreamPool[index].Value.QueueFree();
            audioStreamPool.RemoveAt(index);
        }
    }
    private float LinearToDb(float linear)
    {
        return linear > 1e-8f ? 20f * (Mathf.Log(linear) / Mathf.Log(10f)) : -80f;
    }

    private void OnSoundFinished(string Path, AudioStreamPlayer player)
    {
        audioStreamPool.RemoveAll(pair => pair.Key == Path);
        audioStreamPool.TrimExcess();
        player.QueueFree();
    }
}
