using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    private GhostShot[] frames;

    private bool isRecording;

    private int recordIndex = 0;
    private float recordTime = 0.0f;

    #region Event Handlers
    public event EventHandler RecordingStarted;
    public event EventHandler RecordingEnded;
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
    #endregion

    void StartRecording()
    {
        if (!IsRecording())
        {
            frames = new GhostShot[600];                // 60 * 10 = 600 (10 seconds)
            recordIndex = 0;
            recordTime = Time.time;

            isRecording = true;
            OnRecordingStart();

            Debug.LogFormat("Recording of {0} started", gameObject.name);
        }
    }

    void StopRecording()
    {
        if (IsRecording())
        {
            frames[recordIndex].isFinal = true;

            isRecording = false;
            OnRecordingEnd();

            Debug.LogFormat("Recording of {0} ended at frame {1}", gameObject.name, recordIndex);
        }
    }

    void Update()
    {
        if (IsRecording())
        {
            RecordFrame();
        }
    }

    private void RecordFrame()
    {
        if (recordIndex < frames.Length)
        {
            recordTime += Time.deltaTime;
            GhostShot newFrame = new GhostShot();

            newFrame.timeMark = recordTime;
            newFrame.posMark = transform.position;
            newFrame.rotMark = transform.rotation;

            frames[recordIndex] = newFrame;

            recordIndex++;
        }
        else
        {
            StopRecording();
        }
    }

    public bool IsRecording()
    {
        return isRecording;
    }
}