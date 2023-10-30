using Service.Abstract.TelegramBot;
using Service.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Core.TelegramBot.Commands
{
    public class TransitionCommand : ICommand
    {
        public string Name { get; set; } = "/transition";

        public string Description { get; set; } = "Переход в уровень меню";

        public string ShortDescription { get; set; } = "Переход";

        public bool IsAuth { get; set; } = false;
        public bool IsDefault { get; set; } = true;

        public TelegramBotCommandType CommandType => TelegramBotCommandType.Custom;
        public bool IsStartMenu => false;

        public bool IsForwardDirection { get; set; } = true;

        private readonly DataManager _dataManager;
        private readonly Dictionary<string, string> _messages;

        public TransitionCommand(DataManager dataManager)
        {
            _dataManager = dataManager;
            _messages = _dataManager.GetData<CommandExecutor>().Messages;
        }

        public async Task Execute(Update update, string[] arg = null)
        {
            long chatId = Get.GetChatId(update);
            ReplyKeyboardMarkup replyKeyboard;
            if (IsForwardDirection)
            {
                _dataManager.GetData<CommandExecutor>().CurrentLevelMenu++;
            }
            else
            {
                _dataManager.GetData<CommandExecutor>().CurrentLevelMenu--;
            }
            int level = _dataManager.GetData<CommandExecutor>().CurrentLevelMenu;
            if (_dataManager.GetData<CommandExecutor>().Menu[level].Count > 2)
            {
                var inlineKeyboardButtons = new List<List<KeyboardButton>>();

                foreach (var miniCommand in _dataManager.GetData<CommandExecutor>().Menu[level])
                {
                    var row = new List<KeyboardButton> { new KeyboardButton(miniCommand.ShortDescription) };
                    inlineKeyboardButtons.Add(row);
                }

                replyKeyboard = new ReplyKeyboardMarkup(inlineKeyboardButtons);
            }
            else
            {
                List<KeyboardButton> inlineKeyboardButtons = new List<KeyboardButton>();
                replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    inlineKeyboardButtons
                });

                foreach (var command in _dataManager.GetData<CommandExecutor>().Menu[level])
                {
                    inlineKeyboardButtons.Add(new KeyboardButton(command.ShortDescription));
                }
            }
            replyKeyboard.ResizeKeyboard = true;
            replyKeyboard.IsPersistent = true;
            await _dataManager.GetData<TelegramBotManager>().SendTextMessage(chatId, _messages[MessageKey.TRANSITION_HELP], replyMarkup: replyKeyboard);
        }
    }
}
