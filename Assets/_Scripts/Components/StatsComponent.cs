using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

//半山腰太挤，你总得去山顶看看//
public class StatsComponent : CoreComponent ,IGameSaveAndLoad
{
    private Enity enity;
    //TODO : 补充所有属性和属性加成方法
    public float HP { get; private set; }
    public float MAXHP { get; private set; }
    public float MP { get; private set; }
    public float MAXMP { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AirMoveSpeed { get; private set; }
    public float DashSpeed { get;private set; }
    public float DashTime { get; private set; }
    public float AttackDamage { get; private set; }
    public float CriticalRate { get; private set; }
    public float CriticalDamage { get; private set; }
    public float StartForce { get;private set; }
    public float EndDecelerateRate { get; private set; }

    private void Start()
    {
        enity = GetComponentInParent<Enity>();

        MoveSpeed = enity.EnityData.MoveVelocity;
        StartForce = enity.EnityData.startForce;
        EndDecelerateRate = enity.EnityData.EndDecelerateRate;
        AirMoveSpeed = enity.EnityData.jumpSpeed;
    }

    public void HPChange(float amount)
    {
        HP += amount;

        //DOOT: UIChange
    }

    public void HPRateChange(float rate)
    {
        HP += HP * rate;
    }

    public void MPChange(float amount)
    {
        MP += amount;
    }

    public void MPRateChange(float rate)
    {
        MP += MP * rate;
    }

    public void MoveSpeedChange(float amount)
    {
        
    }

    public void AttackDamageChange(float rate)
    {
        
    }

    public void CriticalRateChange(float rate)
    {
        
    }

    //TODO : 完善存档的方法、内容

    #region GameDataSaveAndLoad

    public string GenerateUniqueID()
    {
        return $"{gameObject.scene.name}/{GameManager.GetPath(transform)}";
    }

    public string CaptureData()
    {
        StatsData data = new StatsData
        {
            HP = this.HP,
            AirMoveSpeed = this.AirMoveSpeed,
            MoveSpeed = this.MoveSpeed,
        };
        return JsonConvert.SerializeObject(data,Formatting.Indented);
    }

    public void RestoreData(string jsonData)
    {
        StatsData data = JsonConvert.DeserializeObject<StatsData>(jsonData);
        HP = data.HP;
        AirMoveSpeed = data.AirMoveSpeed;
        MoveSpeed = data.MoveSpeed;
    }

    public class StatsData
    {
        public float HP;
        public float AirMoveSpeed;
        public float MoveSpeed;
    }

    private void OnEnable()
    {
        GameSaveAndLoadSystem.RegisterSaveable(this);
    }

    private void OnDestroy()
    {
        GameSaveAndLoadSystem.UnregisterSaveable(GenerateUniqueID());
    }

    #endregion
}
