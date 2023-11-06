var startSaveData;


// Сохранение данных телеграм бота
async function saveTelegramBotParams() {
    try {
        let saveData = buildSaveData();
        const response = await $.ajax({
            url: "/BonusWebApp/SettingsApi/UpdateTelegramBot",
            method: "POST",
            data: { viewModel: saveData }
        });
        initStartData();
        return response;
    }
    catch (error) {
        return false;
    }
}

// Собирает данные телеграм бота
function buildSaveData() {
    var data = {
        TelegramBotId: $("#TelegramBotId").val(),
        BotName: $("#BotName").val(),
        BotUserName: $("#BotUserName").val(),
        TokenApi: $("#TokenAPI").val(),
        WebHookUrl: $("#WebHookUrl").val(),
        TosUrl: $("#TosUrl").val(),
        AcceptPromotionsBySms: $("#AcceptPromotionsBySms").prop("checked"),
        /*        AcceptElectronicReceipts: $("#AcceptElectronicReceipts").prop("checked"),*/
        HelloText: $("#HelloText").val()
    };
    return data;
}

// Иниц-я стартовых данных
function initStartData() {
    startSaveData = buildSaveData();
}

// Старт/Стоп телеграм бота
async function switchOnOffTelegramBot(post, telegramBotId) {
    try {
        const response = await $.ajax({
            url: post,
            method: "POST",
            data: { telegramBotId: telegramBotId }
        });
        return response;
    }
    catch (error) {
        return false;
    }
}

// Получение статуса телеграмм бота
async function getStateTelegramBot(post, telegramBotId) {
    try {
        const response = await $.ajax({
            url: post,
            method: "POST",
            data: { telegramBotId: telegramBotId }
        });
        return response;
    }
    catch (error) {
        return false;
    }
}

// Отображет уведомление под кнопкой
function showNotification(message, type) {
    var notification = $("#popupNotification").data("kendoNotification");
    notification.show({ content: message }, type);
}

// Показывает процесс загрузки
function showLoader(isShow) {
    kendo.ui.progress($("#telegramBotEdit"), isShow);
}

// Есть ли не сохраненные данные
function hasUnsaveData() {
    var currentData = buildSaveData();
    return !isEqualData(currentData, startSaveData);
}

// Равны ли данные между собой
function isEqualData(dataFisrt, dataSecond) {
    return JSON.stringify(dataFisrt) === JSON.stringify(dataSecond);
}

// Проверка сохранения
function checkSaveData() {
    window.addEventListener('beforeunload', function (event) {
        if (hasUnsaveData()) {
            event.returnValue = 'Вы внесли изменения, которые могут не сохраниться.';
        }
    });
}