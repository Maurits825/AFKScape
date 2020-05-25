public class Vorkath : Monster
{
    public Vorkath()
        : base("Vorkath")
    {
        GetDropTableHandler(bossName);
        dropTableDict = CreateDropTableDictionary(null);
    }
}
