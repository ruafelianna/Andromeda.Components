using Andromeda.Components.Menu.Abstractions;
using ReactiveUI.Fody.Helpers;

namespace Andromeda.Components.Menu
{
    public class MenuEndpoint : MenuItem, IMenuEndpoint
    {
        public MenuEndpoint(
            string guid, int sortKey, object? content
        ) : base(guid, sortKey)
        {
            Content = content;
        }

        [Reactive]
        public object? Content { get; set; }
    }
}
