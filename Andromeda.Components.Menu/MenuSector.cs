using Andromeda.Components.Menu.Abstractions;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Andromeda.Components.Menu
{
    public class MenuSector : MenuItem, IMenuSector
    {
        public MenuSector(
            string guid, int sortKey, IEnumerable<IMenuItem> items
        ) : base(guid, sortKey)
        {
            _menuItemsCache = new(x => x.Guid);

            AddMenuItems(items);

            _menuItemsCache
                .Connect()
                .SortBy(x => x.SortKey)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _menuItems)
                .Subscribe();
        }

        private readonly SourceCache<IMenuItem, string> _menuItemsCache;
        private readonly ReadOnlyObservableCollection<IMenuItem> _menuItems;
        public IEnumerable<IMenuItem> MenuItems => _menuItems;

        public void AddMenuItem(IMenuItem item)
            => _menuItemsCache.AddOrUpdate(item);

        public void AddMenuItems(IEnumerable<IMenuItem> items)
            => _menuItemsCache.AddOrUpdate(items);

        public void RemoveMenuItem(IMenuItem item)
            => _menuItemsCache.Remove(item);

        public void RemoveMenuItems(IEnumerable<IMenuItem> items)
            => _menuItemsCache.Remove(items);
    }
}
