window.Transfert = window.Transfert || {};
window.Transfert.Document = window.Transfert.Document || {};

$(document).ready(function () {
    Transfert.Document.initMenuSelectMode();
    Transfert.Document.applyMode(_transfertCurrentMode || "all");
});

Transfert.Document.getModeLabel = function (mode) {
    if (mode === "mine") {
        return "Mes documents";
    }

    if (mode === "shared") {
        return "Partagés avec moi";
    }

    return "Tous les documents";
};

Transfert.Document.getModeIcon = function (mode) {
    if (mode === "mine") {
        return "fas fa-user color-green2 adjust-icon-menu";
    }

    if (mode === "shared") {
        return "fas fa-share-alt color-green2 adjust-icon-menu";
    }

    return "fas fa-list color-green2 adjust-icon-menu";
};

Transfert.Document.getMenuItems = function (mode) {
    return [
        {
            icon: Transfert.Document.getModeIcon(mode),
            text: Transfert.Document.getModeLabel(mode),
            items: [
                {
                    icon: "fas fa-list adjust-icon-submenu",
                    text: "Tous les documents",
                    onClick: function () {
                        Transfert.Document.applyMode("all");
                    }
                },
                {
                    icon: "fas fa-user adjust-icon-submenu",
                    text: "Mes documents",
                    onClick: function () {
                        Transfert.Document.applyMode("mine");
                    }
                },
                {
                    icon: "fas fa-share-alt adjust-icon-submenu",
                    text: "Partagés avec moi",
                    onClick: function () {
                        Transfert.Document.applyMode("shared");
                    }
                }
            ]
        }
    ];
};

Transfert.Document.initMenuSelectMode = function () {
    $("#menuSelectMode").dxMenu({
        items: Transfert.Document.getMenuItems(_transfertCurrentMode || "all")
    });
};

Transfert.Document.applyMode = function (mode) {
    _transfertCurrentMode = mode;

    $("#gridAllDocumentsContainer").hide();
    $("#gridMyDocumentsContainer").hide();
    $("#gridSharedDocumentsContainer").hide();

    if (mode === "mine") {
        $("#gridMyDocumentsContainer").show();
        Transfert.Document.refreshGrid("myDocumentsGrid");
    }
    else if (mode === "shared") {
        $("#gridSharedDocumentsContainer").show();
        Transfert.Document.refreshGrid("sharedDocumentsGrid");
    }
    else {
        $("#gridAllDocumentsContainer").show();
        Transfert.Document.refreshGrid("allDocumentsGrid");
    }

    var menu = $("#menuSelectMode").dxMenu("instance");
    if (menu) {
        menu.option("items", Transfert.Document.getMenuItems(mode));
    }
};

Transfert.Document.refreshGrid = function (gridId) {
    var grid = $("#" + gridId).dxDataGrid("instance");
    if (grid) {
        grid.refresh();
    }
};
