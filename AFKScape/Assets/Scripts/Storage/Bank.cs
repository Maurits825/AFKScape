using System.Numerics;

public class Bank : Storage
{
    public Bank()
    {
        totalSlots = 5000;
    }

    public override void RaiseItemAddedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
        EventManager.Instance.BankItemAdded(id, amount, amounDiff);
    }

    public override void RaiseItemRemovedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
        EventManager.Instance.BankItemRemoved(id, amount, amounDiff);
    }
}
