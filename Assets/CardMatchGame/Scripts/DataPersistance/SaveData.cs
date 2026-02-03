using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int rows;
    public int columns;
    public List<CardInfo> cardInfoList;
    public int score;
    public int turnsTaken;
    public int totalMatches;
}

[System.Serializable]
public struct CardInfo
{
   public int ID;
   public bool IsMatched; 
}

