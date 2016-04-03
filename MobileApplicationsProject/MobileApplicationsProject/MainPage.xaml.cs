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
        private List<string> cardNamesList = new List<string>();
        ///////////////////////////List<string> cardNamesList = new List<string>();
        List<string> fileNamesList = new List<string>(); ///NOT USED!!!!

        string cardNamesString; //////////////////////////////////might not need to be here
        //List<string> cardNamesList = new List<string>();
        private string fileName = "";
        string httpCardName = "";
        private string currentDeckFile = "";
        private string cardToDelete = "";

        bool inDecklist = false;

        public MainPage()
        {
            this.InitializeComponent();


            initApp(cardNamesList);

        }


        private async void initApp(List<string> cardNamesList)
        {


            //RootObject foundCard = await FindCards.GetCardData("shock");

            //testTxtBx.Text = foundCard.editions[0].multiverse_id.ToString(); //=====================================TEST

            //string image = String.Format(foundCard.editions[0].image_url);
            //showCardImg.Source = new BitmapImage(new Uri(image, UriKind.Absolute));

            await fileActions("*", "*", cardNamesList);
            //await fileActions("*", "*");
            //return null;
        }

        /* private async Task<List<string>> makeFile(string request, string fileName, List<string> cardNamesList)
         {

             StorageFile textFile = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
             Return cardNamesList;
         }
         */

        //private async Task fileActions(string request, string fileName)
        private async Task<List<string>> fileActions(string request, string fileName, List<string> cardNamesList)  //OPTIONS: create file, add to file,  open file, delete file, save file 
        {
            //var allFiles = "";
            fileName += ".txt";
            // List<string> cardNamesList = new List<string>();
            //String cardNamesString;

            //CREATE/OPEN APP FOLDER
            StorageFolder appFolder = await rootFolder.CreateFolderAsync("mtgDeckBuilder", CreationCollisionOption.OpenIfExists);


            if (request == "makeFile")
            {

                //CREATE FILE IN APP FOLDER
                StorageFile textFile = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                ////////////////////String cardNamesString = await FileIO.ReadTextAsync(textFile);

                //INIT CONTENT
                ////////////////////////await FileIO.WriteTextAsync(textFile, "INIT CONTENT");

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
            displayFiles(allFiles);

            try
            {

                //FETCH WANTED FILE FROM ALL FILES
                StorageFile wantedFile = allFiles.FirstOrDefault(x => x.Name == fileName);


                switch (request)
                {
                    case "deleteFile":
                        //DELETE FILE FROM FOLDER
                        await wantedFile.DeleteAsync();

                        //FETCH ALL FILES
                        allFiles = await appFolder.GetFilesAsync();//==========================================

                        //DISPLAY ALL FILES 
                        displayFiles(allFiles);

                        testTxtBx.Text = "FILE DELETED"; //==========================================

                        break;
                    default:


                        switch (request)
                        {

                            case "openFile":

                                //CLEAR LIST
                                //cardNamesList.Clear();


                                //PUT WANTED FILE CONTENTS INTO STRING
                                //cardNamesString = String.Empty;
                                cardNamesString = await FileIO.ReadTextAsync(wantedFile);


                                //MAKE LIST OF CARD NAMES FROM STRING
                                ///////////cardNamesList.Clear();
                                cardNamesList = makeCardNamesList(cardNamesString, cardNamesList); //, cardNamesList); ////////////////////////cardNamesString ============================

                                //DISPLAY CARDNAMES
                                displayCards(cardNamesList);

                                //MAKE CALLS USING CARDNAMES


                                //POPULATE (SOMETHING?) WITH CARD DATA

                                //testTxtBx.Text = "FILE OPENED"; //++++++++++++++++++++++++++++++++++
                                break;
                            case "addToFile": //FILE MUST BE OPENED FIRST!!! 

                                try
                                {




                                    testTxtBx.Text = httpCardName; /////////////////////////////////////////////////////////TEST
                                    //add word to list
                                    addToCardNamesList(httpCardName, cardNamesList);               //(httpCardName);

                                    //turn word to string 
                                    cardNamesString = makeCardNamesString();



                                    //write string into file 
                                    await FileIO.WriteTextAsync(wantedFile, cardNamesString); //WRITES to file CHANGE TXT BLOCK TO LIST STRING



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


                                cardNamesList = removeFromCardList(cardToDelete, cardNamesList);

                                cardToDelete = String.Empty;

                                cardNamesString = makeCardNamesString();

                                await FileIO.WriteTextAsync(wantedFile, cardNamesString);

                                displayCards(cardNamesList);

                               
                                testTxtBx.Text = "REMOVED FROM FILE"; //++++++++++++++++++++++++++++++++++
                                                                      //fileActions("openFile", "testFile3.txt"); //++++++++++++++
                                break;
                        }
                        break;
                }



            }
            catch (Exception e)
            {
                fileNameTxtBx.PlaceholderText = "TEST MESSAGE FROM TRY CATCH";
            }


            //}// ==========ELSE




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

            listItemBox.Items.Clear();

            testTxtBx2.Text = String.Empty; ////////////////////////////////////TEST

            foreach (StorageFile file in allFiles)
            {

                getListItemName listItemName = new getListItemName();
                listItemName.name = file.DisplayName;
                listItemBox.Items.Add(listItemName);
                testTxtBx2.Text += file.DisplayName + " "; /////////////////////////////////////////////////////////TEST


            }

        }


        private void displayCards(List<string> cardNamesList)
        {

            listItemBox.Items.Clear();

            inDecklist = true;

            testTxtBx2.Text = String.Empty; ////////////////////////////////////TEST

            foreach (string card in cardNamesList)
            {
                getListItemName listItemName = new getListItemName();
                listItemName.name = card;
                listItemBox.Items.Add(listItemName);
                testTxtBx2.Text += card + " "; /////////////////////////////////////////////////////////TEST

            }
        }




        private List<string> makeCardNamesList(string cardNamesString, List<string> cardNamesList)
        //private List<string> makeCardNamesList(string cardNamesString)
        {
            //make cardNamesList from cardNamesString 
            cardNamesList.Clear();

            if (!String.IsNullOrEmpty(cardNamesString))
            {
                String[] cardNamesArray = cardNamesString.Split(',');
                foreach (string element in cardNamesArray)
                {
                    if (!String.IsNullOrEmpty(element))
                    {
                        cardNamesList.Add(element);
                    }
                }

                /*testTxtBx2.Text = String.Empty;//====================================================TEST

                for (int i = 0; i < cardNamesList.Count; i++) //====================================================TEST
                {
                    testTxtBx2.Text += cardNamesList[i].ToString() + " "; 
                }
                */
            }


            return cardNamesList;

        }

        private List<string> addToCardNamesList(string cardName, List<string> cardNamesList)
        {

            cardNamesList.Add(cardName);

            return cardNamesList;
        }

        private List<string> removeFromCardList(string cardName, List<string> cardNamesList)
        {
            int indexToRemove = -1;
            for (int i = 0; i < cardNamesList.Count; i++)
            {
                string card = cardNamesList.ElementAt(i);
                if (card.Equals(cardName))
                {
                    indexToRemove = i;
                    break;
                }

            }

            if (indexToRemove != -1)
            {
                cardNamesList.RemoveAt(indexToRemove);
            }

            return cardNamesList;
        }

        private string makeCardNamesString()
        {

            //make cardNamesString from cardNamesList

            cardNamesString = String.Empty;

            foreach (string card in cardNamesList)
            {
                cardNamesString += card + ",";
            }

            return cardNamesString;
        }

        private string formatUserInput(string txtBxUserInput)
        {
            //trim leading and trailing whitespace. convert to lowercase. replace spaces with hyphens
            string userInput = txtBxUserInput.Trim().ToLower().Replace(" ", "-");

            ///testTxtBx.Text = userInput; /////////////=====================================================

            return userInput;

        }




        private async void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            fileName = formatUserInput(fileNameTxtBx.Text);

            await fileActions("openFile", fileName, cardNamesList); //++++++++++++++
            //await fileActions("openFile", fileName);


            testTxtBx2.Text = String.Empty;//====================================================TEST

            for (int i = 0; i < cardNamesList.Count; i++) //====================================================TEST
            {
                testTxtBx2.Text += cardNamesList[i].ToString() + " ";
            }


        }


        private async void makeFileBtn_Click(object sender, RoutedEventArgs e)
        {

            fileName = formatUserInput(fileNameTxtBx.Text);

            fileNameTxtBx.Text = String.Empty;

            await fileActions("makeFile", fileName, cardNamesList); //+++++++++++++++++++
            //await fileActions("makeFile", fileName); //+++++++++++++++++++

        }


        private async void addToFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (inDecklist)
            {
                await fileActions("addToFile", currentDeckFile, cardNamesList);
            }
            else
            {
                await fileActions("addToFile", fileName, cardNamesList);
            }
        }

        private async void deleteFileBtn_Click(object sender, RoutedEventArgs e)
        {

            fileName = formatUserInput(fileNameTxtBx.Text);

            await fileActions("deleteFile", fileName, cardNamesList);
            //await fileActions("deleteFile", fileName);


        }


        private async void deleteCardBtn_Click(object sender, RoutedEventArgs e)
        {
            fileName = formatUserInput(fileNameTxtBx.Text);
            await fileActions("deleteFile", fileName, cardNamesList);
            //await fileActions("deleteFile", fileName);
            testTxtBx.Text = "CARD DELETED"; //+++++++++++++++++++++++++++++++++
        }


        private async void findCardBtn_Click(object sender, RoutedEventArgs e)
        {


            httpCardName = formatUserInput(findCardTxtBx.Text);
            findCardTxtBx.Text = String.Empty;


            try
            {
                findCardTxtBx.PlaceholderText = "Enter Card name";
                RootObject foundCard = await FindCards.GetCardData(httpCardName);

                testTxtBx.Text = foundCard.editions[0].multiverse_id.ToString(); //=====================================TEST


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


        private async void itemDeleteBtn_click(object sender, RoutedEventArgs e)
        {

            ///////////////////////ask user if sure for delete!! ==============================================================



            getListItemName listItemName = (sender as Button).DataContext as getListItemName;

            if (listItemName != null)
            {

                string itemName = listItemName.name;

                if (inDecklist)
                {
                    cardToDelete = itemName;
                    await fileActions("removeFromFile", currentDeckFile, cardNamesList); //++++++++++++++
                    //await fileActions("removeFromFile", itemName); //++++++++++++++
                }
                else
                {

                    await fileActions("deleteFile", itemName, cardNamesList); //++++++++++++++
                    //await fileActions("deleteFile", itemName); //++++++++++++++
                }



                testTxtBx2.Text = String.Empty;//====================================================TEST

                for (int i = 0; i < cardNamesList.Count; i++) //====================================================TEST
                {
                    testTxtBx2.Text += cardNamesList[i].ToString() + " ";
                }
            }

        }

        private async void listItemBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            getListItemName listItemName = (sender as ListBox).SelectedItem as getListItemName;

            if (listItemName != null)
            {


                string itemName = listItemName.name;

                if (inDecklist)
                {

                    RootObject foundCard = await FindCards.GetCardData(itemName);

                    testTxtBx.Text = foundCard.editions[0].multiverse_id.ToString(); //=====================================TEST

                    string image = String.Format(foundCard.editions[0].image_url);
                    showCardImg.Source = new BitmapImage(new Uri(image, UriKind.Absolute));

                }
                else
                {
                    currentDeckFile = itemName;
                    await fileActions("openFile", currentDeckFile, cardNamesList); //++++++++++++++
                    //await fileActions("openFile", itemName); //++++++++++++++
                }




                testTxtBx2.Text = String.Empty;//====================================================TEST

                for (int i = 0; i < cardNamesList.Count; i++) //====================================================TEST
                {
                    testTxtBx2.Text += cardNamesList[i].ToString() + " ";
                }
            }

        }

        private async void closeCardListBtn_Click(object sender, RoutedEventArgs e)
        {
            inDecklist = false; //reset 
            currentDeckFile = String.Empty; //reset 

            await fileActions("*", "*", cardNamesList);
            //await fileActions("*", "*");
        }
    }

    public class getListItemName
    {
        public string name { get; set; }
    }
}
