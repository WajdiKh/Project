window.Transfert = window.Transfert || {};
window.Transfert.Document = window.Transfert.Document || {};

$(document).ready(function () {
    Transfert.Document.initMenuSelectMode();
    Transfert.Document.initAddButton();
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
    } else if (mode === "shared") {
        $("#gridSharedDocumentsContainer").show();
        Transfert.Document.refreshGrid("sharedDocumentsGrid");
    } else {
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
        labelLocation: "top",
        items: [
            {
                dataField: "Name",
                label: { text: "Nom" },
                isRequired: true
            },
            {
                dataField: "Description",
                label: { text: "Description" },
                editorType: "dxTextArea",
                editorOptions: {
                    height: 90
                }
            },
            {
                dataField: "RecipientEmail",
                label: { text: "Email destinataire" },
                isRequired: true
            },
            {
                dataField: "ExpiryDelayHours",
                label: { text: "Durée d'expiration" },
                editorType: "dxSelectBox",
                editorOptions: {
                    dataSource: [
                        { value: 24, text: "24h" },
                        { value: 48, text: "48h" },
                        { value: 72, text: "72h" }
                    ],
                    valueExpr: "value",
                    displayExpr: "text",
                    value: 48
                }
            },
            {
                dataField: "EncryptionKey",
                label: { text: "Clé de chiffrement" }
            },
            {
                dataField: "File",
                label: { text: "Fichier" },
                editorType: "dxFileUploader",
                editorOptions: {
                    multiple: false,
                    uploadMode: "useForm",
                    accept: ".zip,.rar,.7z,.tar,.gz",
                    maxFileSize: 10485760,
                    selectButtonText: "Sélectionner un fichier",
                    labelText: "",
                    onValueChanged: function (e) {
                        var form = $("#addDocumentForm").dxForm("instance");
                        var data = form.option("formData");

                        data.File = e.value;

                        form.option("formData", data);
                    }
                }
            }
        ]
    });

    $("#btnSubmitAddDocument").dxButton({
        text: "Ajouter",
        type: "success",
        onClick: function () {
            Transfert.Document.submitAddDocument();
        }
    });

    $("#addDocumentPopup").dxPopup("instance").show();
};

Transfert.Document.submitAddDocument = function () {
    var form = $("#addDocumentForm").dxForm("instance");
    var data = form.option("formData");

    if (!data.Name || !data.RecipientEmail) {
        DevExpress.ui.notify("Champs obligatoires manquants", "error", 3000);
        return;
    }

    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(data.RecipientEmail)) {
        DevExpress.ui.notify("Email invalide", "error", 3000);
        return;
    }

    if (!data.File || data.File.length === 0) {
        DevExpress.ui.notify("Fichier obligatoire", "error", 3000);
        return;
    }

    var file = data.File[0];

    if (!file) {
        DevExpress.ui.notify("Fichier obligatoire", "error", 3000);
        return;
    }

    var allowedExtensions = [".zip", ".rar", ".7z", ".tar", ".gz"];
    var extension = file.name.substring(file.name.lastIndexOf(".")).toLowerCase();

    if (allowedExtensions.indexOf(extension) === -1) {
        DevExpress.ui.notify("Le fichier doit être une archive : zip, rar, 7z, tar ou gz", "error", 3000);
        return;
    }

    if (file.size > 10485760) {
        DevExpress.ui.notify("La taille du fichier ne doit pas dépasser 10Mo", "error", 3000);
        return;
    }

    var formData = new FormData();
    formData.append("Name", data.Name);
    formData.append("Description", data.Description || "");
    formData.append("RecipientEmail", data.RecipientEmail);
    formData.append("ExpiryDelayHours", data.ExpiryDelayHours || 48);
    formData.append("EncryptionKey", data.EncryptionKey || "");
    formData.append("File", file);

    $.ajax({
        url: "/fr/transfert/document/add",
        method: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            DevExpress.ui.notify("Document ajouté", "success", 3000);

            $("#addDocumentPopup").dxPopup("instance").hide();

            Transfert.Document.refreshGrid("myDocumentsGrid");
            Transfert.Document.refreshGrid("allDocumentsGrid");
        },
        error: function (xhr) {
            DevExpress.ui.notify(xhr.responseText || "Erreur lors de l'ajout", "error", 3000);
        }
    });
};

Transfert.Document.initAddDocumentValidators = function () {
    $("#Name").dxValidator({
        validationGroup: "AddNewDocumentFormValidationGroup",
        validationRules: [
            { type: "required", message: "Le nom du fichier est obligatoire." },
            { type: "stringLength", max: 255, message: "Le nom du fichier ne doit pas dépasser 255 caractères." }
        ]
    });

    $("#Description").dxValidator({
        validationGroup: "AddNewDocumentFormValidationGroup",
        validationRules: [
            { type: "stringLength", max: 1000, message: "La description ne doit pas dépasser 1000 caractères." }
        ]
    });

    $("#EncryptionKey").dxValidator({
        validationGroup: "AddNewDocumentFormValidationGroup",
        validationRules: [
            { type: "stringLength", max: 255, message: "La clé de chiffrement ne doit pas dépasser 255 caractères." }
        ]
    });

    $("#RecipientEmail").dxValidator({
        validationGroup: "AddNewDocumentFormValidationGroup",
        validationRules: [
            { type: "required", message: "L'e-mail est obligatoire." },
            { type: "email", message: "Le format de l'e-mail est invalide." },
            { type: "stringLength", max: 320, message: "L'e-mail ne doit pas dépasser 320 caractères." }
        ]
    });

    $("#FileUploader").dxValidator({
        validationGroup: "AddNewDocumentFormValidationGroup",
        validationRules: [
            {
                type: "custom",
                message: "Le fichier est obligatoire.",
                validationCallback: function () {
                    var fileInput = $("#FileUploader")[0];
                    return fileInput && fileInput.files && fileInput.files.length > 0;
                }
            }
        ]
    });
};