using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

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

    private void OnEnable()
    {
        EventManager.Instance.ActivatedBattle += OnBattleActivated;
    }

    private void OnDisable()
    {
        EventManager.Instance.ActivatedBattle -= OnBattleActivated;
    }

    private void Activate()
    {
        normalCamera.SetActive(false);
        combatCamera.SetActive(true);
        crossfade.SetTrigger("StarTranstion");
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }


    private void OnBattleActivated(bool activate)
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
        if(cont == 0)
        {
            GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
            playerUnit = playerGO.GetComponent<Unit2>();
            cont++;
        }
        

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit2>();

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
        miss = Random.Range(0, 101);
        Debug.Log(miss);
        if(miss > 80)
        {
            dialogueText.text = "You missed!";
        }
        else
        {
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
        Debug.Log(miss);
        if (miss > 50)
        {
            dialogueText.text = enemyUnit.unitName + " missed!";
        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " attacks!";
            yield return new WaitForSeconds(1f);

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

    IEnumerator PlayerHeal()
    {
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

        StartCoroutine(PlayerHeal());
    }

    private IEnumerator VictoryToGameCore()
    {
        yield return new WaitForSeconds(2f);
        EventManager.Instance.DeativateBattleCam();
    }
    private IEnumerator DefeatToGameOverScreen()
    {
        yield return new WaitForSeconds(2f);
    }

}
