using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using System.ComponentModel;

namespace ColorSwatch
{
    [Inheritable]
    // The aspect will be applicable to types, so it will inherit from the provided TypeAspect class.
    internal class NotifyPropertyChangedAttribute : TypeAspect
    {
        public override void BuildAspect(IAspectBuilder<INamedType> builder)
        {
            // Then, we use the ImplementInterface implement the INotifyPropertyChanged interface.
            builder.Advice.ImplementInterface(builder.Target, typeof(INotifyPropertyChanged),
                OverrideStrategy.Ignore);

            // We also override the properties using the OverridePropertySetter template
            // to ensure that all change notifications are properly triggered.
            foreach (var property in builder.Target.Properties.Where(p => 
                    !p.IsAbstract && p.Writeability == Writeability.All))
            {
                builder.Advice.OverrideAccessors(property, null, nameof(this.OverridePropertySetter));
            }
        }

        // Finally we add the PropertyChanged event of the INotifyPropertyChanged interface.
        [InterfaceMember] public event PropertyChangedEventHandler? PropertyChanged;

        [Introduce(WhenExists = OverrideStrategy.Ignore)]
        protected void OnPropertyChanged(string name) =>
            this.PropertyChanged?.Invoke(meta.This, new PropertyChangedEventArgs(name));

        [Template]
        private dynamic OverridePropertySetter(dynamic value)
        {
            if (value != meta.Target.Property.Value)
            {
                meta.Proceed();
                this.OnPropertyChanged(meta.Target.Property.Name);
            }

            return value;
        }
    }
}
