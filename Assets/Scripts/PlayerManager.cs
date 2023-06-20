using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    private GameManager gameManager;
    private FixedJoystick fixedJoystick;
    private Animator animator;
    private PathCreator pathCreator;
    private PhotonView pv;
    private TextMeshProUGUI nickNameText;
    private ScoreboardManager scoreboardManager;
    private string playerName;
    private float rotateSpeed = 100f;
    private float forwardSpeed;
    private float initialSpeed = 11f;
    Vector3 direction, addedPos;
    private bool isFinished = false;
    public string nickName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        pathCreator = GameObject.Find("PathCreator").GetComponent<PathCreator>();
        fixedJoystick = GameObject.Find("Canvas").transform.Find("FixedJoystick").GetComponent<FixedJoystick>();
        scoreboardManager = GameObject.Find("Canvas").transform.Find("ScoreboardPanel").GetComponent<ScoreboardManager>();
        nickNameText = transform.Find("Canvas").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            forwardSpeed = initialSpeed;
            playerName = "Player" + PlayerPrefs.GetString("SelectedCharacter", Characters.instance.GetCharacter(0).characterName);
            nickName = pv.Owner.NickName;
            nickNameText.text = nickName;
            nickNameText.color = new Color32(255, 0, 0, 255);
            Debug.Log("player name: " + pv.Owner.NickName);
            AudioManager.instance.PlayRaceAudio();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            if (fixedJoystick.Vertical != 0 || fixedJoystick.Horizontal != 0)
            {
                RunAnimation();
            }
            else
            {
                IdleAnimation();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.tag == "Finish")
            {
                fixedJoystick.gameObject.SetActive(false);
                forwardSpeed = 0;
                isFinished = true;
                JumpAnimation();
                scoreboardManager.MarkPlayerFinished(nickName);
                AudioManager.instance.PlayLevelEndAudio();
                gameManager.CenterScoreboard();
                gameManager.ShowExitButton();
            }

            if(other.tag == "Obstacle")
            {
                pv.RPC("RPC_SlowDown", RpcTarget.All, null);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground" && pv.IsMine)
        {
            transform.position = pathCreator.path.GetClosestPointOnPath(transform.position);
        }
    }


    private void IdleAnimation()
    {
        animator.SetTrigger("Idle");
    }

    private void RunAnimation()
    {
        animator.SetTrigger("Run");
    }

    private void JumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            if (fixedJoystick.Vertical != 0 || fixedJoystick.Horizontal != 0)
            {
                addedPos = new Vector3(fixedJoystick.Horizontal * forwardSpeed * Time.fixedDeltaTime, 0, fixedJoystick.Vertical * forwardSpeed * Time.fixedDeltaTime);
                transform.position += addedPos;

                direction = (Vector3.forward * fixedJoystick.Vertical + Vector3.right * fixedJoystick.Horizontal);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * rotateSpeed);
            }
        }
    }


    public string GetPlayerName()
    {
        return playerName;
    }

    [PunRPC]
    public void RPC_DestroyPlayer()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void RPC_SpeedUp()
    {
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        forwardSpeed = initialSpeed*2;
        yield return new WaitForSeconds(2f);
        forwardSpeed = initialSpeed;
    }

    [PunRPC]
    public void RPC_SlowDown()
    {
        StartCoroutine(SlowDown());
    }

    IEnumerator SlowDown()
    {
        forwardSpeed = initialSpeed/2;
        yield return new WaitForSeconds(2f);
        forwardSpeed = initialSpeed;
    }
}
