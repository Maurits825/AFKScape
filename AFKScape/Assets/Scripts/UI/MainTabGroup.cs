public class MainTabGroup : TabGroup
{
    public override void RaiseEvents(int index)
    {
        EventManager.Instance.MainTabClicked(index);
    }
}
