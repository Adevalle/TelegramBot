using Telegram.Bot;
using System;
using TelegramBot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;



namespace TelegramBot
{
    class Program
    {
        private static List<BotCommand> commandsList;
        private static TelegramBotClient botClient { get; } = new TelegramBotClient("6504774319:AAEQXddGiub5q6aIhitcUHxi4Y4kBE3XED0");
        static void Main(string[] args)
        {
            botClient.StartReceiving(Update, Error);
            Console.ReadLine();
            

        }
        async private static Task Update(ITelegramBotClient botClient, Update update, CancellationToken tocken)
        {
            var massege = update.Message;
            Console.WriteLine($"{massege.Chat.Id}");
            Console.WriteLine($"Text: {massege.Text}");
            
            ReplyKeyboardMarkup inline = new(new[] { new KeyboardButton("Курс"), new KeyboardButton("USC/RUB") }) { ResizeKeyboard = true };
            if (massege.Text != null)
            {
              

                if (massege.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(massege.Chat.Id, $"Здравствуйте {massege.Chat.FirstName} , данный бот помогает в конвертации различных валют. Выберете интересующую вас валюту: ", replyMarkup: inline);
                    
                    return;
                }
                if (massege.Text.ToLower() == "/help")
                {
                    await botClient.SendTextMessageAsync(massege.Chat.Id, $"Данный бот помогает в конвертации валюты. Вы можете выбрать из предложеного списка, или укахать валюту самомтоятельно.", replyMarkup: new ReplyKeyboardRemove());
                        ;
                    return;
                }
                else if (massege.Text.Contains("/"))
                {
                    await botClient.SendTextMessageAsync(massege.Chat.Id, "Неизвестная команда, повторите попытку");
                    return;
                }
                else if (massege.Text != null)
                {
                    await botClient.SendTextMessageAsync(massege.Chat.Id, "Я не знаю что это такое, повторите попытку");
                    return;
                }

            }
           
        }

         private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

       

       
    }
}

