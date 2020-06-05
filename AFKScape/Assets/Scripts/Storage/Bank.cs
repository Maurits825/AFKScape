using System.Numerics;

public class Bank : Storage
{
    public int amount = 1;
    public bool isActive = false;

    public Bank()
    {
        totalSlots = 5000;
        EventManager.Instance.OnBankActiveChanged += SetActive;
        EventManager.Instance.OnBankAmountChanged += SetAmount;
    }

    public override void RaiseItemAddedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
        EventManager.Instance.BankItemAdded(id, amount, amounDiff);
    }

    public override void RaiseItemRemovedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
        EventManager.Instance.BankItemRemoved(id, amount, amounDiff);
    }

    private void SetActive(bool state)
    {
        isActive = state;
    }

    private void SetAmount(int value)
    {
        amount = value;
    }
}
