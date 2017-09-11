using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostSystem_UIManager : MonoBehaviour
{
    public GhostSystem recorder;

    public GameObject ButtonStartRecord;
    public GameObject ButtonEndRecord;

    public GameObject ButtonStartReplay;
    public GameObject ButtonEndReplay;

    public GameObject IconReplay;
    public GameObject IconRecord;

    private void Awake()
    {
        ToggleRecordUI(false);
        ToggleReplayUI(false);

        recorder.RecordingStarted += OnRecordingStart;
        recorder.RecordingEnded += OnRecordingEnd;

        recorder.ReplayStarted += OnReplayStart;
        recorder.ReplayEnded += OnReplayEnd;
    }

    void OnRecordingStart(object source, EventArgs e)
    {
        ToggleRecordUI(true);
    }

    void OnRecordingEnd(object source, EventArgs e)
    {
        ToggleRecordUI(false);
    }

    void OnReplayStart(object source, EventArgs e)
    {
        ToggleReplayUI(true);
    }

    void OnReplayEnd(object source, EventArgs e)
    {
        ToggleReplayUI(false);
    }

    /// <summary>
    /// Toggles the record UI.
    /// </summary>
    /// <param name="s">if set to <c>true</c> then it's assumed that Recording has started.</param>
    public void ToggleRecordUI(bool s)
    {
        ButtonStartRecord.SetActive(!s);
        ButtonEndRecord.SetActive(s);

        IconRecord.SetActive(s);
    }

    /// <summary>
    /// Toggles the replay UI.
    /// </summary>
    /// <param name="s">if set to <c>true</c> then it's assumed that Replaying has started.</param>
    public void ToggleReplayUI(bool s)
    {
        ButtonStartReplay.SetActive(!s);
        ButtonEndReplay.SetActive(s);

        IconReplay.SetActive(s);
    }
} 