window.Transfert = window.Transfert || {};
window.Transfert.Document = window.Transfert.Document || {};

$(document).ready(function () {
    Transfert.Document.initMenuSelectMode();
    Transfert.Document.applyMode(_transfertCurrentMode || "all");
    Transfert.Document.initAddButton();
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

Transfert.Document.initAddButton = function () {
    $("#btnAddDocument").dxButton({
        text: "Ajouter",
        icon: "add",
        type: "success",
        onClick: function () {
            Transfert.Document.openAddPopup();
        }
    });
};

Transfert.Document.openAddPopup = function () {

    $("#addDocumentForm").dxForm({
        formData: {},
        items: [
            {
                dataField: "Name",
                label: { text: "Nom" },
                isRequired: true
            },
            {
                dataField: "Description",
                label: { text: "Description" }
            },
            {
                dataField: "RecipientEmail",
                label: { text: "Email destinataire" },
                isRequired: true
            },
            {
                dataField: "File",
                label: { text: "Fichier" },
                editorType: "dxFileUploader",
                editorOptions: {
                    multiple: false,
                    accept: ".zip,.rar,.7z,.tar,.gz",
                    maxFileSize: 10485760 // 10MB
                }
            }
        ]
    });

Transfert.Document.submitAddDocument = function () {

    var form = $("#addDocumentForm").dxForm("instance");
    var data = form.option("formData");

    if (!data.Name || !data.RecipientEmail || !data.File) {
        DevExpress.ui.notify("Champs obligatoires manquants", "error", 3000);
        return;
    }

    var file = data.File[0];

    if (!file) {
        DevExpress.ui.notify("Fichier obligatoire", "error", 3000);
        return;
    }

    if (file.size > 10485760) {
        DevExpress.ui.notify("Fichier > 10Mo", "error", 3000);
        return;
    }

    var formData = new FormData();
    formData.append("Name", data.Name);
    formData.append("Description", data.Description);
    formData.append("RecipientEmail", data.RecipientEmail);
    formData.append("File", file);

    $.ajax({
        url: "/Transfert/Document/Add",
        method: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            DevExpress.ui.notify("Document ajouté", "success", 3000);

            $("#addDocumentPopup").dxPopup("instance").hide();
            Transfert.Document.refreshGrid("myDocumentsGrid");
        },
        error: function () {
            DevExpress.ui.notify("Erreur lors de l'ajout", "error", 3000);
        }
    });
};

    $("#btnSubmitAddDocument").dxButton({
        text: "Ajouter",
        type: "success",
        onClick: function () {
            Transfert.Document.submitAddDocument();
        }
    });

    $("#addDocumentPopup").dxPopup("instance").show();
};
