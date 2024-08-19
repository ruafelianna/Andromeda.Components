using System.Collections.Generic;

namespace Andromeda.Components.Menu.Abstractions
{
    public interface IMenuSector : IMenuItem
    {
        IEnumerable<IMenuItem> MenuItems { get; }

        public void AddMenuItem(IMenuItem item);

        public void AddMenuItems(IEnumerable<IMenuItem> items);

        public void RemoveMenuItem(IMenuItem item);

        public void RemoveMenuItems(IEnumerable<IMenuItem> items);
    }
}
