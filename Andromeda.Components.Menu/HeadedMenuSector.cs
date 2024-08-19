using Andromeda.Components.Menu.Abstractions;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;

namespace Andromeda.Components.Menu
{
    public class HeadedMenuSector : MenuSector, IHeadedMenuSector
    {
        public HeadedMenuSector(
            string guid,
            int sortKey,
            string header,
            IEnumerable<IMenuItem> items
        ) : base(guid, sortKey, items)
        {
            Header = header;
        }

        [Reactive]
        public string Header { get; set; }
    }
}
