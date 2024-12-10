[System.Serializable]
public class LevelData {
    public int LevelNumber;
    public int Stars;

    public LevelData(int levelNumber, int stars) {
        LevelNumber = levelNumber;
        Stars = stars;
    }
}
