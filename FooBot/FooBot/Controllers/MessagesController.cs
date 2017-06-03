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
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                var texto = activity.Text;
                var luisResponse = await LuisServicio.ParseUserConversation(texto);
                var replyMessage = string.Empty;

                //Comprobamos que Luis ha identificado bien el texto
                if (luisResponse.intents.Count() > 0)
                {
                    //Los "intents" se ordenan dependiendo de la probalidad de que sea la correcta, por eso usamos el [0]
                    switch(luisResponse.intents[0].intent)
                    {
                        case "Tiempo":
                            var lugar = luisResponse.entities[0].entity;
                            replyMessage = await GetTiempo(lugar);
                            break;

                        case "Chiste":
                            replyMessage = await GetJoke();
                            break;
                    }
                } else
                {
                    replyMessage = "Perdona, pero no te entiendo";
                }
                
                Activity reply;
                reply = activity.CreateReply(replyMessage);
              

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<string> GetTiempo(string lugar)
        {
            var tiempoItem = await TiempoServicio.GetTiempo(lugar);
            string returnValue;
            if("ERROR" == tiempoItem.Status)
            {
                returnValue = $"Lo siento, no he podido encontrar el lugar {lugar}";
            } else
            {
                returnValue = $"Hoy en {tiempoItem.location.name} está el tiempo {tiempoItem.current.condition.text}.";
            }
            return returnValue;
        }

        private async Task<string> GetJoke()
        {
            string joke = await JokeServicio.getRandomJoke();

            // Si no hemos podido conseguir un chiste, devolvemos un mensaje standard
            if (String.IsNullOrEmpty(joke))
            {
                joke = "Ahora no me apetece...";
            }

            return joke;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}