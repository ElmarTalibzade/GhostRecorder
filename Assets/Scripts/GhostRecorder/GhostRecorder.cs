using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    private GhostShot[] frames;

    private bool isRecording;

    private int recordIndex = 0;
    private float recordTime = 0.0f;            // in milliseconds

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

    public void StartRecording(float duration)
    {
        if (!IsRecording())
        {
            frames = new GhostShot[(int)(60 * duration)];
            recordIndex = 0;
            recordTime = Time.time * 1000;

            isRecording = true;
            OnRecordingStart();

            Debug.LogFormat("Recording of {0} started", gameObject.name);
        }
    }

    public void StopRecording()
    {
        if (IsRecording())
        {
            frames[recordIndex - 1].isFinal = true;

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
            recordTime += Time.smoothDeltaTime * 1000;
            GhostShot newFrame = new GhostShot()
            {
                timeMark = recordTime,
                posMark = transform.position,
                rotMark = transform.rotation
            };

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

    public GhostShot[] GetFrames()
    {
        return frames;
    }
}