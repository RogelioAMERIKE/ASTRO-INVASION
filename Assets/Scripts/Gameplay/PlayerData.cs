using UnityEngine;
using System;

public static class PlayerData
{
    private const string CoinsKey = "TotalCoins";
    private const string DamageLevelKey = "DamageLevel";
    private const int MaxLevel = 3;

    public static event Action OnCoinsChanged;
    public static event Action OnUpgradeChanged;

    private static int _coins;
    private static int _damageLevel;
    private static bool _loaded;

    public static int Coins
    {
        get
        {
            Load();
            return _coins;
        }
    }

    public static bool IsMaxUpgrade
    {
        get
        {
            Load();
            return _damageLevel >= MaxLevel;
        }
    }

    public static int BulletDamage
    {
        get
        {
            Load();
            if (_damageLevel == 1) return 2;
            if (_damageLevel == 2) return 3;
            if (_damageLevel == 3) return 5;
            return 1;
        }
    }

    public static int NextUpgradePrice
    {
        get
        {
            Load();
            if (_damageLevel == 0) return 15;
            if (_damageLevel == 1) return 20;
            if (_damageLevel == 2) return 30;
            return -1;
        }
    }

    public static void AddCoins(int amount)
    {
        if (amount <= 0) return;

        Load();
        _coins += amount;
        PlayerPrefs.SetInt(CoinsKey, _coins);
        PlayerPrefs.Save();

        if (OnCoinsChanged != null) OnCoinsChanged();
    }

    public static bool BuyDamageUpgrade()
    {
        Load();
        if (_damageLevel >= MaxLevel) return false;

        int price = NextUpgradePrice;
        if (_coins < price) return false;

        _coins -= price;
        _damageLevel++;

        PlayerPrefs.SetInt(CoinsKey, _coins);
        PlayerPrefs.SetInt(DamageLevelKey, _damageLevel);
        PlayerPrefs.Save();

        if (OnCoinsChanged != null) OnCoinsChanged();
        if (OnUpgradeChanged != null) OnUpgradeChanged();
        return true;
    }

    public static void ResetCoins()
    {
        _coins = 0;
        _loaded = true;
        PlayerPrefs.SetInt(CoinsKey, 0);
        PlayerPrefs.Save();

        if (OnCoinsChanged != null) OnCoinsChanged();
    }

    public static void ResetUpgrades()
    {
        Load();
        _damageLevel = 0;
        PlayerPrefs.SetInt(DamageLevelKey, 0);
        PlayerPrefs.Save();

        if (OnUpgradeChanged != null) OnUpgradeChanged();
    }

    private static void Load()
    {
        if (_loaded) return;

        _coins = PlayerPrefs.GetInt(CoinsKey, 0);
        _damageLevel = Mathf.Clamp(PlayerPrefs.GetInt(DamageLevelKey, 0), 0, MaxLevel);
        _loaded = true;
    }
}
