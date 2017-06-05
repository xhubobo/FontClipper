using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FontClipper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.btn_font_ori.ToolTip = "Select original font path.";
            this.btn_font_new.ToolTip = "Select new font path to save as.";
        }

        private void btn_font_ori_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".ttf";
            dialog.Filter = "font file|*.ttf";
            if (dialog.ShowDialog() == true)
            {
                this.tb_font_path_ori.Text = dialog.FileName;
                this.tb_font_path_ori.Tag = dialog.SafeFileName;
            }
        }

        private void btn_font_new_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Title = "New font";
            dialog.DefaultExt = ".ttf";
            dialog.Filter = "font file|*.ttf";
            dialog.AddExtension = true;
            if (dialog.ShowDialog() == true)
            {
                this.tb_font_path_new.Text = dialog.FileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string fontPathOri = this.tb_font_path_ori.Text;
            if (string.IsNullOrEmpty(fontPathOri))
            {
                MessageBox.Show("Please select original font!");
                return;
            }

            string fontPathNew = this.tb_font_path_new.Text;
            if (string.IsNullOrEmpty(fontPathNew))
            {
                var currentAssembly = System.Reflection.Assembly.GetEntryAssembly();
                var currentDirectory = new System.IO.FileInfo(currentAssembly.Location).DirectoryName;
                if (currentDirectory != null)
                {
                    fontPathNew = System.IO.Path.Combine(currentDirectory, this.tb_font_path_ori.Tag.ToString());
                    this.tb_font_path_new.Text = fontPathNew;
                }
                else
                {
                    MessageBox.Show("Please set new font path!");
                    return;
                }
            }

            string subsetString = this.tb_font_content.Text.Trim();
            if (string.IsNullOrEmpty(subsetString))
            {
                MessageBox.Show("Please input subset string");
                return;
            }

            try
            {
                var sfntlyFontHelper = new SfntlyFontHelper();
                sfntlyFontHelper.ClipFont(subsetString, fontPathOri, fontPathNew);
                MessageBox.Show("The new font has been generated success!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
