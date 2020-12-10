using System;
using System.Collections.Generic;
using System.Numerics;

public class EventManager
{
    public static EventManager Instance { get; private set; }

    //TODO this is just silly
    public static void SetIntance(EventManager inst)
    {
        Instance = inst;
    }

    //---- Skills ----
    public event Action<string> OnSkillButtonClicked;

    public void SkillButtonClicked(string skillName)
    {
        OnSkillButtonClicked?.Invoke(skillName);
    }

    public event Action<Skill> OnSkillSelected;

    public void SkillSelected(Skill skill)
    {
        OnSkillSelected?.Invoke(skill);
    }

    public event Action<List<TrainingMethod>> OnDrawTrainingMethods;

    public void DrawTrainingMethods(List<TrainingMethod> trainingMethods)
    {
        OnDrawTrainingMethods?.Invoke(trainingMethods);
    }

    public event Action<int> OnTrainingMethodClicked;

    public void TrainingMethodClicked(int index)
    {
        OnTrainingMethodClicked?.Invoke(index);
    }

    public event Action<string, int, int> OnLevelUp;

    public void LevelUp(string skillName, int lvl, int totalLvl)
    {
        OnLevelUp?.Invoke(skillName, lvl, totalLvl);
    }

    public event Action<int> OnXpGained;

    public void XpGained(int xp)
    {
        OnXpGained?.Invoke(xp);
    }

    public event Action OnSkillingStarted;

    public void SkillingStarted()
    {
        OnSkillingStarted?.Invoke();
    }

    //---- Bosses ----
    public event Action<string> OnBossClicked;

    public void BossClicked(string bossName)
    {
        OnBossClicked?.Invoke(bossName);
    }

    public event Action<int> OnBossKilled;

    public void BossKilled(int killCount)
    {
        OnBossKilled?.Invoke(killCount);
    }

    //---- Clues ----
    public event Action<string> OnClueClicked;

    public void ClueClicked(string clueName)
    {
        OnClueClicked?.Invoke(clueName);
    }

    public event Action<int> OnClueCompleted;

    public void ClueCompleted(int killCount)
    {
        OnClueCompleted?.Invoke(killCount);
    }

    //---- Inventory ----
    public event Action<long, BigInteger> OnInvItemAdded;

    public void InvItemAdded(long id, BigInteger amount)
    {
        OnInvItemAdded?.Invoke(id, amount);
    }

    public event Action<long, BigInteger> OnInvItemRemoved;

    public void InvItemRemoved(long id, BigInteger amount)
    {
        OnInvItemRemoved?.Invoke(id, amount);
    }

    //---- Bank ----
    public event Action<long, BigInteger> OnBankItemAdded;

    public void BankItemAdded(long id, BigInteger amount)
    {
        OnBankItemAdded?.Invoke(id, amount);
    }

    public event Action<long, BigInteger> OnBankItemRemoved;

    public void BankItemRemoved(long id, BigInteger amount)
    {
        OnBankItemRemoved?.Invoke(id, amount);
    }

    public event Action<bool> OnBankActiveChanged;

    public void BankActiveChanged(bool isActive)
    {
        OnBankActiveChanged?.Invoke(isActive);
    }

    public event Action<int> OnBankAmountChanged;

    public void BankAmountChanged(int amount)
    {
        OnBankAmountChanged?.Invoke(amount);
    }

    //---- Equiped items ----
    public event Action<long, Equipment.EquipmentSlot, BigInteger> OnItemEquipped;

    public void ItemEquipped(long id, Equipment.EquipmentSlot slot, BigInteger amount)
    {
        OnItemEquipped?.Invoke(id, slot, amount);
    }

    public event Action<long, Equipment.EquipmentSlot> OnItemUnEquipped;

    public void ItemUnEquipped(long id, Equipment.EquipmentSlot slot)
    {
        OnItemUnEquipped?.Invoke(id, slot);
    }

    public event Action<EquipmentStats> OnUpdateTotalEquipmentStats;

    public void UpdateTotalEquipmentStats(EquipmentStats stats)
    {
        OnUpdateTotalEquipmentStats?.Invoke(stats);
    }

    //---- Last loot ----
    public event Action<Dictionary<long, BigInteger>> OnUpdateLastLoot;

    public void UpdateLastLoot(Dictionary<long, BigInteger> items)
    {
        OnUpdateLastLoot?.Invoke(items);
    }

    //---- Slots ----
    public event Action<Slot.State, long> OnSlotClicked;

    public void SlotClicked(Slot.State state, long id)
    {
        OnSlotClicked?.Invoke(state, id);
    }

    //---- Tabs ----
    public event Action<int> OnMainTabClicked;

    public void MainTabClicked(int tabInd)
    {
        OnMainTabClicked?.Invoke(tabInd);
    }

    public event Action<int> OnPlayerTabClicked;

    public void PlayerTabClicked(int tabInd)
    {
        OnPlayerTabClicked?.Invoke(tabInd);
    }

    //---- Popup ----
    public event Action<string> OnShowPopUpMsg;

    public void ShowPopUpMsg(string msg)
    {
        OnShowPopUpMsg?.Invoke(msg);
    }
}
