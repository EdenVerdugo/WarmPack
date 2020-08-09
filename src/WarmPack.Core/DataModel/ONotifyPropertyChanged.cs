using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WarmPack.DataModel
{
    public abstract class ONotifyPropertyChanged<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(Expression<Func<T, object>> property)
        {

            var propertyName = Helpers.ExpressionsHelper.GetPropertyName<T>(property);

            OnPropertyChanged(propertyName);
        }
    }
}
