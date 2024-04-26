using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeLetterManager : MonoBehaviour
{
    [SerializeField] FridgeLetter prefabLetter;
    [SerializeField] float letterSpacing = 5;

    // Start is called before the first frame update
    void Start()
    {
        SpawnWord("lasagna", Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnLetter(char letter, Vector3 position)
    {
        FridgeLetter fletter = Instantiate(prefabLetter, position, Quaternion.identity, transform);
        fletter.SetLetter(letter);
        fletter.SetDestination(position);
        fletter.SetColor();
    }

    void SpawnWord(string word, Vector3 startLocation)
    {
        for(int i = 0; i < word.Length; i++)
        {
            SpawnLetter(word[i], startLocation + (Vector3.right * (-letterSpacing * i)));
        }
    }
}
