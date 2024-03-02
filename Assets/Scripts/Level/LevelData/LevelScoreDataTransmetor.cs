using UnityEngine;

public class LevelScoreDataTransmetor : MonoBehaviour
{
    public static LevelScoreDataTransmetor s_Instance;

    public float score = 0.0f;

    void Awake()
    {
        if(!s_Instance)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
