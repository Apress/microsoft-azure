// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Bot.Builder.Azure;
using JoeBot.Bots;
using System.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class QnABot<T> : ActivityHandler where T : Microsoft.Bot.Builder.Dialogs.Dialog
    {
        protected readonly BotState ConversationState;
        protected readonly Microsoft.Bot.Builder.Dialogs.Dialog Dialog;
        protected readonly BotState UserState;
        //Cosmos DB stuff
        private const string CosmosServiceEndpoint = "https://bot-conversations-cosmos.documents.azure.com:443/";
        private const string CosmosDBKey = "Icf4UWsdRR9LKCwBjWERthtM7sqUIgAZxyxenJH4lJ4cOJXBfQ74544Bc7A1p31tOdvBqbPBu9nAPQFs3fqENw==";
        private const string CosmosDBDatabaseName = "bot-conversations-cosmos";
        private const string CosmosDBCollectionName = "bot-storage";

        // Create Cosmos DB  Storage.  
        private static readonly CosmosDbStorage query = new CosmosDbStorage(new CosmosDbStorageOptions
        {
            AuthKey = CosmosDBKey,
            CollectionId = CosmosDBCollectionName,
            CosmosDBEndpoint = new Uri(CosmosServiceEndpoint),
            DatabaseId = CosmosDBDatabaseName,
        });
        //End Cosmos DB stuff
        public QnABot(ConversationState conversationState, UserState userState, T dialog)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {    // Run the Dialog with the new message Activity.
            await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);

            {
                // preserve user input.  
                var conversation = turnContext.Activity.Text;
                // make empty local logitems list.  
                TrackConversation logItems = null;

                // see if there are previous messages saved in storage.  
                try
                {
                    string[] conversationList = { "ConversationLog" };
                    logItems = query.ReadAsync<TrackConversation>(conversationList).Result?.FirstOrDefault().Value;
                }
                catch
                {
                    // Inform the user an error occured.  
                    await turnContext.SendActivityAsync("Sorry, something went wrong reading your stored messages!");
                }

                // If no stored messages were found, create and store a new entry.  
                if (logItems is null)
                {
                    // add the current utterance to a new object.  
                    logItems = new TrackConversation();
                    logItems.ConversationList.Add(conversation);
                    // set initial turn counter to 1.  
                    logItems.TurnNumber++;

                    // Show user new user message.  
                    //await turnContext.SendActivityAsync($"Echo" + turnContext.Activity.Text);

                    // Create Dictionary object to hold received user messages.  
                    var changes = new Dictionary<string, object>();
                    {
                        changes.Add("ConversationLog", logItems);
                    }
                    try
                    {
                        // Save the user message to your Storage.  
                        await query.WriteAsync(changes, cancellationToken);
                    }
                    catch
                    {
                        // Inform the user an error occured.  
                        await turnContext.SendActivityAsync("Sorry, something went wrong storing your message!");
                    }
                }
                // Else, our Storage already contained saved user messages, add new one to the list.  
                else
                {
                    // add new message to list of messages to display.  
                    logItems.ConversationList.Add(conversation);
                    // increment turn counter.  
                    logItems.TurnNumber++;

                    // show user new list of saved messages.  
                    //await turnContext.SendActivityAsync($"Echo " + turnContext.Activity.Text);

                    // Create Dictionary object to hold new list of messages.  
                    var changes = new Dictionary<string, object>();
                    {
                        changes.Add("ConversationLog", logItems);
                    };

                    try
                    {
                        // Save new list to your Storage.  
                        await query.WriteAsync(changes, cancellationToken);
                    }
                    catch
                    {
                        // Inform the user an error occured.  
                        await turnContext.SendActivityAsync("Sorry, something went wrong storing your message!");
                    }
                }
            }
        }    


        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
                }
            }
        }
    }
}
