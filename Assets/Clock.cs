using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip dong;

    [SerializeField] Transform minuteHand;
    Vector3 minuteHandStart;
    [SerializeField] Transform hourHand;
    Vector3 hourHandStart;

    [SerializeField] float rotationSpeed;

    float hour, minute;

    [SerializeField] float handAngle = 0;
    float angleGoal = 0;
    bool hasGoal = false;

    float inBetweenTimer = 0;

    int sequenceIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        minuteHandStart = minuteHand.localEulerAngles;
        hourHandStart = hourHand.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasGoal)
        {
            if(handAngle < angleGoal)
            {
                handAngle += rotationSpeed * Time.deltaTime;
                if (handAngle > 360)
                {
                    handAngle = handAngle - 360;
                    angleGoal -= 360;
                }
            }
            else
            {
                handAngle = angleGoal;
                hasGoal = false;
                inBetweenTimer = 1;
                sequenceIndex++;
                if(sequenceIndex > GM.I.clockSequence.Count - 1)
                {
                    sequenceIndex = 0;
                    inBetweenTimer *= 4;
                }
            }
        }
        else
        {
            if(inBetweenTimer > 0)
            {
                inBetweenTimer -= Time.deltaTime;
            }
            else
            {
                SetHourDestination(GM.I.clockSequence[sequenceIndex]);
            }
        }

        hourHand.localRotation = Quaternion.AngleAxis(handAngle, Vector3.right);
        minuteHand.localRotation = Quaternion.AngleAxis(AngleOfCurrentMinute(), Vector3.right);
    }

    void SetHourDestination(int hour)
    {
        float newAngle = HourToAngle(hour);
        if (newAngle > handAngle) 
        {
            angleGoal = newAngle;
        }
        else
        {
            angleGoal = newAngle + 360;
        }
        hasGoal = true;
    }

    float AngleToHour(float angle)
    {
        float rotationAngle = (angle - 90 + 360) % 360;
        float rotationRatio = rotationAngle / 360;
        float maxHourAmount = 12;

        return maxHourAmount * rotationRatio;
    }

    float HourToAngle(float hour)
    {
        float hourRatio = hour/12;
        float maxAngle = 360;
        float preAdjustmentAngle = maxAngle * hourRatio;

        return (preAdjustmentAngle + 90) % 360;
    }

    float MinuteToAngle(float minute)
    {
        float minuteRatio = minute / 60;
        float maxAngle = 360;
        float preAdjustmentAngle = maxAngle * minuteRatio;

        return (preAdjustmentAngle + 90) % 360;
    }

    float AngleOfCurrentMinute()
    {
        return MinuteToAngle(DateTime.Now.Minute);
    }

    public void PlayDongSound()
    {
        audioSource.clip = dong;
        audioSource.Play();
    }

    void Reset()
    {
        
    }
}
