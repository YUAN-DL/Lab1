using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataLibrary;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    class MyTextshow : INotifyPropertyChanged
    {
        public string show;//显示
        public event PropertyChangedEventHandler PropertyChanged;
        public string Show
        {
            get { return show; }
            set
            {
                show = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Show"));
            }
        }
    }

    public partial class MainWindow : Window
    {
        ViewData viewData = new ViewData();
        VMf vMf = new VMf();
        VMGird vMGird = new VMGird();
        VMBenchmark vMBenchmark = new VMBenchmark();
        public MainWindow()
        {
            InitializeComponent();
            grid_sample.DataContext = viewData.vMBenchmark.vMtimes;
            grid_VMA.DataContext = viewData.vMBenchmark.vMAccuracies;
        }


        private bool UnsavedChanges()
        {
            MessageBoxResult mes = MessageBox.Show("Do you want to save the data as a file?", "Save", MessageBoxButton.YesNoCancel);
            if (mes == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "filename";
                dlg.DefaultExt = ".txt";
                dlg.Filter = "TXTFiles(.txt)|*.txt";
                if ((bool)dlg.ShowDialog())
                    viewData.Save(dlg.FileName);
            }
            else if (mes == MessageBoxResult.Cancel)
                return true;
            return false;
        }
        private void New_btn_Click(object sender, RoutedEventArgs e)
        {
            if (viewData.CollectionChangedAfterSave)
                UnsavedChanges();
            viewData = new ViewData();
            grid_sample.DataContext = viewData.vMBenchmark.vMtimes;
            grid_VMA.DataContext = viewData.vMBenchmark.vMAccuracies;
            tname.Text = "";
            textBox.Text = "";
            MessageError();
        }
        private void Open_btn_Click(object sender, RoutedEventArgs e)
        {
            if (viewData.CollectionChangedAfterSave)
                UnsavedChanges();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if ((bool)dlg.ShowDialog())
            {
                viewData = new ViewData();
                viewData.Load(dlg.FileName);
                grid_sample.DataContext = viewData.vMBenchmark.vMtimes;
                grid_VMA.DataContext = viewData.vMBenchmark.vMAccuracies;
            }
            MessageError();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Inf";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXTFiles(.txt)|*.txt";
            if ((bool)dlg.ShowDialog())
                viewData.Save(dlg.FileName);
            MessageError();
        }
        private void AddVMtime_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] words = tname.Text.Split(';');
                vMGird.Left_endpoint = float.Parse(words[0]);
                vMGird.Right_endpoint = float.Parse(words[1]);
                vMGird.Length_vector = int.Parse(textBox.Text);
                if (ComboBox1.Text == "VMSLN and VMDLN")
                {
                    vMf = VMf.VMSLN_VMDLN;
                }
                else if (ComboBox1.Text == "VMSLGamma and VMDLGamma")
                {
                    vMf = VMf.VMSLGamma_VMDLGamma;
                }
                viewData.AddVMtime(vMf, vMGird);
                information_block.Text = DateTime.Now + "\n" + "Successfully added a new element VMtime";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Формат ввода не верный");
            }
            finally
            {

            }
        }

        private void AddVMAccuracy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] words = tname.Text.Split(';');
                vMGird.Left_endpoint = float.Parse(words[0]);
                vMGird.Right_endpoint = float.Parse(words[1]);
                vMGird.Length_vector = int.Parse(textBox.Text);
                if (ComboBox1.Text == "VMSLN and VMDLN")
                {
                    vMf = VMf.VMSLN_VMDLN;
                }
                else if (ComboBox1.Text == "VMSLGamma and VMDLGamma")
                {
                    vMf = VMf.VMSLGamma_VMDLGamma;
                }
                viewData.AddVMAccuracy(vMf, vMGird);
                information_block.Text = DateTime.Now + "\n" + "Successfully added a new element VMAccuracy";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Формат ввода не верный");
            }
            finally
            {

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs cl)
        {
            if (viewData.CollectionChangedAfterSave)
                UnsavedChanges();
            MessageError();
        }


        public void MessageError()
        {
            if (viewData.ErrorMessage != null)
            {
                MessageBox.Show(viewData.ErrorMessage, "Error");
                viewData.ErrorMessage = null;
            }
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListBox1.SelectedItem != null)
                    Block1.Text = ListBox1.SelectedItem.ToString();
                if (ListBox2.SelectedItem != null)
                {
                    Max_error_block.Text = ListBox2.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Element is empty");
            }
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {



        }
        private void TextBox_Leave(object sender, System.EventArgs e)
        {
            // Reset the colors and selection of the TextBox after focus is lost.

        }
        private void tname_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
