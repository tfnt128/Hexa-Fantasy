using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    [SerializeField]
    private int movementPoints = 20;
    public int MovementPoints { get => movementPoints; }

    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f;

    private GlowHighlight glowHighlight;
    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;

    private Animator anim;
    private Rigidbody rb;

    public Animator crossfade;

    public int MP = 0;

    public ScoreManager score;

    public GameObject scorePanel;
    public GameObject winPanel;
    public GameObject selectSystem;

    public bool canPickUp;
    
    public AudioManager audio;

    private void Awake()
    {
        glowHighlight = GetComponent<GlowHighlight>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow();
    }

    public void MoveThroughPath(List<Vector3> currentPath)
    {
        canPickUp = false;
        anim.SetBool("IsWalking", true);
        pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
    }

    private IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
    {
        Quaternion startRotation = transform.localRotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
        {
            float timeElapsed = 0;
            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration; // 0-1
                transform.localRotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }
            transform.localRotation = endRotation;
        }
        StartCoroutine(MovementCoroutine(endPosition));
    }

    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;
        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }

        transform.position = endPosition;

        if (pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position!");

            StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
        }
        else
        {
            canPickUp = true;
            Debug.Log("Movement finished!");
            anim.SetBool("IsWalking", false);
            MovementFinished?.Invoke(this);
        }
    }
    public int id = -1;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("win"))
        {
            if(score.score >= 5)
            {
                scorePanel.SetActive(false);
                winPanel.SetActive(true);
                selectSystem.SetActive(false);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<Enemy_ID>().EnemyID == 0)
            {
                id = 0;
            }
            else
            {
                id = 1;
            }
            StartCoroutine(battleCamTransition(id));
            Destroy(other.gameObject, 1f);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("magic") && canPickUp)
        {
            
            StartCoroutine(destroyBook(other));

        }
    }

    IEnumerator destroyBook(Collider other)
    {
        anim.SetTrigger("PickUp");
        canPickUp = false;
        yield return new WaitForSecondsRealtime(.5f);
        audio.grabBook.Play();
        MP++;
        Destroy(other.gameObject);

    }
    IEnumerator battleCamTransition(int id)
    {
        audio.music.Stop();
        audio.musicBattle.Play();
        crossfade.SetTrigger("Battle");
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;

        EventManager.Instance.ActivateBattleCam(id);
    }
}
