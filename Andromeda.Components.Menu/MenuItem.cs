using Andromeda.Components.Menu.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Andromeda.Components.Menu
{
    public class MenuItem : ReactiveObject, IMenuItem
    {
        public MenuItem(string guid, int sortKey)
        {
            Guid = guid;
            SortKey = sortKey;
        }

        public string Guid { get; }

        [Reactive]
        public int SortKey {  get; set; }
    }
}
