#Mobile Appications Project#

###Jason Thorne. G00317349###

This app was written in Visual Studio 2015, with the Microsoft.Net.Http client installed.  

This is an app designed to allow players of the card game ‘Magic: The Gathering’ to build virtual copies of their deck lists of cards.

It does this by allowing the user to create and save decks to their device. These decks are displayed as list items in the initially loaded state of the app. When one of these is opened by clicking it, the app changes state to allow the user to search for cards, to enter into the decklist, or to view the cards they have already saved in the deck.

Cards are found by the app making calls to the API, found at [deckbrew.com/api] (https://deckbrew.com/api/). 
They can be added to the deck by clicking the ‘+’ button which appears in the card searching state. They are displayed as list items in the same way as the decklists were, and can be clicked to view the card. 

When an added card is clicked, another call is made to the API to retrieve the card information. This time by using the cards name, which has been received from the file saved on the user’s device. There is also a back button in this state, allowing users to return to the deck list viewing state. 

Cards and Deck lists may be removed from the app, by clicking the delete icon beside their corresponding name in the list. 

The app has Multilanguage instructions informing the user of how to use it. In the non English languages, the message also informs them to enter the card names in English, as that is the language required for the API calls. 

Some card names to search for: 

*	Goblin
*	Lightning bolt
*	Terror
*	Counterspell
