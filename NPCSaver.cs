using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSaver : ScriptableObject {
    public int strength, intelligence, social;
    public string pickedname = "", description = "";
    public bool friendly, goodmannered, teamplayer, gunpro, meleepro, peaceful;
    public int currentTextureCount = 0;
}
