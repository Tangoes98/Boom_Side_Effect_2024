using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFX : MonoBehaviour
{
    public static EnvironmentFX Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    [SerializeField] List<GameObject> _boomEntrance;
    [SerializeField] List<GameObject> _boomVFX;

    private void Start()
    {
        foreach (var item in _boomVFX)
        {
            item.SetActive(false);
        }
    }




    public void PlayBoomVFX(float disableTime)
    {
        foreach (var item in _boomVFX)
        {
            item.SetActive(true);
        }
        StartCoroutine(LastUntilDisable(disableTime));
        DisableBoomEntrance();

        //*Play Sound
        AudioManager.Instance.PlayBoomSFX(AudioManager.BoomSFX.Cannon);

    }
    IEnumerator LastUntilDisable(float disableTime)
    {
        var timer = new WaitForSeconds(disableTime);
        yield return timer;
        foreach (var item in _boomVFX)
        {
            item.SetActive(false);
        }
    }

    void DisableBoomEntrance()
    {
        _boomEntrance.ForEach((p) => p.SetActive(false));
    }

}
