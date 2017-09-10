using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostActor : MonoBehaviour
{
    private GhostShot[] frames;

    private bool isRecording;
    private bool isReplaying;

    private int recordIndex = 0;
    private int playbackIndex = 0;

    #region Event Handlers
    public event EventHandler RecordingStarted;
    public event EventHandler RecordingEnded;

    public event EventHandler ReplayStarted;
    public event EventHandler ReplayEnded;
    #endregion

    public void RecordActor()
    {

    }

    public void ReplayActor()
    {

    }
}