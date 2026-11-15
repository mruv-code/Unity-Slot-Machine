
using System.Collections;
using TMPro;
using UnityEngine;

public class SlotGameManager : MonoBehaviour
{
    [Header("Reel References")]
    [SerializeField] private SlotReel[] reels;

    [Header("Symbol Data")]
    [SerializeField] private SlotSymbolData symbolData;

    [Header("Lever Animation")]
    [SerializeField] private LeverAnimator leverAnimator;

    [Header("UI References")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text betText;
    [SerializeField] private TMP_Text winText;

    [Header("Winning Popup")]
    [SerializeField] private GameObject winPopup;
    [SerializeField] private TMP_Text popupMessage;

    [Header("Exit Popup")]
    [SerializeField] private GameObject exitPopup;

    [Header("Audio")]
    [SerializeField] private SlotAudioManager audioManager;

    [Header("Spin Settings")]
    [SerializeField] private float spinDuration = 2.5f;
    [SerializeField] private float reelStopDelay = 0.5f;

    [Header("Game Settings")]
    [SerializeField] private int startingBalance = 1000;
    [SerializeField] private int currentBet = 10;
    [SerializeField] private int minimumBet = 10;
    [SerializeField] private int maximumBet = 100;
    [SerializeField] private int betStep = 10;

    private int balance;
    private int totalWinnings;
    private bool isSpinning;

    private void Start()
    {
        balance = startingBalance;
        currentBet = Mathf.Clamp(currentBet, minimumBet, maximumBet);

        if (reels == null || reels.Length != 3)
        {
            Debug.LogError("Please assign exactly three reels!");
            return;
        }

        if (symbolData == null ||
            symbolData.symbols == null ||
            symbolData.symbols.Length == 0)
        {
            Debug.LogError("Slot Symbol Data is missing!");
            return;
        }

        UpdateUI();

        if (winText != null)
            winText.text = "GOOD LUCK!";

        Debug.Log("Slot Machine Ready!");
    }

    public void Spin()
    {
        if (isSpinning)
            return;

        if (winPopup != null && winPopup.activeSelf)
            return;

        if (exitPopup != null && exitPopup.activeSelf)
            return;

        if (balance < currentBet)
        {
            if (winText != null)
                winText.text = "NOT ENOUGH BALANCE!";

            return;
        }

        isSpinning = true;
        balance -= currentBet;

        if (audioManager != null)
            audioManager.PlaySpinSound();

        if (leverAnimator != null)
            leverAnimator.SetPulled(true);

        if (winText != null)
            winText.text = "SPINNING...";

        UpdateUI();

        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        SlotSymbol[] results = new SlotSymbol[3];

        for (int i = 0; i < reels.Length; i++)
        {
            int randomIndex = Random.Range(0, symbolData.symbols.Length);
            results[i] = symbolData.symbols[randomIndex];
        }

        for (int i = 0; i < reels.Length; i++)
        {
            StartCoroutine(
                SpinReelWithDelay(
                    reels[i],
                    results[i],
                    i * reelStopDelay
                )
            );
        }

        float totalDuration =
            spinDuration +
            (reels.Length - 1) * reelStopDelay +
            0.9f;

        yield return new WaitForSeconds(totalDuration);

        if (audioManager != null)
            audioManager.StopSpinSound();

        CheckWinningCombination(results);

        Debug.Log(
            "Final Results: " +
            results[0].symbolName + " | " +
            results[1].symbolName + " | " +
            results[2].symbolName
        );

        if (leverAnimator != null)
            leverAnimator.SetPulled(false);

        UpdateUI();
        isSpinning = false;
    }

    private IEnumerator SpinReelWithDelay(
    SlotReel reel,
    SlotSymbol result,
    float delay
)
    {
        yield return new WaitForSeconds(delay);

        yield return reel.SpinTo(result, spinDuration);

        if (audioManager != null)
            audioManager.PlayReelStop();
    }

    private void CheckWinningCombination(SlotSymbol[] results)
    {
        bool isWin =
            results[0].symbolName == results[1].symbolName &&
            results[1].symbolName == results[2].symbolName;

        if (isWin)
        {
            if (audioManager != null)
                audioManager.PlayWin();
            int payout = Mathf.RoundToInt(
                results[0].payout * (currentBet / 10f)
            );

            balance += payout;
            totalWinnings += payout;

            if (winText != null)
                winText.text = "YOU WON! +" + payout;

            if (winPopup != null)
                winPopup.SetActive(true);

            if (popupMessage != null)
                popupMessage.text = "YOU WON " + payout + " CREDITS!";

            Debug.Log("JACKPOT! YOU WON!");
            Debug.Log("Winning Symbol: " + results[0].symbolName);
            Debug.Log("Payout: " + payout);
        }
        else
        {
            if (audioManager != null)
                audioManager.PlayLose();
            if (winText != null)
                winText.text = "TRY AGAIN!";

            Debug.Log("No Win!");
        }
    }

    public void IncreaseBet()
    {
        if (isSpinning)
            return;

        currentBet = Mathf.Min(
            currentBet + betStep,
            maximumBet
        );

        UpdateUI();
    }

    public void DecreaseBet()
    {
        if (isSpinning)
            return;

        currentBet = Mathf.Max(
            currentBet - betStep,
            minimumBet
        );

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (balanceText != null)
            balanceText.text = "BALANCE: " + balance;

        if (betText != null)
            betText.text = "BET: " + currentBet;
    }

    public void OpenExitPopup()
    {
        if (isSpinning)
            return;

        if (winPopup != null && winPopup.activeSelf)
            return;

        if (exitPopup != null)
            exitPopup.SetActive(true);
    }

    public void CloseExitPopup()
    {
        if (exitPopup != null)
            exitPopup.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CloseWinPopup()
    {
        if (winPopup != null)
            winPopup.SetActive(false);
    }
}
