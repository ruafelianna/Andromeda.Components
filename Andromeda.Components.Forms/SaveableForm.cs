using Andromeda.Components.Forms.Abstractions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Andromeda.Components.Forms
{
    public abstract class SaveableForm<TSelf> :
        ValidatableForm,
        ISaveableForm
        where TSelf : SaveableForm<TSelf>
    {
        public SaveableForm(
            FormState initState,
            Func<TSelf, Task<bool>> save,
            IObservable<bool>? canSaveExt = null
        ) : base(initState)
        {
            var canSaveSources = new List<IObservable<bool>>(3)
            {
                ValidationContext.Valid,
                IsChanged.Select(x => !x)
            };

            if (canSaveExt is not null)
            {
                canSaveSources.Add(canSaveExt);
            }

            CanSave = Observable
                 .CombineLatest(canSaveSources)
                 .Select(list => list.All(x => x));

            Save = ReactiveCommand.CreateFromTask(
                async () => await save(This),
                CanSave
            );
        }

        public IObservable<bool> CanSave { get; }

        public abstract IObservable<bool> IsChanged { get; }

        public IReactiveCommand Save { get; }

        protected TSelf This => (TSelf)this;
    }
}
