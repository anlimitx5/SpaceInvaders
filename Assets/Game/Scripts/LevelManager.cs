using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public LevelStats[] levels;
    private void Awake()
    {
        Singleton();
    }
    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        
    }
}
