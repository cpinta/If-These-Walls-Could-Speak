using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SafeDial : Clickable
{
    AudioSource audioSource;
    [SerializeField] AudioClip[] acTicks;

    Quaternion destination;
    [SerializeField] float rotationLerp = 10;
    int currentIndex = 0;
    [SerializeField] TMP_Text[] tmpChars = new TMP_Text[6];
    char[] charChoices = new char[6];
    Quaternion origin;

    public bool solved = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        interactText = "Spin";
        //Debug.Log(transform.rotation);
        destination = transform.localRotation;
        origin = transform.localRotation;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Quaternion.Angle(transform.rotation, destination) > 1)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, destination, rotationLerp * Time.deltaTime);
        }
        else
        {
            transform.localRotation = destination;
        }
    }

    public override void Interact(Entity entity)
    {
        Rotate();
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    void Rotate()
    {
        currentIndex++;
        if(currentIndex > charChoices.Length-1)
        {
            currentIndex = 0;
        }
        destination = origin * Quaternion.Euler(0, 0, 60 * currentIndex);
        Debug.Log("SafeDial rotated to :" + tmpChars[currentIndex].text);
        PlayTickSound();
    }

    public void SetLetter(int index, char letter)
    {
        charChoices[index] = letter;
        tmpChars[index].text = letter.ToString();
    }

    void PlayTickSound()
    {
        audioSource.PlayOneShot(acTicks[Random.Range(0, acTicks.Length)]);
    }

    public char CurrentLetter()
    {
        return charChoices[currentIndex];
    }

    public override void ResetGame()
    {
        currentIndex = 0;
        destination = transform.localRotation;
        origin = transform.localRotation;
        interactText = "Spin";
    }
}
