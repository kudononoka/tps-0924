﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private static int _score = 0; public static int score { get { return _score; } }
    [SerializeField] private static int _point = 0; public static int point { get { return _point; } }
    private static int damageNumber = 0; public static int damageNum { get { return damageNumber; } }
    private static int enemyNumber = 0; public static int EnemyNum { get { return enemyNumber; } set { enemyNumber = value; } }
    [SerializeField] GameObject[] _enemies;
    [Header("出現範囲"), SerializeField] float movingRange;
    [Header("出現範囲"), SerializeField] float movingRange2;

    [SerializeField] GameObject _sellCanvas;
    [SerializeField] GameObject _normalCanvas;
    [SerializeField] Text SellCanvasPointText;
    [SerializeField] Text MainCanvasPointText;
    [SerializeField] Text MainCanvasScoreText;
    [SerializeField] Text TimerText;
    [SerializeField] Text StageCountText;
    [SerializeField] PlayerHP playerHp;

    private int SurvivingEnemies = 0;

    [Tooltip("Stageが変わるごとにPlayerの位置を初期化する")]
    Transform player;

    
    private int _stageCount = 1; public int StageCount => _stageCount;

    private float _timer = 0;
    [Header("戦闘時間"), SerializeField] float _battleTime = 60;
    [Header("休憩時間"), SerializeField] float _breakTime = 15;
    bool isBreak; public bool IsBreak => isBreak;
   
    bool isGame; public bool IsGame { get { return isGame; } set { isGame = value; } }

    public static GameManager Instance　=> instance;

    
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this as GameManager;
        }
        else if(instance == this)
        {
            return;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        EnemyInstate();
        _normalCanvas.SetActive(true);
        _sellCanvas.SetActive(false);
        isGame = true;
 
        _timer = _battleTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        SellCanvasPointText.text = $"Point {_point}";
        MainCanvasPointText.text = $"Point {_point}";
        MainCanvasScoreText.text = $"Score {_score}";
        TimerText.text = String.Format("{0:00}", (int)_timer);
        StageCountText.text = $"{_stageCount}";
        if (_timer <= 0 && isBreak)
        {
            EnemyInstate();
            player.position = Vector3.zero;
            _normalCanvas.SetActive(true);
            _sellCanvas.SetActive(false);
            _timer = _battleTime;
            isBreak = !isBreak;
        }
        if (_timer <= 0 && !isBreak)
        {
            GameObjectFind("Enemy");
            GameObjectFind("Bomb");
            Debug.Log(SurvivingEnemies);
            if(SurvivingEnemies == 0)
            {
                Score(500);
            }
            
            
            
            if (_stageCount == 3)
            {
                SceneManager.LoadScene("Result");
            }
            else
            {
                _timer = _breakTime;
                _normalCanvas.SetActive(false);
                _sellCanvas.SetActive(true);

                _stageCount++;
                isBreak = !isBreak;
            }
        }
        damageNumber = playerHp.DamageNum;
        
        if(!isGame)
        {
            SceneManager.LoadScene("Result");
        }
        
    }

    public void Point(int point)
    {
        _point += point;
    }
    public void CostPoint(int costpoint)
    {
        _point -= costpoint;
    }

    public void Score(int score)
    {
        _score += score;
    }
    
    

    void GameObjectFind(string name)
    {
        
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(name);
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null)
                {
                    return;
                }
                SurvivingEnemies++;
                Destroy(enemy);
            }
        
        
        
    }

    void EnemyInstate()
    {
        foreach (var enemy in _enemies)
        {
           
            if (enemy.name == "MiniSphere" || enemy.name == "PunchRobo (1)")
            {
                int num = UnityEngine.Random.Range(10, 15);
                for (int i = 0; i < num; i++)
                {
                    Vector3 position = new Vector3(UnityEngine.Random.Range(-movingRange, movingRange), 0, UnityEngine.Random.Range(-movingRange, movingRange));
                    Vector3 PerfectPos = RangeSareti(position);
                    Instantiate(enemy, PerfectPos, Quaternion.identity);
                }
            }
            else if(enemy.name == "robotSphere")
            {
                Vector3 position = new Vector3(UnityEngine.Random.Range(-movingRange, movingRange), 0, UnityEngine.Random.Range(-movingRange, movingRange));
                Instantiate(enemy, position, Quaternion.identity);
            }
        }
    }

    private Vector3 RangeSareti(Vector3 pos)
    {

        while(Vector3.Distance(player.position, pos) < 20)
        {
            pos = new Vector3(UnityEngine.Random.Range(-movingRange, movingRange), 0, UnityEngine.Random.Range(-movingRange, movingRange));
        }
        return pos;

    }
}
