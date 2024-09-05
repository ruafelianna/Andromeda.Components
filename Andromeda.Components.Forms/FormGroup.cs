using Andromeda.Components.Forms.Abstractions;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Andromeda.Components.Forms
{
    public abstract class FormGroup : ReactiveObject, IFormGroup
    {
        public FormGroup()
        {
            _formsCache = new(x => x.Order);

            _formsCache
                .Connect()
                .SortBy(x => x.Order)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _forms)
                .Subscribe();
        }

        private readonly SourceCache<IForm, int> _formsCache;
        private readonly ReadOnlyObservableCollection<IForm> _forms;
        public IEnumerable<IForm> Forms => _forms;

        public void AddForm(IForm form)
            => _formsCache.AddOrUpdate(form);

        public void AddForms(IEnumerable<IForm> forms)
            => _formsCache.AddOrUpdate(forms);

        public void RemoveForm(IForm form)
            => _formsCache.Remove(form);

        public void RemoveForms(IEnumerable<IForm> forms)
            => _formsCache.Remove(forms);
    }
}
