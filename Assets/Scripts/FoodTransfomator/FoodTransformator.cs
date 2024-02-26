using System;
using UnityEngine;

public class FoodTransformator : FoodCollector
{
    [SerializeField] protected ItemLauncher launcher;
    
    [SerializeField] protected Transform foodSpawnPoint;
    [SerializeField] protected float releaseForce = 5f;

    float _timer = 0f;
    [SerializeField] float cookingTime = 5f;
    bool _cooking = false;

    [Header(("Cooking parameters"))]
    [SerializeField] bool needChopped;
    [SerializeField] bool needBaked;
    [SerializeField] bool needFried;

    void Awake() { launcher = GetComponent<ItemLauncher>(); }

    protected override void OnFoodCollected()
    {
        canCollect = false;
        _timer = cookingTime;
        _cooking = true;
        Destroy(collectedFoodGo);
    }

    void Update()
    {
        if (!_cooking)
            return;
        
        if (_timer > 0f)
            _timer -= Time.deltaTime;
        else
            ReleaseFood();
    }

    protected virtual void ReleaseFood()
    {
        collectedFoodData = null;
        collectedFoodGo = null;
        canCollect = true;
        _cooking = false;
    }
}
