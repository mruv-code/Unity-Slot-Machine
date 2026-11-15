
using UnityEngine;

public class LeverAnimator : MonoBehaviour
{
    [Header("Lever Images")]
    [SerializeField] private GameObject defaultLever;
    [SerializeField] private GameObject pulledLever;

    private void Awake()
    {
        SetPulled(false);
    }

    public void SetPulled(bool pulled)
    {
        defaultLever.SetActive(!pulled);
        pulledLever.SetActive(pulled);
    }
}
