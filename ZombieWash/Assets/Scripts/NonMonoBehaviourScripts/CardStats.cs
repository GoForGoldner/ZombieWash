using System.Collections.Generic;

public class CardStats {
    public int Health { get; private set; }
    private int _startingHealth;

    public List<TaskScriptableObject> Tasks;

    public CardStats(CardScriptableObject cardScriptableObject) {
        _startingHealth = Health = cardScriptableObject.Health;
        Tasks = new(cardScriptableObject.Tasks);
    }

    public void LowerHealth() {
        Health--;
    }

    public bool CardDied() {
        return Health == 0;
    }

    public void ResetHealth() {
        Health = _startingHealth;
    }
}
