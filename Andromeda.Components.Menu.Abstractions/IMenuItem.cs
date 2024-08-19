namespace Andromeda.Components.Menu.Abstractions
{
    public interface IMenuItem
    {
        string Guid { get; }

        int SortKey { get; }
    }
}
