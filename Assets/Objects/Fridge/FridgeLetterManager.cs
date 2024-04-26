using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeLetterManager : MonoBehaviour
{
    [SerializeField] FridgeLetter prefabLetter;
    [SerializeField] float letterSpacing = 0.1f;
    [SerializeField] float lineSpacing = 0.1f;
    [SerializeField] Vector2 lettersXLimits;
    [SerializeField] Vector2 lettersYLimits;

    [SerializeField] List<string> messageQueue = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnPhrase("brug and spagh");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnLetter(char letter, Vector3 position)
    {
        FridgeLetter fletter = Instantiate(prefabLetter, position, Quaternion.identity, transform);
        fletter.transform.localPosition = position;
        fletter.SetLetter(letter);
        fletter.SetDestination(position);
        fletter.SetColor();
    }

    Vector3 SpawnWord(string word, Vector3 startLocation)
    {
        for(int i = 0; i < word.Length; i++)
        {
            SpawnLetter(word[i], startLocation + (Vector3.right * (letterSpacing * i)));
        }
        return new Vector3(lettersXLimits.x, startLocation.y, 0) + (Vector3.right * (letterSpacing * (word.Length - 1)));
    }

    void SpawnPhrase(string phrase)
    {
        Vector3 cursorLocation = new Vector3(lettersXLimits.x, lettersYLimits.y, 0);

        string[] strings = phrase.Split(new char[] { ' ' });
        for(int i=0; i < strings.Length; i++)
        {
            if ((strings[i].Length * letterSpacing) + cursorLocation.x > lettersXLimits.y - 0.1f)
            {
                cursorLocation = new Vector3(lettersXLimits.x, cursorLocation.y - lineSpacing, 0);
            }
            cursorLocation = SpawnWord(strings[i], cursorLocation);
        }
    }

    public void AddMessageToQueue(string message)
    {
        messageQueue.Add(message);
    }

    void ClearCurrentMessage()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayLatestQueueInMessage()
    {
        ClearCurrentMessage();
        SpawnPhrase(messageQueue[0]);
        messageQueue.RemoveAt(0);
    }

}
