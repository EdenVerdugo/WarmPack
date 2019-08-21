using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WarmPack.DataModel;
using WarmPack.Extensions;
using WarmPack.Helpers;

namespace WarmPack.Windows.Search
{
    internal class SearcherDataBinding
    {
        public string PropertyName { get; set; }
        public string HeaderName { get; set; }
        public string StringFormat { get; set; }
        public SearcherTextAlignment TextAlignment { get; set; }
    }

    public enum SearcherTextAlignment
    {
        Center = TextAlignment.Center,
        Justify = TextAlignment.Justify,
        Left = TextAlignment.Left,
        Right = TextAlignment.Right
    }

    public enum SearcherHorizontalContentAligment
    {
        Center = HorizontalAlignment.Center,
        Left = HorizontalAlignment.Left,
        Right = HorizontalAlignment.Right,
        Stretch = HorizontalAlignment.Stretch
    }

    public enum SearcherVerticarContentAligment
    {
        Center = VerticalAlignment.Center,
        Top = VerticalAlignment.Top,
        Bottom = VerticalAlignment.Bottom,
        Stretch = HorizontalAlignment.Stretch
    }

    public enum SearchMode
    {
        OnPressEnter,
        OnPressKey
    }

    public class Searcher<T> : ONotifyPropertyChanged<Searcher<T>>
    {        
        private List<SearcherDataBinding> _DataGridColumnsNamesBinding = null;
        private bool _firstShow = true;


        private readonly Func<string, IEnumerable<T>> _SearchAction = null;
        private string _LastSearch = null;


        private bool _IsSearching;
        public bool IsSearching
        {
            get
            {
                return _IsSearching;
            }
            set
            {
                _IsSearching = value;
                OnPropertyChanged(s => s.IsSearching);
            }
        }

        private SearchMode _SearchMode;
        public SearchMode SearchMode
        {
            get
            {
                return _SearchMode;
            }
            set
            {
                _SearchMode = value;
                OnPropertyChanged(s => s.SearchMode);
            }
        }


        private bool _SearchTextIsEnabled;
        public bool SearchTextIsEnabled
        {
            get
            {
                return _SearchTextIsEnabled;
            }
            set
            {
                _SearchTextIsEnabled = value;
                OnPropertyChanged(s => s.SearchTextIsEnabled);
            }
        }

        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                _SearchText = value;
                OnPropertyChanged(s => s.SearchText);

                if (SearchMode == SearchMode.OnPressKey)
                    _Search();
            }
        }

        private T _CurrentItem;
        public T CurrentItem
        {
            get
            {
                return _CurrentItem;
            }
            set
            {
                _CurrentItem = value;
                OnPropertyChanged(s => s.CurrentItem);
            }
        }

        private ObservableCollection<T> _ItemsDataSource;
        public ObservableCollection<T> ItemsDataSource
        {
            get
            {
                return _ItemsDataSource;
            }
            set
            {
                _ItemsDataSource = value;
                OnPropertyChanged(s => s.ItemsDataSource);
            }
        }

        SearcherView _View = null;

        private Window _Owner;
        public Window Owner
        {
            get
            {
                return _Owner;
            }
            set
            {
                _Owner = value;

                OnPropertyChanged(p => p.Owner);
            }
        }

        public Searcher(Func<string, IEnumerable<T>> searchFunction)
        {
            _SearchAction = searchFunction;

            _ItemsDataSource = new ObservableCollection<T>();
            SearchMode = SearchMode.OnPressEnter;

            //_UIColumns = new List<Microsoft.Windows.Controls.DataGridColumn>();
            _DataGridColumnsNamesBinding = new List<SearcherDataBinding>();

            SearchTextIsEnabled = true;
        }


        public void AddColumnToShow(Expression<Func<T, object>> column)
        {
            AddColumnToShow(column, string.Empty, string.Empty, SearcherTextAlignment.Left);
        }

        public void AddColumnToShow(Expression<Func<T, object>> column, SearcherTextAlignment textAlignment)
        {
            AddColumnToShow(column, string.Empty, string.Empty, textAlignment);
        }

        public void AddColumnToShow(Expression<Func<T, object>> column, string columnHeaderName)
        {
            AddColumnToShow(column, columnHeaderName, string.Empty, SearcherTextAlignment.Left);
        }

        public void AddColumnToShow(Expression<Func<T, object>> column, string columnHeaderName, SearcherTextAlignment textAlignment)
        {
            AddColumnToShow(column, columnHeaderName, string.Empty, textAlignment);
        }

        public void AddColumnToShow(Expression<Func<T, object>> column, string columnHeaderName, string stringFormat)
        {
            AddColumnToShow(column, columnHeaderName, stringFormat, SearcherTextAlignment.Left);
        }

        public void AddColumnToShow(Expression<Func<T, object>> column, string columnHeaderName, string stringFormat, SearcherTextAlignment textAlignment)
        {
            var propertyName = ExpressionsHelper.GetPropertyName<T>(column);

            var columnName = string.IsNullOrEmpty(columnHeaderName) ? propertyName : columnHeaderName;

            var binding = new SearcherDataBinding()
            {
                HeaderName = columnName,
                PropertyName = propertyName,
                StringFormat = stringFormat,
                TextAlignment = textAlignment
            };

            _DataGridColumnsNamesBinding.Add(binding);
            //var col = new Microsoft.Windows.Controls.DataGridTextColumn()
            //{
            //    Header = columnName,
            //    IsReadOnly = true,
            //    Binding = new Binding(propertyName)                
            //};
            //col.Binding.StringFormat = stringFormat;

            ////_View.SearchItemsDataGridView.Columns.Add(col);
            //_UIColumns.Add(col);
        }

        private void _Search()
        {
            if (IsSearching)
                return;

            SearchTextIsEnabled = false;

            WarmPack.Threading.Task.RunTask(() =>
            {
                CurrentItem = default(T);
                IsSearching = true;

                if (_LastSearch != SearchText)
                {
                    ItemsDataSource = null;

                    ItemsDataSource = _SearchAction(SearchText).ToObservableCollection();
                    _LastSearch = SearchText;
                }
            })
            .Completed(() =>
            {
                _View.Dispatcher.Invoke(new Action(() =>
                {
                    _View.SearchItemsDataGridView.ItemsSource = ItemsDataSource;

                    if (ItemsDataSource?.Count == 1 && _firstShow)
                    {
                        CurrentItem = ItemsDataSource.FirstOrDefault();
                        _View.Close();
                    }

                    if (_firstShow)
                    {
                        _firstShow = false;
                        _View.Opacity = 100;
                        _View.SearchText.SelectionStart = SearchText.Length;
                    }

                    SearchTextIsEnabled = true;
                    IsSearching = false;

                }));
            });

        }

        private void SetKeyBindings()
        {
            var enterKeyBinding = new KeyBinding();
            enterKeyBinding.Command = UIEnterCommand;
            enterKeyBinding.Key = Key.Enter;

            var downKeyBinding = new KeyBinding();
            downKeyBinding.Command = UIDownCommand;
            downKeyBinding.Key = Key.Down;

            var upKeyBinding = new KeyBinding();
            upKeyBinding.Command = UIUpCommand;
            upKeyBinding.Key = Key.Up;

            var escKeyBinding = new KeyBinding();
            escKeyBinding.Command = CloseCommand;
            escKeyBinding.Key = Key.Escape;

            _View.SearchText.InputBindings.Add(enterKeyBinding);
            _View.SearchText.InputBindings.Add(downKeyBinding);
            _View.SearchText.InputBindings.Add(upKeyBinding);

            _View.InputBindings.Add(escKeyBinding);
            _View.SearchItemsDataGridView.InputBindings.Add(enterKeyBinding);
        }


        public T Search(string searchText)
        {
            _firstShow = true;

            _View = new SearcherView();
            _View.Owner = Owner;
            _View.DataContext = this;
            _View.Topmost = true;

            SetKeyBindings();

            int index = 0;

            foreach (var col in _DataGridColumnsNamesBinding)
            {
                var style = new Style();
                style.Setters.Add(new Setter(System.Windows.Controls.TextBlock.TextAlignmentProperty, (TextAlignment)col.TextAlignment));

                var headerStyle = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));

                var hAligment = new HorizontalAlignment();                
                switch (col.TextAlignment)
                {
                    case SearcherTextAlignment.Left:
                        hAligment = HorizontalAlignment.Left;
                        break;
                    case SearcherTextAlignment.Right:
                        hAligment = HorizontalAlignment.Right;
                        break;
                    case SearcherTextAlignment.Center:
                        hAligment = HorizontalAlignment.Center;
                        break;
                    case SearcherTextAlignment.Justify:
                        hAligment = HorizontalAlignment.Stretch;
                        break;                    
                }                

                headerStyle.Setters.Add(new Setter(System.Windows.Controls.Control.HorizontalContentAlignmentProperty, hAligment));

                var datagridCol = new System.Windows.Controls.DataGridTextColumn()
                {
                    Header = col.HeaderName,
                    IsReadOnly = true,
                    Binding = new Binding(col.PropertyName)
                };
                datagridCol.Binding.StringFormat = col.StringFormat;
                datagridCol.CellStyle = style;
                datagridCol.HeaderStyle = headerStyle;

                if (index == (_DataGridColumnsNamesBinding.Count - 1))
                    datagridCol.Width = new System.Windows.Controls.DataGridLength(100, System.Windows.Controls.DataGridLengthUnitType.Star);

                _View.SearchItemsDataGridView.Columns.Add(datagridCol);

                index++;
            }

            this.SearchText = searchText;

            _Search();

            _View.AllowsTransparency = true;
            _View.Opacity = 0;
            //_View.ShowInTaskbar = false;

            _View.ShowDialog();

            return CurrentItem;
        }


        private System.Windows.Input.ICommand _CloseCommand;
        public System.Windows.Input.ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new RelayCommand(() => Close());
                }

                return _CloseCommand;
            }
        }

        private void Close()
        {
            CurrentItem = default(T);
            _View.Close();
        }



        private System.Windows.Input.ICommand _UIEnterCommand;
        public System.Windows.Input.ICommand UIEnterCommand
        {
            get
            {
                if (_UIEnterCommand == null)
                {
                    _UIEnterCommand = new RelayCommand<Object>(p => EnterCommand(p));
                }

                return _UIEnterCommand;
            }
        }

        private void EnterCommand(Object o)
        {
            if (IsSearching)
                return;

            if (_SearchText != _LastSearch)
            {
                _Search();
            }
            else
            {
                if (_CurrentItem != null)
                    _View.Close();
            }
        }



        private System.Windows.Input.ICommand _UIUpCommand;
        public System.Windows.Input.ICommand UIUpCommand
        {
            get
            {
                if (_UIUpCommand == null)
                {
                    _UIUpCommand = new RelayCommand<Object>(p => UpCommand(p));
                }

                return _UIUpCommand;
            }
        }

        private void UpCommand(Object o)
        {
            if (IsSearching)
                return;


            ////acciones
            //var index = ItemsDataSource.IndexOf(CurrentItem);
            ////index = index < 0 ? 0 : index;

            //if (index > 0)
            //    this.CurrentItem = ItemsDataSource[index - 1];            

            if (_View.SearchItemsDataGridView.SelectedIndex > 0)
                _View.SearchItemsDataGridView.SelectedIndex = _View.SearchItemsDataGridView.SelectedIndex - 1;

            _View.SearchItemsDataGridView?.ScrollIntoView(_View.SearchItemsDataGridView.SelectedItem);
        }


        private System.Windows.Input.ICommand _UIDownCommand;
        public System.Windows.Input.ICommand UIDownCommand
        {
            get
            {
                if (_UIDownCommand == null)
                {
                    _UIDownCommand = new RelayCommand<Object>(p => DownCommand(p));
                }

                return _UIDownCommand;
            }
        }

        private void DownCommand(Object o)
        {
            if (IsSearching)
                return;

            //var index = ItemsDataSource.IndexOf(CurrentItem);
            ////index = index < 0 ? 0 : index;

            //if (index < (ItemsDataSource.Count - 1))
            //    this.CurrentItem = ItemsDataSource[index + 1];

            if (_View.SearchItemsDataGridView.SelectedIndex < (_View.SearchItemsDataGridView.Items?.Count - 1))
                _View.SearchItemsDataGridView.SelectedIndex = _View.SearchItemsDataGridView.SelectedIndex + 1;

            _View.SearchItemsDataGridView?.ScrollIntoView(_View.SearchItemsDataGridView.SelectedItem);
        }
    }
}
