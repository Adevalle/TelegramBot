using Telegram.Bot;
using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System.Net;
using System.ComponentModel.Design.Serialization;
using TekegrammBot;
using System.Collections.Generic;

namespace TelegramBot
{
 

    class Program
    {
      
        static int count = 0;

        private static Data DataConv = new Data();

        private static Dictionary<long, UserState> botState = new Dictionary<long, UserState>();
        private static Dictionary<string, string > convertbutton = new Dictionary<string, string>();
        private static ReplyKeyboardMarkup inline;
        private static TelegramBotClient botClient { get; } = new TelegramBotClient("YOU KEY");
        static void Main(string[] args)
        {
            var Convert = System.IO.File.Exists("E:\\чеба\\Progect\\TekegrammBot\\Currency.json");
            convertbutton = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText("E:\\чеба\\Progect\\TekegrammBot\\Currency.json"));
            
            botClient.StartReceiving(Update, Error);            
            Console.ReadLine();

        }
        


        async private static Task Update(ITelegramBotClient botClient, Update update, CancellationToken tocken)
        {
               
            
            
            var massege = update.Message;
            var state = botState.ContainsKey(update.Message.Chat.Id) ? botState[update.Message.Chat.Id] : null;
            if (state != null) 
            {
                switch (state.State)
                {
                    case State.Choose:
                        state.State = State.Choose;
                        if (massege != null)
                        {   
                            if (count == 0)
                            {
                                await botClient.SendTextMessageAsync(massege.Chat.Id, $"{ListCurrensy()}");
                                await botClient.SendTextMessageAsync(massege.Chat.Id, $"Базовая валюта USD. Выше вы видете список валюты, которую можно выбрать для конвертации." +
                                    $"Напишите целевую валюту из списка в формате \"USD\" (без скобок)");
                                
                                count++;
                            }
                            else if (count == 1)
                            {
                                DataConv.to = massege.Text.ToUpper().ToString();
                                await botClient.SendTextMessageAsync(massege.Chat.Id, $"Выбранная валюта  USD / {DataConv.to} ");
                                count = 2;
                                await botClient.SendTextMessageAsync(massege.Chat.Id, $"Введите число валюты", replyMarkup: new ReplyKeyboardRemove());
                            }
                            else if (count == 2)
                            {
                                ;
                                var amount = massege.Text.Replace(".",",");
                                DataConv.amount = Convert.ToDouble(amount);
                                await botClient.SendTextMessageAsync(massege.Chat.Id, $"Введенное число: {DataConv.amount}");
                                await botClient.SendTextMessageAsync(massege.Chat.Id, $"Провести конвертацию?", replyMarkup: Y_NCommand());
                                count = 0;
                                botState[update.Message.Chat.Id] = null; 
                            }
                        }
                    break;
                }
                
            }
            else
            {
                
                if (massege.Text != null)
                {
                    Console.WriteLine($"{massege.Chat.Id}");
                    Console.WriteLine($"Text: {massege.Text}");
                    switch (massege.Text)
                    {
                        case "/start":
                            await botClient.SendTextMessageAsync(massege.Chat.Id, $"Здравствуйте {massege.Chat.FirstName} , данный бот помогает в конвертации различных валют. Введите интересующую вас валюту или вукажите команду /choose , чтобы выбрать валюту: ", replyMarkup: new ReplyKeyboardRemove());
                            return;

                        case "/help":
                            await botClient.SendTextMessageAsync(massege.Chat.Id, $"Данный бот помогает в конвертации валюты. Вы можете выбрать из предложеного списка, или указать валюту самомтоятельно.\nПочта для обратной связи: ", replyMarkup: new ReplyKeyboardRemove());
                            return;

                        case "/website":
                            await botClient.SendTextMessageAsync(massege.Chat.Id, $"Перейти на сайт", replyMarkup: Inline());
                            return;
                        case "/choose":
                            await botClient.SendTextMessageAsync(massege.Chat.Id, $"Выбрана команда конвертации. Пришлите любой символ"/* replyMarkup: Bot_Command()*/);
                            botState[update.Message.Chat.Id] = new UserState { State = State.Choose };
                            break;
                        case "Да":
                            var result = ConverterApi(DataConv.from, DataConv.to, DataConv.amount);
                            await botClient.SendTextMessageAsync(massege.Chat.Id, $"У вас есть {DataConv.amount} USD = {result} {DataConv.result}", replyMarkup: new ReplyKeyboardRemove());
                            return;
                        default:
                            await botClient.SendTextMessageAsync(massege.Chat.Id, "Я не знаю что это такое, повторите попытку");
                            return;
                    }
                }
                return;
            }
        }

        private static object ListCurrensy()
        {
            string dictionaryString = "";
            foreach (KeyValuePair<string, string> keyValues in convertbutton)
            {
                dictionaryString += keyValues.Key + " : "+ keyValues.Value+ " \n " ;
            }
            return dictionaryString.TrimEnd(',', ' ') + "";
        }
    

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static ReplyKeyboardMarkup Y_NCommand() //основные комманды 
        {

            ReplyKeyboardMarkup inline = new(new[] { new KeyboardButton("Да"), new KeyboardButton("Нет") }) { ResizeKeyboard = true };

            return inline;

        }
        private static ReplyKeyboardMarkup Bot_Command() //основные комманды 
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new List<KeyboardButton[]>());
            var list = new List<KeyboardButton>();
            foreach (var key in convertbutton) 
            {
                list.Add(new KeyboardButton($"{key.Key}"));
                
            }
            replyKeyboard = new ReplyKeyboardMarkup(list);
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;


        }
        private static string ChooseConvert(Message massege)
        {
            DataConv.to = massege.Text;
            return DataConv.to;

        }


        private static InlineKeyboardMarkup GetButtontext()
        { 
            InlineKeyboardMarkup replyKeyboardMarkup = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                        {new [] {
                                                 InlineKeyboardButton.WithCallbackData("Текст для первой кнопки", callbackData: "11"),
                                                 InlineKeyboardButton.WithCallbackData("Текст второй кнопки","callback2"),
                                                 },
                                       });
            


            return replyKeyboardMarkup;
        }

        private static InlineKeyboardMarkup Inline() //Кнопки
        {

            InlineKeyboardButton inline = InlineKeyboardButton.WithUrl($"Перейти на сайт с конвертером.", url: $"https://cash.rbc.ru/converter.html?from=USD&to=RUR&sum=1&date=&rate=cbrf");

            return inline;

        }

        private static ReplyKeyboardMarkup ReplayKeybord(Message massege)
        {
            ReplyKeyboardMarkup inline = null;

            return inline;
        }

        private static object ConverterApi(string from, string to, double amount)
        {
            
            string url = "https://currencyapi.net/api/v1/rates?key=KEYCONVERTOR&output=JSON'";
            HttpWebRequest httpwebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpwebRequest.GetResponse();
            using (StreamReader streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                string response = streamreader.ReadToEnd();

                ResponseData convertResponse = JsonConvert.DeserializeObject<ResponseData>(response);
                double result = 0;
                foreach (var key in convertResponse.rates)
                {
                    if (DataConv.to == key.Key)
                    {
                        result = amount * Convert.ToDouble(key.Value);
                        return result;
                    }


                }
                return "(Не найдено совпадений)";
            }

        }
    }
}




//WebRequest req = WebRequest.Create(@"" + Http);
//WebResponse resp = req.GetResponse();
//Stream stream = resp.GetResponseStream();
//StreamReader streamReader = new StreamReader(stream);
//string text = streamReader.ReadToEnd();
//text = text.Substring(text.IndexOf(classteg) + classteg.Length);
//text = text.Remove(text.IndexOf("</div>"));
//return text;



//JsonTextReader reader = new JsonTextReader(new StringReader("Buttons.json"));
//reader.SupportMultipleContent = true;
//while (true)
//{
//    if (!reader.Read())
//        break;
//    JsonSerializer js = new JsonSerializer();
//    Buttons jsonfile = js.Deserialize<Buttons>(reader);


//    jsontext.Add(jsonfile);

//}
//for (int i = 0; i < jsontext.Count; i++)
//{
//    Console.WriteLine($" //{jsontext.Count}//");
//}



//List<Buttons> jsontext = new List<Buttons>();