using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MobileApplicationsProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        string message;

        public MainPage()
        {
            this.InitializeComponent();
            
            folderTest();
        }


        private async void folderTest()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            StorageFolder myFolder = await folder.CreateFolderAsync("myFolder", CreationCollisionOption.OpenIfExists);

            StorageFile textFile = await myFolder.CreateFileAsync("textFile.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(textFile, "Message from file");

            var allFiles = await myFolder.GetFilesAsync();

            StorageFile wantedFile = allFiles.FirstOrDefault(x => x.Name == "textFile.txt");

            message = await FileIO.ReadTextAsync(wantedFile);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            testTextBox.Text = message;
        }

    }
}
