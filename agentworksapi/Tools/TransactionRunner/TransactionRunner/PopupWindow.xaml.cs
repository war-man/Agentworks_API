using System;
using System.ComponentModel;
using System.Windows;
using TransactionRunner.ViewModels;

namespace TransactionRunner
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow
    {
        private readonly IPopupViewModel _viewModel;

        private PopupWindow(IPopupViewModel viewModel, Size size)
        {
            InitializeComponent();
            _viewModel = viewModel;

            _viewModel.RequestClose += ViewModel_RequestClose;
            DataContext = _viewModel;
            Owner = Application.Current.MainWindow;

            if (size != default(Size))
            {
                SizeToContent = SizeToContent.Manual;
                Width = size.Width;
                Height = size.Height;
                ResizeMode = ResizeMode.CanResize;
            }
            else
            {
                SizeToContent = SizeToContent.WidthAndHeight;
                ResizeMode = ResizeMode.NoResize;
            }
        }

        private void ViewModel_RequestClose(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Called on OnClosed event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            _viewModel.RequestClose -= ViewModel_RequestClose;
        }

        /// <summary>
        /// Shows ViewModel as dialog with option to gather result
        /// </summary>
        /// <typeparam name="TViewModel">Type of ViewModel</typeparam>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="viewModel">ViewModel instance</param>
        /// <param name="getResult">Expression to get result from ViewModel</param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static TResult ShowDialogWithResult<TViewModel, TResult>(TViewModel viewModel, Func<TViewModel, TResult> getResult, Size size = default(Size)) where TViewModel: IPopupViewModel
        {
            PopupWindow window = new PopupWindow(viewModel, size);
            window.ShowDialog();
            return getResult(viewModel);
        }

        /// <summary>
        /// Shows ViewModel as dialog
        /// </summary>
        /// <typeparam name="TViewModel">Type of ViewModel</typeparam>
        /// <param name="viewModel">ViewModel instance</param>
        /// <param name="size"></param>
        public static void ShowDialog<TViewModel>(TViewModel viewModel, Size size = default(Size)) where TViewModel : IPopupViewModel
        {
            PopupWindow window = new PopupWindow(viewModel, size);
            window.ShowDialog();
        }

        private void PopupWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialize();
        }

        private void PopupWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !_viewModel.CanClose();
        }
    }
}