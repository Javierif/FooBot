using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using FooBot.Services;
using FooBot.Controllers;
using System.Collections.Generic;

namespace FooBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                IAController _IAController = new IAController();
                Activity reply;

                if (activity.Type == ActivityTypes.Message && !string.IsNullOrEmpty(activity.Text) && activity.Text.ToUpper().Contains("DEVFOOBOT"))
                {
                   string respuesta = await _IAController.analizaFrase(activity.Text);
                   reply = activity.CreateReply(respuesta);
                   await connector.Conversations.ReplyToActivityAsync(reply);
                }
            } catch(Exception e){ }
            await HandleSystemMessage(activity);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            switch (message.Type)
            {
                case ActivityTypes.DeleteUserData:
                    break;
                case ActivityTypes.ConversationUpdate:
                    break;
                case ActivityTypes.ContactRelationUpdate:
                    break;
                case ActivityTypes.Typing:
                    break;
                case ActivityTypes.Ping:
                    break;
            }
            return null;
        }
    }
}