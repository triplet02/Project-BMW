using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum Bgm { Ending, Stage1, Stage2, Stage3, Title, Mute }
    public enum Sfx { Coin, Drink, Collision, Portal, Cat, Mouse, Dog, Immune }

    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    Bgm currentBgm;

    void Awake(){
        instance = this;
        Init();
    }

    void Init(){
        //bgmPlayer initialize
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

        //sfxPlayer initialize
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        
        for(int index = 0 ; index < sfxPlayers.Length ; index++){
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx){
        for(int index = 0 ; index < sfxPlayers.Length ; index++){
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying){
                continue;
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBGM()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Bgm nextBgm = BgmForCurrentScene(currentScene);
        Debug.Log(currentBgm.ToString() + " / " + nextBgm.ToString());
        if(currentBgm != nextBgm)
        {
            Debug.Log(currentBgm.ToString() + " / " + nextBgm.ToString());
            bgmPlayer.Stop();
            if(bgmClips.Length <= (int)nextBgm)
            {
                return;
            }
            bgmPlayer.clip = bgmClips[(int)nextBgm];
            bgmPlayer.Play();
            currentBgm = nextBgm;
        }
    }
    Bgm BgmForCurrentScene(Scene scene)
    {
        string currentSceneName = scene.name;

        switch (currentSceneName)
        {
            case "SideView Gameplay 1":
                return Bgm.Stage1;
            case "SideView Gameplay 2":
                return Bgm.Stage2;
            case "SideView Gameplay 3":
                return Bgm.Stage3;
            case "TopView Gameplay 1":
                return Bgm.Stage1;
            case "TopView Gameplay 2":
                return Bgm.Stage2;
            case "TopView Gameplay 3":
                return Bgm.Stage3;
            case "Gameover":
                return Bgm.Ending;
            case "Opening":
                return Bgm.Mute;
            default:
                return Bgm.Title;
        }
    }
}
