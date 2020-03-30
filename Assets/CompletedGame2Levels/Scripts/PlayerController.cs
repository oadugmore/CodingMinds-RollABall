using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Text countText;
    public Text timeText;
    public Text winText;
    public GameObject level1;
    public GameObject level2;
    public Transform level2Start;

    private int time;
    private Rigidbody rb;
    private int count;
    private bool gameOver;
    private bool onLevel2;
    private int level1Collectables;
    private int level2Collectables;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        time = 30;
        gameOver = false;
        onLevel2 = false;
        winText.text = "";
        level1Collectables = level1.GetComponentsInChildren<Rotator>().Length;
        level2Collectables = level2.GetComponentsInChildren<Rotator>().Length;
        SetCountText();
        StartCoroutine(Timer());
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Pick Up") && !gameOver)
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            if (count == level1Collectables && !onLevel2)
            {
                Win();
                Invoke("StartSecondLevel", 5f);
            }
            else if (count == level2Collectables && onLevel2)
            {
                Win();
            }
        }
        else if (other.gameObject.CompareTag("Die"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            StartSecondLevel();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    void Win()
    {
        winText.text = "You Win!";
        gameOver = true;
    }

    void Lose()
    {
        winText.text = "You Lose!";
        gameOver = true;
    }

    void StartSecondLevel()
    {
        gameOver = false;
        time = 30;
        winText.text = "";
        count = 0;
        onLevel2 = true;
        SetCountText();
        StopAllCoroutines();
        StartCoroutine(Timer());
        transform.position = level2Start.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        var collectables = level2.GetComponentsInChildren<Rotator>(true);
        foreach (Rotator r in collectables)
        {
            r.gameObject.SetActive(true);
        }
    }

    IEnumerator Timer()
    {
        while(time > 0)
        {
            time--;
            timeText.text = "Time: " + time;
            yield return new WaitForSeconds(1);
        }
        if (!gameOver)
        {
            Lose();
        }
    }
}