using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MobileApplicationsProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        ////////////////////////string cardNamesString = "";
        StorageFolder rootFolder = ApplicationData.Current.LocalFolder; //open root folder
        StorageFile textFile;


        ///////////////List<string> cardNamesList = new List<string>();

        public MainPage()
        {
            this.InitializeComponent();
        }


        private async void fileActions(string request, string fileName)  //OPTIONS: create file, add to file,  open file, delete file, save file 
        {
            List<string> cardNamesList = new List<string>();

            //CREATE/OPEN APP FOLDER
            StorageFolder appFolder = await rootFolder.CreateFolderAsync("mtgDeckBuilder", CreationCollisionOption.OpenIfExists);
            

            if (request == "makeFile")
            {
                //CREATE FILE IN APP FOLDER
                textFile = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                testTxtBx.Text = "FILE MADE"; //++++++++++++++++++++++++++++++++++
            }
            else
            {

                //FETCH ALL FILES
                var allFiles = await appFolder.GetFilesAsync();


                try
                {

                    //FETCH WANTED FILE FROM ALL FILES
                    StorageFile wantedFile = allFiles.FirstOrDefault(x => x.Name == fileName);


                    switch (request)
                    {
                        case "deleteFile":
                            //DELETE FILE FROM FOLDER
                            await wantedFile.DeleteAsync();
                            testTxtBx.Text = "FILE DELETED"; //==========================================
                            break;
                        default: 
 
                            switch (request)
                            {

                               case "openFile":

                                    //PUT WANTED FILE CONTENTS INTO STRING
                                    String cardNamesString = await FileIO.ReadTextAsync(wantedFile);

                                    //MAKE LIST OF CARD NAMES FROM STRING
                                     cardNamesList = makeCardNamesList("a,b,c", cardNamesList); ////////////////////////cardNamesString ============================

                                    //MAKE CALLS USING CARDNAMES

                                    //POPULATE (SOMETHING?) WITH CARD DATA

                                    testTxtBx.Text = "FILE OPENED"; //++++++++++++++++++++++++++++++++++
                                    break;
                                case "addToFile": //FILE MUST BE OPENED FIRST!!! 

                                    try
                                    {
                                       
                                        //write list into string. 
                                        //write string into file 
                                        await FileIO.WriteTextAsync(textFile, testTxtBx.Text); //WRITES to file CHANGE TXT BLOCK TO LIST STRING
                                        testTxtBx.Text = "FILE APPENDED"; //++++++++++++++++++++++++++++++++++
                                    }
                                    catch
                                    {
                                        testTxtBx.Text = "A Decklist must be opened first"; //may not be error!! //=============================
                                    }

                                    break;
                                case "removeFromFile":

                                    //method for removing from premade list. passing in name of card to be removed. then call 'open file again'
                                    testTxtBx.Text = "REMOVED FROM FILE"; //++++++++++++++++++++++++++++++++++
                                                                          //fileActions("openFile", "testFile3.txt"); //++++++++++++++
                                    break;
                              }   
                        break;             
                    }
                }
                catch
                {
                    testTxtBx.Text = "ERROR: FILE NOT FOUND";
                }
                

            }




            //CREATE FILE
            //StorageFile textFile = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            //WRITE TO FILE
            //invoke method here to write list to file. 

            //////////////// await FileIO.WriteTextAsync(textFile, testTxtBx.Text.ToString()); //WRITES to file +++++++++WRITES TO FILE (TURN ON!!)


            //PUT WANTED FILE CONTENTS INTO STRING
            // message = await FileIO.ReadTextAsync(wantedFile);
            // break;


          


        }

        private List<string> makeCardNamesList(string cardNamesString, List<string> cardNamesList)
        {
            //make cardNamesList from cardNamesString 

            String [] cardNamesArray = cardNamesString.Split(',');
            foreach (string element in cardNamesArray)
            {
                cardNamesList.Add(element);      
            }

            for (int i = 0; i < cardNamesList.Count; i++) //====================================================TEST
            {
                testTxtBx2.Text += cardNamesList[i].ToString() + " "; 
            }
            
            return null;

        }

        private string addToCardNamesList(List<string> cardNamesList, string cardName)
        {

            return null;
        }

        private string makeCardNamesString(List<string> cardNamesList, string cardNamesString)
        {
            //make cardNamesString from cardNamesList

            foreach (string element in cardNamesList)
            {
                cardNamesString += element + ",";
            }

            return cardNamesString;
        }

        private string makeHttpCardName(string txtBxCardName)
        {
            //trim leading and trailing whitespace. convert to lowercase. replace spaces with hyphens
            string httpCardName = txtBxCardName.Trim().ToLower().Replace(" ", "-"); 

            testTxtBx.Text = httpCardName; /////////////=====================================================
           
            return httpCardName;
          
        }


        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {

            fileActions("openFile", "testFile3.txt"); //++++++++++++++
        }

        private void makeFileBtn_Click(object sender, RoutedEventArgs e)
        {
            fileActions("makeFile", "testFile3.txt"); //+++++++++++++++++++
        }

        private void addToFileBtn_Click(object sender, RoutedEventArgs e)
        {
            fileActions("addToFile", "testFile3.txt"); //++++++++++++++
        }

        private void deleteFileBtn_Click(object sender, RoutedEventArgs e)
        {
            fileActions("deleteFile", "testFile3.txt"); //++++++++++++++
        }


        private void deleteCardBtn_Click(object sender, RoutedEventArgs e)
        {
            testTxtBx.Text = "CARD DELETED"; //+++++++++++++++++++++++++++++++++
        }


        private async void findCardBtn_Click(object sender, RoutedEventArgs e)
        {

           
            string httpCardName = makeHttpCardName(findCardTxtBx.Text);
            findCardTxtBx.Text = String.Empty;
          

            try
            {
                findCardTxtBx.PlaceholderText = "Enter Card name";
                RootObject foundCard = await FindCards.GetCardData(httpCardName); 

                testTxtBx.Text = foundCard.editions[0].multiverse_id.ToString(); //=====================================

                string image = String.Format(foundCard.editions[0].image_url);
                showCardImg.Source = new BitmapImage(new Uri(image, UriKind.Absolute));
               
            }
            catch
            {

                findCardTxtBx.PlaceholderText = "Card not found!"; 
            }

        }

       

        private void findCardTxtBx_Tapped(object sender, TappedRoutedEventArgs e) //CHECK THIS!! ++++++++++++++++++++++++++
        {
            findCardTxtBx.Text = String.Empty;
        }

       
    }
}
