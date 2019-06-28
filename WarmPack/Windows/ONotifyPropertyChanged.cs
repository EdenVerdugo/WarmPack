using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Windows
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
