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

    public void PlayBoomVFX(float disableTime, int entranceIndex)
    {
        _boomVFX[entranceIndex].SetActive(true);
        StartCoroutine(LastUntilDisable(disableTime, entranceIndex));
        DisableBoomEntrance(entranceIndex);

        //*Play Sound
        AudioManager.Instance.PlayBoomSFX(AudioManager.BoomSFX.Cannon);

    }
    IEnumerator LastUntilDisable(float disableTime, int entranceIndex)
    {
        var timer = new WaitForSeconds(disableTime);
        yield return timer;
        _boomVFX[entranceIndex].SetActive(false);
    }

    void DisableBoomEntrance(int index)
    {
        _boomEntrance[index].SetActive(false);
    }

}
