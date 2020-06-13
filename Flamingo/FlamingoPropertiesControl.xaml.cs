namespace Flamingo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for FlamingoPropertiesControl.
    /// </summary>
    public partial class FlamingoPropertiesControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlamingoPropertiesControl"/> class.
        /// </summary>
        public FlamingoPropertiesControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]        

        private void btnOpenInputFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogBox = new Microsoft.Win32.OpenFileDialog();

            dialogBox.DefaultExt = ".xml";
            dialogBox.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            
            Nullable<bool> result = dialogBox.ShowDialog();

            if (result == true)
            {                
                string inputFilename = dialogBox.FileName;
                txtInputFile.Text = inputFilename;
                Settings.Default.Input = inputFilename;
            }
        }

        private void btnOpenOutputFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogBox = new Microsoft.Win32.OpenFileDialog();

            dialogBox.DefaultExt = ".xml";
            dialogBox.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            Nullable<bool> result = dialogBox.ShowDialog();

            if (result == true)
            {
                string outputFilename = dialogBox.FileName;
                txtOutputFile.Text = outputFilename;
                Settings.Default.Output = outputFilename;
            }
        }
    }
}