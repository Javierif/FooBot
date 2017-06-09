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
            try
            {
                if (activity.Type == ActivityTypes.Message && !string.IsNullOrEmpty(activity.Text) && activity.Text.ToUpper().Contains("devfoobot"))
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
                                if(luisResponse.entities.Count() > 0)
                                {
                                    var lugar = luisResponse.entities[0].entity;
                                    replyMessage = await GetTiempo(lugar);
                                }

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
            } catch(Exception e ){ }
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