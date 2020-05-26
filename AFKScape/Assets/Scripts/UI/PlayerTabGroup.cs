public class PlayerTabGroup : TabGroup
{
    public override void RaiseEvents(int index)
    {
        EventManager.Instance.PlayerTabClicked(index);
    }
}
