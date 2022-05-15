using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 225;
    public float tiltSmooth = 2;
    public Vector3 startPos;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAudio;

    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager game;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;
        rigidbody.simulated = false;
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (game.GameOver) return;
        if (Input.GetMouseButtonDown(0))
        {
            tapAudio.Play();
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ScoreZone")
        {
            //register a score event
            OnPlayerScored(); //event sent to GameManager;
            //play a sound
            scoreAudio.Play();
            if (0 <= GameManager.Instance.Score && GameManager.Instance.Score < 5)
            {
                sprite.color = Color.white;
            }
            if (5<=GameManager.Instance.Score && GameManager.Instance.Score<10)
            {
            sprite.color = Color.blue;
            }
            else if (10 <= GameManager.Instance.Score && GameManager.Instance.Score < 15)
            {
                sprite.color = Color.black;
            }
            else if (15 <= GameManager.Instance.Score && GameManager.Instance.Score < 20)
            {
                sprite.color = Color.yellow;
            }
            else if (20 <= GameManager.Instance.Score && GameManager.Instance.Score < 25)
            {
                sprite.color = Color.cyan;
            }
            else if (25 <= GameManager.Instance.Score && GameManager.Instance.Score < 30)
            {
                sprite.color = Color.green;
            }
            else if (30 <= GameManager.Instance.Score && GameManager.Instance.Score < 31)
            {
                sprite.color = Color.clear;
            }
            else if (31 <= GameManager.Instance.Score && GameManager.Instance.Score < 35)
            {
                sprite.color = Color.red;
            }

        }
        if (collision.gameObject.tag == "DeadZone")
        {
            rigidbody.simulated = false;
            //register a dead event
            OnPlayerDied(); //event sent to GameManager;
            //play a sound
            dieAudio.Play();
        }
    }
}
