﻿using NLog;
using Service.Abstract;
using Telegram.Bot.Types;

namespace Service.Core.TelegramBot
{
    /// <summary>
    /// Распределитель обновлений взависимости от пользователей
    /// </summary>
    public class UpdateDistributor
    {
        private Dictionary<long, CommandExecutor> _listeners;

        private ILogger _logger;
        private ICustomerManager _customerManager;
        private ISettingsManager _settingsManager;
        private TelegramBotManager _telegramBotManager;

        public UpdateDistributor() => _listeners = new Dictionary<long, CommandExecutor>();

        public void Init(
            ICustomerManager customerManager,
            ISettingsManager settingsManager,
            TelegramBotManager telegramBotManager,
            ILogger logger)
        {
            _customerManager = customerManager;
            _settingsManager = settingsManager;
            _telegramBotManager = telegramBotManager;
            _logger = logger;
        }

        /// <summary>
        /// Получает обновление иденти-я по userId
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task GetUpdate(Update update)
        {
            long userId = Get.GetUserId(update);
            if (!_listeners.TryGetValue(userId, out CommandExecutor listener))
            {
                DataManager dataManager = new DataManager(_telegramBotManager);
                dataManager.AddData(_customerManager, true);
                dataManager.AddData(_settingsManager, true);
                dataManager.AddData(_logger, true);
                listener = new CommandExecutor(dataManager);
                await listener.Init(userId);
                _listeners.Add(userId, listener);
            }
            else
            {
                listener.DataManager.AddData(_customerManager, true);
                listener.DataManager.AddData(_settingsManager, true);
                listener.DataManager.AddData(_logger, true);
            }
            await listener.GetUpdate(update);
        }

        /// <summary>
        /// Заново обновляет по userId
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task ReUpdate(long userId)
        {
            if (!_listeners.TryGetValue(userId, out CommandExecutor listener))
            {
                DataManager dataManager = new DataManager(_telegramBotManager);
                dataManager.AddData(_customerManager, true);
                dataManager.AddData(_settingsManager, true);
                dataManager.AddData(_logger, true);
                listener = new CommandExecutor(dataManager);
                await listener.Init(userId);
                _listeners.Add(userId, listener);
            }
            else
            {
                listener.DataManager.AddData(_customerManager, true);
                listener.DataManager.AddData(_settingsManager, true);
                listener.DataManager.AddData(_logger, true);
            }
            await listener.ReUpdate();
        }

        /// <summary>
        /// Очистка у исполнителя данных
        /// </summary>
        /// <param name="update"></param>
        public void Dispose(Update update)
        {
            long userId = Get.GetUserId(update);
            Dispose(userId);
        }

        public void Dispose(long userId)
        {
            if (_listeners.TryGetValue(userId, out CommandExecutor listener))
            {
                listener.DataManager.Dispose();
            }
        }

        /// <summary>
        /// Иници-я всех исполнителей
        /// </summary>
        /// <returns></returns>
        public async Task InitAllListener()
        {
            foreach (var listener in _listeners)
            {
                listener.Value.DataManager.AddData(_customerManager, true);
                listener.Value.DataManager.AddData(_settingsManager, true);
                listener.Value.DataManager.AddData(_logger, true);
                await listener.Value.Init(listener.Key);
            }
        }

        /// <summary>
        /// Очистка всех исполнителей.
        /// </summary>
        /// <returns></returns>
        public void ClearAllListener() => _listeners?.Clear();
    }
}