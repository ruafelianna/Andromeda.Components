using ReactiveUI;
using System.Collections.Generic;

namespace Andromeda.Components.Forms.Abstractions
{
    public interface IFormGroup : IReactiveObject
    {
        IEnumerable<IForm> Forms { get; }

        public void AddForm(IForm form);

        public void AddForms(IEnumerable<IForm> forms);

        public void RemoveForm(IForm form);

        public void RemoveForms(IEnumerable<IForm> forms);
    }
}
