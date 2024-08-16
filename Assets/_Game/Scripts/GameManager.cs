using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List<GameObject> LevelButtons = new List<GameObject>();
    [SerializeField] private List<GameObject> Level = new List<GameObject>(); 
   [SerializeField] private GameObject StartUI; 
   [SerializeField] private GameObject LevelUI; 
   [SerializeField] private GameObject WinUI; 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Player Player;
    private int level;
    private int currentLevelIndex;
    private GameObject currentLevel;
    private bool[] levelsCompleted;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
        LoadLevelStatus();
      
    }
    private void Start()
    {
        StartUI.SetActive(true);
        UpdateLevelButtons();
    }
    private void Update()
    {
        PlayWinUI();
    }
  
    private void PlayWinUI()
    {
        if (Player == null) { return; }   
        if (Player.isWin)
        {
            WinUI.gameObject.SetActive(true);
            levelsCompleted[currentLevelIndex] = true;
            SaveLevelStatus();
            Player.isWin = false;
        }
    }
  
    private void UpdateLevelButtons()
    {
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            if (i == 0 || levelsCompleted[i - 1]) 
            {
                LevelButtons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                LevelButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    private void LoadLevelStatus()
    {
        levelsCompleted = new bool[Level.Count];

        for (int i = 0; i < Level.Count; i++)
        {
            levelsCompleted[i] = PlayerPrefs.GetInt("LevelCompleted_" + i, 0) == 1;
        }
    }

    private void SaveLevelStatus()
    {
        for (int i = 0; i < Level.Count; i++)
        {
            PlayerPrefs.SetInt("LevelCompleted_" + i, levelsCompleted[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
        
    }
    public void Play()
    {
        LevelUI.SetActive(true);
        StartUI.SetActive(false);
    }
    public void CreateLevel(int i)
    {
        LevelUI.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }

        currentLevel = Instantiate(Level[i]);
        currentLevelIndex = i;
        Player = FindObjectOfType<Player>();
    }
    public void ReplayLevel()
    {
        CreateLevel(currentLevelIndex);
        WinUI.SetActive(false);
    }
    public void NextLevel()
    {
        CreateLevel(currentLevelIndex + 1);
        WinUI.SetActive(false);
    }
    public void ResetLevel()
    {
        PlayerPrefs.DeleteAll();
    }
}
