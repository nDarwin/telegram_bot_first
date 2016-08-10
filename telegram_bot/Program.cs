using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using SimpleJSON;


namespace telegram_bot
{
    class Program
    {

        public static string Token = @"токен"; //статичная строк. переменная Токен
        public static int LastUpdateID = 0; //статичная числ. переменная для того что бы бот отвечал 1 раз. offset 


        static void Main(string[] args)
        {
            while (true)
            {
                GetUpdates(); //метод клиента 
                Thread.Sleep(1000); //Обновление раз в 1 сек.
            }
        }
            static void GetUpdates() 
                //делаем GET запрос на api telegram передаём ключ указываем метод(webclient) получаем текст парсим JSON и разбираем.
                {
                using (var webClient = new WebClient()) 
                    {
                    Console.WriteLine("Запрос обновления {0}", LastUpdateID + 1);

                    string response = webClient.DownloadString("https://api.telegram.org" + Token + "/getUpdates" + "?offset=" + (LastUpdateID + 1));

                    var N = JSON.Parse(response); 

                    foreach (JSONNode r in N["result"].AsArray)
                    {
                        LastUpdateID = r[update_id].AsInt;
                        Console.WriteLine("Пришло сообщение: {0}", r["message"]["text"]);
                        SendMessage("Я получил твоё сообщение!", r[message]["chat"]["id"].AssInt);
                    }

                }
            }

                    static void SendMessage(string message, int chatid)
                    {
                        using (var webClient = new WebClient())
                        {
                            var pars = new NameValueCollection();

                            pars.Add("text", message);
                            pars.Add("chat_id", chatid.ToString());

                            webClient.UploadValues("https://api.telegram.org/bot" + Token + "/sendMessage", pars);
                        }
                    }

        }

    }

