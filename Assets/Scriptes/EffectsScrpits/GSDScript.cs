using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//Global Saving Dump Script - Script to save data that shouldn't reset when loading new scene
public class GSDScript : MonoBehaviour
{
    //flags for the doors status
    public bool ArmoryDoor = false;
    public bool KitchenDoor = false;
    public bool LibraryDoor = false;
    public bool GardenDoor = false;
    public bool CatacombsDoor = false;
    public bool ThroneRoomDoor = false;
    public float doorCloserTime = 0;
    //Saves the player position when entering a room from a hub of rooms (still not in use)
    public Vector3 PlayerPosHall = new Vector3();
    //Saves the Score and kills
    public int Score = 0;
    public int Kills = 0;
    //Flags for item status
    public bool Sugar = false;
    public bool Orb = false;
    public int Thrones = 0;
    public int Book = 0;
    public bool Mushroom = false;
    //Saves the time the last time color changed
    float MushroomTime = 0;
    //Saves the time the last time text updated
    float textUpdateTime = 0;
    //Flag Bryant status
    public bool Bryant = true;
    //Saves the position of the new highscore
    int newHighscore = 0;
    //Flag for if the time bonus was collected
    bool bonusCollected = false;
    //Saves the last death time
    public float deathTime = 0;
    float victoryTime = 0;
    //Holds the current scene
    Scene CurrentScene;
    //Saves the audio source component
    public AudioSource audioSource;
    public int life = 3;
    public float ImmunityTime = 0;
    float blinkTime = 0;
    bool blink = true;

    //Called in initialization
    void Start()
    {
        //Saves the audio source component
        audioSource = GetComponent<AudioSource>();
    }

    //Called once per frame
    void Update()
    {
        LifeCheck();
        //If pressed ESC close the game
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        //Saves current scene
        CurrentScene = SceneManager.GetActiveScene();
        //If in victory screen goes to Victory function
        if (CurrentScene.name == "VictoryScreen")
            Victory();
        //If in start screen starts the background audio
        if (CurrentScene.name == "StartScreen")
            audioSource.Play();
        //If in victory screen stops the background audio and goes to Reset function
        if (CurrentScene.name == "DeathScreen")
        {
            audioSource.Stop();
            Reset();
        }
        //If in HallRoom scene, updates PlayerPosHall 
        if (CurrentScene.name == "HallRoom")
            PlayerPosHall = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //If took the Mushroom item, goes to High function
        if (Mushroom)
            High();
        //Make it so that this game object won't reset when loading new scene
        DontDestroyOnLoad(this.gameObject);

    }
    void LifeCheck()
    {
        if (!GameObject.Find("Life"))
            return;
        if (life == 0)
        {
            SceneManager.LoadScene("DeathScreen");
            return;
        }
        if (ImmunityTime > Time.time)
        {
            if (blinkTime < Time.time)
            {
                blinkTime = Time.time + 0.1f;
                if (blink)
                {
                    blink = false;
                    GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().sortingLayerName = "BTS";
                }
                else
                {
                    blink = true;
                    GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                }
            }
        }
        else GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        if (life != GameObject.Find("Life").GetComponent<Animator>().GetInteger("Life"))
        {
            if (life != GameObject.Find("Life").GetComponent<Animator>().GetInteger("Life") - 1) life++;
            ImmunityTime = Time.time + 2f;
            GameObject.Find("DuncanJr").GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/DuncanSounds/ShortScream");
            GameObject.Find("DuncanJr").GetComponent<AudioSource>().Play();
            GameObject.Find("DuncanJr").GetComponent<DuncanControl>().currentClipName = "ShortScream";
            GameObject.Find("Life").GetComponent<Animator>().SetInteger("Life", life);
        }

    }
    //Victory Function (when in the victory screen)
    void Victory()
    {
        //Shows the highscores
        ShowHighScores();
        //Resets the highscores
        //ResetHighScore();
        //If time bonus wasn't collected, goes to Time Bonus function 
        if (!bonusCollected)
            Bonus();
        if (Time.time - victoryTime >= 1f)
            GameObject.Find("PressEnter").GetComponent<SpriteRenderer>().sortingLayerName = "BackEffects";
        else
            GameObject.Find("PressEnter").GetComponent<SpriteRenderer>().sortingLayerName = "BTS";
        //Holds the name, score and kills text component
        TextMeshProUGUI name = GameObject.Find("Name").GetComponent<TextScript>().printer;
        TextMeshProUGUI score = GameObject.Find("Score").GetComponent<TextScript>().printer;
        TextMeshProUGUI kills = GameObject.Find("Kills").GetComponent<TextScript>().printer;
        TextMeshProUGUI time = GameObject.Find("Time").GetComponent<TextScript>().printer;
        //If score higher than the fifth highest score, sets new highscore
        if (Score > PlayerPrefs.GetInt("score5"))
            SetNewHighscore();
        //Goes over each input char
        foreach (char c in Input.inputString)
        {
            //If backspace was pressed, delete the last char(if there is more than 0 chars)
            if (c == '\b')
            {
                if (name.text.Length != 0)
                {
                    name.text = name.text.Substring(0, name.text.Length - 1);
                }
            }
            //If enter was pressed, goes to Reset function
            else if (((c == '\n') || (c == '\r')) && Time.time - victoryTime >= 1f)
                Reset();
            //Else add the char to name (while it is shorter than 28)
            else if (name.text.Length < 13)
                name.text += c;
        }
        if (victoryTime != 0)
        {
            int deltaTime = (int)(victoryTime - deathTime);
            int min = deltaTime / 60;
            string Min;
            if (min > 10) Min = min + "";
            else Min = "0" + min;
            int sec = deltaTime % 60;
            string Sec;
            if (sec > 10) Sec = sec + "";
            else Sec = "0" + sec;
            time.text = Min + ":" + Sec;
        }
        //Updates the text every 0.1 sec
        if (textUpdateTime <= Time.time)
            textUpdateTime = Time.time + 0.1f;
        //Else returns
        else
            return;
        //Adds to score (the text) + 10, until it reaches score
        int scoreNum;
        int.TryParse(score.text, out scoreNum);
        if (Score > scoreNum && Score != 0)
        {
            score.text = "" + (scoreNum + 10);
        }
        //Adds to Kills (the text) + 10, until it reaches kills
        int killNum;
        int.TryParse(kills.text, out killNum);
        if (Kills > killNum && Kills != 0)
        {
            kills.text = "" + (killNum + 1);
        }
    }

    //Shows the high scores
    void ShowHighScores()
    {
        //Holds number1,2,3,4,5 text component
        TextMeshProUGUI number1 = GameObject.Find("Number1").GetComponent<TextScript>().printer;
        TextMeshProUGUI number2 = GameObject.Find("Number2").GetComponent<TextScript>().printer;
        TextMeshProUGUI number3 = GameObject.Find("Number3").GetComponent<TextScript>().printer;
        TextMeshProUGUI number4 = GameObject.Find("Number4").GetComponent<TextScript>().printer;
        TextMeshProUGUI number5 = GameObject.Find("Number5").GetComponent<TextScript>().printer;
        //Check if the corresponding playerprefs has value, if not than set their int to 0 and string to "nobody - 0"
        //Updates the text component to the corresponding text component to his playerprefs
        if (PlayerPrefs.GetString("number1", "none") == "none")
        {
            PlayerPrefs.SetInt("score1", 0);
            PlayerPrefs.SetString("number1", "nobody - " + PlayerPrefs.GetInt("score1"));
        }
        number1.text = PlayerPrefs.GetString("number1");
        if (PlayerPrefs.GetString("number2", "none") == "none")
        {
            PlayerPrefs.SetInt("score2", 0);
            PlayerPrefs.SetString("number2", "nobody - " + PlayerPrefs.GetInt("score2"));
        }
        number2.text = PlayerPrefs.GetString("number2");
        if (PlayerPrefs.GetString("number3", "none") == "none")
        {
            PlayerPrefs.SetInt("score3", 0);
            PlayerPrefs.SetString("number3", "nobody - " + PlayerPrefs.GetInt("score3"));
        }
        number3.text = PlayerPrefs.GetString("number3");
        if (PlayerPrefs.GetString("number4", "none") == "none")
        {
            PlayerPrefs.SetInt("score4", 0);
            PlayerPrefs.SetString("number4", "nobody - " + PlayerPrefs.GetInt("score4"));
        }
        number4.text = PlayerPrefs.GetString("number4");
        if (PlayerPrefs.GetString("number5", "none") == "none")
        {
            PlayerPrefs.SetInt("score5", 0);
            PlayerPrefs.SetString("number5", "nobody - " + PlayerPrefs.GetInt("score5"));
        }
        number5.text = PlayerPrefs.GetString("number5");

    }

    //Resets the highscores
    void ResetHighScore()
    {
        //Set all the playerPrefs int to 0 and strings to "nobody - 0"
        PlayerPrefs.SetInt("score1", 0);
        PlayerPrefs.SetString("number1", "nobody - " + PlayerPrefs.GetInt("score1"));
        PlayerPrefs.SetInt("score2", 0);
        PlayerPrefs.SetString("number2", "nobody - " + PlayerPrefs.GetInt("score2"));
        PlayerPrefs.SetInt("score3", 0);
        PlayerPrefs.SetString("number3", "nobody - " + PlayerPrefs.GetInt("score3"));
        PlayerPrefs.SetInt("score4", 0);
        PlayerPrefs.SetString("number4", "nobody - " + PlayerPrefs.GetInt("score4"));
        PlayerPrefs.SetInt("score5", 0);
        PlayerPrefs.SetString("number5", "nobody - " + PlayerPrefs.GetInt("score5"));
    }

    //Adds time bonus to the score
    void Bonus()
    {
        victoryTime = Time.time;
        //If took less than 4 min, adds 200 to score
        if (victoryTime - deathTime < 200f) Score += 200;
        //Else if took less than 8 min, adds 100 to score
        else if (victoryTime - deathTime < 480f) Score += 100;
        //If took less than 15 min, adds 50 to score
        else if (victoryTime - deathTime < 900f) Score += 50;
        if (life == 3) Score += 400;
        if (life == 2) Score += 200;
        //Turn the flag for the time bonus collection
        bonusCollected = true;
    }

    //Sets a new highscore 
    void SetNewHighscore()
    {
        //If already calculated what place he will be, changes only the correct place
        if (newHighscore != 0)
        {
            switch (newHighscore)
            {
                case 1:
                    PlayerPrefs.SetInt("score1", Score);
                    PlayerPrefs.SetString("number1", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score1"));
                    break;
                case 2:
                    PlayerPrefs.SetInt("score2", Score);
                    PlayerPrefs.SetString("number2", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score2"));
                    break;
                case 3:
                    PlayerPrefs.SetInt("score3", Score);
                    PlayerPrefs.SetString("number3", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score3"));
                    break;
                case 4:
                    PlayerPrefs.SetInt("score4", Score);
                    PlayerPrefs.SetString("number4", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score4"));
                    break;
                case 5:
                    PlayerPrefs.SetInt("score5", Score);
                    PlayerPrefs.SetString("number5", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score5"));
                    break;
            }
            return;
        }
        //If score is higher than fifth place, replaces him
        if (Score > PlayerPrefs.GetInt("score5"))
        {
            PlayerPrefs.SetInt("score5", Score);
            PlayerPrefs.SetString("number5", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score5"));
            newHighscore = 5;
        }
        //If score is higher than fourth place,copies fourth place to fifth and then replaces him
        if (Score > PlayerPrefs.GetInt("score4"))
        {
            PlayerPrefs.SetInt("score5", PlayerPrefs.GetInt("score4"));
            PlayerPrefs.SetString("number5", PlayerPrefs.GetString("number4"));
            PlayerPrefs.SetInt("score4", Score);
            PlayerPrefs.SetString("number4", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score4"));
            newHighscore = 4;
        }
        //If score is higher than third place,copies third place to fourth and then replaces him
        if (Score > PlayerPrefs.GetInt("score3"))
        {
            PlayerPrefs.SetInt("score4", PlayerPrefs.GetInt("score3"));
            PlayerPrefs.SetString("number4", PlayerPrefs.GetString("number3"));
            PlayerPrefs.SetInt("score3", Score);
            PlayerPrefs.SetString("number3", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score3"));
            newHighscore = 3;
        }
        //If score is higher than secoond place,copies second place to third and then replaces him
        if (Score > PlayerPrefs.GetInt("score2"))
        {
            PlayerPrefs.SetInt("score3", PlayerPrefs.GetInt("score2"));
            PlayerPrefs.SetString("number3", PlayerPrefs.GetString("number2"));
            PlayerPrefs.SetInt("score2", Score);
            PlayerPrefs.SetString("number2", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score2"));
            newHighscore = 2;
        }
        //If score is higher than first place,copies first place to second and then replaces him
        if (Score > PlayerPrefs.GetInt("score1"))
        {
            PlayerPrefs.SetInt("score2", PlayerPrefs.GetInt("score1"));
            PlayerPrefs.SetString("number2", PlayerPrefs.GetString("number1"));
            PlayerPrefs.SetInt("score1", Score);
            PlayerPrefs.SetString("number1", GameObject.Find("Name").GetComponent<TextScript>().printer.text + " - " + PlayerPrefs.GetInt("score1"));
            newHighscore = 1;
        }
    }

    //Reset everything for a new run
    void Reset()
    {
        //Resets all the flags and Kills
        ArmoryDoor = false;
        KitchenDoor = false;
        LibraryDoor = false;
        GardenDoor = false;
        CatacombsDoor = false;
        ThroneRoomDoor = false;
        Mushroom = false;
        Kills = 0;
        Sugar = false;
        Thrones = 0;
        Book = 0;
        Orb = false;
        Bryant = true;
        life = 3;
        newHighscore = 0;
        doorCloserTime = 0;
        //Updates death time
        deathTime = Time.time;
        victoryTime = 0;
        bonusCollected = false;
         //Reset player position
         PlayerPosHall = new Vector3(0, -3.46f);
        //If player pressed R or the came from victory screen, reset score, play the background audio and returns to Hallroom
        if (Input.anyKey || CurrentScene.name == "VictoryScreen")
        {
            Score = 0;
            audioSource.Play();
            SceneManager.LoadScene("HallRoom");
        }
    }

    //Randomizes the color of every object in the game 
    void High()
    {
        //Randomizes only every 1 sec
        if (Time.time < MushroomTime)
            return;
        MushroomTime = Time.time + 1f;
        //Picks random color for every tag
        Color colorBackgrounds = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color colorPlayer = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color colorDoors = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color colorFires = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color colorLamps = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color colorEnemies = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color colorItems = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color colorInfo = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //change the color for every object in each tag
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Backgrounds"))
            go.GetComponent<Renderer>().material.color = colorBackgrounds;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            go.GetComponent<Renderer>().material.color = colorPlayer;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Doors"))
            go.GetComponent<Renderer>().material.color = colorDoors;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Fires"))
            go.GetComponent<Renderer>().material.color = colorFires;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Lamps"))
            go.GetComponent<Renderer>().material.color = colorLamps;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemies"))
            go.GetComponent<Renderer>().material.color = colorEnemies;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Items"))
            go.GetComponent<Renderer>().material.color = colorItems;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Info"))
            go.GetComponent<Renderer>().material.color = colorInfo;

    }
}
