using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource collid;
    public AudioSource damage;
    public AudioSource perfect;

    public static AudioPlayer Instance;

    //private Dictionary<AudioSource, float> m_collids = new Dictionary<AudioSource, float>();
    private List<AudioSource> m_collids = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
        SimplePool.Preload(collid.gameObject, 10);
    }

    private void Update()
    {
        List<AudioSource> newList = new List<AudioSource>();
        foreach(AudioSource audio in m_collids)
        {
            if(audio.isPlaying)
            {
                newList.Add(audio);
            }
            else
            {
                SimplePool.Despawn(audio.gameObject);
            }
        }
        //Dictionary<AudioSource, float> delete = new Dictionary<AudioSource, float>();
        //foreach(AudioSource audio in m_collids.Keys)
        //{
        //    m_collids[audio] -= Time.deltaTime;
        //    if(m_collids[audio] <= 0)
        //    {
        //        delete.Add(audio, m_collids[audio]);
        //    }
        //}

        //Dictionary<AudioSource, float> newDic = new Dictionary<AudioSource, float>();
        //foreach (AudioSource audio in m_collids.Keys)
        //{
        //    if(m_collids[audio] >0)
        //    {
        //        newDic.Add(audio,m_collids[audio]);
        //    }
        //}
        //m_collids = newDic;

        //foreach(AudioSource audio in delete.Keys)
        //{
        //    SimplePool.Despawn(audio.gameObject);
        //}
    }

    public void PlayCollid()
    {
        //collid.Play();

        AudioSource col = SimplePool.Spawn(collid.gameObject, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<AudioSource>();
        col.Play();
        m_collids.Add(col);
    }

    public void PlayDamage()
    {
        damage.Play();
    }

    public void PlayPerfect()
    {
        perfect.Play();
    }
}
