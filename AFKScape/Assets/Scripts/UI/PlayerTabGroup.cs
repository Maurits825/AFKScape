using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTabGroup : TabGroup
{
    public override void RaiseEvents(int index)
    {
        EventManager.Instance.PlayerTabClicked(index);
    }
}
