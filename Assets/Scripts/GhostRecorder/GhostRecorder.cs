using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public GameObject recordTarget;                 // GameObject whose position will be recorded
    public GameObject replayGhostTarget;                  // GameObject which will act as a Ghost of a record target during replay

    private GhostShot[] shots;                       // Stores the snapshots of the latest recording

    public float maxRecordTime = 10;                // Maximum recording time in seconds
    public float snapshotFrequency = 0.1f;          // How often should position and rotation be recorded?

    public bool isRecording;
    public bool isReplaying;

    public event EventHandler RecordingStarted;
    public event EventHandler RecordingEnded;

    public event EventHandler ReplayStarted;
    public event EventHandler ReplayEnded;

    private int recordingIndex = 0;
    private int replayIndex = 0;

    public float replayTimescale = 1;              // change this value below 1 to make replays slower and above 1 to make them faster

    private float recordingTime = 0.0f;            // the total number of seconds for recording
    private float currentReplayTime = 0.0f;         // current time at which the replay is at, used for replaying the recording

    public void StartRecording()
    {
        if (!IsRecording() && !IsReplaying())
        {
            shots = new GhostShot[(int)(maxRecordTime / snapshotFrequency)];           //create an array of potential recordings
            recordingIndex = 0;
            recordingTime = Time.time;

            isRecording = true;
            OnRecordingStart();

            InvokeRepeating("RecordSnapshot", 0, snapshotFrequency);                    // start recording snapshots at a specified frequency
        }
    }

    void RecordSnapshot()
    {
        if (IsRecording())
        {
            if (recordingIndex < shots.Length)
            {
                recordingTime += Time.deltaTime;                                // increase the elapsed time
                GhostShot newShot = new GhostShot(false);                       // create a new GhostShot

                newShot.timeBegin = recordingTime;                              // mark the beginning time
                newShot.timeEnd = newShot.timeBegin + snapshotFrequency;        // mark the end time by adding snapshotFrequency

                if (recordingIndex > 0)                                         // if this isn't the first shot...
                {
                    newShot.posBegin = shots[recordingIndex - 1].posEnd;        // ...then mark the previous position as a beginning position for this shot
                    newShot.rotBegin = shots[recordingIndex - 1].rotEnd;
                }
                else
                {
                    newShot.posBegin = recordTarget.transform.position;         // ...otherwise mark the current position as the beginning position for this shot
                    newShot.rotBegin = recordTarget.transform.rotation;
                }

                newShot.posEnd = recordTarget.transform.position;
                newShot.rotEnd = recordTarget.transform.rotation;

                shots[recordingIndex] = newShot;                                // allocate the new shot to a specified array item

                recordingIndex++;                                               // increase the index
            }
            else
            {
                StopRecording();
            }
        }
    }

    public void StopRecording()
    {
        if (IsRecording())
        {
            CancelInvoke();

            if (recordingIndex < shots.Length)              // if the recording has ended prematurely
            {
                Debug.Log("Recording ended prematurely at " + recordingIndex);
                shots[recordingIndex - 1].isFinal = true;
            }
            else
            {
                shots[shots.Length - 1].isFinal = true;     // otherwise, mark the last shot as Final
            }

            isRecording = false;

            OnRecordingEnd();
        }
    }

    public void StartReplay()
    {
        if (!IsReplaying())
        {
            replayIndex = 0;

            replayGhostTarget.transform.position = shots[0].posBegin;
            replayGhostTarget.transform.rotation = shots[0].rotBegin;

            replayGhostTarget.SetActive(true);

            isReplaying = true;

            OnReplayStart();
        }
    }

    private void Update()
    {
        if (IsReplaying())
        {
            if (replayIndex < shots.Length)                                             // if the current index is within the boundaries of the array...
            {
                if (!shots[replayIndex].isFinal)                                        // the current shot is not final...
                {
                    if (currentReplayTime < shots[replayIndex].timeBegin)               // if the current replay time is before the current shot begin mark...
                    {
                        if (replayIndex != 0)
                        {
                            replayIndex--;                                              // ...go index lower and skip the current iteration
                        }
                        else
                        {
                            currentReplayTime = shots[replayIndex].timeBegin;
                        }
                    }
                    else if (currentReplayTime > shots[replayIndex].timeEnd)            // if the current replay time is higher than the current shot end mark...
                    {
                        replayIndex++;                                                  // ...go index higher and skip the current iteration
                    }
                    else                                                                // if the current replay time is within the current shot time-frame...
                    {
                        replayGhostTarget.transform.position = shots[replayIndex].PosLerp(currentReplayTime);          // lerps the position and applies it onto the ghost
                        replayGhostTarget.transform.rotation = shots[replayIndex].RotLerp(currentReplayTime);

                        currentReplayTime += Time.deltaTime * replayTimescale;
                    }
                }
                else
                {
                    StopReplay();
                }
            }
            else
            {
                StopReplay();
            }
        }
    }

    public void StopReplay()
    {
        if (IsReplaying())
        {
            replayIndex = 0;
            currentReplayTime = 0.0f;

            replayGhostTarget.SetActive(false);
            isReplaying = false;

            OnReplayEnd();
        }
    }

    public bool IsRecording()
    {
        return isRecording;
    }

    public bool IsReplaying()
    {
        return isReplaying;
    }

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
}