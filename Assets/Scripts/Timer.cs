using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    string timerName;
    [SerializeField]
    float time = 5f;
    [SerializeField]
    bool autostart = true;
    [SerializeField]
    UnityEvent action;

    void Start()
    {
        if(autostart)
        {
            StartCoroutine(StartTimer(time));
        }
    }
    public IEnumerator StartTimer(float time)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}
