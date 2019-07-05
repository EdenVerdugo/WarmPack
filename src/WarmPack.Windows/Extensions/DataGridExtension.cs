using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace WarmPack.Windows.Extensions
{
    public static class DataGridExtension
    {
        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static DataGridCell GetCell(this DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }

        public static void EditCell(this DataGrid dataGrid, int column)
        {
            if (dataGrid.SelectedItem != null)
            {
                var row = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem) as DataGridRow;
                if (row == null)
                {
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                    WaitFor(TimeSpan.Zero, DispatcherPriority.SystemIdle);

                    row = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem) as DataGridRow;
                }
                if (row != null)
                {
                    DataGridCell cell = GetCell(dataGrid, row, column);
                    if (cell != null)
                    {
                        cell.Focus();
                        dataGrid.BeginEdit();
                    }
                }
            }
        }

        private static void WaitFor(TimeSpan time, DispatcherPriority priority)
        {
            DispatcherTimer timer = new DispatcherTimer(priority);
            timer.Tick += new EventHandler(OnDispatched);
            timer.Interval = time;
            DispatcherFrame dispatcherFrame = new DispatcherFrame(false);
            timer.Tag = dispatcherFrame;
            timer.Start();
            Dispatcher.PushFrame(dispatcherFrame);
        }

        private static void OnDispatched(object sender, EventArgs args)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Tick -= new EventHandler(OnDispatched);
            timer.Stop();
            DispatcherFrame frame = (DispatcherFrame)timer.Tag;
            frame.Continue = false;
        }
    }
}
