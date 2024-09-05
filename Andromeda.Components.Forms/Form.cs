using Andromeda.Components.Forms.Abstractions;
using Andromeda.Components.Forms.Extensions;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace Andromeda.Components.Forms
{
    public class Form : ReactiveObject, IForm
    {
        public Form(FormState initState)
        {
            State = initState;

            var stateObservable = this
                .WhenAnyValue(x => x.State);

            _formFieldsCache = new(x => x.PropertyName);

            _formFieldsCache.AddOrUpdate(
                GetType()
                    .GetProperties()
                    .Where(pi => pi.IsFormField())
                    .Select(pi => new FormFieldInfo(
                        pi.Name,
                        pi.PropertyType,
                        pi.Order(),
                        pi.Label(),
                        pi.Hint(),
                        stateObservable.Select(s => pi.CanRead(s)),
                        stateObservable.Select(s => pi.CanWrite(s)),
                        pi.DataType()
                    ))
            );

            _formFieldsCache
                .Connect()
                .SortBy(x => x.Order)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _formFields)
                .Subscribe();
        }

        [Reactive]
        public string? Header { get; set; }

        [Reactive]
        public int Order { get; set; }

        [Reactive]
        public FormState State { get; set; }

        private readonly SourceCache<IFormFieldInfo, string> _formFieldsCache;
        private readonly ReadOnlyObservableCollection<IFormFieldInfo> _formFields;
        public IEnumerable<IFormFieldInfo> FormFields => _formFields;
    }
}
