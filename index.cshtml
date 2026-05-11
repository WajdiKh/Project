@using BacaratWeb.ViewModel.Transfert
@inject IViewTranslator translator

@{
    var currentMode = ViewBag.Mode ?? "all";
    var language = Context.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.ToString().ToCamelCase();
}


<div class="smallCaps text-green mt-2 mb-2">
    <div id="menuSelectMode" class="align-menu-items-center"></div>
</div>

<script>
    var _transfertCurrentMode = '@currentMode';
</script>

<div id="transfertContent">
    <div id="gridAllDocumentsContainer" style="display:none;">
        @(Html.DevExtreme().DataGrid<DocumentViewModel>()
            .ID("allDocumentsGrid")
            .ShowBorders(true)
            .ColumnAutoWidth(true)
            .SearchPanel(s => s.Visible(true).Placeholder("Rechercher..."))
            .Columns(columns =>
            {
                columns.AddFor(m => m.Name).Caption("Nom du document");
                columns.AddFor(m => m.Description).Caption("Description");
                columns.AddFor(m => m.ContentType).Caption("Type");
                columns.AddFor(m => m.FileSize).Caption("Taille");
                columns.AddFor(m => m.UploadDate).Caption("Date d'ajout");
                columns.AddFor(m => m.ExpiryDate).Caption("Date d'expiration");
                columns.AddFor(m => m.OwnerName).Caption("Propriétaire");
                
                columns.AddFor(m => m.Email).Caption("Destinataire");
                
                columns.Add()
                .Caption("Statut")
                .DataField($"StatutDocumentName{language}");
            })
            .DataSource(ds => ds.Mvc()
                .Area("Transfert")
                .Controller("Document")
                .LoadAction("GetAllDocuments"))
        )
    </div>

    
    <div id="gridMyDocumentsContainer" style="display:none;">
    <div class="mb-5">
        <div id="btnAddDocument"></div>
    </div>
    @(Html.DevExtreme().DataGrid<DocumentViewModel>()
        .ID("myDocumentsGrid")
        .ShowBorders(true)
        .ColumnAutoWidth(true)
        .SearchPanel(s => s.Visible(true).Placeholder("Rechercher..."))
        .Columns(columns =>
        {
            columns.AddFor(m => m.Name).Caption("Nom du document");
            columns.AddFor(m => m.Description).Caption("Description");
            columns.AddFor(m => m.ContentType).Caption("Type");
            columns.AddFor(m => m.FileSize).Caption("Taille");
            columns.AddFor(m => m.UploadDate).Caption("Date d'ajout");
            columns.AddFor(m => m.ExpiryDate).Caption("Date d'expiration");
            columns.AddFor(m => m.Email).Caption("Destinataire");

            columns.Add()
                .Caption("Statut")
                .DataField($"StatutDocumentName{language}");
        })
        .DataSource(ds => ds.Mvc()
            .Area("Transfert")
            .Controller("Document")
            .LoadAction("GetMyDocuments"))
    )
    </div>

    <div id="gridSharedDocumentsContainer" style="display:none;">
        @(Html.DevExtreme().DataGrid<DocumentViewModel>()
            .ID("sharedDocumentsGrid")
            .ShowBorders(true)
            .ColumnAutoWidth(true)
            .SearchPanel(s => s.Visible(true).Placeholder("Rechercher..."))
            .Columns(columns =>
            {
                columns.AddFor(m => m.Name).Caption("Nom du document");
                columns.AddFor(m => m.Description).Caption("Description");
                columns.AddFor(m => m.ContentType).Caption("Type");
                columns.AddFor(m => m.FileSize).Caption("Taille");
                columns.AddFor(m => m.UploadDate).Caption("Date d'ajout");
                columns.AddFor(m => m.ExpiryDate).Caption("Date d'expiration");
                columns.AddFor(m => m.OwnerName).Caption("Propriétaire");

                columns.Add()
                    .Caption("Statut")
                    .DataField($"StatutDocumentName{language}");
            })
            .DataSource(ds => ds.Mvc()
                .Area("Transfert")
                .Controller("Document")
                .LoadAction("GetSharedWithMeDocuments"))
        )
    </div>
</div>

@(Html.DevExtreme().Popup()
    .ID(TransfertConstants.AddNewDocumentPopupId)
    .ElementAttr(new Dictionary<string, object>
        { { "class", "popup" } })
    .ShowTitle(true)
    .DragEnabled(true)
    .ShowCloseButton(true)
    .ResizeEnabled(false)
    .TitleTemplate(@<text>
        <div class="d-flex flex-row align-items-center justify-content-between">
            <div class="font-size-16px text-green text-font-weight-600 smallCaps text-green">
                <i class="fas fa-pen mr-2" aria-hidden="true"></i>@translator.Common["Add"]
            </div>
            <button type="button"
                class="popup-close-btn"
                aria-label="@translator.Common["Close"]"
                onclick="Transfert.Document.closeAddDocumentPopup();"
                style="cursor: pointer; font-size: 24px; background: none; border: none; padding: 0; color: inherit;">
                &times;
            </button>
        </div>
    </text>))
