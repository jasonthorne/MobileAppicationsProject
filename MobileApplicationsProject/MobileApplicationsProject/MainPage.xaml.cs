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

       
        StorageFolder rootFolder = ApplicationData.Current.LocalFolder; //opens root folder
        private List<string> cardNamesList = new List<string>();

        private string cardNamesString = "";
        private string fileName = "";
        private string httpCardName = "";
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

            await fileActions("*", "*", cardNamesList);

        }


        private async Task<List<string>> fileActions(string request, string fileName, List<string> cardNamesList)
        {

            fileName += ".txt";


            //CREATE/OPEN APP FOLDER
            StorageFolder appFolder = await rootFolder.CreateFolderAsync("mtgDeckBuilder", CreationCollisionOption.OpenIfExists);


            if (request == "makeFile")
            {

                //CREATE FILE IN APP FOLDER
                StorageFile textFile = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            }



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
                        allFiles = await appFolder.GetFilesAsync();

                        //DISPLAY ALL FILES 
                        displayFiles(allFiles);



                        break;
                    default:


                        switch (request)
                        {

                            case "openFile":



                                //PUT WANTED FILE CONTENTS INTO STRING

                                cardNamesString = await FileIO.ReadTextAsync(wantedFile);


                                //MAKE LIST OF CARD NAMES FROM STRING

                                cardNamesList = makeCardNamesList(cardNamesString, cardNamesList);

                                //DISPLAY CARDNAMES
                                displayCards(cardNamesList);

                                //MAKE CALLS USING CARDNAMES


                                //POPULATE (SOMETHING?) WITH CARD DATA


                                break;
                            case "addToFile":

                                try
                                {


                                    //add word to list
                                    addToCardNamesList(httpCardName, cardNamesList);

                                    //turn word to string 
                                    cardNamesString = makeCardNamesString();

                                    //write string into file 
                                    await FileIO.WriteTextAsync(wantedFile, cardNamesString);

                                    //display cardnames
                                    displayCards(cardNamesList);



                                }
                                catch (Exception e)
                                {

                                }

                                break;
                            case "removeFromFile":


                                cardNamesList = removeFromCardList(cardToDelete, cardNamesList);

                                cardToDelete = String.Empty;

                                cardNamesString = makeCardNamesString();

                                await FileIO.WriteTextAsync(wantedFile, cardNamesString);

                                displayCards(cardNamesList);


                                break;
                        }
                        break;
                }

            }
            catch (Exception e)
            {

            }

            return cardNamesList;
        }




        private void displayFiles(IReadOnlyList<StorageFile> allFiles)
        {

            listItemBox.Items.Clear();


            foreach (StorageFile file in allFiles)
            {

                getListItemName listItemName = new getListItemName();
                listItemName.name = file.DisplayName;
                listItemBox.Items.Add(listItemName);

            }

        }


        private void displayCards(List<string> cardNamesList)
        {

            listItemBox.Items.Clear();

            inDecklist = true;

            foreach (string card in cardNamesList)
            {
                getListItemName listItemName = new getListItemName();
                listItemName.name = card;
                listItemBox.Items.Add(listItemName);

            }
        }




        private List<string> makeCardNamesList(string cardNamesString, List<string> cardNamesList) //make cardNamesList from cardNamesString 
        {

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

        private string makeCardNamesString()  //make cardNamesString from cardNamesList
        {

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

            return userInput;

        }



        /*
        private async void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            fileName = formatUserInput(fileNameTxtBx.Text);

            await fileActions("openFile", fileName, cardNamesList); 

        }*/


        private async void makeFileBtn_Click(object sender, RoutedEventArgs e)
        {

            fileName = formatUserInput(fileNameTxtBx.Text);

            fileNameTxtBx.Text = String.Empty;

            await fileActions("makeFile", fileName, cardNamesList);

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



        private async void findCardBtn_Click(object sender, RoutedEventArgs e)
        {


            httpCardName = formatUserInput(findCardTxtBx.Text);
            findCardTxtBx.Text = String.Empty;


            try
            {
                findCardTxtBx.PlaceholderText = "Enter Card name";
                RootObject foundCard = await FindCards.GetCardData(httpCardName);

                string image = String.Format(foundCard.editions[0].image_url);
                showCardImg.Source = new BitmapImage(new Uri(image, UriKind.Absolute));


            }
            catch (Exception ex)
            {

                findCardTxtBx.PlaceholderText = "Card not found!";
            }

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
                    await fileActions("removeFromFile", currentDeckFile, cardNamesList);

                }
                else
                {

                    await fileActions("deleteFile", itemName, cardNamesList);

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



                    string image = String.Format(foundCard.editions[0].image_url);
                    showCardImg.Source = new BitmapImage(new Uri(image, UriKind.Absolute));

                }
                else
                {
                    VisualStateManager.GoToState(this, "cardsState", false); //change app to cardsState
                    currentDeckFile = itemName;
                    await fileActions("openFile", currentDeckFile, cardNamesList);

                }

            }

        }

        private async void closeCardListBtn_Click(object sender, RoutedEventArgs e)
        {
            inDecklist = false; //reset 

            currentDeckFile = String.Empty; //reset 

            await fileActions("*", "*", cardNamesList);

        }
    }

    public class getListItemName
    {
        public string name { get; set; }
    }
}
