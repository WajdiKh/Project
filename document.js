window.Transfert = window.Transfert || {};
window.Transfert.Document = window.Transfert.Document || {};

$(document).ready(function () {
    Transfert.Document.initMenuSelectMode();
    Transfert.Document.applyMode(_transfertCurrentMode || "all");
});

Transfert.Document.initMenuSelectMode = function () {
    $("#menuSelectMode").dxMenu({
        items: [
            {
                icon: "fas fa-folder-open color-green2 adjust-icon-menu",
                text: Transfert.Document.getModeLabel(_transfertCurrentMode),
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
        ]
    });
};

Transfert.Document.getModeLabel = function (mode) {
    if (mode === "mine") {
        return "Mes documents";
    }

    if (mode === "shared") {
        return "Partagés avec moi";
    }

    return "Tous les documents";
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

    Transfert.Document.reloadMenuLabel(mode);
};

Transfert.Document.reloadMenuLabel = function (mode) {
    var menu = $("#menuSelectMode").dxMenu("instance");
    if (!menu) {
        return;
    }

    menu.option("items", [
        {
            icon: "fas fa-folder-open color-green2 adjust-icon-menu",
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
    ]);
};

Transfert.Document.refreshGrid = function (gridId) {
    var grid = $("#" + gridId).dxDataGrid("instance");
    if (grid) {
        grid.refresh();
    }
};