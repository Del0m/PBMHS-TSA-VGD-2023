//This class is used to check for a player's score or stats
//* recommend to make this script a required script for other scripts *
public class PlayerScore 
{
    //Can be called from any script
    public int Score = 0;

    //Used to easily track and modify a certain player from a group of players
    //* USE THIS VARIABLE TO AVOID DOING 0 or 1 SINCE ScoreManager.cs doesn't pick the same player, just refer to this script sitting on the desired player *
    public int playerIndex = 0;
}
