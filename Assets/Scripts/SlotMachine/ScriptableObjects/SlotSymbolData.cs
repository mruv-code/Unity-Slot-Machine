
using UnityEngine;

[System.Serializable]
public class SlotSymbol
{
    public string symbolName;
    public Sprite symbolSprite;

    [Min(0)]
    public int payout;
}

[CreateAssetMenu(
    fileName = "SlotSymbolData",
    menuName = "Slot Machine/Symbol Data"
)]
public class SlotSymbolData : ScriptableObject
{
    public SlotSymbol[] symbols;
}
