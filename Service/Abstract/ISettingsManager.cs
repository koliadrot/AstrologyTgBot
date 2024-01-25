namespace Service.Abstract
{
    using Service.Enums;
    using Service.ViewModels.TelegramModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISettingsManager
    {
        TelegramBotParamsViewModel GetTelegramBot(int id = 0);
        Task UpdateTelegramBot(TelegramBotParamsViewModel viewModel, bool isSuperAdmin);
        Task UpdateTelegramBotMenu(TelegramBotParamsViewModel viewModel);
        ICollection<TelegramBotCommandViewModel> GetTelegramBotCommands(int id = 0);
        Task CreateTelegramBotCommand(TelegramBotCommandViewModel viewModel);
        Task UpdateTelegramBotCommand(TelegramBotCommandViewModel viewModel);
        Task DeleteTelegramBotCommand(TelegramBotCommandViewModel viewModel);
        ICollection<TelegramBotRegisterConditionViewModel> GetTelegramBotRegisterConditions(int id = 0);
        Task UpdateTelegramBotRegisterCondition(TelegramBotRegisterConditionViewModel viewModel);
        ICollection<TelegramBotParamMessageViewModel> GetTelegramBotMessages(int id = 0);
        Task UpdateTelegramBotMessage(TelegramBotParamMessageViewModel viewModel);
        Task CreateTelegramBotMessage(TelegramBotParamMessageViewModel viewModel);
        void SaveStatusTelegramBot(TelegramBotStatusType status, int id = 0);
    }
}
