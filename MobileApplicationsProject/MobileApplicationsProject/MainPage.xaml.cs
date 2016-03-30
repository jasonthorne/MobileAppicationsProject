using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
        List<string> cardNamesList = new List<string>();
        string cardNamesString; //////////////////////////////////might not need to be here
        //List<string> cardNamesList = new List<string>();
        string fileName; 

        public MainPage()
        {
            this.InitializeComponent();
        }

        //private async void fileActions(string request, string fileName)
        private async Task<List<string>> fileActions(string request, string fileName, List<string> cardNamesList)  //OPTIONS: create file, add to file,  open file, delete file, save file 
        {

            fileName += ".txt";
           // List<string> cardNamesList = new List<string>();
            //String cardNamesString;

            //CREATE/OPEN APP FOLDER
            StorageFolder appFolder = await rootFolder.CreateFolderAsync("mtgDeckBuilder", CreationCollisionOption.OpenIfExists);


            if (request == "makeFile")
            {
                //CREATE FILE IN APP FOLDER
                textFile = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                ////////////////////String cardNamesString = await FileIO.ReadTextAsync(textFile);

                //INIT CONTENT
                await FileIO.WriteTextAsync(textFile, "testContent");

                //////////////OPEN FILE
                //////////////////////fileActions("openFile", fileName);

                //////////////testTxtBx.Text = await FileIO.ReadTextAsync(textFile); //++++++++++++++++++++++++++++++++++TEST
            }
            //else
            //{


               ///refreshFiles(); ///////////////////////DO THIS SOON!!! 

               
                //FETCH ALL FILES
                var allFiles = await appFolder.GetFilesAsync();
                
                //DISPLAY ALL FILES 
                displayFiles(allFiles); ///////////////////ONLY DISPLAYS!!!


               // try
                //{

                    //FETCH WANTED FILE FROM ALL FILES
                    StorageFile wantedFile = allFiles.FirstOrDefault(x => x.Name == fileName);


                    switch (request)
                    {
                        case "deleteFile":
                            //DELETE FILE FROM FOLDER
                            await wantedFile.DeleteAsync();


                    //FETCH ALL FILES
                    var allFiles2 = await appFolder.GetFilesAsync();//==========================================

                    //DISPLAY ALL FILES 
                    displayFiles(allFiles2); ///////////////////ONLY DISPLAYS!!!//==========================================

                    testTxtBx.Text = "FILE DELETED"; //==========================================

                            break;
                        default:

                                
                    switch (request)
                            {

                               case "openFile":

                                    //PUT WANTED FILE CONTENTS INTO STRING
                                    //String 
                                    cardNamesString = await FileIO.ReadTextAsync(wantedFile);

                                    //MAKE LIST OF CARD NAMES FROM STRING
                                    cardNamesList = makeCardNamesList(cardNamesString); //, cardNamesList); ////////////////////////cardNamesString ============================

                                    //DISPLAY CARDNAMES
                                    displayCards(cardNamesList);

                            //MAKE CALLS USING CARDNAMES


                            //POPULATE (SOMETHING?) WITH CARD DATA

                            //testTxtBx.Text = "FILE OPENED"; //++++++++++++++++++++++++++++++++++
                            break;
                                case "addToFile": //FILE MUST BE OPENED FIRST!!! 

                                    try
                                    {


                                //////////============================================TEST

                                    //add word to list
                                    addToCardNamesList("testAdd");

                                    //turn word to string 
                                    cardNamesString = makeCardNamesString();
                                    
                                    

                                //write string into file 
                                await FileIO.WriteTextAsync(textFile, cardNamesString); //WRITES to file CHANGE TXT BLOCK TO LIST STRING

                                //DISPLAY CARDNAMES
                                displayCards(cardNamesList);


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



            //}
            //catch
            //{
            //fileNameTxtBx.PlaceholderText = "File not found!";
            // }


            //} ==========ELSE




            //CREATE FILE
            //StorageFile textFile = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            //WRITE TO FILE
            //invoke method here to write list to file. 

            //////////////// await FileIO.WriteTextAsync(textFile, testTxtBx.Text.ToString()); //WRITES to file +++++++++WRITES TO FILE (TURN ON!!)


            //PUT WANTED FILE CONTENTS INTO STRING
            // message = await FileIO.ReadTextAsync(wantedFile);
            // break;
            return cardNamesList;
        }

      
        private void displayFiles(IReadOnlyList<StorageFile> allFiles)
        {
            testTxtBx2.Text = String.Empty; ////////////////////////////////////TEST
            foreach (StorageFile file in allFiles)
            {
                testTxtBx2.Text += file.DisplayName + " ";
                //MAKE BUTTONS HERE!! ===================================================================
            }

        }


        private void displayCards(List<string> cardNamesList)
        {
            testTxtBx2.Text = String.Empty; ////////////////////////////////////TEST
            foreach (string card in cardNamesList)
            {
                testTxtBx2.Text += card + " ";
                //MAKE BUTTONS HERE!! ===================================================================
            }
        }



        //private List<string> makeCardNamesList(string cardNamesString, List<string> cardNamesList)
        private List<string> makeCardNamesList(string cardNamesString)
        {
            //make cardNamesList from cardNamesString 

            String [] cardNamesArray = cardNamesString.Split(',');
            foreach (string element in cardNamesArray)
            {
                cardNamesList.Add(element);      
            }

            /*testTxtBx2.Text = String.Empty;//====================================================TEST

            for (int i = 0; i < cardNamesList.Count; i++) //====================================================TEST
            {
                testTxtBx2.Text += cardNamesList[i].ToString() + " "; 
            }
            */

            return cardNamesList;

        }

        private List<string> addToCardNamesList(string cardName)
        {
            cardNamesList.Add(cardName);

            return cardNamesList;
        }

        private string makeCardNamesString()
        {
            //make cardNamesString from cardNamesList

            foreach (string element in cardNamesList)
            {
                cardNamesString += element + ",";
            }

            return cardNamesString;
        }

        private string formatUserInput(string txtBxUserInput)
        {
            //trim leading and trailing whitespace. convert to lowercase. replace spaces with hyphens
            string userInput = txtBxUserInput.Trim().ToLower().Replace(" ", "-"); 

            ///////////////////////////testTxtBx.Text = userInput; /////////////=====================================================
           
            return userInput;
          
        }




        private async void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            fileName = formatUserInput(fileNameTxtBx.Text);

            await fileActions("openFile", fileName, cardNamesList); //++++++++++++++


            testTxtBx2.Text = String.Empty;//====================================================TEST

            for (int i = 0; i < cardNamesList.Count; i++) //====================================================TEST
            {
                testTxtBx2.Text += cardNamesList[i].ToString() + " ";
            }

            
        }


        private async void makeFileBtn_Click(object sender, RoutedEventArgs e)
        {
            fileName = formatUserInput(fileNameTxtBx.Text);

            await fileActions("makeFile", fileName, cardNamesList); //+++++++++++++++++++
        }


        private async void addToFileBtn_Click(object sender, RoutedEventArgs e)
        {

            await fileActions("addToFile", fileName, cardNamesList); //++++++++++++++
        }

        private async void deleteFileBtn_Click(object sender, RoutedEventArgs e)
        {

            fileName = formatUserInput(fileNameTxtBx.Text);

            await fileActions("deleteFile", fileName, cardNamesList);

            ///fileActions("deleteFile", "testFile3.txt"); //++++++++++++++
        }


        private async void deleteCardBtn_Click(object sender, RoutedEventArgs e)
        {
            fileName = formatUserInput(fileNameTxtBx.Text);

            await fileActions("deleteFile", fileName, cardNamesList);
            testTxtBx.Text = "CARD DELETED"; //+++++++++++++++++++++++++++++++++
        }


        private async void findCardBtn_Click(object sender, RoutedEventArgs e)
        {

           
            string httpCardName = formatUserInput(findCardTxtBx.Text);
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
