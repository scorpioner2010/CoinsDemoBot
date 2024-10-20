using System;
using UnityEngine;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using System.Threading;
using System.Threading.Tasks;

public class TelegramBotManager : MonoBehaviour
{
    private static ITelegramBotClient botClient;
    private CancellationTokenSource cts;

    void Start()
    {
        botClient = new TelegramBotClient("7504020451:AAGa7AfMQAK6mm64fvf2zRgvFEo-Xpls1ug"); // Заміни на свій токен бота
        cts = new CancellationTokenSource();

        // Запускаємо обробку повідомлень за допомогою Polling
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { } // Отримуємо всі типи оновлень
        };

        botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);
        ConsoleController.Log("Бот запущений!");
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update == null)
        {
            ConsoleController.LogError("Оновлення відсутнє.");
            return;
        }

        // Логи для отриманих повідомлень
        if (update.Message != null)
        {
            ConsoleController.Log("Отримано повідомлення: " + update.Message.Text);
        }
        else
        {
            ConsoleController.LogError("Повідомлення відсутнє.");
        }

        try
        {
            // Перевірка на команду /getgametaras
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message != null)
            {
                var message = update.Message;

                if (!string.IsNullOrEmpty(message.Text) && message.Text.ToLower() == "/getgametaras")
                {
                    ConsoleController.Log("/getgametaras отримано, відправляємо гру.");

                    await botClient.SendGameAsync(
                        chatId: message.Chat.Id, // ID чату для гри
                        gameShortName: "coinclicker" // Коротке ім'я гри
                    );
                }
            }

            // Обробка callback-запитів
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery && update.CallbackQuery != null)
            {
                var callbackQuery = update.CallbackQuery;

                // Перевірка на застарілість запиту
                if (callbackQuery.Message != null && (DateTime.UtcNow - callbackQuery.Message.Date).TotalSeconds > 60)
                {
                    ConsoleController.Log("Запит застарілий, ігноруємо.");
                    return;
                }

                // Відправляємо посилання на гру
                await botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    url: "https://scorpioner2010.github.io/CoinsDemoBot/" // Посилання на гру
                );

                ConsoleController.Log("Запит на запуск гри відправлено.");
            }
        }
        catch (Exception ex)
        {
            ConsoleController.LogError($"Помилка обробки оновлення: {ex.Message}");
        }
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        ConsoleController.LogError($"Виникла помилка: {exception.Message}");
        return Task.CompletedTask;
    }

    void OnApplicationQuit() => cts.Cancel();
}