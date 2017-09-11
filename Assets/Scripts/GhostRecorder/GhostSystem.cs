using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    private GhostRecorder[] recorders;
    private GhostActor[] ghostActors;

    private CameraFollow cameraFollow;

    public Transform playerControlled;
    public Transform playerGhost;

    public float recordDuration = 10;

    private void Start()
    {
        recorders = FindObjectsOfType<GhostRecorder>();
        ghostActors = FindObjectsOfType<GhostActor>();
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    private bool isRecording;
    private bool isReplaying;

    public void StartRecording()
    {
        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].StartRecording(recordDuration);
        }

        OnRecordingStart();
    }

    public void StopRecording()
    {
        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].StopRecording();
        }

        OnRecordingEnd();
    }

    public void StartReplay()
    {
        for (int i = 0; i < ghostActors.Length; i++)
        {
            ghostActors[i].StartReplay();
        }

        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].GetComponent<Renderer>().enabled = false;
        }

        cameraFollow.followTarget = playerGhost;

        OnReplayStart();
    }

    public void StopReplay()
    {
        for (int i = 0; i < ghostActors.Length; i++)
        {
            ghostActors[i].StopReplay();
        }

        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].GetComponent<Renderer>().enabled = true;
        }

        cameraFollow.followTarget = playerControlled;

        OnReplayEnd();
    }

    #region Event Handlers
    public event EventHandler RecordingStarted;
    public event EventHandler RecordingEnded;
    public event EventHandler ReplayStarted;
    public event EventHandler ReplayEnded;
    #endregion

    #region Event Invokers
    protected virtual void OnRecordingStart()
    {
        if (RecordingStarted != null)
        {
            RecordingStarted.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnRecordingEnd()
    {
        if (RecordingEnded != null)
        {
            RecordingEnded.Invoke(this, EventArgs.Empty);
        }
    }


    protected virtual void OnReplayStart()
    {
        if (ReplayStarted != null)
        {
            ReplayStarted.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnReplayEnd()
    {
        if (ReplayEnded != null)
        {
            ReplayEnded.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion
}