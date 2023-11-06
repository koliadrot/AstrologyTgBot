var startMenuData;

// Сохранение данных телеграм бота
async function saveTelegramBotMenu() {
    try {
        let saveData = buildSaveMenuData();
        const response = await $.ajax({
            url: "/BonusWebApp/SettingsApi/UpdateTelegramBotMenu",
            method: "POST",
            data: { viewModel: saveData }
        });
        initStartMenuData();
        return response;
    }
    catch (error) {
        return false;
    }
}

// Иниц-я стартовых данных
function initStartMenuData() {
    startMenuData = buildSaveMenuData();
}

// Сбор данных для сохраннеия
function buildSaveMenuData() {
    var authData = buildTreeData("authMenuTree");
    var startData = buildTreeData("startMenuTree");
    const dataMenuList = {
        StartMenu: startData,
        AuthMenu: authData,
    };
    const formattedData = JSON.stringify(dataMenuList);

    var saveData = {
        TelegramBotId: $("#TelegramBotId").val(),
        Menu: formattedData,
    };
    return saveData;
}

// Сбор команд в объект с конкретного меню
function buildTreeData(contanier) {
    const authMenuTree = document.getElementById(`${contanier}`);
    const data = {
        Items: []
    };

    const commandNodes = authMenuTree.querySelectorAll('.command-node');
    const stack = [data];

    commandNodes.forEach(commandNode => {
        const level = parseInt(commandNode.getAttribute('data-level'));
        const guid = commandNode.getAttribute('data-guid');

        var subcommandDropdown = $(`[data-id="${guid}"] .command-dropdown`);
        var commandName = subcommandDropdown[1].value;
        const name = commandNode.querySelector('.command-name').value;

        const formattedCommand = {
            CommandName: `${commandName}-${level}`,
            Name: name,
            SubMenu: null
        };

        while (level < stack.length - 1) {
            stack.pop();
        }

        if (level > 0) {
            if (!stack[stack.length - 1].SubMenu) {
                stack[stack.length - 1].SubMenu = {
                    Items: []
                };
            }
            stack[stack.length - 1].SubMenu.Items.push(formattedCommand);
        } else {
            stack[stack.length - 1].Items.push(formattedCommand);
        }
        stack.push(formattedCommand);
    });

    return data;
}

// Построение меню после регистрации
function buildAuthMenu(menuData) {
    if (menuData.AuthMenu && menuData.AuthMenu.Items && menuData.AuthMenu.Items.length > 0) {
        $(`#authMenuTree`).empty();
        buildMenuTree(menuData.AuthMenu.Items, null, 0, 'authMenuTree');
    }
}

// Построение меню до регистрации
function buildStartMenu(menuData) {
    if (menuData.StartMenu && menuData.StartMenu.Items && menuData.StartMenu.Items.length > 0) {
        $(`#startMenuTree`).empty();
        buildMenuTree(menuData.StartMenu.Items, null, 0, 'startMenuTree');
    }
}

// Построения меню на основе JSON-объекта
function buildMenuTree(items, parentGuid, level, contanier) {
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        var newGuid = parentGuid;

        var isRootCommand = parentGuid == null;
        if (isRootCommand) {
            newGuid = addRootCommand(contanier);
        }
        else {
            newGuid = addSubCommand(newGuid, level - 1, contanier);
        }
        setValueCommandByGuid(newGuid, item.CommandName, item.Name);

        if (item.SubMenu && item.SubMenu.Items && item.SubMenu.Items.length > 0) {
            buildMenuTree(item.SubMenu.Items, newGuid, level + 1, contanier);
        }
    }
}

// Проверяем контейнер меню после регистрации?
function IsAuthMenu(contanier) {
    return contanier === "authMenuTree";
}

// Получаем тип контейнера меню по понкту меню
function GetContanierByNode(node) {
    var startMenuTree = node.closest('#startMenuTree');
    var authMenuTree = node.closest('#authMenuTree');

    if (startMenuTree.length > 0) {
        return "startMenuTree";
    }
    else if (authMenuTree.length > 0) {
        return "authMenuTree";
    }
    else {
        return "unknown";
    }
}

// Генерация guid
function generateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0,
            v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

// Переключение состояния всех комманд
function switchAllCommands(contanier, isShow) {
    var nodes = $(`#${contanier}`).find('.command-node');

    nodes.each(function (index, item) {
        var stateIcon = isShow ? "fa-arrow-right" : "fa-arrow-down"
        var guid = item.getAttribute('data-guid');
        var level = item.getAttribute('data-level');

        var iconSpan = $(`[data-id="${guid}"]`).find(".switchPanelButton").find("span.fa-light");
        if (iconSpan.hasClass(stateIcon) && $(`[data-id="${guid}"]`).find(".switchPanelButton").css("display") != "none") {
            switchPanelCommand(guid, level);
        }
    });
}

// Смена команды у конкретного списка команд через guid
function setValueCommandByGuid(guid, value, text) {
    var subcommandDropdown = $(`[data-id="${guid}"] .command-dropdown`);
    subcommandDropdown.each(function () {
        var dropdownList = $(this).data("kendoDropDownList");
        if (dropdownList != undefined) {
            var cleanedValue = value.replace(/-[\d]+$/, "");
            dropdownList.value(cleanedValue);
            dropdownList.trigger("change");
            if (text.trim() !== '') {
                $(`[data-id="${guid}"]`).find(".command-name").val(text);
            }
            updateMainButtons();
        }
    });
}

// Принудительно показать/скрыть у команды меню
function switchPanelCommand(guid, level) {
    var currentCommandNode = $(`[data-guid="${guid}"][data-level="${level}"]`);
    var iconSpan = $(`[data-id="${guid}"]`).find(".switchPanelButton").find("span.fa-light");
    if (iconSpan.hasClass("fa-arrow-down")) {
        currentCommandNode.children(`.command-node[data-level="${parseInt(level) + 1}"]`).hide();
        iconSpan.removeClass("fa-arrow-down").addClass("fa-arrow-right");
    }
    else {
        currentCommandNode.children(`.command-node[data-level="${parseInt(level) + 1}"]`).show();
        iconSpan.removeClass("fa-arrow-right").addClass("fa-arrow-down");
    }
    var contanier = GetContanierByNode(currentCommandNode);
    updateStylesBasedOnMaxLevel(contanier);
}

// Удаляет команду из меню
function removeCommand(guid, level, isOnlyChildren) {
    var commandToRemove = $(`[data-guid="${guid}"][data-level="${level}"]`);
    var contanier = GetContanierByNode(commandToRemove);
    if (isOnlyChildren) {
        commandToRemove.find('.command-node').remove();
    }
    else {
        commandToRemove.remove();
    }
    updateMainButtons();
    updateStylesBasedOnMaxLevel(contanier);
}

// Есть ли не сохраненные данные
function hasUnsaveMenuData() {
    var currentData = buildSaveMenuData();
    return JSON.stringify(currentData) !== JSON.stringify(startMenuData)
}

// Проверка сохранения
function checkSaveMenuData() {
    window.addEventListener('beforeunload', function (event) {
        if (hasUnsaveMenuData()) {
            event.returnValue = 'Вы внесли изменения, которые могут не сохраниться.';
        }
    });
}

// Обновляет состояния кнопок сохранения и отмены
function updateMainButtons() {
    var saveButton = $("#saveButton");
    var cancelButton = $("#cancelButton");

    if (hasUnsaveMenuData()) {
        saveButton.removeAttr("disabled");
        saveButton.addClass("btn-primary");
        cancelButton.removeAttr("disabled");
    }
    else {
        saveButton.attr("disabled", "disabled");
        saveButton.removeClass("btn-primary");
        cancelButton.attr("disabled", "disabled");
    }
}

// Обновление отступов у пунктов меню
function updateStylesBasedOnMaxLevel(contanier) {
    var maxLevel = 0;
    var elementsWithLevel = $(`#${contanier}`).find('[data-level]');
    elementsWithLevel.each(function (index, item) {
        var level = parseInt(item.getAttribute('data-level'));
        var node = $(item.closest(".command-node"));
        if (node.css("display") != "none" && node.is(":visible") && !isNaN(level) && level > maxLevel) {
            maxLevel = level;
        }
    });

    var marginDropdownElements = $(`#${contanier}`).find('.margin-dropdown');
    marginDropdownElements.each(function (index, item) {
        var level = parseInt(item.getAttribute('level'));
        var delta = maxLevel - level;
        item.style.marginLeft = `${((maxLevel + delta) * 55)}px`;
    });

    var marginNodeElements = $(`#${contanier}`).find('.margin-node');
    marginNodeElements.each(function (index, item) {
        var level = parseInt(item.getAttribute('data-level'));
        if (!isNaN(level)) {
            var marginLeft = (55);
            item.style.marginLeft = `${marginLeft}px`;
        }
    });
}