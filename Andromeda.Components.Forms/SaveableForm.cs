using Andromeda.Components.Forms.Abstractions;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Andromeda.Components.Forms.Consts;

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
            _selfType = This.GetType();

            _propertiesWithDefaults = FormFields
                .Select(x => new PropertyInfo?[] {
                    _selfType.GetProperty(x.PropertyName),
                    _selfType.GetProperty(
                        $"{DefaultPrefix}{(x.PropertyName
                            .StartsWith(NumberPrefix)
                                ? x.PropertyName[NumberPrefix.Length..]
                                : x.PropertyName
                        )}",
                        BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.NonPublic
                    )
                })
                .Where(x => x[1] is not null);

            IsChanged = this
                .WhenAnyPropertyChanged(_propertiesWithDefaults
                    .SelectMany(x => x)
                    .Select(x => x!.Name)
                    .ToArray()
                )
                .Select(vm => {
                    foreach (var prop in _propertiesWithDefaults)
                    {
                        var (val, def) = GetProps(prop!);

                        if (val != def)
                        {
                            return true;
                        }
                    }

                    return false;
                });

            var canSaveSources = new List<IObservable<bool>>(3)
            {
                ValidationContext.Valid,
                IsChanged
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

        public IObservable<bool> IsChanged { get; }

        public IReactiveCommand Save { get; }

        protected TSelf This => (TSelf)this;

        private readonly IEnumerable<PropertyInfo?[]> _propertiesWithDefaults;

        private readonly Type _selfType;

        protected void InitDefaultsAuto()
        {
            foreach (var prop in _propertiesWithDefaults)
            {
                var type = prop[0]!.PropertyType;
                prop[1]!.SetValue(This, type.IsValueType
                    ? Activator.CreateInstance(type)
                    : null
                );
            }
        }

        protected void SetDefaultsAuto()
        {
            foreach (var prop in _propertiesWithDefaults)
            {
                var (_, def) = GetProps(prop!);

                prop[0]!.SetValue(This, def);
            }
        }

        private (object? val, object? def) GetProps(
            PropertyInfo[] pi
        )
        {
            object? GetPropVal(int index)
                => pi[index]!.GetValue(This);

            return (GetPropVal(0), GetPropVal(1));
        }
    }
}
