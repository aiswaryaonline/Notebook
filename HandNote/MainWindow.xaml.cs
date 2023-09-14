using HandNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HandNoteMainViewModel vm = this.DataContext as HandNoteMainViewModel;
            if(vm != null)
            {
                vm.LoadSettings();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            HandNoteMainViewModel vm = this.DataContext as HandNoteMainViewModel;
            if (vm != null)
            {
                vm.SaveSettings();
            }
        }
    }
}
