public class AmountToggleTab : TabGroup
{
    private static int[] amounts = { 1, 5, 10, 0, -1 };
    public override void RaiseEvents(int index)
    {
        if (index == 3)
        {
            //TODO ask for custom amount
        }
        else
        {
            EventManager.Instance.BankAmountChanged(amounts[index]);
        }
    }
}
