using FooBot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FooBot.Services
{
    public class TiempoServicio
    {
        public static async Task<TiempoItem> GetTiempo(string lugar)
        {
            string escapedString = Uri.EscapeDataString(lugar);
            //El $ delante es una forma de hacer un string.format rápido para asignar el parametro.
            string uri = $"http://api.apixu.com/v1/current.json?key=a27ce9eeba084165a8a104719171405&q={escapedString}";
            var tiempoItem = new TiempoItem();
            //Using sirve para crear un bloque de ejecución donde liberará los recursos al salir.
            using (var client = new HttpClient())
            {
                
                var msg = await client.GetAsync(uri);
                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    tiempoItem = JsonConvert.DeserializeObject<TiempoItem>(jsonResponse);
                    tiempoItem.Status = "OK";
                } else
                {
                    tiempoItem.Status = "ERROR";
                }             

            }
            return tiempoItem;
        }
    }
}