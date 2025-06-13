using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class GameData
{
    public PlayerData playerData;

    public GameData()
    {
        playerData = new PlayerData();
    }
}

[Serializable]
public class PlayerData
{
    public float health;
    public float hunger;
    public List<ItemData> items;

    public PlayerData()
    {
        health = 100;
        hunger = 150;
        items = new List<ItemData>();
    }

    public PlayerData(float health, float hunger, List<ItemData> items)
    {
        this.health = health;
        this.hunger = hunger;
        this.items = items;
    }

}

[Serializable]
public class ItemData
{
    public string id;
    public int count;

    public ItemData(string id, int count)
    {
        this.id = id;
        this.count = count;

    }
}