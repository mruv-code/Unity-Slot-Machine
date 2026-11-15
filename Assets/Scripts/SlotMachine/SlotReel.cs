
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotReel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlotSymbolData symbolData;
    [SerializeField] private GameObject symbolPrefab;
    [SerializeField] private RectTransform symbolsContent;

    [Header("Symbol Layout")]
    [SerializeField] private float symbolSize = 80f;
    [SerializeField] private float symbolSpacing = 12f;
    [SerializeField] private int symbolCount = 11;

    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 900f;
    [SerializeField] private float stopDuration = 0.8f;

    private readonly List<Image> symbolImages = new List<Image>();

    private float symbolStep;
    private float boundary;
    private float totalHeight;

    private bool isInitialized;

    private void Start()
    {
        InitializeReel();
    }

    public void InitializeReel()
    {
        if (symbolData == null || symbolData.symbols == null ||
            symbolData.symbols.Length == 0 ||
            symbolPrefab == null || symbolsContent == null)
        {
            Debug.LogError("Reel references are missing!", this);
            return;
        }

        symbolCount = Mathf.Max(5, symbolCount);

        // Use an odd number so one symbol can sit exactly in the center.
        if (symbolCount % 2 == 0)
            symbolCount++;

        symbolStep = symbolSize + symbolSpacing;
        totalHeight = symbolStep * symbolCount;
        boundary = totalHeight / 2f;

        foreach (Transform child in symbolsContent)
            Destroy(child.gameObject);

        symbolImages.Clear();

        symbolsContent.anchorMin = new Vector2(0.5f, 0.5f);
        symbolsContent.anchorMax = new Vector2(0.5f, 0.5f);
        symbolsContent.pivot = new Vector2(0.5f, 0.5f);
        symbolsContent.anchoredPosition = Vector2.zero;
        symbolsContent.sizeDelta =
            new Vector2(symbolSize, totalHeight);

        for (int i = 0; i < symbolCount; i++)
        {
            GameObject newSymbol = Instantiate(
                symbolPrefab,
                symbolsContent
            );

            RectTransform rect =
                newSymbol.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            rect.sizeDelta = new Vector2(symbolSize, symbolSize);

            rect.anchoredPosition = new Vector2(
                0f,
                (i - symbolCount / 2) * symbolStep
            );

            Image image = newSymbol.GetComponent<Image>();

            if (image == null)
            {
                Debug.LogError("Symbol prefab needs an Image component!");
                Destroy(newSymbol);
                continue;
            }

            image.sprite = GetRandomSymbol().symbolSprite;
            image.preserveAspect = true;
            image.raycastTarget = false;

            symbolImages.Add(image);
        }

        isInitialized = symbolImages.Count == symbolCount;
    }

    private SlotSymbol GetRandomSymbol()
    {
        int index = Random.Range(0, symbolData.symbols.Length);
        return symbolData.symbols[index];
    }

    public IEnumerator SpinTo(SlotSymbol winningSymbol, float spinDuration)
    {
        if (!isInitialized || winningSymbol == null)
            yield break;

        float elapsed = 0f;

        // Continuous spinning.
        while (elapsed < spinDuration)
        {
            MoveSymbols(spinSpeed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Gradually slow down.
        float stopTimer = 0f;

        while (stopTimer < stopDuration)
        {
            stopTimer += Time.deltaTime;

            float progress = Mathf.Clamp01(stopTimer / stopDuration);

            float currentSpeed = Mathf.Lerp(
                spinSpeed,
                0f,
                progress * progress
            );

            MoveSymbols(currentSpeed * Time.deltaTime);

            yield return null;
        }

        // Align all symbols into evenly spaced positions.
        AlignSymbols();

        // Select the center symbol as the final result.
        int centerIndex = symbolCount / 2;

        symbolImages[centerIndex].sprite =
            winningSymbol.symbolSprite;

        Debug.Log("Reel stopped on: " + winningSymbol.symbolName);
    }

    private void MoveSymbols(float movement)
    {
        foreach (Image image in symbolImages)
        {
            RectTransform rect =
                image.GetComponent<RectTransform>();

            Vector2 position = rect.anchoredPosition;

            position.y += movement;

            // Move symbols that pass the top back to the bottom.
            if (position.y >= boundary)
                position.y -= totalHeight;

            rect.anchoredPosition = position;
        }
    }

    private void AlignSymbols()
    {
        // Sort symbols from bottom to top.
        symbolImages.Sort((a, b) =>
        {
            float yA = a.GetComponent<RectTransform>()
                .anchoredPosition.y;

            float yB = b.GetComponent<RectTransform>()
                .anchoredPosition.y;

            return yA.CompareTo(yB);
        });

        // Reposition them with equal spacing.
        for (int i = 0; i < symbolImages.Count; i++)
        {
            RectTransform rect =
                symbolImages[i].GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(
                0f,
                (i - symbolCount / 2) * symbolStep
            );

            rect.SetSiblingIndex(i);
        }
    }
}
