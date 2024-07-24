using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance { get; private set; }
    public List<GameObject> characters;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public GameObject GetCharacter(int index, GameObject previousCharacter = null)
    {
        if(characters.Count < 1) return previousCharacter;
        if(index < 0) index = characters.Count + index;
        
        GameObject character = characters[index];
        characters.RemoveAt(index);

        if(previousCharacter != null)
        {
            if(index != 0)
                characters.Insert(0, previousCharacter);
            else
                characters.Add(previousCharacter);
        }

        return character;
    }
}
