using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NewsParser.Core
{
    internal abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public virtual void Set<T>(ref T field, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(field, value)) throw new ArgumentNullException(nameof(field));
            OnPropertyChanged(PropertyName);
            field = value;
        }
    }
}
