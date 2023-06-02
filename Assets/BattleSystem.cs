using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public Unit player;
    

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject enemy2Prefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit2 playerUnit;
    Unit2 enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public GameObject normalCamera;
    public GameObject combatCamera;

    public Animator crossfade;

    bool playerMiss = false;
    bool enemyMiss = false;

    Enemy_ID enemy;

    public ShakeController playerCanvasShake;
    public ShakeController enemyCanvasShake;

    public ColorTransitionExample transitionColor;

    public ScoreManager score;

    public GameObject goldText;

    private void OnEnable()
    {
        EventManager.Instance.ActivatedBattle += OnBattleActivated;
    }

    private void OnDisable()
    {
        EventManager.Instance.ActivatedBattle -= OnBattleActivated;
    }
    private void Update()
    {
        
    }

    private void Activate()
    {
        normalCamera.SetActive(false);
        combatCamera.SetActive(true);
        crossfade.SetTrigger("StarTranstion");
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }


    private void OnBattleActivated(bool activate, int id)
    {

        if (activate)
        {
            Activate();
        }
        else
        {

            StartCoroutine(battleCamTransition());
        }

    }
    IEnumerator battleCamTransition()
    {
        crossfade.SetTrigger("Battle");
        yield return new WaitForSecondsRealtime(0.5f);        
        combatCamera.SetActive(false);
        normalCamera.SetActive(true);
        crossfade.SetTrigger("StarTranstion");
    }

    int cont = 0;
    public IEnumerator SetupBattle()
    {
        goldText.SetActive(false);
        if (cont == 0)
        {
            GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
            playerUnit = playerGO.GetComponent<Unit2>();
            cont++;
        }
        if(player.id == 0)
        {
            GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
            enemyUnit = enemyGO.GetComponent<Unit2>();
        }
        else
        {
            GameObject enemyGO = Instantiate(enemy2Prefab, enemyBattleStation);
            enemyUnit = enemyGO.GetComponent<Unit2>();
        }

        

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    int damageAmount;
    int miss;
    IEnumerator PlayerAttack()
    {
        playerBattleStation.GetComponentInChildren<MoveObjectExample>().MoveToTargetPoint();
        miss = Random.Range(0, 101);
        Debug.Log(miss);
        if(miss > 80)
        {
            dialogueText.text = "You missed!";
            yield return new WaitForSeconds(.8f);
        }
        else
        {
            yield return new WaitForSeconds(.2f);
            enemyBattleStation.GetComponentInChildren<BlinkEffectExample>().TriggerBlinkEffect();

            enemyCanvasShake.StartShake(.5f, 1f);

            int damageAmount = Random.Range(playerUnit.damage, 13);

            bool isDead = enemyUnit.TakeDamage(damageAmount);
            Debug.Log(damageAmount);

            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "The attack is successful!";
        }

        

        yield return new WaitForSeconds(2f);

        if (enemyUnit.currentHP <= 0)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        miss = Random.Range(0, 101);
        enemyBattleStation.GetComponentInChildren<MoveObjectExample>().MoveToTargetPoint();
        if (miss > 50)
        {
            dialogueText.text = enemyUnit.unitName + " missed!";
            yield return new WaitForSeconds(.8f);
        }
        else
        {
            
            dialogueText.text = enemyUnit.unitName + " attacks!";
            yield return new WaitForSeconds(.2f);

            playerBattleStation.GetComponentInChildren<BlinkEffectExample>().TriggerBlinkEffect();
            playerCanvasShake.StartShake(.5f, 1f);
            int damageAmount = Random.Range(playerUnit.damage, 10);
            bool isDead = playerUnit.TakeDamage(damageAmount);

            playerHUD.SetHP(playerUnit.currentHP);
        }
        

        

        yield return new WaitForSeconds(1f);

        if (playerUnit.currentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            StartCoroutine(VictoryToGameCore());
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
            StartCoroutine(DefeatToGameOverScreen());
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }
    ChangeColorPlayer playerSprite;
    IEnumerator PlayerHeal()
    {
        playerSprite = playerBattleStation.GetComponentInChildren<ChangeColorPlayer>();
        playerSprite.StartTransition();
        transitionColor.StartTransition();
        playerUnit.Heal(5);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        if(player.MP > 0)
        {
            StartCoroutine(PlayerHeal());
            player.MP--;
        }
        else
        {
            StartCoroutine(notEnought());
        }
        
    }

    private IEnumerator VictoryToGameCore()
    {
        yield return new WaitForSeconds(2f);
        dialogueText.text = "You found 1 Gold!";        
        score.score++;
        yield return new WaitForSeconds(2f);
        EventManager.Instance.DeativateBattleCam(-1);
        Destroy(enemyBattleStation.GetComponentInChildren<Unit2>().gameObject);
        goldText.SetActive(true);

    }
    private IEnumerator DefeatToGameOverScreen()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator notEnought()
    {
        dialogueText.text = "Not enought MP";
        yield return new WaitForSeconds(2f);
        dialogueText.text = "Choose an action:";
    }

}
