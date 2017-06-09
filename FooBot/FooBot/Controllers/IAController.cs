using FooBot.Model;
using FooBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FooBot.Controllers
{
    public class IAController
    {


        public async Task<string> analizaFrase(string text)
        {
            LuisResponse luisResponse = await LuisServicio.ParseUserConversation(text);
            string replyMessage = string.Empty;

            //Comprobamos que Luis ha identificado bien el texto
            if (luisResponse.intents.Count() > 0)
            {
                //Los "intents" se ordenan dependiendo de la probalidad de que sea la correcta, por eso usamos el [0]
                switch (luisResponse.intents[0].intent)
                {
                    case "Tiempo":
                        replyMessage = await getTiempo(luisResponse);
                        break;

                    case "Chiste":
                        replyMessage = await getJoke();
                        break;
                    default:
                        replyMessage = getNoEntiendo();
                        break;
                }
            }
            else
            {
                replyMessage = getNoEntiendo();
            }
            return replyMessage;
        }


        private async Task<string> getTiempo(LuisResponse luisResponse)
        {
            string returnValue = String.Empty;
            if (luisResponse.entities.Count() > 0)
            {
                var lugar = luisResponse.entities[0].entity;
                var tiempoItem = await TiempoServicio.GetTiempo(lugar);
                if ("ERROR" == tiempoItem.Status)
                {
                    returnValue = $"Lo siento, no he podido encontrar el lugar {lugar}";
                }
                else
                {
                    returnValue = $"Hoy en {tiempoItem.location.name} está el tiempo {tiempoItem.current.condition.text}.";
                }
            } else
            {
                returnValue = "Ahora mismo no puedo responderte";
            }
            return returnValue;
        }

        private async Task<string> getJoke()
        {
            string joke = await JokeServicio.getRandomJoke();

            // Si no hemos podido conseguir un chiste, devolvemos un mensaje standard
            if (String.IsNullOrEmpty(joke))
            {
                joke = "Ahora no me apetece...";
            }

            return joke;
        }


        public string getNoEntiendo()
        {
            BDController bdConexion = new BDController();
            bdConexion.initBD();
            List<string> replyMessage = bdConexion.getFrases(2);
            int aleatorio = Util.GetRandomNumber(0, replyMessage.Count());
            return replyMessage[aleatorio];
        }

    }
}