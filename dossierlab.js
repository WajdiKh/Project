
var _isEditDossierLab = false; var _isReportingDossierLab = false; var _isReportingTiers = false; var isSearchTiers = false; var _isEditLabAttachment = false;
var _labIsNewContributeur;
var attchmentDataJson, oldAttchmentDataJson = [];
function addPhysicalPerson() {
    showLoadPanel(_labTranslatableMessageAddingNewPP);
    var countPhysicalPerson = $('.physicalPerson').length;
    var isDeclarationtracfin = $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value");
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        url: '/' + culture + '/Lab/ServiceLab/AddPersonnePhysiqueForm?order=' + countPhysicalPerson + '&directionId=' + directionId,
    }).done(function (data) {
        $("#accordionPhysicalPersonsList").append(data);
        if (isDeclarationtracfin) {
            $(".isPersonneDs").removeClass("dx-hidden");
            $("#PersonnePhysiqueLabIsDeclarationTracfin" + countPhysicalPerson).dxCheckBox("instance").option("value", true);
        }
        $("#collapsePhysicalPerson" + countPhysicalPerson).collapse('toggle');
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#bloc2").offset().top + ($("#headingPhysicalPerson" + countPhysicalPerson).height() * (countPhysicalPerson + 1)) + 20
        }, 500);
        setRelationAffaireValidator();

        resetIdentificationSpecifique();

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function addNonConnu() {
    showLoadPanel(_labTranslatableMessageAddingNewNC);
    var countNonConnu = $('.nonConnu').length;
    var isDeclarationtracfin = $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value");
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        url: '/' + culture + '/Lab/ServiceLab/AddNonConnu?order=' + countNonConnu + '&directionId=' + directionId,
    }).done(function (data) {
        $("#accordionNonConnusList").append(data);
        if (isDeclarationtracfin) {
            $(".isPersonneDs").removeClass("dx-hidden");
            $("#DossierLabNonConnusIsDeclarationTracfin" + countNonConnu).dxCheckBox("instance").option("value", true);
        }
        $("#collapseNonConnu" + countNonConnu).collapse('toggle');
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#bloc2").offset().top + ($("#headingNonConnu" + countNonConnu).height() * (countNonConnu + 1)) + 20
        }, 500);


    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });

}

function addMoralPerson() {
    showLoadPanel(_labTranslatableMessageAddingNewPM);
    var countMoralPerson = $('.moralPerson').length;
    var isDeclarationtracfin = $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value");
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        url: '/' + culture + '/Lab/ServiceLab/AddPersonneMoraleForm?Order=' + countMoralPerson + '&directionId=' + directionId,
    }).done(function (data) {
        $("#accordionMoralPersonsList").append(data);
        //paysnaissanceaddpersonnephysique_load();
        if (isDeclarationtracfin) {
            $(".isPersonneDs").removeClass("dx-hidden");
            $("#DossierLabPersonneMoralesIsDeclarationTracfin" + countMoralPerson).dxCheckBox("instance").option("value", true);
        }
        $("#collapseMoralPerson" + countMoralPerson).collapse('toggle');
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#bloc2").offset().top + ($("#headingMoralPerson" + countMoralPerson).height() * (countMoralPerson + 1)) + 20
        }, 500);

        resetIdentificationSpecifique();

        setRelationAffaireValidator();

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}



function updateHeaderIdentityMoralPerson(id) {
    var raisonSociale = $("#PersonneMoraleLabRaisonSociale" + id).dxTextBox("instance").option('value');
    var numeroImmatriculation = $("#PersonneMoraleLabNumeroImmatriculation" + id).dxTextBox("instance").option('value');

    var hasNumeroImmatriculation = numeroImmatriculation != null && $.trim(numeroImmatriculation) != "";
    var typeRelationAffaire = $("#PersonneMoraleLabTypeRelationAffaireLabId" + id).dxSelectBox("instance").option('displayValue');

    $("#identityMoralPerson" + id).text((raisonSociale != null ? raisonSociale : '') + (hasNumeroImmatriculation ? ' (' : '') + (numeroImmatriculation != null ? numeroImmatriculation : '') + (hasNumeroImmatriculation ? ')' : '') + (typeRelationAffaire != null ? ' - ' + typeRelationAffaire : ''));

}

function updateHeaderIdentityPhysicalPerson(id) {
    var prenoms = $("#PersonnePhysiqueLabPrenoms" + id).dxTextBox("instance").option('value');
    var nomNaissance = $("#PersonnePhysiqueLabNomNaissance" + id).dxTextBox("instance").option('value');
    var dateNaissance = $("#PersonnePhysiqueLabDateNaissance" + id).dxDateBox("instance").option('value');
    var typeRelationAffaire = $("#PersonnePhysiqueLabTypeRelationAffaireLabId" + id).dxSelectBox("instance").option('displayValue');
    var formattedDate = "";
    var hasDateNaissance = false;
    if (dateNaissance != null && dateNaissance != "") {
        dateNaissance = new Date(dateNaissance)
        if (dateNaissance.getDate() != null && $.trim(dateNaissance)) {

            var dd = String(dateNaissance.getDate()).padStart(2, '0');
            var mm = String(dateNaissance.getMonth() + 1).padStart(2, '0');
            var yyyy = dateNaissance.getFullYear();
            if (culture == "fr") {
                formattedDate = dd + '/' + mm + '/' + yyyy;
            }
            if (culture == "en") {
                formattedDate = yyyy + '-' + mm + '-' + dd;
            }
            hasDateNaissance = true;
        }
    }

    $("#identityPhysicalPerson" + id).text((prenoms != null ? prenoms : '') + ' ' + (nomNaissance != null ? nomNaissance : '') + (hasDateNaissance ? ' (' : '') + (formattedDate) + (hasDateNaissance ? ')' : '') + (typeRelationAffaire != null ? ' - ' + typeRelationAffaire : ''));
}


function gridDeleteDsSelected_Lab_OnClick() {
    showConfirmationDeleteDsLabDialog();
}

function gridExportDsSelected_Lab_OnClick() {
    var rowSelected = getSelectedRowDossier();
    if (rowSelected) {
        window.location.href = '/' + culture + '/Lab/ServiceLab/DeclarationTracfinExportXml/?cryptedId=' + rowSelected.CryptedId + '&code=' + rowSelected.CodeUnique
    }
}

function gridExportDsV2Selected_Lab_OnClick() {
    var rowSelected = getSelectedRowDossier();
    if (rowSelected) {
        $.ajax({
            method: "GET",
            cache: false,
            data: { cryptedId: rowSelected.CryptedId, code: rowSelected.CodeUnique },
            url: '/' + culture + '/Lab/ServiceLab/HasDeclarationV2TracfinExportXml',
        }).done(function (data) {
            if (data) {
                window.location.href = '/' + culture + '/Lab/ServiceLab/DeclarationV2TracfinExportXml/?cryptedId=' + rowSelected.CryptedId + '&code=' + rowSelected.CodeUnique
            }
            else {
                DevExpress.ui.notify(_labTranslatableMessageNoGeneratedXml, "error");
            }
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
        });
    }
    else {
        DevExpress.ui.notify(_labTranslatableMessageDsLabRequired, "error");
    }
}

function gridShowDetailDsSelected_Lab_OnClick() {
    var selectedRows = getSelectedRowDossier();
    if (selectedRows !== undefined) {
        $.ajax({
            method: "GET",
            cache: false,
            data: { id: selectedRows.CryptedId },
            url: '/' + culture + '/Lab/ServiceLab/VisualisationDS',
        }).done(function (data) {
            if (data) {
                showVisualisationDSModalPreview(data);
            }
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
        });
    }
}

function showVisualisationDSModalPreview(contentHtml) {
    $("#VisualisationDSModalPreview").dxPopup({
        contentTemplate: function () {
            const content = $("<div />").append(contentHtml);
            content.dxScrollView({
                height: "100%",
                width: "100%"
            });
            return content;
        },
    });
    $("#VisualisationDSModalPreview").dxPopup('instance').show();
}
function gridPrendreEnChargeSelected_Lab_OnClick() {
    showConfirmationPrendreEnChargeLabDialog();
}

function setRowColorDatagridLab(rowInfo) {
    if (rowInfo.rowType === 'data') {
        if (rowInfo.data.EVisibleDossier === '5')
            rowInfo.rowElement.css('background', '#FF5E4D');
    }
}

function showUnauthorizedAccessDialog() {
    var UnauthorizedAccessDialog = DevExpress.ui.dialog.custom({
        title: "Authorisation",
        message: "Vous ne pouvez pas ouvrir ce dossier.Veillez demander les droits nécessaires",
        buttons: [{ text: "Fermer", onClick: function () { return false; } }]
    });
    UnauthorizedAccessDialog.show();
}

function enableOrDisableEffectiveAt(idEffectiveAt, checkbox) {
    $('#' + idEffectiveAt).prop('disabled', true);
    if (checkbox.checked)
        $('#' + idEffectiveAt).prop('disabled', false);
}


function updateInputFileName(id) {
    let fileName = htmlEncode($('#' + id).val()).split('\\').pop();
    $('#' + id).next('.custom-file-label').addClass("selected").html(fileName);
    if ($('#' + id).hasClass("noFileSelected")) {
        var rowCountAttachment = $('.attachmentRow').length + 1;
        $('#' + id).removeClass("noFileSelected");
    }
}

function clearPopUpContent() {
    var popup = $("#labPopup").dxPopup('instance')
    popup.option("contentTemplate", '');
}

function origine_lab_OnValueChanged() {
    if ($("#OrigineLabId").dxSelectBox("instance").option('selectedItem') != null) {
        var fonctionLabId = $("#OrigineLabId").dxSelectBox("instance").option('selectedItem').FonctionLabId;
        var callBack = function () {
            if (fonctionLabId == 2 || fonctionLabId == 3) {
                if ($("#dossier-lab-scenario-container").hasClass("dx-hidden")) {
                    $("#dossier-lab-scenario-container-body").addClass("dx-hidden");
                    $("#dossier-lab-scenario-container").removeClass("dx-hidden");
                    $("#loader-dossier-lab-scenario-container").removeClass("dx-hidden");
                    $("#loader-dossier-lab-scenario-container").html('<div class="d-flex"><div id="small-indicator-dossier-lab-scenario-container"></div> </div>');
                    $("#small-indicator-dossier-lab-scenario-container").dxLoadIndicator({
                        height: 20,
                        width: 20
                    });


                    $.ajax({
                        method: "GET",
                        cache: false,
                        dataType: "html",
                        contentType: "application/json; charset=utf-8",
                        url: '/' + culture + '/Lab/ServiceLab/AddDossierLabScenarioForm?order=0'
                    }).done(function (data) {
                        $("#dossier-lab-scenario-items-container").html(data);
                        $("#dossier-lab-scenario-container-body").removeClass("dx-hidden");
                        $("#loader-dossier-lab-scenario-container").addClass("dx-hidden");



                    }).fail(function (e) {
                        if (e.status == 401) {
                            var popup = $("#modalReconnect").dxPopup('instance');
                            popup.show();
                            return;
                        }
                        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
                    }).always(function (e) {

                    });
                }

                $("#dossier-lab-scenario-container").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $("#dossier-lab-scenario-container").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
            }
            else {
                $("#dossier-lab-scenario-items-container").html("");
                $("#dossier-lab-scenario-container").addClass("dx-hidden");
            }

        }

        if (fonctionLabId == 1 || fonctionLabId == 3) {

            if ($("#dossier-lab-operation-container").hasClass("dx-hidden")) {
                $("#dossier-lab-operation-container-body").addClass("dx-hidden");
                $("#dossier-lab-operation-container").removeClass("dx-hidden");
                $("#loader-dossier-lab-operation-container").removeClass("dx-hidden");
                $("#loader-dossier-lab-operation-container").html('<div class="d-flex"><div id="small-indicator-dossier-lab-operation"></div> </div>');
                $("#small-indicator-dossier-lab-operation").dxLoadIndicator({
                    height: 20,
                    width: 20
                });

                $.ajax({
                    method: "GET",
                    cache: false,
                    dataType: "html",
                    contentType: "application/json; charset=utf-8",
                    url: '/' + culture + '/Lab/ServiceLab/AddDossierLabOperationForm?order=0'
                }).done(function (data) {
                    $("#dossier-lab-operation-items-container").html(data);
                    $("#dossier-lab-operation-container-body").removeClass("dx-hidden");
                    $("#loader-dossier-lab-operation-container").addClass("dx-hidden");
                }).fail(function (e) {
                    if (e.status == 401) {
                        var popup = $("#modalReconnect").dxPopup('instance');
                        popup.show();
                        return;
                    }
                    DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
                }).always(function (e) {
                    callBack();
                    hideLoadPanel();
                });
            }

            else {
                callBack();

            }

            $("#dossier-lab-operation-container").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
            $("#dossier-lab-operation-container").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
        }
        else {
            $("#dossier-lab-operation-items-container").html("");
            $("#dossier-lab-operation-container").addClass("dx-hidden");
            callBack();
        }
    }
}

function origine_lab_OnInitialized(cryptedDossierId) {
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');
    if (directionId > 0) {
        $.ajax({
            method: "GET",
            dataType: 'json',
            cache: false,
            data: { cryptedDossierId: cryptedDossierId, directionId: directionId },
            url: '/' + culture + '/Lab/ServiceLab/GetOriginesByDossierId',
        }).done(function (data) {
            $("#OrigineLabId").dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        loadMode: "raw",
                        load: function () {
                            return data;
                        }
                    })
                })
            });

        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }

        }).always(function (e) {
        });
    }
}

function HideDocumentAttechment(idAttachment) {
    $("#Precision_if_OtherTypeDiv" + idAttachment).addClass("dx-hidden");
    $("#IdentityNumberDiv" + idAttachment).addClass("dx-hidden");
    $("#AuthorityReleaseDiv" + idAttachment).addClass("dx-hidden");
    $("#SourceFileDocumentDiv" + idAttachment).addClass("dx-hidden");
    $("#CountryRelaseDiv" + idAttachment).addClass("dx-hidden");
    $("#DateDocumentDiv" + idAttachment).addClass("dx-hidden");
    $("#DateValidityDiv" + idAttachment).addClass("dx-hidden");
    $("#LevelCNIDiv" + idAttachment).addClass("dx-hidden");
}
function ResetDocumentAttechment(idAttachment) {
    if (!$("#CountryRelaseDiv" + idAttachment).hasClass("dx-hidden")) {
        var mesSelector = $("#CountryRelaseId" + idAttachment).dxSelectBox("instance");
        if (mesSelector) mesSelector.option('value', 0);
    }
    if (!$("#DateValidityDiv" + idAttachment).hasClass("dx-hidden")) {
        var mesSelector1 = $("#DateValidity" + idAttachment).dxDateBox("instance");
        if (mesSelector1) mesSelector1.option('value', "");
    }

    if (!$("#DateDocumentDiv" + idAttachment).hasClass("dx-hidden")) {
        var mesSelector2 = $("#DateDocument" + idAttachment).dxDateBox("instance");
        if (mesSelector2) mesSelector2.option('value', "");
    }
    if (!$("#LevelCNIDiv" + idAttachment).hasClass("dx-hidden")) {
        var mesSelector3 = $("#LevelCNI" + idAttachment).dxCheckBox("instance");
        if (mesSelector3) mesSelector3.option('value', null);
    }

    $("#IdentityNumber" + idAttachment).val("");
    $("#AuthorityRelease" + idAttachment).val("");
    $("#Precision_if_OtherType" + idAttachment).val("");
    $("#SourceFileDocument" + idAttachment).val("");
}
function resetAttachmentHiddenFields(idAttachment) {
    if ($("#Precision_if_OtherTypeDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#Precision_if_OtherType" + idAttachment).dxTextBox("instance").reset();
    }

    if ($("#IdentityNumberDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#IdentityNumber" + idAttachment).dxTextBox("instance").reset();
    }

    if ($("#AuthorityReleaseDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#AuthorityRelease" + idAttachment).dxTextBox("instance").reset();
    }

    if ($("#SourceFileDocumentDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#SourceFileDocument" + idAttachment).dxTextBox("instance").reset();
    }

    if ($("#CountryRelaseDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#CountryRelaseId" + idAttachment).dxSelectBox("instance").reset();
    }

    if ($("#DateDocumentDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#DateDocument" + idAttachment).dxDateBox("instance").reset();
    }

    if ($("#DateValidityDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#DateValidity" + idAttachment).dxDateBox("instance").reset();
    }

    if ($("#LevelCNIDiv" + idAttachment).hasClass("dx-hidden")) {
        $("#LevelCNI" + idAttachment).dxCheckBox("instance").reset();
    }
}
function CategorieDocument_OnSelectionChanged(e) {
    var idAttachment = e.element.attr("id").replace(/^\D+/g, '');

    disablePrecisionFieldsValidators(idAttachment);

    HideDocumentAttechment(idAttachment);

    var typeDocumentLabSelector = "#TypeDocumentLabId" + idAttachment;
    var typeDocumentLabContainerSelector = "#TypeDocumentLabDiv" + idAttachment;

    $(typeDocumentLabContainerSelector).addClass("dx-hidden");

    if (e.selectedItem && e.selectedItem.Id == 8) {
        var validationGroup = getAttachmentsPopupFormValidationGroup(idAttachment);
        var validationRules = [
            {
                type: "required",
            }];

        showFieldWithValidationRules(typeDocumentLabContainerSelector, typeDocumentLabSelector, validationGroup, validationRules);
    }
    validateDossierLabAttachmentForm(idAttachment);
}
function GetAttachmentId(controlId) {
    return controlId.replace(/^\D+/g, '');
}
function CategorieDocumentDossierLab_OnValueChanged(id, bResetTypeDocument) {
    var idAttachment = GetAttachmentId(id);

    disablePrecisionFieldsValidators(idAttachment);

    HideDocumentAttechment(idAttachment);

    var typeDocumentLabSelector = "#TypeDocumentLabId" + idAttachment;
    var typeDocumentLabContainerSelector = "#TypeDocumentLabDiv" + idAttachment;

    disableFieldValidators(typeDocumentLabSelector);

    $(typeDocumentLabContainerSelector).addClass("dx-hidden");

    var value = $("#" + id).dxSelectBox("instance").option("value");

    if (value == 8) {
        var validationGroup = getAttachmentsPopupFormValidationGroup(idAttachment);
        var validationRules = [
            {
                type: "required",
            }];

        showFieldWithValidationRules(typeDocumentLabContainerSelector, typeDocumentLabSelector, validationGroup, validationRules);
    }

    $("#DocumentPersonneAssocieDiv" + idAttachment).toggleClass(_commonDxHiddenClassName, shouldPersonneAssocieeBeHidden(value));

    if (bResetTypeDocument) {
        $(typeDocumentLabSelector).dxSelectBox("instance").reset();
    }

    validateDossierLabAttachmentForm(idAttachment);
}

function shouldPersonneAssocieeBeHidden(categoryId) {
    return !_labIsDeclarationTracfin || categoryId != _labEnvoiTracfinCategorieDocumentId;
}

function disablePrecisionFieldsValidators(idAttachment) {
    var dateDocumentSelector = "#DateDocument" + idAttachment;
    var identityNumberSelector = "#IdentityNumber" + idAttachment;
    var authorityReleaseSelector = "#AuthorityRelease" + idAttachment;
    var countryRelaseSelector = "#CountryRelaseId" + idAttachment;
    var dateValiditySelector = "#DateValidity" + idAttachment;
    var sourceFileDocumentSelector = "#SourceFileDocument" + idAttachment;
    var precision_if_OtherTypeSelector = "#Precision_if_OtherType" + idAttachment;
    var levelCNISelector = "#LevelCNI" + idAttachment;

    disableFieldValidators(dateDocumentSelector);
    disableFieldValidators(identityNumberSelector);
    disableFieldValidators(authorityReleaseSelector);
    disableFieldValidators(countryRelaseSelector);
    disableFieldValidators(dateValiditySelector);
    disableFieldValidators(levelCNISelector);
    disableFieldValidators(sourceFileDocumentSelector);
    disableFieldValidators(precision_if_OtherTypeSelector);
}
function TypeDocumentDossierLab_OnValueChanged(id) {
    var idAttachment = GetAttachmentId(id);

    disablePrecisionFieldsValidators(idAttachment);

    HideDocumentAttechment(idAttachment);

    var dateDocumentSelector = "#DateDocument" + idAttachment;
    var dateDocumentContainerSelector = "#DateDocumentDiv" + idAttachment;
    var identityNumberSelector = "#IdentityNumber" + idAttachment;
    var identityNumberContainerSelector = "#IdentityNumberDiv" + idAttachment;
    var authorityReleaseSelector = "#AuthorityRelease" + idAttachment;
    var authorityReleaseContainerSelector = "#AuthorityReleaseDiv" + idAttachment;
    var countryRelaseSelector = "#CountryRelaseId" + idAttachment;
    var countryRelaseContainerSelector = "#CountryRelaseDiv" + idAttachment;
    var dateValiditySelector = "#DateValidity" + idAttachment;
    var dateValidityContainerSelector = "#DateValidityDiv" + idAttachment;
    var sourceFileDocumentSelector = "#SourceFileDocument" + idAttachment;
    var sourceFileDocumentContainerSelector = "#SourceFileDocumentDiv" + idAttachment;
    var precision_if_OtherTypeSelector = "#Precision_if_OtherType" + idAttachment;
    var precision_if_OtherTypeContainerSelector = "#Precision_if_OtherTypeDiv" + idAttachment;
    var levelCNISelector = "#LevelCNI" + idAttachment;
    var levelCNIContainerSelector = "#LevelCNIDiv" + idAttachment;

    toggleDisplayPersonneAssocieeRequired(idAttachment);

    var selectedItem = $("#" + id).dxSelectBox("instance").option("selectedItem");

    if (selectedItem == null || !selectedItem || !selectedItem.Code) {
        resetAttachmentHiddenFields(idAttachment);
        return;
    }

    var validationGroup = getAttachmentsPopupFormValidationGroup(idAttachment);

    var validationRules = [
        {
            type: "required",
        }];

    var selectedType = selectedItem.Code;

    if (selectedType == "TA1") {
        showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
    }
    else if (selectedType == "TA2") {
        showFieldWithValidationRules(identityNumberContainerSelector, identityNumberSelector, validationGroup, validationRules);
        showFieldWithValidationRules(authorityReleaseContainerSelector, authorityReleaseSelector, validationGroup, validationRules);
        showFieldWithValidationRules(countryRelaseContainerSelector, countryRelaseSelector, validationGroup, validationRules);
        showFieldWithValidationRules(dateValidityContainerSelector, dateValiditySelector, validationGroup, validationRules);
        showFieldWithValidationRules(levelCNIContainerSelector, levelCNISelector, null, null);
    }
    else if (selectedType == "TA3" || selectedType == "TA5" || selectedType == "TA9") {
        showFieldWithValidationRules(identityNumberContainerSelector, identityNumberSelector, validationGroup, validationRules);
        showFieldWithValidationRules(authorityReleaseContainerSelector, authorityReleaseSelector, validationGroup, validationRules);
        showFieldWithValidationRules(countryRelaseContainerSelector, countryRelaseSelector, validationGroup, validationRules);
        showFieldWithValidationRules(dateValidityContainerSelector, dateValiditySelector, validationGroup, validationRules);
    }
    else if (selectedType == "TA10") {
        showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
        showFieldWithValidationRules(sourceFileDocumentContainerSelector, sourceFileDocumentSelector, validationGroup, validationRules);
    }
    else if (selectedType == "TA17" || selectedType == "TA19") {
        showFieldWithValidationRules(precision_if_OtherTypeContainerSelector, precision_if_OtherTypeSelector, validationGroup, validationRules);
        showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
    }
    else {
        showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
    }

    resetAttachmentHiddenFields(idAttachment);
}

function toggleDisplayPersonneAssocieeRequired(idAttachment) {
    toggleDisplayFieldRequired("PersonneAssocieId" + idAttachment, isPersonneAssocieeRequired(idAttachment));
}

function TypeDocumentLab_OnSelectionChanged(e) {
    var idAttachment = e.element.attr("id").replace(/^\D+/g, '');

    disablePrecisionFieldsValidators(idAttachment);

    ResetDocumentAttechment(idAttachment);

    HideDocumentAttechment(idAttachment);

    var dateDocumentSelector = "#DateDocument" + idAttachment;
    var dateDocumentContainerSelector = "#DateDocumentDiv" + idAttachment;
    var identityNumberSelector = "#IdentityNumber" + idAttachment;
    var identityNumberContainerSelector = "#IdentityNumberDiv" + idAttachment;
    var authorityReleaseSelector = "#AuthorityRelease" + idAttachment;
    var authorityReleaseContainerSelector = "#AuthorityReleaseDiv" + idAttachment;
    var countryRelaseSelector = "#CountryRelaseId" + idAttachment;
    var countryRelaseContainerSelector = "#CountryRelaseDiv" + idAttachment;
    var dateValiditySelector = "#DateValidity" + idAttachment;
    var dateValidityContainerSelector = "#DateValidityDiv" + idAttachment;
    var sourceFileDocumentSelector = "#SourceFileDocument" + idAttachment;
    var sourceFileDocumentContainerSelector = "#SourceFileDocumentDiv" + idAttachment;
    var precision_if_OtherTypeSelector = "#Precision_if_OtherType" + idAttachment;
    var precision_if_OtherTypeContainerSelector = "#Precision_if_OtherTypeDiv" + idAttachment;
    var levelCNISelector = "#LevelCNI" + idAttachment;
    var levelCNIContainerSelector = "#LevelCNIDiv" + idAttachment;

    var selectedType = e.selectedItem && e.selectedItem.Code;
    if (selectedType) {

        var validationGroup = getAttachmentsPopupFormValidationGroup(idAttachment);
        var validationRules = [
            {
                type: "required",
            }];

        if (selectedType == "TA1") {
            showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
        }
        else if (selectedType == "TA2") {
            showFieldWithValidationRules(identityNumberContainerSelector, identityNumberSelector, validationGroup, validationRules);
            showFieldWithValidationRules(authorityReleaseContainerSelector, authorityReleaseSelector, validationGroup, validationRules);
            showFieldWithValidationRules(countryRelaseContainerSelector, countryRelaseSelector, validationGroup, validationRules);
            showFieldWithValidationRules(dateValidityContainerSelector, dateValiditySelector, validationGroup, validationRules);
            showFieldWithValidationRules(levelCNIContainerSelector, levelCNISelector, null, null);

            if (!$(countryRelaseContainerSelector).hasClass("dx-hidden")) {
                var mesSelector = $(countryRelaseSelector).dxSelectBox("instance");
                if (mesSelector) mesSelector.option('value', 0);
            }
        }
        else if (selectedType == "TA3" || selectedType == "TA5" || selectedType == "TA9") {
            showFieldWithValidationRules(identityNumberContainerSelector, identityNumberSelector, validationGroup, validationRules);
            showFieldWithValidationRules(authorityReleaseContainerSelector, authorityReleaseSelector, validationGroup, validationRules);
            showFieldWithValidationRules(countryRelaseContainerSelector, countryRelaseSelector, validationGroup, validationRules);
            showFieldWithValidationRules(dateValidityContainerSelector, dateValiditySelector, validationGroup, validationRules);
        }
        else if (selectedType == "TA10") {
            showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
            showFieldWithValidationRules(sourceFileDocumentContainerSelector, sourceFileDocumentSelector, validationGroup, validationRules);
        }
        else if (selectedType == "TA17" || selectedType == "TA19") {
            showFieldWithValidationRules(precision_if_OtherTypeContainerSelector, precision_if_OtherTypeSelector, validationGroup, validationRules);
            showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
        }
        else {
            showFieldWithValidationRules(dateDocumentContainerSelector, dateDocumentSelector, validationGroup, validationRules);
        }
    }
}
function initAuthorityRelease(levelCNICheckBoxId) {
    var idAttachment = GetAttachmentId(levelCNICheckBoxId);

    if ($("#LevelCNIDiv" + idAttachment).hasClass("dx-hidden")) return;

    var checked = $("#" + levelCNICheckBoxId).dxCheckBox("instance").option("value");

    if (checked == true) {
        $("#AuthorityRelease" + idAttachment).dxValidator("instance").option("validationRules", []);
        $("#AuthorityRelease" + idAttachment).dxTextBox({ value: null })
        $("#AuthorityReleaseDiv" + idAttachment).addClass("dx-hidden");
    }
    else {
        $("#AuthorityRelease" + idAttachment).dxValidator({
            validationRules: [{
                type: 'required',
                message: '@messageErrorAuthorityReleaseRequired'
            }]
        });
        $("#AuthorityReleaseDiv" + idAttachment).removeClass("dx-hidden");
    }
}
function showRequiredField(containerSelector, fieldSelector, validationGroup) {
    var validationRules = [
        {
            type: "required",
        }];
    showFieldWithValidationRules(containerSelector, fieldSelector, validationGroup, validationRules);
}
function showFieldWithValidationRules(containerSelector, fieldSelector, validationGroup, validationRules, asterisk) {
    $(containerSelector).removeClass("dx-hidden");

    $(fieldSelector).dxValidator({
        validationGroup: validationGroup,
        validationRules:
            validationRules && validationRules.map(function (rule) {
                if (!rule.type) {
                    return;
                }
                return {
                    type: rule.type,
                    message: $(fieldSelector).attr("data-error-" + rule.type)
                };
            })
    });

    if (!asterisk || validationRules == null || !validationRules || validationRules.length == 0)
        return;

    $(fieldSelector).addClass('mandatory');
    asterisk.show();
}
function addDossierLabAttachment(index) {
    if (!validateAttachmentsPopupForm(index)) {
        return;
    }

    var documentNameTextBox = $("#DocumentName" + index).dxTextBox("instance");
    var documentNameTextBoxValue = documentNameTextBox.option("value");
    var documentName;

    if (!isNullOrUndefined(documentNameTextBoxValue))
        documentName = htmlEncode(documentNameTextBoxValue.trim());

    if (!documentName || !documentName.trim()) {

        documentName = getAttachmentFileInfo(index).fileName;

        if (documentName && documentName.trim()) {
            documentName = documentName.toUpperCase().trim();
        }
        else {
            documentName = "";
        }
        documentNameTextBox.option("value", documentName);
        $("input[name='Attachments\[" + index + "\].DocumentName']").attr("value", documentName);
    }

    if ($("#FileUploader" + index).prop('files').length == 0) {
        restoreAttachmentFile(index);
    }

    $("#attachment" + index).addClass('dx-hidden');
    $("#attachmentsTable").removeClass('dx-hidden');
    $("#btn-container" + index).remove();
    $('#buttons-container').remove();
    $('#modalAttachments').dxPopup('hide');

    refreshAttachment(index);
}
function addAttachment(index) {
    if (!validateAttachmentsPopupForm(index)) {
        return;
    }

    var documentNameTextBox = $("#DocumentName" + index).dxTextBox("instance");
    var documentName = documentNameTextBox.option("value");
    if (!documentName || !documentName.trim()) {
        documentName = $("#FileUploader" + index).prop('files')[0].name;
        if (documentName && documentName.trim()) {
            documentName = documentName.toUpperCase().trim();
        }
        else {
            documentName = "";
        }
        documentNameTextBox.option("value", documentName);
        $("input[name='Attachments\[" + index + "\].DocumentName']").attr("value", documentName);
    }

    $("#attachment" + index).addClass('dx-hidden');
    $("#attachmentsTable").removeClass('dx-hidden');
    $("#btn-container" + index).remove();
    $('#buttons-container').remove();
    $('#modalAttachments').dxPopup('hide');

    refreshDataAttachment();
}
function refreshAttachment(i) {
    var CategorieDocumentId = $("#CategorieDocumentId" + i).dxSelectBox("instance").option('value');
    var CategorieDocumentName = htmlEncode($("#CategorieDocumentId" + i).dxSelectBox("instance").option('text'));
    var DocumentName = $("#DocumentName" + i).dxTextBox("instance").option("value");
    var IdentityNumberTextBox = $("#IdentityNumber" + i).dxTextBox("instance");
    var IdentityNumber = "";
    var AuthorityReleaseTextBox = $("#AuthorityRelease" + i).dxTextBox("instance");
    var AuthorityRelease = "";
    var Precision_if_OtherTypeTextBox = $("#Precision_if_OtherType" + i).dxTextBox("instance");
    var Precision_if_OtherType = "";
    var SourceFileDocumentTextBox = $("#SourceFileDocument" + i).dxTextBox("instance");
    var SourceFileDocument = "";

    var personneAssocieSelectBox = $("#PersonneAssocieId" + i).dxSelectBox("instance");
    var PersonneAssocieId = personneAssocieSelectBox.option('value');
    var PersonneAssocie = personneAssocieSelectBox.option('text');

    var fileInfo = getAttachmentFileInfo(i);
    var FileSize = fileInfo.fileSize;
    var FileName = fileInfo.fileName;

    var TypeDocumentLabId;
    var TypeDocumentLabName = "";
    var TypeDocumentLabCode = "";
    var CountryRelase;
    var CountryRelaseId = "";
    var CountryRelaseName = "";
    var DateValidity = "";
    var DateDocument = "";
    var LevelCNI = "";


    if (CategorieDocumentId === 8) {

        var TypeDocumentLabSelectBox = $("#TypeDocumentLabId" + i).dxSelectBox("instance");
        if (!isNullOrUndefined(TypeDocumentLabSelectBox)) {
            TypeDocumentLabId = TypeDocumentLabSelectBox.option('value');
            TypeDocumentLabName = TypeDocumentLabSelectBox.option('text');

            var selectedTypeDocumentLab = TypeDocumentLabSelectBox.option('selectedItem');
            if (!isNullOrUndefined(selectedTypeDocumentLab)) {
                TypeDocumentLabCode = selectedTypeDocumentLab.Code;
            }
        }

        if (!$("#CountryRelaseDiv" + i).hasClass("dx-hidden")) {
            CountryRelase = $("#CountryRelaseId" + i).dxSelectBox("instance");
        }
        if (!$("#DateValidityDiv" + i).hasClass("dx-hidden")) {
            var mesSelector1 = $("#DateValidity" + i).dxDateBox("instance");
            if (mesSelector1) {
                var formattedDateValidity = moment(mesSelector1.option('value')).format("DD/MM/YYYY");
                DateValidity = formattedDateValidity !== "Invalid date" && formattedDateValidity !== undefined ? formattedDateValidity : "";
            }
        }

        if (!$("#DateDocumentDiv" + i).hasClass("dx-hidden")) {
            var mesSelector2 = $("#DateDocument" + i).dxDateBox("instance");
            if (mesSelector2) {
                var formattedDateDocument = moment(mesSelector2.option('value')).format("DD/MM/YYYY");
                DateDocument = formattedDateDocument !== "Invalid date" && formattedDateDocument !== undefined ? formattedDateDocument : "";
            }
        }
        if (!$("#LevelCNIDiv" + i).hasClass("dx-hidden")) {
            var mesSelector3 = $("#LevelCNI" + i).dxCheckBox("instance");
            if (mesSelector3) LevelCNI = mesSelector3.option('value');
        }

        IdentityNumber = !isNullOrUndefined(IdentityNumberTextBox) ? IdentityNumberTextBox.option("value") : "";
        AuthorityRelease = !isNullOrUndefined(AuthorityReleaseTextBox) ? AuthorityReleaseTextBox.option("value") : "";
        Precision_if_OtherType = !isNullOrUndefined(Precision_if_OtherTypeTextBox) ? Precision_if_OtherTypeTextBox.option("value") : "";
        SourceFileDocument = !isNullOrUndefined(SourceFileDocumentTextBox) ? SourceFileDocumentTextBox.option("value") : "";
        CountryRelaseId = !isNullOrUndefined(CountryRelase) ? CountryRelase.option('value') : "";
        CountryRelaseName = !isNullOrUndefined(CountryRelase) ? CountryRelase.option('text') : "";
    }

    var attachmentToUpsert = {
        index: i,
        DocumentName,
        CategorieDocumentId,
        CategorieDocumentName,
        TypeDocumentLabId,
        TypeDocumentLabCode,
        TypeDocumentLabName,
        CountryRelaseId,
        CountryRelaseName,
        FileName,
        IdentityNumber,
        AuthorityRelease,
        Precision_if_OtherType,
        SourceFileDocument,
        DateValidity,
        DateDocument,
        LevelCNI,
        FileSize,
        PersonneAssocieId,
        PersonneAssocie
    };

    upsertDataGridAttachmentItem(attachmentToUpsert);

    upsertFormAttachmentsCollectionItem(attachmentToUpsert);

}

function getAttachmentFileInfo(i) {
    var uploadedFile = getUploadedFile(i);

    if (uploadedFile) {
        return {
            fileName: uploadedFile.name,
            fileSize: uploadedFile.size
        };
    }

    var fileName = $("#" + _labFormAttachmentsCollectionItemFieldIdPrefix + "_FileName" + i).val();
    var size = parseInt($("#" + _labFormAttachmentsCollectionItemFieldIdPrefix + "_FileSize" + i).val(), 10);

    return {
        fileName,
        fileSize: isNaN(size) ? 0 : size
    }
}

function upsertDataGridAttachmentItem(attachmentToUpsert) {
    var attachmentToUpsertIndex = attchmentDataJson
        .findIndex(d => d.index == attachmentToUpsert.index);

    if (attachmentToUpsertIndex >= 0) {
        var attachmentFiles = getAttachmentFiles("FileUploader" + attachmentToUpsertIndex);

        if (attachmentFiles.length == 0) {
            // Copy the old file Id if no new file was attached
            attachmentToUpsert.Id = attchmentDataJson[attachmentToUpsertIndex].Id
        }

        attchmentDataJson[attachmentToUpsertIndex] = attachmentToUpsert;
    }
    else {
        attchmentDataJson.push(attachmentToUpsert);
    }

    refreshAttachmentsDataGrid(attchmentDataJson);
}
function refreshAttachmentsDataGrid(dataSource) {
    var gridAttachments = $("#gridAttachmentsLab").dxDataGrid("instance");
    gridAttachments.option("dataSource", dataSource);
    gridAttachments.refresh(true);
}
function upsertFormAttachmentsCollectionItem(obj) {
    if ($("#createOrUpdateDossierLab_formAttachmentsCollection [id$='" + obj.index + "']").length == 0) {
        addNewFormAttachmentsCollectionItem(obj.index, () => refreshFormsAttachmentsCollectionItem(obj));
    }
    else {
        refreshFormsAttachmentsCollectionItem(obj);
    }
}
function refreshFormsAttachmentsCollectionItem(newObj) {
    if (isNullOrUndefined(newObj)) return;

    if (isNullOrUndefined(newObj.index)) return;
    Object.keys(newObj).forEach(function (key) {
        var fieldSelector = "#" + _labFormAttachmentsCollectionItemFieldIdPrefix + "_" + key + newObj.index;

        $(fieldSelector).val(isNullOrUndefined(newObj[key]) ? "" : newObj[key]);
    });
}

function addNewFormAttachmentsCollectionItem(i, callback) {
    showLoadPanel();

    var cryptedDossierId = $("#CryptedId").val();

    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        url: '/' + culture + '/Lab/ServiceLab/AddFormAttachmentsCollectionItem',
        data: { order: i, cryptedDossierId }
    }).done(function (data) {
        $("#createOrUpdateDossierLab_formAttachmentsCollection").append(data);

        if (!isNullOrUndefined(callback)) callback();
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorOccured, "error");
    }).always(function () {
        hideLoadPanel();
    });
}

function validateAttachmentsPopupForm(index) {
    var validationGroup = getAttachmentsPopupFormValidationGroup(index);
    var validationGroupConfig = DevExpress.validationEngine.getGroupConfig(validationGroup);
    if (validationGroupConfig) {
        var validationResult = DevExpress.validationEngine.validateGroup(validationGroup);
        return validationResult.isValid;
    }
    return true;
}
function initDataAttachments(cryptedDossierId) {

    oldAttchmentDataJson = [];
    $.ajax({
        method: "GET",
        dataType: 'json',
        cache: false,
        data: { cryptedDossierId: cryptedDossierId },
        url: '/' + culture + '/Lab/ServiceLab/GetAttachmentByDossierId',
    }).done(function (data) {
        if (data != undefined) {
            data.forEach((item, i) => {

                var DateValidity = moment(item.DateValidity).format("DD/MM/YYYY");
                var DateDocument = moment(item.DateDocument).format("DD/MM/YYYY");

                oldAttchmentDataJson.push({
                    "index": -1,
                    "Id": item.CryptedDocumentLabId,
                    "DocumentName": item.DocumentName == undefined ? "" : item.DocumentName,
                    "CategorieDocumentId": item.CategorieDocument !== null ? item.CategorieDocument.Id : "",
                    "CategorieDocumentName": item.CategorieDocument !== null ? item.CategorieDocument.NameFr : "",
                    "TypeDocumentLabId": item.TypeDocumentLabId,
                    "TypeDocumentLabCode": item.TypeDocumentLab !== null ? item.TypeDocumentLab.Code : "",
                    "TypeDocumentLabName": item.TypeDocumentLab !== null ? item.TypeDocumentLab.NameFr : "",
                    "CountryRelaseId": item.CountryRelase !== null ? item.CountryRelase.Id : "",
                    "CountryRelaseName": item.CountryRelase !== null ? item.CountryRelase.NameFr : "",
                    "FileName": item.FileName == undefined ? "" : item.FileName,
                    "IdentityNumber": item.IdentityNumber == undefined ? "" : item.IdentityNumber,
                    "AuthorityRelease": item.AuthorityRelease == undefined ? "" : item.AuthorityRelease,
                    "SourceFileDocument": item.SourceFileDocument == undefined ? "" : item.SourceFileDocument,
                    "Precision_if_OtherType": item.Precision_if_OtherType == undefined ? "" : item.Precision_if_OtherType,
                    "DateValidity": DateValidity !== "Invalid date" && DateValidity !== undefined ? DateValidity : "",
                    "DateDocument": DateDocument !== "Invalid date" && DateDocument !== undefined ? DateDocument : "",
                    "LevelCNI": item.LevelCNI,
                    "IsDeletedNew": item.IsDeleted,
                    "FileSize": item.DocumentLab && item.DocumentLab.FileSize
                });
            });


            refreshDataAttachment();
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });
}
function initAttachmentsDataGrid(attachments) {
    attchmentDataJson = [];

    attachments.forEach((item, i) => {
        var DateValidity = moment(item.DateValidity).format("DD/MM/YYYY");
        var DateDocument = moment(item.DateDocument).format("DD/MM/YYYY");

        attchmentDataJson.push({
            "index": i,
            "Id": item.CryptedDocumentLabId,
            "DocumentName": item.DocumentName == undefined ? "" : item.DocumentName,
            "CategorieDocumentId": item.CategorieDocumentId,
            "CategorieDocumentName": item.CategorieDocument !== null ? item.CategorieDocument.NameFr : "",
            "TypeDocumentLabId": item.TypeDocumentLabId,
            "TypeDocumentLabCode": item.TypeDocumentLab !== null ? item.TypeDocumentLab.Code : "",
            "TypeDocumentLabName": item.TypeDocumentLab !== null ? item.TypeDocumentLab.NameFr : "",
            "CountryRelaseId": item.CountryRelase !== null ? item.CountryRelase.Id : "",
            "CountryRelaseName": item.CountryRelase !== null ? item.CountryRelase.NameFr : "",
            "FileName": item.FileName == undefined ? "" : item.FileName,
            "IdentityNumber": item.IdentityNumber == undefined ? "" : item.IdentityNumber,
            "AuthorityRelease": item.AuthorityRelease == undefined ? "" : item.AuthorityRelease,
            "SourceFileDocument": item.SourceFileDocument == undefined ? "" : item.SourceFileDocument,
            "Precision_if_OtherType": item.Precision_if_OtherType == undefined ? "" : item.Precision_if_OtherType,
            "DateValidity": DateValidity !== "Invalid date" && DateValidity !== undefined ? DateValidity : "",
            "DateDocument": DateDocument !== "Invalid date" && DateDocument !== undefined ? DateDocument : "",
            "LevelCNI": item.LevelCNI,
            "IsDeletedNew": item.IsDeleted,
            "FileSize": item.DocumentLab && item.DocumentLab.FileSize,
            "PersonneAssocieId": item.PersonneAssocieId,
            "PersonneAssocie": getLibellePersonneAssociee(item.PersonneAssocieId)
        });
    });
    refreshAttachmentsDataGrid(attchmentDataJson);
}
function getLibellePersonneAssociee(id) {
    if (isNullOrUndefined(id))
        return null;

    var tiers = getTiersIntegresDS().filter(t => t.value == id);

    if (tiers.length == 0)
        return null;

    return tiers[0].text;
}
function refreshDataAttachment() {
    var index = $('.attachmentRow').length;

    attchmentDataJson = [];
    attchmentDataJson = attchmentDataJson.concat(oldAttchmentDataJson);

    for (let i = 0; i < index; i++) {
        var CategorieDocumentId = $("#CategorieDocumentId" + i).dxSelectBox("instance").option('value');
        var categorieDocumentName = $("#CategorieDocumentId" + i).dxSelectBox("instance").option('text');
        var documentName = $("#DocumentName" + i).dxTextBox("instance").option("value");
        var IdentityNumber = $("#IdentityNumber" + i).dxTextBox("instance");
        var AuthorityRelease = $("#AuthorityRelease" + i).dxTextBox("instance");
        var Precision_if_OtherType = $("#Precision_if_OtherType" + i).dxTextBox("instance");
        var SourceFileDocument = $("#SourceFileDocument" + i).dxTextBox("instance");
        var fileSize = $("#partialAttachment_fileSize" + i).dxTextBox("instance").option("value");
        var fileName = $("#FileUploader" + i).prop('files')[0].name;

        if (CategorieDocumentId === 8) {

            var typeDocumentLabId;
            var typeDocumentLabName = "";
            var typeDocumentLabCode = "";

            var selectedTypeDocumentSelectBox = $("#TypeDocumentLabId" + i).dxSelectBox("instance");
            var selectedTypeDocument = selectedTypeDocumentSelectBox.option('selectedItem');
            if (!isNullOrUndefined(selectedTypeDocument)) {
                typeDocumentLabId = selectedTypeDocument.Id;
                typeDocumentLabName = selectedTypeDocumentSelectBox.option('text');
                typeDocumentLabCode = selectedTypeDocument.Code
            }

            var DateValidity, DateDocument = ""; var LevelCNI;

            if (!$("#CountryRelaseDiv" + i).hasClass("dx-hidden")) {
                var CountryRelase = $("#CountryRelaseId" + i).dxSelectBox("instance");
            }
            if (!$("#DateValidityDiv" + i).hasClass("dx-hidden")) {
                var mesSelector1 = $("#DateValidity" + i).dxDateBox("instance");
                if (mesSelector1) DateValidity = moment(mesSelector1.option('value')).format("DD/MM/YYYY");
            }

            if (!$("#DateDocumentDiv" + i).hasClass("dx-hidden")) {
                var mesSelector2 = $("#DateDocument" + i).dxDateBox("instance");
                if (mesSelector2) DateDocument = moment(mesSelector2.option('value')).format("DD/MM/YYYY");
            }
            if (!$("#LevelCNIDiv" + i).hasClass("dx-hidden")) {
                var mesSelector3 = $("#LevelCNI" + i).dxCheckBox("instance");
                if (mesSelector3) LevelCNI = mesSelector3.option('value');
            }

            var isDel = $("#IsDeletedNew" + i).val();
            if (isDel !== 'true') {
                attchmentDataJson.push({
                    "index": i,
                    "DocumentName": documentName,
                    "CategorieDocumentId": CategorieDocumentId,
                    "CategorieDocumentName": categorieDocumentName,
                    "TypeDocumentLabId": typeDocumentLabId,
                    "TypeDocumentLabCode": typeDocumentLabCode,
                    "TypeDocumentLabName": typeDocumentLabName,
                    "CountryRelaseId": CountryRelase !== undefined ? CountryRelase.option('value') : "",
                    "CountryRelaseName": CountryRelase !== undefined ? CountryRelase.option('text') : "",
                    "FileName": fileName,
                    "IdentityNumber": IdentityNumber !== undefined ? IdentityNumber.option("value") : "",
                    "AuthorityRelease": AuthorityRelease !== undefined ? AuthorityRelease.option("value") : "",
                    "Precision_if_OtherType": Precision_if_OtherType !== undefined ? Precision_if_OtherType.option("value") : "",
                    "SourceFileDocument": SourceFileDocument !== undefined ? SourceFileDocument.option("value") : "",
                    "DateValidity": DateValidity !== "Invalid date" && DateValidity !== undefined ? DateValidity : "",
                    "DateDocument": DateDocument !== "Invalid date" && DateDocument !== undefined ? DateDocument : "",
                    "LevelCNI": LevelCNI,
                    "IsDeletedNew": $("#SourceFileDocument" + i).val() == true,
                    "FileSize": fileSize
                });
            }
        } else {
            attchmentDataJson.push({
                "index": i,
                "DocumentName": documentName,
                "CategorieDocumentId": CategorieDocumentId,
                "CategorieDocumentName": categorieDocumentName,
                "TypeDocumentLabId": 0,
                "TypeDocumentLabCode": '',
                "TypeDocumentLabName": '',
                "FileName": fileName,
                "IdentityNumber": '',
                "AuthorityRelease": '',
                "SourceFileDocument": '',
                "Precision_if_OtherType": '',
                "CountryRelase": '',
                "DateValidity": '',
                "DateDocument": '',
                "LevelCNI": '',
                "IsDeletedNew": false,
                "FileSize": fileSize
            });
        }
    }

    var gridAttachments = $("#gridAttachmentsLab").dxDataGrid("instance");
    gridAttachments.option("dataSource", attchmentDataJson);
    gridAttachments.refresh(true);
}
function deleteNewAttachmentDetails(e) {
    var clonedItem = $.extend({}, e.row.data);

    var suffix = clonedItem.index;

    var confirmationDeleteFile = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatableMessageDeleteAttachmentConfirmation,
        buttons:
            [{
                text: _commonTranslatableMessageYes,
                onClick: function () { return true; }
            },
            {
                text: _commonTranslatableMessageNo,
                onClick: function () { return false; }
            }]
    });

    confirmationDeleteFile
        .show()
        .done(function (dialogResult) {
            if (dialogResult) {
                $("#" + _labFormAttachmentsCollectionItemFieldIdPrefix + "_IsDeletedNew" + suffix).val('true');
                $("#inputFile" + suffix).remove();
                disableButton("btnDeleteNewAttachment" + suffix);
                $("#trNewAttachment" + suffix).animate({ backgroundColor: jQuery.Color("#fceca8") }, 2000, null, function () {
                    $("#trNewAttachment" + suffix).children('td')
                        .animate({ 'padding-top': 0, 'padding-bottom': 0 })
                        .wrapInner('<div />')
                        .children()
                        .slideUp(function () {
                            $(this).closest('tr').hide();
                        });
                });
                attchmentDataJson = attchmentDataJson.filter(a => a.index != suffix);
                refreshAttachmentsDataGrid(attchmentDataJson);
            }
        });
}
function deleteExistAttachment(index) {
    var confirmationDeleteFile = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatableMessageDeleteAttachmentConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });

    confirmationDeleteFile.show().done(function (dialogResult) {
        if (dialogResult) {
            $('#IsDeleted' + index).val('true');
            disableButton("btnDeleteAttachment" + index);
            disableButton("btnDownloadFile" + index);
            $("#trAttachment" + index).animate({ backgroundColor: jQuery.Color("#fceca8") }, 2000, null, function () {
                $("#trAttachment" + index).children('td')
                    .animate({ 'padding-top': 0, 'padding-bottom': 0 })
                    .wrapInner('<div />')
                    .children()
                    .slideUp(function () {
                        $(this).closest('tr').hide();
                    });
            });
        }
    });
}
function showAddNewAttachmentPopup() {
    showLoadPanel(_commonTranslatableMessageLoading);
    var order = $("." + _labFormAttachmentsCollectionItemClass).length;
    var isDs = checkExistDs;
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        url: '/' + culture + '/Lab/ServiceLab/AddNewAttachmentForm',
        data: { order: order, isDs: isDs }
    }).done(function (data) {
        var popup = $("#modalAttachments").dxPopup('instance');
        popup.registerKeyHandler("escape", function () {
            cancelEditAttachment(order);
        });

        _isEditLabAttachment = false;

        popup.show();

        $("#attachments-container").append(data);

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}
function showFormAttachments() {
    showLoadPanel(_commonTranslatableMessageLoading);
    var order = $('.attachmentRow').length;
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        url: '/' + culture + '/Lab/ServiceLab/AddAttachmentForm',
        data: { order: order }
    }).done(function (data) {
        var popup = $("#modalAttachments").dxPopup('instance');
        popup.registerKeyHandler("escape", function (arg) {
            $("#attachment" + order).remove();
            $("#modalAttachments").dxPopup("hide");
        });
        popup.show();

        $("#attachments-container").append(data);

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}
function showEditAttachmentForm(item) {
    showLoadPanel(_commonTranslatableMessageLoading);

    var order = item.row.data.index;
    var isDs = checkExistDs();


    $.ajax({
        method: "POST",
        cache: false,
        dataType: "html",
        url: '/' + culture + '/Lab/ServiceLab/EditAttachmentForm',
        data: { item: item.row.data, order, isDs: isDs }
    }).done(function (data) {
        var popup = $("#modalAttachments").dxPopup('instance');
        popup.registerKeyHandler("escape", function () {
            cancelEditAttachment(order);
        });

        _isEditLabAttachment = true;

        popup.show();

        backupAttachmentFile(order);

        $("#attachment" + order).remove();

        $("#attachments-container").append(data);

        restoreAttachmentFile(order);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function () {
        hideLoadPanel();
    });
}
function displayAttachmentFileName(index) {
    var attachmentFileName = getAttachmentFileInfo(index).fileName;

    if (isUndefinedOrWhiteSpaces(attachmentFileName))
        return;

    $("#selected-document-name-container" + index).html(htmlEncode(attachmentFileName));
}
function cancelEditAttachment(index) {
    if (_isEditLabAttachment) {
        restoreAttachmentFile(index);
        $("#attachment" + index).hide();
    }
    else {
        $("#attachment" + index).remove();
    }
    $("#modalAttachments").dxPopup("hide");
}
function initAttachmentValidation(index) {
    if (!_IsNewDeclarationTracfin) return;

    var categoryDivSelector = "#CategorieDocumentDiv" + index;

    var categorySelectBoxSelector = "#CategorieDocumentId" + index;

    var validationGroup = getAttachmentsPopupFormValidationGroup(index);

    showRequiredField(categoryDivSelector, categorySelectBoxSelector, validationGroup);

    setPersonneAssocieValidation(index, validationGroup);
}

function setPersonneAssocieValidation(index, validationGroup) {
    setPersonneAssocieCustomValidator(index, validationGroup);

    toggleDisplayPersonneAssocieeRequired(index);
}

function setPersonneAssocieCustomValidator(index, validationGroup) {
    $("#PersonneAssocieId" + index).dxValidator({
        validationGroup,
        validationRules: [
            {
                type: 'custom',
                ignoreEmptyValue: false,
                reevaluate: true,
                validationCallback: function (e) {
                    return isPersonneAssocieeRequired(index) ? !isNullOrUndefined(e.value) : true;
                },
                message: _labTranslatableMessagePersonneAssocieIdRequired
            }]
    });
}

function isPersonneAssocieeRequired(index) {
    var typeDocument = $("#TypeDocumentLabId" + index)
        .dxSelectBox("instance")
        .option("selectedItem");

    if (isNullOrUndefined(typeDocument) || _labTypeDocumentsForRequiredPersonneAssocie.indexOf(typeDocument.Code) === -1) {
        return false;
    }

    return true;
}

function backupAttachmentFile(index) {
    var files = getAttachmentFiles("FileUploader" + index);

    if (files.length == 0) return;

    _labAttachmentFiles[index] = files[0];
}
function restoreAttachmentFile(index) {
    var file = _labAttachmentFiles[index];

    if (!file) return;

    var dataTransfer = new DataTransfer();

    dataTransfer.items.add(file);

    $("#FileUploader" + index).prop('files', dataTransfer.files)
}
function downloadDocumentLab(item) {
    var attachmentFiles = getAttachmentFiles("FileUploader" + item.row.data.index);

    if (attachmentFiles.length > 0) {
        downloadNewFile(attachmentFiles[0]);
        return;
    }
    var _dossierCryptedId = window._dossierCryptedId || '';
    // Vérification et initialisation de _dossierCryptedId
    if (!_dossierCryptedId || _dossierCryptedId === '') {
        // Récupération de la valeur depuis l'input CryptedDossierLabId
        _dossierCryptedId = CryptedDossierLabId.value || '';
    }
    // Vérification supplémentaire après initialisation
    if (!_dossierCryptedId || _dossierCryptedId === '') {
        // Afficher un message d'erreur si l'ID est toujours vide
        alert("Erreur : Identifiant du dossier non disponible");
        return;
    }
    // Construction de l'URL avec l'ID vérifié
    var baseUrl = '/' + culture;
    var endpoint = item.row.data.CategorieDocumentId == 10
        ? '/Lab/ServiceLab/DownloadLabDeclarationTracfinFile'
        : '/Lab/ServiceLab/DownloadLabDocument';

    // Correction de l'espace dans l'URL (remplacé &dossierLabCryptedId par &dossierLabCryptedId=)
    var url = baseUrl + endpoint + '?CryptedId=' + item.row.data.Id + '&dossierLabCryptedId=' + _dossierCryptedId;

    // Redirection vers l'URL construite
    window.location.href = url;
}
function directionOnValueChangedCreateUpdateDossierLab(cryptedDossierId, isReadOnly, callBack) {
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');

    if (directionId != null && directionId > 0) {

        getComboBoxListsByDirectionCreateUpdateDossierLab(directionId, cryptedDossierId, isReadOnly, callBack);
    }
    else {
        DossierLab_direction_OnValueChanged_OrganismeLabView();

        var countPhysicalPerson = $('.physicalPerson').length;
        var countMoralPerson = $('.moralPerson').length;
        var countNonConnu = $('.nonConnu').length;
        for (let i = 0; i < countPhysicalPerson; i++) {
            if ($("#physicalPersonToDelete" + i).val().toLowerCase() === 'false') {
                $("#PersonnePhysiqueLabTypeClientId" + i.toString()).dxSelectBox("instance").option("dataSource", []);
            }
        }
        for (let i = 0; i < countMoralPerson; i++) {
            if ($("#moralPersonToDelete" + i).val().toLowerCase() === 'false') {
                $("#PersonneMoraleLabTypeClientId" + i.toString()).dxSelectBox("instance").option("dataSource", []);
            }
        }
        $("#EntiteId").dxSelectBox("instance").option("dataSource", []);
        $("#CategorieId").dxSelectBox("instance").option("dataSource", []);
        $("#OrigineLabId").dxSelectBox("instance").option("dataSource", []);
        $("#SecteurEconomiqueId").dxSelectBox("instance").option("dataSource", []);

        if (callBack) {
            callBack();
        }
    }
}
function getComboBoxListsByDirectionCreateUpdateDossierLab(directionId, cryptedDossierId, isReadOnly, callBack) {


    var onlyActive = false;
    var isEntiteReadOnly = false;

    if ($("#IsReadOnly").length > 0)
        onlyActive = !$.parseJSON($("#IsReadOnly").val());



    if ($("#EntiteId").length > 0 && $("#EntiteId").dxSelectBox("instance") != undefined) {
        isEntiteReadOnly = $("#EntiteId").dxSelectBox("instance").option("readOnly");
        if (!isEntiteReadOnly) {
            $("#EntiteId").dxSelectBox("instance").option("dataSource", []);
            $("#EntiteId").dxSelectBox("instance").option("visible", false);

            $("#loader-entite-container").removeClass("dx-hidden");
            $("#loader-entite-container").html('<div class="d-flex"><div id="small-indicator-entite"></div><div class="pl-2">' + _labTranslatableMessagLoadingEntitiesOfDirection + '</div> </div>');

            $("#small-indicator-entite").dxLoadIndicator({
                height: 20,
                width: 20
            });

            $("#EntiteId").dxSelectBox({
                dataSource: []
            });
        }
        $(".input-customer-type-individual").addClass("dx-hidden");
        $(".loader-customer-type-individual").removeClass("dx-hidden");

        $(".loader-customer-type-individual").html('<div class="d-flex"><div class="small-indicator-customer-type-individual"></div><div style="padding-top: 2px;font-size: 11px;" class="pl-2">' + _labTranslatableMessagLoadingTypesClientOfDirection + '</div> </div>');
        $(".small-indicator-customer-type-individual").dxLoadIndicator({
            height: 20,
            width: 20
        });
    }

    if ($("#OrigineLabId").dxSelectBox("instance") != undefined) {

        $("#OrigineLabId").dxSelectBox({
            dataSource: []
        });
    }


    if ($("#SecteurEconomiqueId").dxSelectBox("instance") != undefined) {


        $("#SecteurEconomiqueId").dxSelectBox({
            dataSource: []
        });

        $(".input-origineLab").addClass("dx-hidden");
        $(".loader-origineLab").removeClass("dx-hidden");

        $(".loader-origineLab").html('<div class="d-flex"><div class="small-indicator-loader-origineLab"></div><div style="padding-top: 2px;font-size: 11px;" class="pl-2">' + _labTranslatableMessagLoadingOrigineLabOfDirection + '</div> </div>');
        $(".small-indicator-loader-origineLab").dxLoadIndicator({
            height: 20,
            width: 20
        });
    }


    if ($("#CategorieId").dxSelectBox("instance") != undefined) {
        $("#CategorieId").dxSelectBox({
            dataSource: []
        });

        $(".input-categorie").addClass("dx-hidden");
        $(".loader-categorie").removeClass("dx-hidden");

        $(".loader-categorie").html('<div class="d-flex"><div class="small-indicator-loader-categorie"></div><div style="padding-top: 2px;font-size: 11px;" class="pl-2">' + _labTranslatableMessagLoadingCategorieLabOfDirection + '</div> </div>');
        $(".small-indicator-loader-categorie").dxLoadIndicator({
            height: 20,
            width: 20
        });
    }



    $.ajax({
        method: "GET",
        cache: false,
        dataType: "json",
        url: '/' + culture + '/Lab/ServiceLab/GetComboBoxListsByDirection?directionId=' + directionId + '&isReadOnly=' + isReadOnly + '&onlyActive=' + onlyActive + '&cryptedDossierId=' + cryptedDossierId,
    }).done(function (data) {
        if (data.status) {
            var countPhysicalPerson = $('.physicalPerson').length;
            var countMoralPerson = $('.moralPerson').length;

            var typesClientPhysiqueLabs = [];
            var typesClientPhysiqueMorales = [];

            // Filtrer les types de clients selon qu'ils sont des personnes physiques ou morales
            if (data.typeClientLabs && data.typeClientLabs.length > 0) {
                typesClientPhysiqueLabs = data.typeClientLabs.filter(function (t) {
                    return t.IsCorporate != true;
                });

                typesClientPhysiqueMorales = data.typeClientLabs.filter(function (t) {
                    return t.IsCorporate != false;
                });
            }

            // Mettre à jour les SelectBox pour les personnes physiques
            for (let i = 0; i < countPhysicalPerson; i++) {
                var selectBoxPhysique = $("#PersonnePhysiqueLabTypeClientId" + i).dxSelectBox('instance');
                if (selectBoxPhysique) {
                    selectBoxPhysique.option("dataSource", typesClientPhysiqueLabs);
                }
            }

            // Mettre à jour les SelectBox pour les personnes morales
            for (let i = 0; i < countMoralPerson; i++) {
                var selectBoxMorale = $("#PersonneMoraleLabTypeClientId" + i).dxSelectBox('instance');
                if (selectBoxMorale) {
                    selectBoxMorale.option("dataSource", typesClientPhysiqueMorales);
                }
            }

            if ($("#EntiteId").dxSelectBox("instance") != undefined) {


                if (!isEntiteReadOnly) {

                    $("#EntiteId").dxSelectBox({
                        dataSource: new DevExpress.data.DataSource({
                            store: new DevExpress.data.CustomStore({
                                loadMode: "raw",
                                load: function () {
                                    return data.entiteLabs;
                                }
                            })
                        })
                    });

                    if (data.entiteLabs.length > 0 && $("#EntiteId").dxSelectBox("instance").option('value') == null) {
                        $("#EntiteId").dxSelectBox("instance").option('value', data.entiteLabs[0].Id);
                    }
                }
            }

            DossierLab_direction_OnValueChanged_OrganismeLabView();

            var categorieLabs = data.categorieLabs;

            $("#CategorieId").dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        loadMode: "raw",
                        load: function () {
                            return categorieLabs;
                        }
                    })
                })
            });


            if (data.categorieLabs.length > 0) {
                if ($("#CategorieId").dxSelectBox("instance").option('value') == null)
                    $("#CategorieId").dxSelectBox("instance").option('value', data.categorieLabs[0].Id);
                else loadStatutDossierLab();
            }

            var secteurEconomiqueLabs = data.secteurEconomiqueLabs;
            $("#SecteurEconomiqueId").dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        loadMode: "raw",
                        load: function () {
                            return secteurEconomiqueLabs;
                        }
                    })
                })
            });

            $("#OrigineLabId").dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        loadMode: "raw",
                        load: function () {
                            return data.origineLabs;
                        }
                    })
                })
            });


            if (data.origineLabs.length > 0 && $("#OrigineLabId").dxSelectBox("instance").option('value') == null) {
                $("#OrigineLabId").dxSelectBox("instance").option('value', data.origineLabs[0].Id);
            }
            if (callBack) {
                callBack();
            }
            checkConfidentialUser();

        }
        else {
            DevExpress.ui.notify(_labTranslatableMessageLoadingEntitiesTypeClientError, "error");
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_labTranslatableMessageLoadingEntitiesTypeClientError, "error");
    }).always(function (e) {
        $("#loader-entite-container").addClass("dx-hidden");
        $("#EntiteId").dxSelectBox("instance").option("visible", true);
        $(".input-customer-type-individual").removeClass("dx-hidden");
        $(".loader-customer-type-individual").addClass("dx-hidden");

        $(".input-origineLab").removeClass("dx-hidden");
        $(".loader-origineLab").addClass("dx-hidden");


        $(".input-categorie").removeClass("dx-hidden");
        $(".loader-categorie").addClass("dx-hidden");
    });
}

function checkConfidentialUser() {
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/' + culture + '/Lab/ServiceLab/CheckConfidentialUser?directionId=' + directionId,
    }).done(function (data) {
        if (data.isConfidential) {
            $("#div-confidentiel").removeClass("dx-hidden");
        }
        else {
            $("#div-confidentiel").addClass("dx-hidden");
            $("#Confidentiel").dxCheckBox("instance").option("value", false);
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_labTranslatableMessageCheckUserConfidentialError, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function validatedatedebut_OnValueChanged(e) {
    var endDate = $("#endDate").dxDateBox("instance").option('value');
    if (endDate == 'undefined')
        endDate = $("#endDate").dxDateBox("instance").option("max");
    var oldDate = e.previousValue;
    if (oldDate == 'undefined')
        oldDate = $("#startDate").dxDateBox("instance").option("min");
    if (new Date(e.value) > new Date(endDate) || e.value == null) {
        $("#startDate").dxDateBox("instance").option('value', new Date(oldDate));
    }
}

function dateReception_OnValueChanged(e) {
    var endDate = $("#DateCreation").dxDateBox("instance").option('value');
    var oldDate = e.previousValue;
    if (new Date(e.value) > new Date(endDate)) {
        $("#DateReception").dxDateBox("instance").option('value', new Date(oldDate));
    }
}


function validatedatefin_OnValueChanged(e) {
    var startDate = $("#startDate").dxDateBox("instance").option('value');
    if (startDate == 'undefined')
        startDate = $("#startDate").dxDateBox("instance").option("min");
    var oldDate = e.previousValue; if (oldDate == 'undefined')
        oldDate = $("#endDate").dxDateBox("instance").option("max");
    if (new Date(startDate) > new Date(e.value) || e.value == null) { $("#endDate").dxDateBox("instance").option('value', new Date(oldDate)); }
}

function directionSearchLab_OnContentReady() {

    var insDirection = $("#DirectionId").dxSelectBox("instance");

    if (insDirection != undefined) {

        var data = insDirection.option("dataSource");
        if (data != undefined && data.store != undefined && data.store._array != undefined && (isManagementMode === true || isReporting === true)) {
            if (data.store._array.length == 1) {
                insDirection.option("value", data.store._array[0].Id);
                if (isManagementMode === true) {
                    insDirection.option("readOnly", true);
                }
                else {
                    insDirection.option("readOnly", false);
                }

            } else {
                insDirection.option("readOnly", false);
            }
        }
    }
}

function directionOnValueChangedSearchDossierLabs() {

    var directionId = 0;
    if ($("#DirectionId").dxSelectBox("instance") != undefined)
        directionId = $("#DirectionId").dxSelectBox("instance").option('value');


    if (directionId != null && directionId > 0) {
        getComboBoxListsByDirectionSearchDossierLabs(directionId);
    }
    else {

        if ($("#EntiteId").dxSelectBox("instance") != undefined)
            $("#EntiteId").dxSelectBox({
                dataSource: []
            });
    }
}

function getComboBoxListsByDirectionSearchDossierLabs(directionId) {
    disableButton("buttonSearch");

    var CategorieSearchId = $("#CategorieSearchId").dxSelectBox("instance");
    if (CategorieSearchId && CategorieSearchId != undefined) {
        $("#CategorieSearchId").dxSelectBox("instance").option("dataSource", []);
    }

    var OrigineSearchId = $("#OrigineSearchId").dxSelectBox("instance");
    if (OrigineSearchId && OrigineSearchId != undefined) {
        $("#OrigineSearchId").dxSelectBox("instance").option("dataSource", []);
    }


    if ($("#EntiteId").dxSelectBox("instance") != undefined) {
        $("#EntiteId").dxSelectBox("instance").option("dataSource", []);
        $("#EntiteId").dxSelectBox("instance").option("visible", false);
        $("#loader-entite-container").removeClass("dx-hidden");
        $("#loader-entite-container").html('<div class="d-flex"><div id="small-indicator-entite"></div><div class="pl-2">' + _labTranslatableMessagLoadingEntitiesOfDirection + '</div> </div>');

        $("#loader-type-client-container").removeClass("dx-hidden");
        $("#loader-type-client-container").html('<div class="d-flex"><div id="small-indicator-type-client"></div><div style="padding-top: 2px;font-size: 11px;" class="pl-2">' + _labTranslatableMessagLoadingTypesClientOfDirection + '</div> </div>');

        $("#small-indicator-entite").dxLoadIndicator({
            height: 20,
            width: 20
        });

        $("#small-indicator-type-client").dxLoadIndicator({
            height: 20,
            width: 20
        });

        $("#EntiteId").dxSelectBox({
            dataSource: []
        });
    }
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "json",
        url: '/' + culture + '/Lab/ServiceLab/GetComboBoxListsByDirection?directionId=' + directionId,
    }).done(function (data) {
        if (data.status) {

            if ($("#EntiteId").dxSelectBox("instance") != undefined) {
                $("#EntiteId").dxSelectBox({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return data.entiteLabs;
                            }
                        })
                    })
                });
            }
            if ($("#CategorieSearchId").dxSelectBox("instance") != undefined) {
                if ($("#CategorieSearchId").dxSelectBox('instance')) {

                    $("#CategorieSearchId").dxSelectBox({
                        dataSource: new DevExpress.data.DataSource({
                            store: new DevExpress.data.CustomStore({
                                loadMode: "raw",
                                load: function () {
                                    return data.categorieLabs;
                                }
                            })
                        })
                    });
                }
            }
            if ($("#OrigineSearchId").dxSelectBox("instance") != undefined) {
                if ($("#OrigineSearchId").dxSelectBox('instance')) {

                    $("#OrigineSearchId").dxSelectBox({
                        dataSource: new DevExpress.data.DataSource({
                            store: new DevExpress.data.CustomStore({
                                loadMode: "raw",
                                load: function () {
                                    return data.origineLabs;
                                }
                            })
                        })
                    });
                }
            }
        }
        else {
            DevExpress.ui.notify(_labTranslatableMessageLoadingEntitiesTypeClientError, "error");
        }

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_labTranslatableMessageLoadingEntitiesTypeClientError, "error");
    }).always(function (e) {
        enableButton("buttonSearch");
        $("#loader-entite-container").addClass("dx-hidden");
        $("#loader-type-client-container").addClass("dx-hidden");
        if ($("#EntiteId").dxSelectBox("instance") != undefined) {
            $("#EntiteId").dxSelectBox("instance").option("visible", true);
        }
    });
}

function loadUtilisateurSourceSearchCritariaLabs() {
    var mode = 2;
    if (isManagementMode != true)
        mode = 1;

    $("#UtilisateurId").dxSelectBox({ dataSource: [] });
    $.ajax({
        method: "GET",
        cache: false,
        dataType: 'json',
        data: { mode: mode },
        url: '/' + culture + '/Lab/ServiceLab/GetUtilisateurByMode',
    }).done(function (data) {

        if (data != undefined && data.length > 0) {
            $("#UtilisateurId").dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        loadMode: "raw",
                        load: function () {
                            return data;
                        }
                    })
                })
            });
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
    }).always(function (e) {

    });
}

function searchDossiersLab() {
    $("#casesFoundCount").removeClass("color-red2");
    $("#casesFoundCount").addClass("text-green");
    $("#casesFoundCount").html('<div id="small-indicator-search-count"></div>');
    $("#small-indicator-search-count").dxLoadIndicator({
        height: 20,
        width: 20
    });

    initParamRechercheTiers();
    if (!isRobot)
        if ($("#startDate").dxDateBox("instance").option("value") === null || $("#endDate").dxDateBox("instance").option("value") === null) {
            DevExpress.ui.notify(_labTranslatableMessageDateIntervalSearchError, "error");
            return;
        }

    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        $("#gridDossiersLabWithTiers").dxDataGrid({ dataSource: [], loadPanel: { enabled: false } });
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        $("#gridDossiersLabWithNoTiers").dxDataGrid({ dataSource: [], loadPanel: { enabled: false } });
    }

    if ($("#gridDossiersLabReporting").length > 0) {
        $("#gridDossiersLabReporting").dxDataGrid({ dataSource: [], loadPanel: { enabled: false } });
    }

    if ($("#gridDossiersLabReportingTiers").length > 0) {
        $("#gridDossiersLabReportingTiers").dxDataGrid({ dataSource: [], loadPanel: { enabled: false } });
    }

    disableButton("buttonInitialize");
    disableButton("buttonSearch");


    var isValidSearch = (isRobot && !isManagementMode) || direction_criteria_required_lab() || ($("#buttonSearch_adm").length > 0);

    if (isValidSearch) {
        $(".loading-screen").css("visibility", "visible");
        $.ajax({
            method: "POST",
            data: $("#searchDossierLabForm").serialize(),
            dataType: "json",
            async: true,
            url: '/' + culture + '/Lab/ServiceLab/SearchDossiersLab',
        }).done(function (data) {
            if (data.status === false && data.message === "TIMEOUT") {
                $("#casesFoundCount").html(_labTranslatableMessageTimeout);
                return;
            }
            if (data.status === false) {
                DevExpress.ui.notify("Search Error : " + data.error, "error", $('div.delegation-center-page').Width);
            }
            var casesFound = _labTranslatableMessageMultipleCasesFound;
            var caseFound = _labTranslatableMessageSingleCaseFound;
            if (_isReportingTiers) {
                casesFound = _labTranslatableMessageMultipleThirdsFound;
                caseFound = _labTranslatableMessageSingleThirdFound;
            }

            $(".loading-screen").css("visibility", "hidden");
            if (data.length.Value > 0) {
                if (data.length.Value > 10000) {
                    $("#casesFoundCount").html('<span style="font-weight: bold">' + data.length.Value + '</span> ' + casesFound + (' ' + (data.length.Value <= data.result.length || data.result.length == 0) ? (_labTranslatableMessageMultipleCasesLimitedFound + ' <span style="font-weight: bold">' + data.result.length + '</span> ' + _labTranslatableMessageMultipleCasesLimitedFound1) : ''));
                } else {
                    $("#casesFoundCount").html(' <span style="font-weight: bold">' + data.length.Value + ' </span> ' + (data.length.Value <= 1 ? caseFound : casesFound));
                }
                $("#casesFoundCount").removeClass("color-red2");
            }

            else {
                $("#casesFoundCount").addClass("color-red2");
                $("#casesFoundCount").removeClass("text-green");
                $("#casesFoundCount").html(_labTranslatableMessageNoCasesFound);
                if (!isNullOrEmpty(data.error)) {
                    $("#casesFoundCount").html(' <span style="font-weight: bold">' + data.error + ' </span> ');
                    DevExpress.ui.notify(data.error, "error");
                }
            }

            if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
                $("#gridDossiersLabWithTiers").dxDataGrid({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return data.result;
                            }
                        })
                    })
                });
            }

            if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {

                $("#gridDossiersLabWithNoTiers").dxDataGrid({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return data.result;
                            }
                        })
                    })
                });
            }

            if ($("#gridDossiersLabReporting").length > 0) {
                $("#gridDossiersLabReporting").dxDataGrid({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return data.result;
                            }
                        })
                    })
                });
            }
            if ($("#gridDossiersLabReportingTiers").length > 0) {
                $("#gridDossiersLabReportingTiers").dxDataGrid({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return data.result;
                            }
                        })
                    })
                });
            }

        }).fail(function (e) {
            $(".loading-screen").css("visibility", "hidden");
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                $("#casesFoundCount").html('');
            }

        }).always(function (e) {
            $(".loading-screen").css("visibility", "hidden");
            enableButton("buttonInitialize");
            enableButton("buttonSearch");
            resizeContainer();
        });
    } else {

        $("#casesFoundCount").addClass("color-red2");
        $("#casesFoundCount").removeClass("text-green");
        $("#casesFoundCount").html(_labTranslatableMessageRequiredCriterias);

        enableButton("buttonInitialize");
        enableButton("buttonSearch");
    }


}

function gridUpdateSelected_Lab_OnClick(e) {
    var rowSelected = getSelectedRowDossier();
    if (rowSelected) {
        location = '/' + culture + '/lab/dossier/edit/' + rowSelected.CryptedId;
    }
}

function gridUpdateSelectedNew_Lab_OnClick(e) {
    var isNewDeclarationTracfin = true;
    var rowSelected = getSelectedRowDossier();
    if (rowSelected) {
        location = '/' + culture + '/lab/dossier/edit/' + rowSelected.CryptedId + '?isNewDeclarationTracfin=' + isNewDeclarationTracfin;
    }
}

function produit_search_OnValueChanged() {
    var produitId = $("#ProduitSearchId").dxSelectBox("instance").option('value');
    if (produitId !== undefined && produitId > 0) {
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: '/' + culture + '/Lab/ServiceLab/GetCategoryByProduit?produitId=' + produitId,
        }).done(function (data) {
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_labTranslatableMessageLoadingCategoriesError, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
    }
}

function payssearchLab_load() {

    $.ajax({
        method: "GET",
        cache: false,
        dataType: 'json',
        url: '/' + culture + '/Lab/ServiceLab/GetPays',
    }).done(function (data) {

        if (data != undefined && data.length > 0) {

            if ($("#PaysId").dxSelectBox('instance')) {

                $("#PaysId").dxSelectBox({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return data;
                            }
                        })
                    })
                });
            }
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
    }).always(function (e) {

    });
}

function gridDeleteSelected_Lab_OnClick() {
    $.ajax({
        method: "GET", cache: false,
        url: '/' + culture + '/Lab/ServiceLab/DeleteDossierLab',
    }).done(function (data) {
        if (data) {
            showPopup(_commonDeleteDossierPopupId, data);
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
    });
}

function gridDeleteDsSelected_Lab_OnClick(e) {

    showConfirmationDeleteDsLabDialog();
}

function ConfirmDeleteDossier_click() {
    var motif = $("#MotifSuppression").dxHtmlEditor("instance").option('value');

    if (motif == null || $.trim(motif.replace('<br>', '')) == '') {
        DevExpress.ui.notify(_commonTranslatableMessageMotifSuppressionObligatoire, "error");

        markRequired("MotifSuppression");

        return;
    }

    showLoadPanel(_commonTranslatableMessageDeleteDossierLab);
    var dataGrid = null;

    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }

    var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;

    $.ajax({
        method: "GET",
        data: { dossierId: cryptedId, motifSuppression: motif },
        dataType: "json",
        url: '/' + culture + '/Lab/ServiceLab/DeleteDossiersLab',
    }).done(function (data) {

        if (data.success) {
            closePopup(_commonDeleteDossierPopupId);
            gridRemoveDossierLab(cryptedId);
            searchDossiersLab();
            DevExpress.ui.notify(_commonTranslatableMessageDeleteDossierLab, "success");
        }
        else {
            DevExpress.ui.notify(data.errorMessage, "error");
        }


    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageDeleteDossierLabError, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function CancelConfirmDeleteDossier_click() {
    closePopup(_commonDeleteDossierPopupId);
}

function reactivateDossierLabSelected_OnClick() {

    showConfirmationReactivateDossiersLabDialog();

}

function showConfirmationReactivateDossiersLabDialog() {
    var confirmationReactivateDossiersLabDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _commonTranslatableMessageReactivateDossierConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });

    confirmationReactivateDossiersLabDialog.show().done(function (dialogResult) {
        if (dialogResult) {
            showLoadPanel(_labTranslatableMessageReactivatingCase);
            var dataGrid = null;


            if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
                dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
            }

            if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
                dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
            }

            var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;

            $.ajax({
                method: "GET",
                data: { cryptedId: cryptedId },
                dataType: "json",
                url: '/' + culture + '/Lab/ServiceLab/ReactivateDossiersLab',
            }).done(function (data) {

                if (data.success) {
                    var keys = dataGrid.getSelectedRowKeys();
                    $.each(keys, function (value) {
                        this.Statut = data.statusDossier;
                        this.Modificateur = data.modificateur;
                        this.DateModification = data.dateModification;
                        this.Id = data.id;
                        dataGrid.refresh();
                    });
                    DevExpress.ui.notify(_commonTranslatableMessageReactivatedDossier, "success");
                }
                else {
                    DevExpress.ui.notify(data.errorMessage, "error");
                }


            }).fail(function (e) {
                if (e.status == 401) {
                    var popup = $("#modalReconnect").dxPopup('instance');
                    popup.show();
                    return;
                }
                DevExpress.ui.notify(_labTranslatableMessageReactivatingCaseError, "error");
            }).always(function (e) {
                hideLoadPanel();
            });
        }
    });
}

function showConfirmationPrendreEnChargeLabDialog() {
    var confirmationPrendreEnChargeLabDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatableMessagePrendreEnChargeConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });

    confirmationPrendreEnChargeLabDialog.show().done(function (dialogResult) {
        if (dialogResult) {
            showLoadPanel(_labTranslatableMessagePrendreEnChargeDossier);
            var dataGrid = null;


            if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
                dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
            }

            if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
                dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
            }

            var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;

            $.ajax({
                method: "GET",
                data: { cryptedId: cryptedId },
                dataType: "json",
                url: '/' + culture + '/Lab/ServiceLab/PrendreEnChargeLab',
            }).done(function (data) {

                if (data.success) {
                    var keys = dataGrid.getSelectedRowKeys();
                    $.each(keys, function (value) {
                        this.Statut = data.statusDossier;
                        this.Id = data.statutLabId;
                        this.Utilisateur = data.Utilisateur;
                        this.Modificateur = data.modificateur;
                        this.DateModification = data.dateModification;
                        dataGrid.refresh();
                    });
                    DevExpress.ui.notify(_labTranslatableMessagePrendreEnChargeDossier, "success");
                }
                else {
                    DevExpress.ui.notify(data.errorMessage, "error");
                }


            }).fail(function (e) {
                if (e.status == 401) {
                    var popup = $("#modalReconnect").dxPopup('instance');
                    popup.show();
                    return;
                }
                DevExpress.ui.notify(_labTranslatableMessagePrendreEnChargeDossierError, "error");
            }).always(function (e) {
                hideLoadPanel();
            });
        }
    });
}

function showConfirmationDeleteDsLabDialog() {
    var confirmationDeleteDsLabDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatableMessageDeleteDsConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });

    confirmationDeleteDsLabDialog.show().done(function (dialogResult) {
        if (dialogResult) {
            showLoadPanel(_labTranslatableMessageDeleteDsConfirmation);
            var dataGrid = null;


            if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
                dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
            }

            if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
                dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
            }

            var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;

            $.ajax({
                method: "GET",
                data: { cryptedId: cryptedId },
                dataType: "json",
                url: '/' + culture + '/Lab/ServiceLab/DeleteDeclationSoupsonLab',
            }).done(function (data) {

                if (data.success) {
                    var keys = dataGrid.getSelectedRowKeys();
                    $.each(keys, function (value) {
                        this.Modificateur = data.libelleModificateur;
                        this.DateModification = data.dateModification;
                        this.IsDeclarationSoupcon = data.isDeclarationSoupcon;

                        dataGrid.refresh();
                    });
                    DevExpress.ui.notify(_commonTranslatableMessageDeleteDsLab, "success");
                }
                else {
                    DevExpress.ui.notify(data.errorMessage, "error");
                }


            }).fail(function (e) {
                if (e.status == 401) {
                    var popup = $("#modalReconnect").dxPopup('instance');
                    popup.show();
                    return;
                }
                DevExpress.ui.notify(_commonTranslatableMessageDeleteDsLabError, "error");
            }).always(function (e) {
                hideLoadPanel();
            });
        }
    });
}

function gridShowDetailsSelected_Lab_OnClick(e) {
    var rowSelected = getSelectedRowDossier();
    if (rowSelected) {
        location = '/' + culture + '/lab/dossier/details/' + rowSelected.CryptedId;
    }
}


function downloadAllDocumentsLab(cryptedId, codeUniqueDossier) {
    window.location.href = '/' + culture + '/Lab/ServiceLab/DownloadLabAllDocuments/?CryptedId=' + cryptedId + '&codeUnique=' + codeUniqueDossier;
}
function downloadAllDocumentsDemandeRequestLab(cryptedId) {
    window.location.href = '/' + culture + '/Lab/ServiceLab/DownloadLabAllDocumentsDemandeInformation/?CryptedId=' + cryptedId + '&codeUnique=' + $("#DemandeInformationLab_CodeUniqueDossier").val() + '&isRequest=' + 1;
}
function downloadAllDocumentsDemandeResponseLab(cryptedId) {
    window.location.href = '/' + culture + '/Lab/ServiceLab/DownloadLabAllDocumentsDemandeInformation/?CryptedId=' + cryptedId + '&codeUnique=' + $("#DemandeInformationLab_CodeUniqueDossier").val() + '&isRequest=' + 0;
}
function downloadDocumentAllLabs() {
    window.location.href = '/' + culture + '/Lab/ServiceLab/DownloadAllLabDocuments'
}
function gridExtraireDetailsSelected_onClick(cryptedId) {
    if (cryptedId) {
        window.location.href = '/' + culture + '/Lab/ServiceLab/DownloadLabDocuments/?CryptedId=' + cryptedId
    }
}

function dataGrid_rowPrepared_handler(e) {
    if (e.rowType !== "data")
        return
}

function paysnaissanceaddpersonnephysique_load() {
    $.ajax({
        method: "GET",
        cache: false,
        dataType: 'json',
        url: '/' + culture + '/Lab/ServiceLab/GetPays',
    }).done(function (data) {

        // Vérifier que les données existent et ne sont pas vides
        if (data && data.length > 0) {

            // Récupérer les éléments une seule fois
            var physicalPersonElements = $('.physicalPerson');

            // Mettre à jour chaque SelectBox avec les données des pays
            physicalPersonElements.each(function (i) {
                var selectBoxId = "#PersonnePhysiqueLabPaysNaissanceId" + i;
                var selectBoxInstance = $(selectBoxId).dxSelectBox('instance');

                if (selectBoxInstance) {
                    selectBoxInstance.option("dataSource", new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return data;
                            }
                        })
                    }));
                }
            });
        }
    }).fail(function (e) {
        // Gérer les erreurs d'authentification
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
        }
    });
}

function removePhysicalPerson(order) {
    $("#rowPhysicalPerson" + order).html('<input name="DossierLabPersonnePhysiques[' + order + '].ToDelete" value="True" id="physicalPersonToDelete' + order + '" type="hidden">');
    $("#rowPhysicalPerson" + order).remove();
}

function removeMoralPerson(order) {
    $("#rowMoralPerson" + order).html('<input name="DossierLabPersonneMorales[' + order + '].ToDelete" value="True" id="moralPersonToDelete' + order + '" type="hidden">');
    $("#rowMoralPerson" + order).remove();
}

function removeNonConnu(order) {
    $("#rowNonConnu" + order).html('<input name="DossierLabNonConnus[' + order + '].ToDelete" value="True" id="nonConnuToDelete' + order + '" type="hidden">');
    $("#rowNonConnu" + order).remove();
}

function showFormAction(orderAction) {
    if (orderAction !== null) {
        var popup = $("#modalAction").dxPopup("instance");
        var isNewAction = $("#rowAction" + orderAction).hasClass("isNewAction");

        popup.show().done(function () {
            $("#LibelleEdit").dxTextBox("instance").option("value", $("#Libelle" + orderAction).dxTextBox("instance").option("value"));
            $("#DescriptionEdit").dxHtmlEditor("instance").option("value", $("#Description" + orderAction).dxHtmlEditor("instance").option("value"));
            $("#DateCreationActionEdit").dxDateBox("instance").option("value", $("#DateCreationAction" + orderAction).dxDateBox("instance").option("value"));
            var isView = $("#DescriptionEdit").dxHtmlEditor("instance").option("disabled") == true;
            var titleTemplate = isView ? _labTranslatableMessageViewAction : (isNewAction ? _labTranslatableMessageAddAction : _labTranslatableMessageEditAction);
            popup.option('titleTemplate', '<div class="d-flex flex-row align-items-center justify-content-between">\
            <div class= "font-size-16px smallCaps text-green"> <i class="fas fa-plus mr-2"></i>'+ titleTemplate + '</div>\
                <span style= "cursor: pointer; font-size: 24px;" onclick = "$(\'#modalAction\').dxPopup(\'hide\');">&times;</span></div > ');


            $("#btnCloseActionForm").dxButton("instance").option("onClick", function () {
                if (isNewAction) {
                    popup.option('onHiding', function () {
                        $('#rowAction' + orderAction).remove();
                    });
                }
                closeFormAction();
            });

            if ($("#btnValidateActionForm").dxButton("instance") != undefined) {
                $("#btnValidateActionForm").dxButton("instance").option("onClick", function () { updateFormAction(orderAction); });
            }
            validateActionForm();
        });
    }
    else {
        showLoadPanel(_commonTranslatableMessageLoading);
        var countActions = $('.actionRow').length;
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            url: '/' + culture + '/Lab/ServiceLab/AddActionForm?Order=' + countActions,
        }).done(function (data) {
            $("#actionsContainer").append(data);
            $("#rowAction" + countActions).append('<input id="actionToDelete' + countActions + '" type="hidden" name="DossierLabActions[' + countActions + '].ToDelete" value="true">');
            showFormAction(countActions);
            hideLoadPanel();
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {

        });
    }
}

function updateFormAction(orderAction) {
    var Libelle = $("#LibelleEdit").dxTextBox("instance").option("value") === null ? "" : $("#LibelleEdit").dxTextBox("instance").option("value").replace(/<[^>]*>?/gm, '');
    $("#Libelle" + orderAction).dxTextBox("instance").option("value", Libelle);
    $("#Description" + orderAction).dxHtmlEditor("instance").option("value", $("#DescriptionEdit").dxHtmlEditor("instance").option("value"));
    $("#DateCreationAction" + orderAction).dxDateBox("instance").option("value", $("#DateCreationActionEdit").dxDateBox("instance").option("value"));
    $("#cellLibelle" + orderAction).html($("#LibelleEdit").dxTextBox("instance").option("value"));
    $("#cellDateCreation" + orderAction).html($("#DateCreationActionEdit").dxDateBox("instance").option("text"));


    if ($("#rowAction" + orderAction).hasClass("isNewAction")) {
        $("#actionsTable").append('<tr id="trAction' + orderAction + '">\
        <td id = "cellLibelle'+ orderAction + '" >' + $("#Libelle" + orderAction).dxTextBox("instance").option("value") + '</td >\
        <td id = "cellDateCreation'+ orderAction + '" >' + $("#DateCreationAction" + orderAction).dxDateBox("instance").option("text") + '</td>\
        <td>'+ htmlEncode($("#hiddenCreator" + orderAction).val()) + '</td>\
        <td>\
            <div id="btnEditActionForm'+ orderAction + '"></div>\
        </td>\
        <td>\
            <div id="btnDeleteActionForm'+ orderAction + '"></div>\
        </td>\
        </tr>');

        $("#DateCreationAction" + orderAction).dxDateBox("instance").option("value", $("#DateCreationActionEdit").dxDateBox("instance").option("value"));
        $("#btnEditActionForm" + orderAction).dxButton({
            text: _labTranslatableMessageEdit,
            type: "normal",
            width: 120,
            icon: "edit",
            elementAttr:
            {
                class: "color-blue"
            },
            onClick: function () {
                showFormAction(orderAction);
            }
        });
        $("#btnDeleteActionForm" + orderAction).dxButton({
            text: _labTranslatableMessageDelete,
            type: "normal",
            width: 120,
            icon: "trash",
            elementAttr:
            {
                class: "color-red"
            },
            onClick: function () {
                showFormConfirmationDeleteAction(orderAction);
            }
        });

        $("#rowAction" + orderAction).removeClass("isNewAction");
        $("#actionToDelete" + orderAction).remove();

    }

    closeFormAction();
}

function closeFormAction() {
    $('#modalAction').dxPopup("instance").hide().done(function (e) {
        $("#LibelleEdit").val("");
        $("#DescriptionEdit").dxHtmlEditor("instance").option("value", "");
        $("#DateCreationActionEdit").dxDateBox("instance").option("value", "");
        $('#modalAction').dxPopup("instance").option("onHiding", null);
    });
}
function cancelVisualisationDSModalPreviewLab_onClick() {
    if ($("#VisualisationDSModalPreview")) {
        $("#VisualisationDSModalPreview").dxPopup('instance').hide();
        $("#VisualisationDSModalPreview").dxPopup('instance').content().empty();
    }
}
function validateActionForm() {
    var libelle = $.trim($("#LibelleEdit").dxTextBox("instance").option("value"));
    var description = $.trim($("#DescriptionEdit").dxHtmlEditor("instance").option('value'));
    var dateCreation = $.trim($("#DateCreationActionEdit").dxDateBox("instance").option('value'));

    var btnValidateActionForm = $("#btnValidateActionForm").dxButton("instance");
    var btnCloseActionForm = $("#btnCloseActionForm").dxButton("instance");
    if (libelle === '' || description === '' || dateCreation === '') {
        if (btnValidateActionForm != undefined) {
            btnValidateActionForm.option("disabled", true);
        }
    }
    else {
        if (btnValidateActionForm != undefined) {
            btnValidateActionForm.option("disabled", false);
        }
    }
}

function showFormConfirmationDeleteAction(id) {
    var confirmDeleteActionDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _commonTranslatableMessageDeleteActionConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });
    confirmDeleteActionDialog.show().done(function (dialogResult) {
        if (dialogResult) {
            $("#rowAction" + id).html('<input type="hidden" name="DossierLabActions[' + id + '].ToDelete" value="true">');
            $("#trAction" + id).addClass("dx-hidden");
        }
    });
}

function categorieGroupeLab_OnValueChanged(existTracfin, callback) {
    resetIdentificationSpecifique();

    toggleDisplayFieldRequired("MotifsSoupcons", false);

    toggleDisplayFieldRequired("DateDeclarationLocale", false);

    var selectedCategorie = $("#CategorieGroupeLabId").dxSelectBox("instance").option('selectedItem');

    var categorieGroupeLabId = null;

    if (selectedCategorie == null && callback) {
        callback();
    }
    else if (!isNullOrUndefined(selectedCategorie)) {
        var isDs = selectedCategorie.IsDs;
        var direction = null;
        var directionInstance = $("#DirectionId").dxSelectBox("instance");

        if (directionInstance) {
            direction = directionInstance.option('selectedItem');
        }

        if (!existTracfin) {
            if (isDs && !isNullOrUndefined(direction) && direction.IsTracfin) {
                // Code pour afficher les boutons et rendre les champs obligatoires
                showButton("btnDeclarationSoupcon");
                showButton("btnDeclarationSoupconNew");
                showButton("btnImportDeclarationSoupconXml");
                showButton("btnImportDeclarationSoupconXmlNew");
            }
            else {
                removeDeclarationtracfin();

                if (isDs && !isNullOrUndefined(direction) && !direction.IsTracfin) {
                    toggleDisplayFieldRequired("MotifsSoupcons", true);
                    toggleDisplayFieldRequired("DateDeclarationLocale", true);
                }
            }
        }

        categorieGroupeLabId = selectedCategorie.Id;

        if (categorieGroupeLabId == 5 || categorieGroupeLabId == 6) {
            $("#dossier-lab-DateReponse-container-body").removeClass("dx-hidden");
            $("#dossier-lab-DateReponse-container").removeClass("dx-hidden");
            $("#dossier-lab-DateReponse-container").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
            $("#dossier-lab-DateReponse-container").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
        }
        else {
            $("#dossier-lab-DateReponse-container").addClass("dx-hidden");
        }

        var dateValidator = DevExpress.ui.dxValidator.getInstance($("#DateDeclarationLocale")[0]);
        if ((categorieGroupeLabId == 1 || categorieGroupeLabId == 2) && direction.IsTracfin === false) {
            $("#date-declaration-locale").removeClass("dx-hidden");

            if (!dateValidator ) {
                $("#DateDeclarationLocale").dxValidator({
                    validationRules: [{
                        type: "required",
                        message: "La date de déclaration locale est obligatoire"

                    }]
                });
            }
            else {
                dateValidator.option("validationRules", [{
                    type: "required",
                    message: "La date de déclaration locale est obligatoire"
                }]);
            }

        } else {
            $("#date-declaration-locale").addClass("dx-hidden");
            $("#DateDeclarationLocale").dxDateBox("instance").option("value", null);
            if (dateValidator) {
                dateValidator.option("validationRules", []);
            }
           

        }
        if (!isDs && callback) {
            callback();
            return;
        }

        if (isDs) {
            var categorieGroupeId = $("#CategorieGroupeLabId").dxSelectBox("instance").option('value');
            var categorieTracfinLabId = $("#CategorieTracfinId0").dxSelectBox("instance") != null ? $("#CategorieTracfinId0").dxSelectBox("instance").option('value') : null;
            $.ajax({
                method: "GET",
                cache: false,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: '/' + culture + '/Lab/ServiceLab/GetCategorieTracfin?categorieGroupeId=' + categorieGroupeId + '&categorieTracfinLabId=' + categorieTracfinLabId,
            }).done(function (data) {
                if ($("#CategorieTracfinId0").dxSelectBox("instance") != null) {

                    $("#CategorieTracfinId0").dxSelectBox("instance").option("dataSource", data);
                    if (categorieTracfinLabId == null || categorieTracfinLabId == 0 || categorieTracfinLabId == 5) {
                        if (categorieGroupeId == 2) {
                            $("#CategorieTracfinId0").dxSelectBox("instance").option('value', 5);
                            $("#IsSoupconTerrorisme0").dxCheckBox("instance").option("value", true);
                        }
                        else if ($("#CategorieTracfinId0").dxSelectBox("instance").option('value') == 5) {
                            $("#CategorieTracfinId0").dxSelectBox("instance").option('value', null);
                        }
                    }
                }
                toggleSectionInformationInternesDsFt(categorieGroupeLabId == 2);
                if (callback) {
                    callback();
                }
            }).fail(function (e) {
                if (e.status == 401) {
                    var popup = $("#modalReconnect").dxPopup('instance');
                    popup.show();
                    return;
                }
                DevExpress.ui.notify(_labTranslatableMessageLoadingCategoriesError, "error");
            }).always(function () {
                if (!callback) {
                    hideLoadPanel();
                }
            });

        }
    }
}

function resetIdentificationSpecifique() {
    var $identificationSpecifiqueElem = $("div[id^='PersonnePhysiqueLabTypeListeId'], div[id^='DossierLabPersonneMoralesTypeListeCriblageId']");

    if ($identificationSpecifiqueElem.length == 0) {
        return;
    }

    var validationRules = [];

    var categorieGroupeLabId = $("#CategorieGroupeLabId").dxSelectBox("instance").option('value');

    if (categorieGroupeLabId == 4 || categorieGroupeLabId == 6) {
        validationRules = [
            {
                type: "required",
                message: _labTranslatableMessageIdentificationSpecifiqueObligatoire
            }
        ];
    }

    $identificationSpecifiqueElem.dxValidator({
        validationRules
    });
}

function loadStatutDossierLab() {
    $("#StatutDossierId").dxSelectBox("instance").option("dataSource", []);
    var directionId = $("#DirectionId").dxSelectBox('instance').option('value');
    var isReadOnly = $.parseJSON($("#IsReadOnly").val());
    var onlyActive = !isReadOnly;
    var isValidation = false;
    if ($("#CategorieId").dxSelectBox("instance").option("selectedItem")) {
        isValidation = $("#CategorieId").dxSelectBox("instance").option("selectedItem").IsValidation;
    }
    var statutId = 0;
    if ($("#StatutDossierId").dxSelectBox("instance") != null)
        statutId = $("#StatutDossierId").dxSelectBox("instance").option("value");
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/' + culture + '/Lab/ServiceLab/LoadStatutDossierLab',
        data: { directionId: directionId, isValidation: isValidation, onlyActive: onlyActive, statutId: statutId },
    }).done(function (data) {

        $("#StatutDossierId").dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    loadMode: "raw",
                    load: function () {
                        return data.statutDossierLabs;
                    }
                })
            })
        });
        if (data.statutDossierLabs.length > 0 && ($("#StatutDossierId").dxSelectBox("instance").option("value") == null || $("#StatutDossierId").dxSelectBox("instance").option("value") == 0)) {
            if (data.statutDossierLabs.filter(word => word.Id == 4).length > 0) {
                $("#StatutDossierId").dxSelectBox("instance").option("value", 4);
            }
            else if (data.statutDossierLabs.filter(word => word.Id == 7).length > 0) {
                $("#StatutDossierId").dxSelectBox("instance").option("value", 7);
            }
        }
        if (!isReadOnly) {
            $("#statut-dossier-lab-container").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
            $("#statut-dossier-lab-container").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
        }


    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function removeDeclarationtracfin() {
    hideButton("btnDeclarationSoupcon");
    hideButton("btnDeclarationSoupconNew");
    hideButton("btnImportDeclarationSoupconXml");
    hideButton("btnImportDeclarationSoupconXmlNew");
    $(".navds").addClass("dx-hidden");
    $(".blocds").addClass("dx-hidden");
    $("#declaration-tracfin-container").html("");
    $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value", false);
    $(".isPersonneDs").addClass("dx-hidden");
    $(".afterPersonneDsField").addClass("pr-xl-1");

}
function setCategorieGroupeDeclarationTracfin() {
    var isDeclarationtracfin = $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value");
    var categorieGroupeLabId = $("#CategorieGroupeLabId").dxSelectBox("instance").option('selectedItem');
    var isDs = false;
    if (categorieGroupeLabId != null)
        isDs = categorieGroupeLabId.IsDs;
    if (isDeclarationtracfin && isDs) {
        var categorieGroupeLabs = $("#CategorieGroupeLabId").dxSelectBox('instance').option().dataSource.store._array.filter(function (t) { return t.IsDs == true; });
        $("#CategorieGroupeLabId").dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    loadMode: "raw",
                    load: function () {
                        return categorieGroupeLabs;
                    }
                })
            })
        });
    }
}
function addDeclarationTracfin() {
    var confirmAddDeclarationTracfin = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageAddDeclarationTracfin,
        message: _labTranslatableMessageQuestionAddDeclarationTracfin,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });
    confirmAddDeclarationTracfin.show().done(function (dialogResult) {

        if (dialogResult) {
            showLoadPanel(_commonTranslatableMessageLoading);
            var directionId = $("#DirectionId").dxSelectBox('instance').option('value');
            $.ajax({
                method: "GET",
                cache: false,
                dataType: "html",
                contentType: "application/json; charset=utf-8",
                url: '/' + culture + '/Lab/ServiceLab/AddDeclarationTracfinForm',
                data: { order: 0, directionId: directionId },
            }).done(function (data) {
                $("#declaration-tracfin-container").append(data);
                $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value", true);
                $("#AccuseReception0").dxCheckBox("instance").option("value", true);
                $(".navds").removeClass("dx-hidden");
                $(".blocds").removeClass("dx-hidden");
                $(".ds").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $(".ds").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $(".blocds").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $(".blocds").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $(".isPersonneDs").removeClass("dx-hidden");
                $(".afterPersonneDsField").removeClass("pr-xl-1");
                $([document.documentElement, document.body]).animate({
                    scrollTop: $("#bloc8").offset().top + ($("#declaration-tracfin-container").height())
                }, 500);
                $("#NombrePersonneMorales0").dxTextBox("instance").option("value", 0);
                $("#NombrePersonnePhysiques0").dxTextBox("instance").option("value", 0);
                hideButton("btnDeclarationSoupcon");
                //$("#DeclarationTracfinsMotifs0").dxTextBox("instance").option("width", "inherit");
                $("#DeclarationTracfinsAnalyses0").dxHtmlEditor("instance").option("width", "inherit");
                $("#type-declaration-container").removeClass("dx-hidden");
                $("#IsDeclarationSoupconDiv").removeClass("dx-hidden");
                $("#date-declaration-locale").addClass("dx-hidden");
                var categorieGroupeLabs = $("#CategorieGroupeLabId").dxSelectBox('instance').option().dataSource.store._array.filter(function (t) { return t.IsDs == true; });
                $("#CategorieGroupeLabId").dxSelectBox({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return categorieGroupeLabs;
                            }
                        })
                    })
                });
                setRelationAffaireValidator();
                toggleDisplayFieldRequired("MotifsSoupcons", false);
                toggleDisplayFieldRequired("DateDeclarationLocale", false);

            }).fail(function (e) {
                if (e.status == 401) {
                    var popup = $("#modalReconnect").dxPopup('instance');
                    popup.show();
                    return;
                }
                DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
            }).always(function (e) {
                hideLoadPanel();
            });
        }
    });
}

function addDeclarationTracfinNew() {
    var confirmAddDeclarationTracfin = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageAddDeclarationTracfin,
        message: _labTranslatableMessageQuestionAddDeclarationTracfin,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });
    confirmAddDeclarationTracfin.show().done(function (dialogResult) {

        if (dialogResult) {
            showLoadPanel(_commonTranslatableMessageLoading);
            var directionId = $("#DirectionId").dxSelectBox('instance').option('value');
            $.ajax({
                method: "GET",
                cache: false,
                dataType: "html",
                contentType: "application/json; charset=utf-8",
                url: '/' + culture + '/Lab/ServiceLab/AddDeclarationTracfinFormNew',
                data: { order: 0, directionId: directionId },
            }).done(function (data) {
                $("#declaration-tracfin-container").append(data);
                $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value", true);
                $("#AccuseReception0").dxCheckBox("instance").option("value", true);
                $(".navds").removeClass("dx-hidden");
                $(".blocds").removeClass("dx-hidden");
                $(".ds").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $(".ds").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $(".blocds").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $(".blocds").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $(".isPersonneDs").removeClass("dx-hidden");
                $(".afterPersonneDsField").removeClass("pr-xl-1");
                $([document.documentElement, document.body]).animate({
                    scrollTop: $("#bloc8").offset().top - 48
                }, 500);
                $("#NombrePersonneMorales0").dxTextBox("instance").option("value", 0);
                $("#NombrePersonnePhysiques0").dxTextBox("instance").option("value", 0);
                hideButton("btnDeclarationSoupconNew");
                //$("#DeclarationTracfinsMotifs0").dxTextBox("instance").option("width", "inherit");
                $("#DeclarationTracfinsAnalyses0").dxHtmlEditor("instance").option("width", "inherit");
                $("#type-declaration-container").removeClass("dx-hidden")
                $("#IsDeclarationSoupconDiv").removeClass("dx-hidden")
                var categorieGroupeLabs = $("#CategorieGroupeLabId").dxSelectBox('instance').option().dataSource.store._array.filter(function (t) { return t.IsDs == true; });
                $("#CategorieGroupeLabId").dxSelectBox({
                    dataSource: new DevExpress.data.DataSource({
                        store: new DevExpress.data.CustomStore({
                            loadMode: "raw",
                            load: function () {
                                return categorieGroupeLabs;
                            }
                        })
                    })
                });
                setRelationAffaireValidator();
                categorieGroupeLab_OnValueChanged(true);
                $.ajax({
                    method: "GET",
                    cache: false,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    url: '/' + culture + '/Lab/ServiceLab/GetProfessionDirection',
                    data: { directionId: directionId },
                }).done(function (retour) {
                    if (retour.status == true) {
                        toggleSectionAssurence(retour.isAssurance);
                        banqueValidation(retour.isBanque);
                    }
                    if (retour.status == false) {
                        banqueValidation(retour.isBanque);
                    }
                });

                disableEnableRequiredFiledOnAddPpPm();

                var isDirectionTracfin = $("#IsDirectionTracfin").val();
                var newValueExpr = "Name" + capitalizeFirstLetter(culture)
                if (isDirectionTracfin == "True")
                    newValueExpr = "Ds" + newValueExpr;
                $("#StatutDossierId").dxSelectBox("option", "displayExpr", newValueExpr);
                var selectBoxDataSource = $("#StatutDossierId").dxSelectBox("getDataSource");
                selectBoxDataSource.reload();
            }).fail(function (e) {
                if (e.status == 401) {
                    var popup = $("#modalReconnect").dxPopup('instance');
                    popup.show();
                    return;
                }
                DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
            }).always(function (e) {
                hideLoadPanel();
            });
        }
    });
}
function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
function direction_searchCritariadossier_load() {

    $("#DirectionId").dxSelectBox({ dataSource: [] });

    var mode = isManagementMode === true ? 2 : 1;
    $.ajax({
        method: "GET",
        cache: false,
        dataType: 'json',
        data: { mode: mode },
        url: '/' + culture + '/Lab/ServiceLab/GetDirectionSearchMode',
    }).done(function (data) {

        if (data != undefined && data.length > 0) {
            $("#DirectionId").dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        loadMode: "raw",
                        load: function () {
                            return data;
                        }
                    })
                })
            });
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
    }).always(function (e) {

    });
}

function getSelectedRowDossier() {
    var dataGrid = null;
    var rowSelected = null;

    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
        if (dataGrid.getSelectedRowsData().length > 0) {
            rowSelected = dataGrid.getSelectedRowsData()[0];
        }
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
        if (dataGrid.getSelectedRowsData().length > 0) {
            rowSelected = dataGrid.getSelectedRowsData()[0];
        }
    }
    return rowSelected;
}

function manageCommandButtonsActivationManagementMode() {

    disableButton("gridUpdateSelected");
    disableButton("gridUpdateSelectedNew");
    disableButton("gridDeleteSelected");
    disableButton("gridReactivateSelected");
    disableButton("gridShowDetailsSelected");
    disableButton("gridShowDetailDsSelected");
    disableButton("gridExtraireDetailsSelected");
    disableButton("btnAuditDossierLab");
    disableButton("gridDuplicateSelected");

    disableButton("gridDeleteDsSelected");
    disableButton("gridTransmissionUpdateARSelected");
    disableButton("gridExportDsSelected");

    var dataGrid = null;
    var rowSelected = null;

    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
        if (dataGrid.getSelectedRowsData().length > 0) {
            rowSelected = dataGrid.getSelectedRowsData()[0];
        }
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
        if (dataGrid.getSelectedRowsData().length > 0) {
            rowSelected = dataGrid.getSelectedRowsData()[0];
        }
    }

    if (rowSelected) {
        var cryptedDossierId = rowSelected.CryptedId;
        $("#actionLabDiv").html("");
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            contentType: "application/json; charset=utf-8",
            url: '/' + culture + '/Lab/ServiceLab/AllowedUserCommandsManagementMode',
            data: { cryptedDossierId: cryptedDossierId }
        }).done(function (data) {
            if (data) {
                $("#actionLabDiv").append(data);
            }
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_labTranslatableMessageLoadingUnauthorizeUpdatingDeletingError, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
    }
}

function manageCommandButtonsActivationViewerMode() {
    disableButton("gridShowDetailsSelected");
    disableButton("gridExtraireDetailsSelected");
    disableButton("gridExportDsSelected");
    disableButton("gridVisualisationDSSelected");
    disableButton("gridShowDetailDsSelected");
    disableButton("btnAuditDossierLab");
    disableButton("gridDuplicateSelected");
    var dataGrid = null;


    var rowSelected = null;
    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
        if (dataGrid.getSelectedRowsData().length > 0) {
            rowSelected = dataGrid.getSelectedRowsData()[0];
        }
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
        if (dataGrid.getSelectedRowsData().length > 0) {
            rowSelected = dataGrid.getSelectedRowsData()[0];
        }
    }

    if (rowSelected) {

        enableButton("btnAuditDossierLab");
        var cryptedDossierId = rowSelected.CryptedId;
        $("#actionLabDiv").html("");

        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            contentType: "application/json; charset=utf-8",
            url: '/' + culture + '/Lab/ServiceLab/AllowedUserCommandsViewerMode',
            data: { cryptedDossierId: cryptedDossierId }
        }).done(function (data) {

            if (data) {
                $("#actionLabDiv").append(data);
            }

        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_labTranslatableMessageLoadingUnauthorizeUpdatingDeletingError, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
    }
}

var isSubmitting = false;

$(document).ready(function () {

    $(document).on("click", ".remDocumentRowSaved", function () {
        var documentRowSaved = $(this).closest('.documentRowSaved');
        var checkbtn = documentRowSaved.find("input[type=CheckBox]").first();

        if (checkbtn.is(':checked')) {
            checkbtn.prop("checked", false);

            documentRowSaved.addClass("list-group-item ");

            documentRowSaved.removeClass("list-group-item-danger");
            documentRowSaved.text("Supprimer")

        } else {
            checkbtn.prop("checked", true);
            documentRowSaved.addClass("list-group-item");
            documentRowSaved.addClass("list-group-item-danger");
            documentRowSaved.text(_labTranslatableMessageCancelDeleting)
        }
    });

    $("#AccordionSearch").dxAccordion({ collapsible: true });

    $("#dossierLabForm").submit(function (event) {
        isSubmitting = true;

        event.preventDefault();
        setRelationAffaireValidator();
        if ($("#XMLDeclarationTracfin").prop('files').length > 0) {
            DevExpress.ui.notify(_labTranslatableMessageLoadingCases, "success");
            isSubmitting = false;
            return;
        }

        if ($("#XMLDeclarationTracfinNew").prop('files').length > 0) {
            DevExpress.ui.notify(_labTranslatableMessageLoadingCases, "success");
            isSubmitting = false;
            return;
        }

        if (!validateAllSections()) {
            isSubmitting = false;
            return;
        }

        showLoadPanel(_labTranslatableMessageSavingCase);

        resetAllHiddenOperationsAssurancesFieldsValues("dossierLabForm");
        resetAllHiddenOperationSuspectesFieldsValues();
        resetAllHiddenOperationImmobilieresFieldsValues("dossierLabForm");
        resetAllHiddenTiersFieldsValues();

        var isDeclarationtracfin = $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value");

        var blocSubmit = JSON.parse($("#BlocSubmit").val().toLowerCase());
        if (isDeclarationtracfin) {
            verifCheckedCategorieGroupeDeclaration(0);
            var blocSubmitIsDsComplementaire = JSON.parse($("#BlocSubmitIsDsComplementaire0").val().toLowerCase());
            var numeroTracFin = $("#NumeroPrecedenteDeclaration0").dxTextBox("instance").option("value");
            if ($('#IsQuestionTypeDeclarationComplementaireRadioGroupe').length) {
                var typeDeclaration = $("#IsQuestionTypeDeclarationComplementaireRadioGroupe").dxRadioGroup("instance").option("value");
            }

            var estComplemenataire = typeDeclaration === 'Complementary' || typeDeclaration === 'Complementaire';

            if (blocSubmitIsDsComplementaire && (numeroTracFin === null || numeroTracFin.length === 0) && estComplemenataire) {
                blocSubmit = blocSubmitIsDsComplementaire;
                $("#numero-precedente-declaration-container").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $("#numero-precedente-declaration-container").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $([document.documentElement, document.body]).animate({
                    scrollTop: $("#idBlocAproposDeLenvoi").offset().top
                }, 500);
                DevExpress.ui.notify(_labTranslatableMessageIsDsComplementaireObligatoire, "error");
                hideLoadPanel();
            }

            var blocSubmitIsDsRuptureRelation = JSON.parse($("#BlocSubmitIsDsRuptureRelation0").val().toLowerCase());
            if (blocSubmitIsDsRuptureRelation && !_IsNewDeclarationTracfin) {
                blocSubmit = blocSubmitIsDsRuptureRelation;

                $("#idCategorieGroupeDeclaration").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $("#idCategorieGroupeDeclaration").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $([document.documentElement, document.body]).animate({
                    scrollTop: $("#bloc8").offset().top
                }, 500);
                DevExpress.ui.notify(_labTranslatableMessageIsDsRuptureRelationObligatoire, "error");
                hideLoadPanel();
            }
            if (blocSubmitIsDsRuptureRelation && _IsNewDeclarationTracfin) {
                blocSubmit = blocSubmitIsDsRuptureRelation;

                $("#date-rupture-relation-container").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $("#date-rupture-relation-container").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $([document.documentElement, document.body]).animate({
                    scrollTop: $("#idBlocAproposDeLenvoi").offset().top
                }, 500);
                DevExpress.ui.notify(_labTranslatableMessageIsDsRuptureRelationObligatoire, "error");
                hideLoadPanel();
            }
            var statutDossierText = $("#StatutDossierId").dxSelectBox("instance").option("text");
            var statutDossierValue = $("#StatutDossierId").dxSelectBox("instance").option("value");
            var isStatutAttenteEnvoieTracfin = _Lang == "Fr" ? statutDossierText == "Attente d'envoi TRACFIN" : statutDossierText == "Wating for sending to TRACFIN";
            var isStatutAttenteValidation = statutDossierValue == 6; //ATTENTE_VALIDATION.
            var blocSubmitCategorieGroupeDeclaration = JSON.parse($("#BlocSubmitCategorieGroupeDeclaration0").val().toLowerCase());
            if (blocSubmitCategorieGroupeDeclaration && _IsNewDeclarationTracfin && (isStatutAttenteEnvoieTracfin || isStatutAttenteValidation)) {
                blocSubmit = blocSubmitCategorieGroupeDeclaration;

                $("#idCategorieGroupeDeclaration").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $("#idCategorieGroupeDeclaration").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $([document.documentElement, document.body]).animate({
                    scrollTop: $("#idBlocAproposDeLenvoi").offset().top
                }, 500);

                DevExpress.ui.notify(_labTranslatableMessageCategorieGroupeDeclarationObligatoireNew, "error");
                hideLoadPanel();

            }
            else if (blocSubmitCategorieGroupeDeclaration && !_IsNewDeclarationTracfin) {
                blocSubmit = blocSubmitCategorieGroupeDeclaration;

                $("#idCategorieGroupeDeclaration").animate({ backgroundColor: jQuery.Color("#fceca8") }, 1000);
                $("#idCategorieGroupeDeclaration").animate({ backgroundColor: jQuery.Color("#ffffff") }, 3000);
                $([document.documentElement, document.body]).animate({
                    scrollTop: $("#bloc8").offset().top
                }, 500);

                DevExpress.ui.notify(_labTranslatableMessageCategorieGroupeDeclarationObligatoire, "error");
                hideLoadPanel();
            }
        }

        if (!blocSubmit) {
            lastValueChangedPromise.then(function () {

                console.log('lastValueChangedPromise : resolved');

                updateInfoDsftFormData();

                var form = $("#dossierLabForm").closest("form");

                var formData = new FormData(form[0]);

                if (isPersonneDsIntegreesDsMissing()) {
                    hideLoadPanel();
                    isSubmitting = false;
                    return;
                }

                if (shouldAlertUserToEmptyPiecesJointesEnvoiTracfin()) {
                    showConfirmPopup(_labTranslatableMessageWarning,
                        _labTranslatableWarningMessageNoPiecesJointesEnvoiTracfin,
                        function () {
                            submitFormToServer(formData);
                        },
                        function () {
                            hideLoadPanel();
                            isSubmitting = false;
                        });

                    return;
                }
                submitFormToServer(formData);
            });
        }
        else {
            hideLoadPanel();
            isSubmitting = false;
        }
    });
});
function isPersonneDsIntegreesDsMissing() {
    return !validateBloc('panelPersonnes', function () {
        if (shouldAddPersonneIntegreDs(getCountTiersIntegresDs())) {
            return _labTranslatableMessageDossierLabAuMoinsUnTierSiDS;
        }
        return "";
    });
}
function resetAllHiddenAutreContributeurFieldsValues() {
    $("div[id^='" + _labDivAutreContributeurPrefix + "']").each(function () {
        if ($(this).css("display") === "none") {
            $(this).find("div[id^='" + _labTextBoxAutreContributeurPrefix + "']")
                .dxTextBox("instance")
                .reset();
        }
    });
}
function updateAutreContributeurFormFieldsValue() {
    $("div[id^='" + _labDivAutreContributeurPrefix + "']").each(function () {
        var instance = $(this).find("div[id^='" + _labTextBoxAutreContributeurPrefix + "']")
            .dxTextBox("instance");

        if (isNullOrUndefined(instance))
            return;

        var value = instance.option("value");

        $(this).find('input[name$=".AutreContributeur"]').val(value);

    });
}
function removeLastAddedEmptyContributeur() {
    $("div[id^='" + _labDivPartialInformationInternesDSFTPrefix + "']").each(function () {
        var lastContributeurItem = $(this).find(".contributor-item").last();
        if (lastContributeurItem.find("div[id^='" + _labDivContributeurPrefix + "']").css("display") === "none") {
            lastContributeurItem.remove();
        }
    });
}
function updateInfoDsftFormData(isDuplication) {
    var categorieGroupeId = $("#CategorieGroupeLabId").dxSelectBox("instance").option('value');

    if (isDuplication || categorieGroupeId != 2) {
        $("div[id^='" + _labDivPartialInformationInternesDSFTPrefix + "']").remove();
        return;
    }

    resetAllHiddenAutreContributeurFieldsValues();
    removeLastAddedEmptyContributeur();
    refreshInfoDsFtFormValues();
}

function refreshInfoDsFtFormValues() {
    $("div[id^='" + _labSelectBoxCategorieContributeurPrefix + "']").each(function () {
        var value = $(this).dxSelectBox("instance").option("value");
        console.log("category", value);
        $(this).parent().find("input.form-field").val(value);
    });

    $("div[id^='" + _labSelectBoxContributeurPrefix + "']").each(function () {
        var value = $(this).dxSelectBox("instance").option("value");
        console.log("contributor", value);
        $(this).parent().find("input.form-field").val(value);
    });

    $("div[id^='" + _labTextBoxAutreContributeurPrefix + "']").each(function () {
        var value = $(this).dxTextBox("instance").option("value");
        console.log("other", value);
        $(this).parent().find("input.form-field").val(value);
    });
}

function submitFormToServer(formData) {
    var isCov = $("#IsCov").dxCheckBox("instance").option("value");
    var isDossiersFluxMultiples = $("#DossiersFluxMultiples").dxCheckBox("instance").option("value");
    var statutDossierId = $("#StatutDossierId").dxSelectBox("instance").option("value");

    if (isCov == true && statutDossierId == 2) {
        hideLoadPanel();
        var confirmationCovCloture = DevExpress.ui.dialog.custom
            ({
                title: _commonTranslatableMessageWarning,
                message: _commonTranslatableMessageCovConfirmation,
                buttons:
                    [
                        {
                            text: _commonTranslatableMessageYes,
                            onClick: function () {
                                showLoadPanel(_labTranslatableMessageSavingCase);
                                $.ajax({
                                    method: "POST",
                                    data: formData,
                                    dataType: "json",
                                    url: '/' + culture + '/Lab/ServiceLab/CreateOrUpdateDossierLab',
                                    processData: false,
                                    contentType: false,
                                }).done(function (data) {
                                    if (data.status) {
                                        DevExpress.ui.notify(_labTranslatableMessageCaseSavedSuccessfully, "success", 3000);

                                        if (data.hasOwnProperty('exitDossier') && data.exitDossier) {
                                            location = '/' + culture + '/lab/dossiers/management';
                                        }
                                        else if (data.hasOwnProperty('cryptedId') && data.cryptedId != null) {
                                            location = '/' + culture + '/lab/dossier/edit/' + data.cryptedId;
                                        }
                                        else {
                                            location = '/' + culture + '/lab/dossiers/management';
                                        }
                                    }
                                    else {
                                        if (data.hasOwnProperty('message')) {
                                            DevExpress.ui.notify(data.message, "error", 3000);
                                        }
                                        else {
                                            DevExpress.ui.notify(_labTranslatableMessageCaseSavingErrorOccuredWithoutMessage, "error", 3000);
                                        }
                                    }
                                }).fail(function (e) {
                                    if (e.status == 401) {
                                        var popup = $("#modalReconnect").dxPopup('instance');
                                        popup.show();
                                        return;
                                    }
                                    if (e.data.hasOwnProperty('message'))
                                        DevExpress.ui.notify(e.message, "error", 3000);
                                    else
                                        DevExpress.ui.notify(_labTranslatableMessageCaseSavingErrorOccured, "error", 3000);
                                }).always(function (e) {
                                    hideLoadPanel();
                                    enableButton('buttonInitialize');
                                    enableButton('buttonSearch');
                                    isSubmitting = false;
                                });
                            }
                        },
                        {
                            text: _commonTranslatableMessageNo,
                            onClick: function () {
                                isSubmitting = false;
                                return true;
                            }
                        }
                    ]
            });
        confirmationCovCloture.show();
    }
    else if (isDossiersFluxMultiples == true && statutDossierId == 2) {
        hideLoadPanel();
        var confirmationCovCloture = DevExpress.ui.dialog.custom
            ({
                title: _commonTranslatableMessageWarning,
                message: _commonTranslatableMessageDossiersFluxMultiplesConfirmation,
                buttons:
                    [
                        {
                            text: _commonTranslatableMessageYes,
                            onClick: function () {
                                showLoadPanel(_labTranslatableMessageSavingCase);
                                $.ajax({
                                    method: "POST",
                                    data: formData,
                                    dataType: "json",
                                    url: '/' + culture + '/Lab/ServiceLab/CreateOrUpdateDossierLab',
                                    processData: false,
                                    contentType: false,
                                }).done(function (data) {
                                    if (data.status) {
                                        DevExpress.ui.notify(_labTranslatableMessageCaseSavedSuccessfully, "success", 3000);

                                        if (data.hasOwnProperty('exitDossier') && data.exitDossier) {
                                            location = '/' + culture + '/lab/dossiers/management';
                                        }
                                        else if (data.hasOwnProperty('cryptedId') && data.cryptedId != null) {
                                            location = '/' + culture + '/lab/dossier/edit/' + data.cryptedId;
                                        }
                                        else {
                                            location = '/' + culture + '/lab/dossiers/management';
                                        }
                                    }
                                    else {
                                        if (data.hasOwnProperty('message')) {
                                            DevExpress.ui.notify(data.message, "error", 3000);
                                        }
                                        else {
                                            DevExpress.ui.notify(_labTranslatableMessageCaseSavingErrorOccuredWithoutMessage, "error", 3000);
                                        }
                                    }
                                }).fail(function (e) {
                                    if (e.status == 401) {
                                        var popup = $("#modalReconnect").dxPopup('instance');
                                        popup.show();
                                        return;
                                    }
                                    if (e.data.hasOwnProperty('message'))
                                        DevExpress.ui.notify(e.message, "error", 3000);
                                    else
                                        DevExpress.ui.notify(_labTranslatableMessageCaseSavingErrorOccured, "error", 3000);
                                }).always(function (e) {
                                    hideLoadPanel();
                                    enableButton('buttonInitialize');
                                    enableButton('buttonSearch');
                                    isSubmitting = false;
                                });
                            }
                        },
                        {
                            text: _commonTranslatableMessageNo,
                            onClick: function () {
                                isSubmitting = false;
                                return true;
                            }
                        }
                    ]
            });
        confirmationCovCloture.show();
    }
    else {
        SaveDossierLab(false, 'False');
    }
}
function shouldAlertUserToEmptyPiecesJointesEnvoiTracfin() {
    return _IsNewDeclarationTracfin &&
        (isDossierEnAttenteEnvoiTracfin() || isDossierEnAttenteValidation()) &&
        isSelectedDirectionTracfin() &&
        getPiecesJointes(_labEnvoiTracfinCategorieDocumentId).length == 0;
}
function getStatutDossier() {
    var statutDossierElement = $("#StatutDossierId");

    if (statutDossierElement.length == 0)
        return null;

    var statutDossierSelectBox = statutDossierElement.dxSelectBox("instance");
    if (isNullOrUndefined(statutDossierSelectBox)) {
        return null;
    }
    return statutDossierSelectBox.option("selectedItem");
}

function isDossierEnAttenteEnvoiTracfin() {
    var statutDossier = getStatutDossier();

    return !isNullOrUndefined(statutDossier) && statutDossier.Code == _labCodeStatutDossierCloture;
}

function isDossierEnAttenteValidation() {
    var statutDossier = getStatutDossier();

    return !isNullOrUndefined(statutDossier) && statutDossier.Code == _labCodeStatutDossierAttenteValidation;
}

function validateAllSections() {

    if (!validateSectionTiers()) return false;

    if (!validateSectionOperationSuspectes()) return false;

    if (!validateSectionOperationsAssurances()) return false;

    if (!validateSectionPiecesJointes()) return false;

    return true;
}

function validateMotifSoupcon() {
    var statutDossierId = $("#StatutDossierId")
        .dxSelectBox("instance")
        .option("value");

    if (statutDossierId === 2 && $("div[id='MotifsSoupcons']").hasClass("mandatory")) {
        return validateHtmlEditor("MotifsSoupcons");
    }

    return true;
}

function validateSectionTiers() {
    if (!validateExistenceTiersDossierClotureSansDs()) return false;

    if (!validateDateNaissancePersonnePhysiqueDS()) return false;

    if (!validateNombreTiersIntegresDs()) return false;

    if (!validateReferencesFinancieres()) return false;

    if (!validateNombreLiensPersonnesIntegreesDs()) return false;

    if (!validateNombreCoordonnesPersonnesPhysiques()) return false;

    return true;
}

function validateExistenceTiersDossierClotureSansDs() {
    return validateBloc('panelPersonnes', existenceTiersDossierClotureSansDsCallback);
}

function existenceTiersDossierClotureSansDsCallback() {
    var statutDossierId = $("#StatutDossierId")
        .dxSelectBox("instance")
        .option("value");

    var selectedCategorie = $("#CategorieGroupeLabId")
        .dxSelectBox("instance")
        .option("selectedItem");

    var isCategorieDs = !isNullOrUndefined(selectedCategorie) && selectedCategorie.IsDs;

    if (statutDossierId === 2 && isCategorieDs && !_labIsDeclarationTracfin) {
        return $(".physicalPerson:visible").length || $(".moralPerson:visible").length ? "" : _labTranslatableMessageTiersRequired;
    }
    return "";
}

function validateDateNaissancePersonnePhysiqueDS() {
    return validateBloc('panelPersonnes', dateNaissancePersonnePhysiqueDSValidationCallback);
}
function dateNaissancePersonnePhysiqueDSValidationCallback() {
    var message;
    $(".physicalPerson").each(function () {
        if ($(this).css('display') == 'none' || !_labIsDeclarationTracfin) return;

        var dateNaissance = $(this).find("[id^='PersonnePhysiqueLabDateNaissance']")
            .dxDateBox("instance")
            .option("value");

        var estimationDateNaissance = $(this).find("[id^='PersonnePhysiqueLabEstimationDateNaissance']")
            .dxTextBox("instance")
            .option("value");

        if (isNullOrUndefined(dateNaissance) && isNullOrWhiteSpace(estimationDateNaissance)) {
            message = _labTranslatableMessageDateNaissanceOuEstimationObligatoire;
            return false;
        }
    });
    return message;
}
function validateNotorieteDefavorableOfVictimePresume() {
    var nbPp = $("div[class*='card physicalPerson']").length;
    var nbPm = $("div[class*='card moralPerson']").length;


    if (nbPp > 0) {
        for (let i = 0; i < nbPp; i++) {
            var indexPp = $("div[id*='rowPhysicalPerson']")[i].id.match(/\d/g).toString();

            var typeImplicationValueP = $("#PersonnePhysiqueLabTypeImplicationId" + indexPp).dxSelectBox("instance").option('value');
            var figureSanctionValueP = $("#PersonnePhysiqueLabFigureSanctionId" + indexPp).dxSelectBox("instance").option('value');
            var activiteCriminelleValueP = $("#PersonnePhysiqueLabActiviteCriminelleId" + indexPp).dxSelectBox("instance").option('value');
            var notorieteDefavorableValueP = $("#PersonnePhysiqueLabNotorieteDefavorable" + indexPp).dxHtmlEditor("instance").option('value');
        }

        if (typeImplicationValueP == 2) {
            if (figureSanctionValueP !== 2 || activiteCriminelleValueP !== 3 || notorieteDefavorableValueP) {
                $("#BlocSubmit").val('true');
                DevExpress.ui.notify(_labTranslatableMessageErrorVictimePresumeConditionsPP, "error");
                return;
            }
        }
    }

    if (nbPm > 0) {
        for (let i = 0; i < nbPm; i++) {
            var indexPm = $("div[id*='rowMoralPerson']")[i].id.match(/\d/g).toString();

            var typeImplicationValueM = $("#PersonneMoraleLabTypeImplicationId" + indexPm).dxSelectBox("instance").option('value');
            var figureSanctionValueM = $("#PersonneMoraleLabFigureSanctionId" + indexPm).dxSelectBox("instance").option('value');
            var activiteCriminelleValueM = $("#PersonneMoraleLabActiviteCriminelleId" + indexPm).dxSelectBox("instance").option('value');
            var notorieteDefavorableValueM = $("#PersonneMoraleLabNotorieteDefavorable" + indexPm).dxHtmlEditor("instance").option('value');
        }

        if (typeImplicationValueM == 2) {
            if (figureSanctionValueM !== 2 || activiteCriminelleValueM !== 3 || notorieteDefavorableValueM) {
                $("#BlocSubmit").val('true');
                DevExpress.ui.notify(_labTranslatableMessageErrorVictimePresumeConditionsPM, "error");
                return;
            }
        }
    }

    $("#BlocSubmit").val('false');

}

function desactivateNotorieteDefavorableIfVictimePresumeSelectedPP(i) {
    var typeImplicationValueP = $("#PersonnePhysiqueLabTypeImplicationId" + i).dxSelectBox("instance").option('value');
    var figureSanctionValueP = $("#PersonnePhysiqueLabFigureSanctionId" + i).dxSelectBox("instance");
    var activiteCriminelleValueP = $("#PersonnePhysiqueLabActiviteCriminelleId" + i).dxSelectBox("instance");
    var notorieteDefavorableValueP = $("#PersonnePhysiqueLabNotorieteDefavorable" + i).dxHtmlEditor("instance");

    if (typeImplicationValueP == 2) {
        figureSanctionValueP.option("value", 2);
        activiteCriminelleValueP.option("value", 3);
        notorieteDefavorableValueP.option("value", "");
        notorieteDefavorableValueP.option("readOnly", true);
    }
    else {
        notorieteDefavorableValueP.option("readOnly", false);

    }
}

function desactivateNotorieteDefavorableIfVictimePresumeSelectedPM(i) {
    var typeImplicationValueM = $("#PersonneMoraleLabTypeImplicationId" + i).dxSelectBox("instance").option('value');
    var figureSanctionValueM = $("#PersonneMoraleLabFigureSanctionId" + i).dxSelectBox("instance");
    var activiteCriminelleValueM = $("#PersonneMoraleLabActiviteCriminelleId" + i).dxSelectBox("instance");
    var notorieteDefavorableValueM = $("#PersonneMoraleLabNotorieteDefavorable" + i).dxHtmlEditor("instance");

    if (typeImplicationValueM == 2) {
        figureSanctionValueM.option("value", 2);
        activiteCriminelleValueM.option("value", 3);
        notorieteDefavorableValueM.option("value", "");
        notorieteDefavorableValueM.option("readOnly", true);
    }
    else {
        notorieteDefavorableValueM.option("readOnly", false);

    }
}

function validateNombreTiersIntegresDs() {
    return validateBloc('panelPersonnes', nombreTiersIntegresDsValidationCallback);
}
function nombreTiersIntegresDsValidationCallback() {
    if (doesNombreTiersIntegresDsExceedMaximum(getCountTiersIntegresDs())) {
        return getErrorMessageNombreMaxTiersAtteint();
    }
    return "";
}
function shouldAddPersonneIntegreDs(nombreTiersIntegresDs) {
    if (nombreTiersIntegresDs > 0) {
        return false;
    }

    var statutSelectBox = $("#StatutDossierId").dxSelectBox("instance");

    if (isNullOrUndefined(statutSelectBox))
        return _labIsDeclarationTracfin;

    var selectedStatutId = statutSelectBox.option("value");

    if (isNullOrUndefined(selectedStatutId))
        return _labIsDeclarationTracfin;

    var noValidationStatuses = [_labIdStatutDossierAttenteDocuments,
        _labIdStatutDossierEnCours,
        _labIdStatutDossierPendingEnCours];

    if (noValidationStatuses.indexOf(selectedStatutId) >= 0)
        return false;

    return _labIsDeclarationTracfin;
}
function getErrorMessageNombreMaxTiersAtteint() {
    return _labTranslatableMessageErrorNombreMaxTiersIntegresDsAtteint
        .format(_labNombreMaximumTiersIntegresDs);
}
function doesNombreTiersIntegresDsExceedMaximum(nombreTiersIntegresDs) {
    return nombreTiersIntegresDs > _labNombreMaximumTiersIntegresDs;
}
function validateReferencesFinancieres() {
    if (!validateReferencesFinancieresForEachTier()) return false;

    // if (!validateNombreReferencesFinancieresTiersIntegresDsParDossier()) return false;

    return true;
}
function validateReferencesFinancieresForEachTier() {
    var result = true;
    $("div[id^='gridSupportFinancier']").each(function () {
        var dataGridElement = $(this);
        if (!validateBloc('blocTiers', function () {
            return validateReferencesFinancieresPersonne(dataGridElement);
        })) {
            result = false;
            return false;
        }
    });
    return result;
}
function validateReferencesFinancieresPersonne(dataGrid) {
    var dataSource = dataGrid.dxDataGrid("instance").getDataSource().items();

    if (_IsNewDeclarationTracfin && !ensureTypeReferenceLabNotNullOrEmpty(dataSource)) {
        return _labTranslatableMessageTypeRefFinancieresObligatoire;
    }

    if (!ensureNombreReferencesParPersonneDoesNotExceedMaxNumber(dataSource)) {
        return getErrorMessageNombreMaxReferencesFinancieresParPersonneAtteint();
    }
}
function ensureTypeReferenceLabNotNullOrEmpty(dataSource) {
    var invalidRowsNumber = dataSource.filter(item => item.TypeReferenceLabId == null ||
        item.TypeReferenceLabId == undefined)
        .length;

    if (invalidRowsNumber > 0) {
        return false;
    }

    return true;
}
function ensureNombreReferencesParPersonneDoesNotExceedMaxNumber(dataSource) {
    if (dataSource == null || !dataSource) return true;

    if (dataSource.length <= _labNombreMaximumReferencesFinancieresParPersonne) return true;

    return false;
}
function validateNombreReferencesFinancieresTiersIntegresDsParDossier() {
    return validateBloc('panelPersonnes', nombreReferencesFinancieresTiersIntegresDsParDossierValidationCallback);
}
function nombreReferencesFinancieresTiersIntegresDsParDossierValidationCallback() {
    return doesNbrReferencesFinancieresTiersIntegresDsExceedMaxNumber() ?
        getErrorMessageNombreMaxReferencesFinancieresAtteint() :
        "";
}
function doesNbrReferencesFinancieresTiersIntegresDsExceedMaxNumber() {
    return getCountReferencesFinancieresTiersIntegresDs() > _labNombreMaximumReferencesFinancieresTiersIntegreDsParDossier;
}
function getErrorMessageNombreMaxReferencesFinancieresAtteint() {
    return _labTranslatableMessageErrorNombreMaxFinancialReferencesAtteint.format(_labNombreMaximumReferencesFinancieresTiersIntegreDsParDossier);
}
function getErrorMessageNombreMaxReferencesFinancieresParPersonneAtteint() {
    return _labTranslatableMessageErrorNombreMaxFinancialReferencesParTiersAtteint
        .format(_labNombreMaximumReferencesFinancieresParPersonne);
}
function validateNombreLiensPersonnesIntegreesDs() {
    return validateBloc('panelPersonnes', nombreLiensPersonnesIntegreesDsValidationCallback);
}
function nombreLiensPersonnesIntegreesDsValidationCallback() {
    return doesNombreLiensPersonnesIntegreesDsParDossierExceedsMaxNumber() ?
        getErrorMessageNombreMaxLiensPersonnesIntegreesDsParDossierAtteint() :
        "";
}
function doesNombreLiensPersonnesIntegreesDsParDossierExceedsMaxNumber() {
    return getCountLiensPersonnesIntegreesDsParDossier() > _labNombreMaximumLiensPersonnesIntegreesDsParDossier;
}
function getErrorMessageNombreMaxLiensPersonnesIntegreesDsParDossierAtteint() {
    return _labTranslatableMessageErrorNombreMaxLiensPersonnesIntegreesDsParDossierAtteint
        .format(_labNombreMaximumLiensPersonnesIntegreesDsParDossier);
}
function validateNombreCoordonnesPersonnesPhysiques() {
    return validateBloc('panelPersonnes', nombreCoordonnesPersonnesPhysiquesValidationCallback);
}
function nombreCoordonnesPersonnesPhysiquesValidationCallback() {
    var errorMessage = "";
    $(".physicalPerson").each(function () {
        if ($(this).css('display') == 'none') {
            return;
        }
        var index = $(this).attr('id').substring("rowPhysicalPerson".length);

        if (doesNombreCoordonneesPersonnesPhysiquesExceedsMaxNumber(index)) {
            errorMessage = getErrorMessageNombreMaxCoordonneesPersonnesPhysiquesAtteint();
            return false;
        }
    });
    return errorMessage;
}
function validateSectionOperationSuspectes() {
    if (!validateNombreOperationsSuspectes()) return false;

    if (!validateNombreOperationsEnCours()) return false;

    if (!validateDateLimiteExecutionOperationEnCours()) return false;

    return true;
}
function validateNombreOperationsSuspectes() {
    return validateBloc('OperationSuspectDeclarationTracfinDiv', nombreOperationsSuspectesValidationCallback);
}
function nombreOperationsSuspectesValidationCallback() {
    return nombreOperationsValidationCallback(_labNombreMaximumOperationsSuspectes, "operationSuspectTable");
}
function nombreOperationsValidationCallback(arrayNombresMax, operationsTableId) {
    if (!doesNombreOperationsExceedMaximum(arrayNombresMax, operationsTableId)) {
        return "";
    }

    var max = getNombreMaximumOperations(arrayNombresMax);

    return getErrorMessageNombreMaxOperationsAtteint(max);
}
function doesNombreOperationsExceedMaximum(arrayNombreMax, operationsTableId) {
    var max = getNombreMaximumOperations(arrayNombreMax);

    if (!max) return false;

    if ($("#" + operationsTableId + "  tbody tr:not(.dx-hidden)").length <= max) return false;

    return true;
}
function getNombreMaximumOperations(arrayNombreMax) {
    var professionCode = $("#createOrUpdateDossierLab_CodeProfession").val();

    if (!professionCode) return undefined;

    return arrayNombreMax[professionCode];
}
function validateNombreOperationsEnCours() {
    return validateBloc('OperationCoursExecutionDeclarationTracfinDiv', nombreOperationsEnCoursValidationCallback);
}
function nombreOperationsEnCoursValidationCallback() {
    return nombreOperationsValidationCallback(_labNombreMaximumOperationsEnCours, "operationEnCoursTable");
}
function validateDateLimiteExecutionOperationEnCours() {
    if ($("#IsOperationCoursExecution").length == 0) {
        return true;
    }

    if (!($("div[id='IsOperationCoursExecution']").dxCheckBox("instance").option("value"))) {
        return true;
    }
    var isValid = true;

    var dateLimiteExecutionPrefix = "DateLimiteExecutionOperationEnCours";
    $(".operationEnCoursRow div[id^='" + dateLimiteExecutionPrefix + "']").each(function () {
        var value = $(this).dxDateBox("instance").option("value");
        var toDayDate = new Date();
        toDayDate.setHours(0, 0, 0, 0);
        var isDateLimiteExecutionOperationEnCoursValid = validateBloc("OperationCoursExecutionDeclarationTracfinDiv", function () {
            if (value == null || !value) {
                return _labTranslatableMessageDateLimiteExecutionOperationEnCoursInvalide;
            }

            value.setHours(0, 0, 0, 0);
            if (value <= toDayDate) {
                return _labTranslatableMessageDateLimiteExecutionOperationEnCoursInvalide;
            }
            return "";
        }, 100);

        if (!isDateLimiteExecutionOperationEnCoursValid) {
            isValid = false;
            return false;
        }
    });
    return isValid;
}
function validateSectionOperationsAssurances() {
    if (!validateOperationsAssurancesEnCoursSusceptiblesOpposition()) return false;

    if (!validateNombreContratsAssuranceNonVie()) return false;

    return true;
}
function validateOperationsAssurancesEnCoursSusceptiblesOpposition() {
    var checkBoxIdPrefix = "partialOperationsCompanieAssurance_OperationEnCoursDexecution";
    var isValid = true;
    $("div[id^='" + checkBoxIdPrefix + "']").each(function () {
        if (!($(this).dxCheckBox("instance").option("value"))) {
            return;
        }

        var order = $(this).attr("id").substring(checkBoxIdPrefix.length);
        var isBlocPrecisionSurFluxValid = validateBloc("divPrecisionSurFlux" + order, function () {
            var errorMessage = "";
            var dataSourceOperations = $("#partialOperationsCompanieAssurance_OperationsSusceptiblesOppositionDatagrid" + order)
                .dxDataGrid("instance")
                .getDataSource()
                .items();

            if (dataSourceOperations.length == 0) {
                errorMessage = _labTranslatableMessageOperationsAssurancesSusceptiblesOppositionObligatoire;
            }
            return errorMessage;
        }, 100);

        if (!isBlocPrecisionSurFluxValid) {
            isValid = false;
            return false;
        }
    });
    return isValid;
}
function validateNombreContratsAssuranceNonVie() {
    var result = true;
    $("div[id^='divPrecisionSurContratConcerne']").each(
        function () {
            var sectionId = $(this).attr('id');
            if (!validateBloc(sectionId, () => nombreContratsAssuranceNonVieValidationCallback(sectionId))) {
                result = false;
                return false;
            }
        });
    return result;
}
function nombreContratsAssuranceNonVieValidationCallback(sectionId) {
    var dataGridContratsConcernes = $("#" + sectionId + " div[id^='partialOperationsCompanieAssurance_NumContratConcerne']")
        .dxDataGrid("instance");

    if (dataGridContratsConcernes == null || !dataGridContratsConcernes) return "";

    var nombreContratsConcernes = dataGridContratsConcernes.getDataSource().items().length;

    return nombreContratsConcernes > _labNombreMaximumContratsConcernes ?
        getErrorMessageNombreMaximumContratsConcernesAtteint() :
        "";
}
function validateSectionPiecesJointes() {
    if (!validatePiecesJointesEnvoiTracfinOnSave()) {
        return false;
    }

    return true;
}
function isSelectedDirectionTracfin() {
    if ($("#DirectionId").length == 0) {
        return false;
    }

    var directionSelectBox = $("#DirectionId").dxSelectBox("instance");

    if (isNullOrUndefined(directionSelectBox)) {
        return false;
    }

    var selectedDirection = directionSelectBox.option("selectedItem");

    return !isNullOrUndefined(selectedDirection) && selectedDirection.IsTracfin;
}
function validatePiecesJointesEnvoiTracfinOnSave() {
    return validateBloc("bloc6", piecesJointesEnvoiTracfinValidationCallback);
}
function piecesJointesEnvoiTracfinValidationCallback() {
    var piecesJointesEnvoiTracfin = getPiecesJointes(_labEnvoiTracfinCategorieDocumentId);
    ; if (piecesJointesEnvoiTracfin.length > _labNombreMaximumPiecesJointesCatgorieTracfin) {
        return getErrorMessageNombreMaximumPiecesJointesEnvoiTracfinParDossierAtteint();
    }

    if (!arePiecesJointesEnvoiTracfinExtensionsValid(piecesJointesEnvoiTracfin)) {
        return getErrorMessageInvalidExtensionPieceJointeEnvoiTracfin();
    }

    var taillePiecesJointesEnvoiTracfin = getTaillePiecesJointes(piecesJointesEnvoiTracfin);

    if (taillePiecesJointesEnvoiTracfin > _labTailleMaxPiecesJointesDeclarationTracfinMo * 1024 * 1024) {
        return getErrorMessageTailleMaximalePiecesJointesEnvoiTracfinAtteinte();
    }
    return "";
}

function arePiecesJointesEnvoiTracfinExtensionsValid(piecesJointesEnvoiTracfin) {
    var result = true;
    piecesJointesEnvoiTracfin.forEach(
        function (pj) {
            if (!checkFileExtension(pj.FileName, _labExtensionsPiecesJointesDeclarationTracfin)) {
                result = false;
                return false;
            }
        });
    return result;
}
function validateFinancialReferences() {
    var financialReferencesValid = true;
    $(".physicalPerson:visible").each(function () {
        financialReferencesValid = validatePersonFinancialReferences(
            $(this).find('[id^="PersonnePhysiqueLabNatureRelationClientId"]'),
            $(this).find('[id^="gridSupportFinancierPersonnePhysique"]'));

        if (!financialReferencesValid) {
            return false;
        }
    });
    if (!financialReferencesValid) {
        return false;
    }
    $(".moralPerson:visible").each(function () {
        financialReferencesValid = validatePersonFinancialReferences(
            $(this).find('[id^="PersonneMoraleLabNatureRelationClientId"]'),
            $(this).find('[id^="gridSupportFinancierPersonneMorale"]'));

        if (!financialReferencesValid) {
            return false;
        }
    });
    return financialReferencesValid;
}
function validatePersonFinancialReferences(selectBoxNatureRelationClient, dataGridReferencesFinancieres) {
    var natureRelationClientId = selectBoxNatureRelationClient.dxSelectBox("instance").option("value");
    if (natureRelationClientId == 1 || natureRelationClientId == 2) {
        var countReferencesFinancieres = dataGridReferencesFinancieres.dxDataGrid("instance").getDataSource().items().length;
        if (countReferencesFinancieres == 0) {
            return false;
        }
    }
    return true;
}
function addCoordonneePhysicalPerson(orderPersonnePhysique) {
    showLoadPanel(_labTranslatableMessageAddingNewCoordonnee);
    var countCoordonneePhysicalPerson = $('#coordonnees-personnephysique-' + orderPersonnePhysique + ' > .coordonneePhysicalPerson').length;
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        url: '/' + culture + '/Lab/ServiceLab/AddCoordonneePersonnePhysiqueForm?orderPersonnePhysique=' + orderPersonnePhysique + '&order=' + countCoordonneePhysicalPerson,
    }).done(function (data) {
        $("#coordonnees-personnephysique-" + orderPersonnePhysique).append(data);
        toggleDisplayButtonAddNewCoordonnee(orderPersonnePhysique);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function toggleDisplayButtonAddNewCoordonnee(index) {
    var canAddNewCoordonnee = getNombreCoordonneesPersonnePhysique(index) < _labNombreMaximumCoordonneesPersonnesPhysiques;
    $("#btnAddNewCoordonneePhysicalPerson" + index).toggle(canAddNewCoordonnee);
}


function addCoordonneeActivite(orderPersonnePhysique) {
    showLoadPanel(_labTranslatableMessageAddingNewCoordonnee);
    if ($("#physicalPersonActivityCoordonnee" + orderPersonnePhysique))
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            contentType: "application/json; charset=utf-8",
            url: '/' + culture + '/Lab/ServiceLab/AddActiviteCoordonneePersonnePhysiqueForm?orderPersonnePhysique=' + orderPersonnePhysique,
        }).done(function (data) {
            if ($("#physicalPersonActivityCoordonnee" + orderPersonnePhysique)) $("#physicalPersonActivityCoordonnee" + orderPersonnePhysique).remove();
            $("#coordonnees-activite-" + orderPersonnePhysique).append(data);
            if ($("#btnAddNewCoordonneeActivite" + orderPersonnePhysique)) $("#btnAddNewCoordonneeActivite" + orderPersonnePhysique).hide();

        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
}

function removeCoordonneePhysicalPerson(orderPersonnePhysique, orderCoordonnee) {
    $("#physicalPersonCoordonnee" + orderPersonnePhysique + '-' + orderCoordonnee).html('<input name="DossierLabPersonnePhysiques[' + orderPersonnePhysique + '].PersonnePhysiqueLab.CoordonneePersonnePhysiqueLabs[' + orderCoordonnee + '].ToDelete" value="True" id="coordonneePersonnePhysiqueLabsToDelete' + orderPersonnePhysique + '-' + orderCoordonnee + '" type="hidden">');
    $("#physicalPersonCoordonnee" + orderPersonnePhysique + '-' + orderCoordonnee).hide();
}


function addCoordonneeMoralePerson(orderPersonneMorale) {
    showLoadPanel(_labTranslatableMessageAddingNewCoordonnee);
    if ($("#moralePersonCoordonnee" + orderPersonneMorale))
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            contentType: "application/json; charset=utf-8",
            url: '/' + culture + '/Lab/ServiceLab/AddCoordonneePersonneMoraleForm?orderPersonneMorale=' + orderPersonneMorale,
        }).done(function (data) {
            if ($("#moralePersonCoordonnee" + orderPersonneMorale)) $("#moralePersonCoordonnee" + orderPersonneMorale).remove();
            $("#coordonnees-personneMorale-" + orderPersonneMorale).append(data);
            if ($("#btnAddNewCoordonneeMoralePerson" + orderPersonneMorale)) $("#btnAddNewCoordonneeMoralePerson" + orderPersonneMorale).hide();
            if ($("#remRowCoordonneePersonneMoraleLab" + orderPersonneMorale)) $("#remRowCoordonneePersonneMoraleLab" + orderPersonneMorale).show();

        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
}

function removeCoordonneeMoralePerson(orderPersonneMorale) {
    $("#moralePersonCoordonnee" + orderPersonneMorale).html('<input name="DossierLabPersonneMorales[' + orderPersonneMorale + '].PersonneMoraleLab.Coordonnee.ToDelete" value="True" id="coordonneePersonneMoraleLabsToDelete' + orderPersonneMorale + '" type="hidden">');
    if ($("#moralePersonCoordonnee" + orderPersonneMorale)) $("#moralePersonCoordonnee" + orderPersonneMorale).hide();
    if ($("#remRowCoordonneePersonneMoraleLab" + orderPersonneMorale)) $("#remRowCoordonneePersonneMoraleLab" + orderPersonneMorale).hide();
    if ($("#btnAddNewCoordonneeMoralePerson" + orderPersonneMorale)) $("#btnAddNewCoordonneeMoralePerson" + orderPersonneMorale).removeClass("dx-state-invisible");
    if ($("#btnAddNewCoordonneeMoralePerson" + orderPersonneMorale)) $("#btnAddNewCoordonneeMoralePerson" + orderPersonneMorale).show();
}

function updateListPieceJointesForm(personnePhysiqueOrder) {
    $("#pieces-jointes-form-container" + personnePhysiqueOrder).html("");
    var data = $("#gridPieceIdentite" + personnePhysiqueOrder).dxDataGrid("instance").getDataSource().items();
    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var typePieceIdentiteId = data[i].TypePieceIdentiteId;
        var numero = data[i].Numero;
        var autorite = data[i].Autorite;
        var paysDelivranceId = data[i].PaysDelivranceId;
        var dateValiditeDebut = formatDateGrid(data[i].DateValiditeDebut);
        var dateValiditeFin = formatDateGrid(data[i].DateValiditeFin);

        $("#pieces-jointes-form-container" + personnePhysiqueOrder).append('<input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].TypePieceIdentiteId" value="' + typePieceIdentiteId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].Numero" value="' + numero + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].Autorite" value="' + autorite + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].PaysDelivranceId" value="' + paysDelivranceId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].DateValiditeDebut" value="' + dateValiditeDebut + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.PieceIdentites[' + i + '].DateValiditeFin" value="' + dateValiditeFin + '" type="hidden">')
    }
}

function updateListSupportFinancierPersonnePhysiquesForm(personnePhysiqueOrder) {
    $("#support-financiers-personne-physique-form-container" + personnePhysiqueOrder).html("");
    var data = $("#gridSupportFinancierPersonnePhysique" + personnePhysiqueOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var typeCompteId = data[i].TypeCompteId;
        var iban = data[i].Iban ? data[i].Iban : "";
        var typeLienSupportId = data[i].TypeLienSupportId;
        var typeReferenceLabId = data[i].TypeReferenceLabId;
        var autreTypeReference = (data[i].AutreTypeReference != null) ? data[i].AutreTypeReference : "";
        var codeBic = (data[i].CodeBic != null) ? data[i].CodeBic : "";
        var codeCib = (data[i].CodeCib != null) ? data[i].CodeCib : "";
        var solde = formatDecimal(data[i].Solde, culture);
        var dateOuvertureCompte = formatDateGrid(data[i].DateOuvertureCompte);
        var clientId = data[i].__KEY__ ? data[i].__KEY__ : "";
        $("#support-financiers-personne-physique-form-container" + personnePhysiqueOrder).append('\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].TypeReferenceLabId" value="' + typeReferenceLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].TypeCompteId" value="' + typeCompteId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].Iban" value="' + iban + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].TypeLienSupportId" value="' + typeLienSupportId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].AutreTypeReference" value="' + autreTypeReference + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].CodeBic" value="' + codeBic + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].CodeCib" value="' + codeCib + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].Solde" value="' + solde + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].DateOuvertureCompte" value="' + dateOuvertureCompte + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.SupportFinancierPersonnePhysiques[' + i + '].ClientId" value="' + clientId + '" type="hidden">')
    }
}
function updateListSupportFinancierNonConnusForm(nonConnuOrder) {
    $("#support-financiers-non-connu-form-container" + nonConnuOrder).html("");
    var data = $("#gridSupportFinancierNonConnus" + nonConnuOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedNonConnuLabId = (data[i].CryptedNonConnuLabId != null) ? data[i].CryptedNonConnuLabId : "";
        var typeCompteId = data[i].TypeCompteId;
        var iban = data[i].Iban ? data[i].Iban : "";
        var typeLienSupportId = data[i].TypeLienSupportId;
        var typeReferenceLabId = data[i].TypeReferenceLabId;
        var autreTypeReference = (data[i].AutreTypeReference != null) ? data[i].AutreTypeReference : "";
        var codeBic = (data[i].CodeBic != null) ? data[i].CodeBic : "";
        var codeCib = (data[i].CodeCib != null) ? data[i].CodeCib : "";
        var solde = formatDecimal(data[i].Solde, culture);
        var dateOuvertureCompte = formatDateGrid(data[i].DateOuvertureCompte);
        var clientId = data[i].__KEY__ ? data[i].__KEY__ : "";

        $("#support-financiers-non-connu-form-container" + nonConnuOrder).append('\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedNonConnuLabId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].TypeReferenceLabId" value="' + typeReferenceLabId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].TypeCompteId" value="' + typeCompteId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].Iban" value="' + iban + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].TypeLienSupportId" value="' + typeLienSupportId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].AutreTypeReference" value="' + autreTypeReference + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].CodeBic" value="' + codeBic + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].CodeCib" value="' + codeCib + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].Solde" value="' + solde + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].DateOuvertureCompte" value="' + dateOuvertureCompte + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.SupportFinancierNonConnus[' + i + '].ClientId" value="' + clientId + '" type="hidden">')
    }
}


function updateListLienPersonnePhysiqueMoralesForm(personneMoraleOrder) {
    $("#lien-personne-physique-morale-form-container" + personneMoraleOrder).html("");
    var data = $("#gridLienPersonnePhysiqueMorale" + personneMoraleOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var nomNaissance = data[i].NomNaissance;
        var prenom = data[i].Prenom;
        var categorieLienPersonnePhysiqueMoraleId = data[i].CategorieLienPersonnePhysiqueMoraleId;
        var typeLienPersonnePhysiqueMoraleId = data[i].TypeLienPersonnePhysiqueMoraleId;
        var dateNaissance = formatDateGrid(data[i].DateNaissance);
        var precisions = data[i].Precisions == undefined ? "" : data[i].Precisions;
        var certitudeLien = data[i].CertitudeLien == undefined ? false : data[i].CertitudeLien;
        var lieuNaissance = data[i].LieuNaissance == undefined ? "" : data[i].LieuNaissance;

        $("#lien-personne-physique-morale-form-container" + personneMoraleOrder).append('<input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].NomNaissance" value = "' + nomNaissance + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].Prenom" value="' + prenom + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].TypeLienPersonnePhysiqueMoraleId" value="' + typeLienPersonnePhysiqueMoraleId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].CategorieLienPersonnePhysiqueMoraleId" value="' + categorieLienPersonnePhysiqueMoraleId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].DateNaissance" value = "' + dateNaissance + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].CertitudeLien" value="' + certitudeLien + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].LieuNaissance" value = "' + lieuNaissance + '" type = "hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonnePhysiques[' + i + '].Precisions" value="' + precisions + '" type="hidden">');

    }
}

function updateListRepresentantsLegauxPersonneMoralesForm(personneMoraleOrder) {
    $("#representants-legaux-form-container" + personneMoraleOrder).html("");
    var data = $("#gridRepresentantsLegauxPersonneMorale" + personneMoraleOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var nomNaissance = data[i].NomNaissance;
        var prenoms = data[i].Prenoms;
        var dateNaissance = formatDateGrid(data[i].DateNaissance);
        var villeNaissance = data[i].VilleNaissance;
        var paysNaissanceId = data[i].PaysNaissanceId;
        var fonction = data[i].Fonction;

        $("#representants-legaux-form-container" + personneMoraleOrder).append('<input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoralesLab.RepresentantLegals[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.RepresentantLegals[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.RepresentantLegals[' + i + '].NomNaissance" value = "' + nomNaissance + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.RepresentantLegals[' + i + '].Prenoms" value="' + prenoms + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.RepresentantLegals[' + i + '].DateNaissance" value="' + dateNaissance + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.RepresentantLegals[' + i + '].VilleNaissance" value = "' + villeNaissance + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.RepresentantLegals[' + i + '].PaysNaissanceId" value = "' + paysNaissanceId + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.RepresentantLegals[' + i + '].Fonction" value="' + fonction + '" type="hidden">');

    }
}

//function CertitudeLien_calculateCellValue(data) {
//    if (data.CertitudeLien == null || data.CertitudeLien == 0) return 0;
//    return (data.CertitudeLien != null ? data.CertitudeLien == 1 ? 1 : 0 : 0);
//}

function CertitudeLien_calculateCellValue(data) {
    if (data.CertitudeLien == null) return null;
    return (data.CertitudeLien != null ? data.CertitudeLien == 1 ? 1 : 0 : 0);
}

function updateListSupportFinancierPersonneMoralesForm(personneMoraleOrder) {
    $("#support-financiers-personne-morale-form-container" + personneMoraleOrder).html("");
    var data = $("#gridSupportFinancierPersonneMorale" + personneMoraleOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var nom = data[i].Nom;
        var prenom = data[i].Prenom;
        var dateNaissance = formatDateGrid(data[i].DateNaissance);
        var villeNaissance = data[i].VilleNaissance;
        var typeCompteId = data[i].TypeCompteId;
        var typeLienSupportId = data[i].TypeLienSupportId;
        var iban = data[i].Iban ? data[i].Iban : "";
        var typeReferenceLabId = data[i].TypeReferenceLabId;
        var autreTypeReference = (data[i].AutreTypeReference != null) ? data[i].AutreTypeReference : "";
        var codeBic = (data[i].CodeBic != null) ? data[i].CodeBic : "";
        var codeCib = (data[i].CodeCib != null) ? data[i].CodeCib : "";
        var solde = formatDecimal(data[i].Solde, culture);
        var dateOuvertureCompte = formatDateGrid(data[i].DateOuvertureCompte);
        var clientId = data[i].__KEY__ ? data[i].__KEY__ : "";

        $("#support-financiers-personne-morale-form-container" + personneMoraleOrder).append('<input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].Nom" value="' + nom + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].Prenom" value="' + prenom + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].DateNaissance" value="' + dateNaissance + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].VilleNaissance" value="' + villeNaissance + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].TypeCompteId" value="' + typeCompteId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].TypeLienSupportId" value="' + typeLienSupportId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].Iban" value = "' + iban + '" type = "hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].TypeReferenceLabId" value="' + typeReferenceLabId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].AutreTypeReference" value="' + autreTypeReference + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].CodeBic" value="' + codeBic + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].CodeCib" value="' + codeCib + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].Solde" value="' + solde + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].DateOuvertureCompte" value="' + dateOuvertureCompte + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.SupportFinancierPersonneMorales[' + i + '].ClientId" value="' + clientId + '" type="hidden">')

    }
}

function updateListLienPersonnePhysiquePhysiquesForm(personnePhysiqueOrder) {
    $("#lien-personne-physique-physique-form-container" + personnePhysiqueOrder).html("");
    var data = $("#gridLienPersonnePhysiquePhysique" + personnePhysiqueOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var nomNaissance = data[i].NomNaissance;
        var prenom = data[i].Prenom;
        var typeLienPersonnePhysiquePhysiqueId = data[i].TypeLienPersonnePhysiquePhysiqueId;
        var categorieLienPersonnePhysiquePhysiqueId = data[i].CategorieLienPersonnePhysiquePhysiqueId;
        var dateNaissance = formatDateGrid(data[i].DateNaissance);
        var precisions = data[i].Precisions == undefined ? "" : data[i].Precisions;
        var certitudeLien = data[i].CertitudeLien == undefined ? false : data[i].CertitudeLien;
        var lieuNaissance = data[i].LieuNaissance == undefined ? "" : data[i].LieuNaissance;

        $("#lien-personne-physique-physique-form-container" + personnePhysiqueOrder).append('<input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].NomNaissance" value="' + nomNaissance + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].Prenom" value="' + prenom + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].TypeLienPersonnePhysiquePhysiqueId" value="' + typeLienPersonnePhysiquePhysiqueId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].CategorieLienPersonnePhysiquePhysiqueId" value="' + categorieLienPersonnePhysiquePhysiqueId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].DateNaissance" value = "' + dateNaissance + '" type = "hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].CertitudeLien" value="' + certitudeLien + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].LieuNaissance" value = "' + lieuNaissance + '" type = "hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonnePhysiques[' + i + '].Precisions" value="' + precisions + '" type="hidden">');

    }
}

function updateListLienNonConnuPhysiquePhysiquesForm(nonConnuOrder) {
    $("#lien-non-connu-physique-physique-form-container" + nonConnuOrder).html("");
    var data = $("#gridNonConnuLienPersonnePhysiquePhysique" + nonConnuOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var cryptedNonConnuLabId = (data[i].CryptedNonConnuLabId != null) ? data[i].CryptedNonConnuLabId : "";
        var nomNaissance = data[i].NomNaissance;
        var categorieLienPersonnePhysiquePhysique = data[i].CategorieLienPersonnePhysiquePhysiqueId;
        var prenom = data[i].Prenom;
        var typeLienPersonnePhysiquePhysiqueId = data[i].TypeLienPersonnePhysiquePhysiqueId;
        var dateNaissance = formatDateGrid(data[i].DateNaissance);
        var precisions = data[i].Precisions == undefined ? "" : data[i].Precisions;
        var certitudeLien = data[i].CertitudeLien == undefined ? false : data[i].CertitudeLien;
        var lieuNaissance = data[i].LieuNaissance == undefined ? "" : data[i].LieuNaissance;


        $("#lien-non-connu-physique-physique-form-container" + nonConnuOrder).append('<input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].CryptedNonConnuLabId" value="' + cryptedNonConnuLabId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].NomNaissance" value="' + nomNaissance + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].CategorieLienPersonnePhysiquePhysiqueId" value="' + categorieLienPersonnePhysiquePhysique + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].Prenom" value="' + prenom + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].TypeLienPersonnePhysiquePhysiqueId" value="' + typeLienPersonnePhysiquePhysiqueId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].DateNaissance" value = "' + dateNaissance + '" type = "hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].CertitudeLien" value="' + certitudeLien + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].LieuNaissance" value = "' + lieuNaissance + '" type = "hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonnePhysiques[' + i + '].Precisions" value="' + precisions + '" type="hidden">');

    }
}

function updateListLienNonConnuMoralesForm(nonConnuOrder) {
    $("#lien-non-connu-morale-physique-form-container" + nonConnuOrder).html("");
    var data = $("#gridNonConnuLienPersonneMoralePhysique" + nonConnuOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {

        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var cryptedNonConnuLabId = (data[i].CryptedNonConnuLabId != null) ? data[i].CryptedNonConnuLabId : "";
        var raisonSociale = data[i].RaisonSociale;
        var categorieLienPersonneMoralePhysiqueId = data[i].CategorieLienPersonneMoralePhysiqueId;
        var formeJuridiqueId = data[i].FormeJuridiqueId;
        var typeLienPersonneMoralePhysiqueId = data[i].TypeLienPersonneMoralePhysiqueId;
        var immatriculation = data[i].Immatriculation == undefined ? "" : data[i].Immatriculation;
        var precisions = data[i].Precisions == undefined ? "" : data[i].Precisions;
        var certitudeLien = data[i].CertitudeLien == undefined ? false : data[i].CertitudeLien;

        $("#lien-non-connu-morale-physique-form-container" + nonConnuOrder).append('\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].CryptedId" value = "' + cryptedId + '" type = "hidden" >\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].CryptedNonConnuLabId" value="' + cryptedNonConnuLabId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].RaisonSociale" value = "' + raisonSociale + '" type = "hidden" >\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].CategorieLienPersonneMoralePhysiqueId" value = "' + categorieLienPersonneMoralePhysiqueId + '" type = "hidden" >\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].FormeJuridiqueId" value="' + formeJuridiqueId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].TypeLienPersonneMoralePhysiqueId" value="' + typeLienPersonneMoralePhysiqueId + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].Immatriculation" value = "' + immatriculation + '" type = "hidden" >\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].CertitudeLien" value="' + certitudeLien + '" type="hidden">\
            <input name="DossierLabNonConnus[' + nonConnuOrder + '].NonConnuLab.LienPersonneMorales[' + i + '].Precisions" value="' + precisions + '" type="hidden">');

    }
}

function loadEntitesByDirection() {
    var directionId = $("#DefaultDirectionId").dxSelectBox("instance").option('value');
    $("#EntiteId").dxSelectBox("instance").option("dataSource", []);
    $("#EntiteId").dxSelectBox("instance").option("visible", false);
    $("#loader-entite-container").removeClass("dx-hidden");
    $("#loader-entite-container").html('<div class="d-flex"><div id="small-indicator-entite"></div><div class="pl-2"></div> </div>');

    $("#loader-type-client-container").removeClass("dx-hidden");
    $("#loader-type-client-container").html('<div class="d-flex"><div id="small-indicator-type-client"></div><div style="padding-top: 2px;font-size: 11px;" class="pl-2"></div> </div>');

    $("#small-indicator-entite").dxLoadIndicator({
        height: 16,
        width: 16
    });

    $("#EntiteId").dxSelectBox({
        dataSource: []
    });
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "json",
        url: '/' + culture + '/Lab/ServiceLab/GetEntitesByDirection?directionId=' + directionId,
    }).done(function (data) {
        if (data.status) {
            $("#EntiteId").dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        loadMode: "raw",
                        load: function () {
                            return data.entites;
                        }
                    })
                })
            });
        }

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify("error Loaod", "error");
    }).always(function (e) {
        enableButton("buttonSearch");
        $("#loader-entite-container").addClass("dx-hidden");
        $("#EntiteId").dxSelectBox("instance").option("visible", true);
    });
}
/*gridLienPersonneMoralePhysique*/
function updateListLienPersonneMoralePhysiquesForm(personnePhysiqueOrder) {
    $("#lien-personne-morale-physique-form-container" + personnePhysiqueOrder).html("");
    var data = $("#gridLienPersonneMoralePhysique" + personnePhysiqueOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var categorieLienPersonneMoralePhysiqueId = data[i].CategorieLienPersonneMoralePhysiqueId;
        var raisonSociale = data[i].RaisonSociale;
        var formeJuridiqueId = data[i].FormeJuridiqueId;
        var typeLienPersonneMoralePhysiqueId = data[i].TypeLienPersonneMoralePhysiqueId;
        var immatriculation = data[i].Immatriculation == undefined ? "" : data[i].Immatriculation;
        var precisions = data[i].Precisions == undefined ? "" : data[i].Precisions;
        var certitudeLien = data[i].CertitudeLien == undefined ? false : data[i].CertitudeLien;

        $("#lien-personne-morale-physique-form-container" + personnePhysiqueOrder).append('<input name="DossierLabPersonnes[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].RaisonSociale" value="' + raisonSociale + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].FormeJuridiqueId" value="' + formeJuridiqueId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].CategorieLienPersonneMoralePhysiqueId" value="' + categorieLienPersonneMoralePhysiqueId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].TypeLienPersonneMoralePhysiqueId" value="' + typeLienPersonneMoralePhysiqueId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].Immatriculation" value = "' + immatriculation + '" type = "hidden" >\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].CertitudeLien" value="' + certitudeLien + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.LienPersonneMorales[' + i + '].Precisions" value="' + precisions + '" type="hidden">');

    }
}


function updateListLienPersonneMoraleMoralesForm(personneMoraleOrder) {
    $("#lien-personne-morale-morale-form-container" + personneMoraleOrder).html("");
    var data = $("#gridLienPersonneMoraleMorale" + personneMoraleOrder).dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var raisonSociale = data[i].RaisonSociale;
        var formeJuridiqueId = data[i].FormeJuridiqueId;
        var typeLienPersonneMoraleMoraleId = data[i].TypeLienPersonneMoraleMoraleId;
        var immatriculation = data[i].Immatriculation == undefined ? "" : data[i].Immatriculation;
        var precisions = data[i].Precisions == undefined ? "" : data[i].Precisions;
        var certitudeLien = data[i].CertitudeLien == undefined ? false : data[i].CertitudeLien;
        var categorieLienPersonneMoraleMoraleId = data[i].CategorieLienPersonneMoraleMoraleId;

        $("#lien-personne-morale-morale-form-container" + personneMoraleOrder).append('\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].CryptedId" value = "' + cryptedId + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].RaisonSociale" value = "' + raisonSociale + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].FormeJuridiqueId" value="' + formeJuridiqueId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].CategorieLienPersonneMoraleMoraleId" value="' + categorieLienPersonneMoraleMoraleId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].TypeLienPersonneMoraleMoraleId" value="' + typeLienPersonneMoraleMoraleId + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].Immatriculation" value = "' + immatriculation + '" type = "hidden" >\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].CertitudeLien" value="' + certitudeLien + '" type="hidden">\
            <input name="DossierLabPersonneMorales[' + personneMoraleOrder + '].PersonneMoraleLab.LienPersonneMorales[' + i + '].Precisions" value="' + precisions + '" type="hidden">');

    }
}

function updateAutresNationalitesForm(personnePhysiqueOrder) {
    $("#autres-nationalites-container" + personnePhysiqueOrder).html("");
    var data = $("#gridAutresNationalites" + personnePhysiqueOrder).dxDataGrid("instance").getDataSource().items();
    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var paysId = data[i].PaysId;

        $("#autres-nationalites-container" + personnePhysiqueOrder).append('<input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.AutreNationalitePersonnePhysiqueLabs[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.AutreNationalitePersonnePhysiqueLabs[' + i + '].PaysId" value="' + paysId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.AutreNationalitePersonnePhysiqueLabs[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">');
    }
}

function getDateCreation(date) {
    var creationDate = date || new Date();
    creationDate = new Date(creationDate);
    //var dd = String(creationDate.getDate()).padStart(2, '0');
    //var mm = String(creationDate.getMonth() + 1).padStart(2, '0');
    //var yyyy = creationDate.getFullYear();
    //var formattedDate = "";
    //if (culture == "fr");
    //{
    //    formattedDate = dd + '/' + mm + '/' + yyyy;
    //}
    //if (culture == "en");
    //{
    //    formattedDate = yyyy + '-' + mm + '-' + dd;
    //}
    //return formattedDate;
    return creationDate;
}

function getUserFullName(userFullName) {
    return (userFullName != null) ? userFullName : $("#hidden-full-name").val();
}
function updateDossierLabActionForm() {
    $("#action-dossier-lab-form-container").html("");
    var data = $("#gridDossierLabAction").dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedCreateurId = (data[i].CryptedCreateurId != null) ? data[i].CryptedCreateurId : "";
        var cryptedModificateurId = (data[i].CryptedModificateurId != null) ? data[i].CryptedModificateurId : "";
        var cryptedDossierLabId = (data[i].CryptedDossierLabId != null) ? data[i].CryptedDossierLabId : "";
        var libelle = (data[i].Libelle != null) ? data[i].Libelle : "";
        var description = (data[i].Description != null) ? data[i].Description : "";
        var dateCreation = (data[i].DateCreation != null) ? formatDateGrid(data[i].DateCreation) : "";
        var dateModification = formatDateGrid(data[i].DateModification);
        $("#action-dossier-lab-form-container").append('<input name = "DossierLabActions[' + i + '].Libelle" value = "' + libelle + '" type = "hidden" >\
            <input name="DossierLabActions[' + i + '].Description" value = "' + description + '" type = "hidden">\
            <input name="DossierLabActions[' + i + '].CryptedId" value = "' + cryptedId + '" type = "hidden">\
            <input name="DossierLabActions[' + i + '].CryptedDossierLabId" value = "' + cryptedDossierLabId + '" type = "hidden">\
            <input name="DossierLabActions[' + i + '].DateCreation" value="' + dateCreation + '" type="hidden">\
            <input name="DossierLabActions[' + i + '].DateModification" value="' + dateModification + '" type="hidden">\
            <input name="DossierLabActions[' + i + '].CryptedCreateurId" value = "' + cryptedCreateurId + '" type = "hidden">\
            <input name="DossierLabActions[' + i + '].CryptedModificateurId" value = "' + cryptedModificateurId + '" type = "hidden">');
    }
}

function updateActiviteProfessionnellePersonnePhysiqueLabsForm(personnePhysiqueOrder) {
    $("#activites-professionnelles-personne-physique-lab" + personnePhysiqueOrder).html("");
    var data = $("#gridActiviteProfessionnellePersonnePhysiqueLabs" + personnePhysiqueOrder).dxDataGrid("instance").getDataSource().items();
    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var activiteProfessionnelle = data[i].ActiviteProfessionnelle;

        $("#activites-professionnelles-personne-physique-lab" + personnePhysiqueOrder).append('<input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.ActiviteProfessionnellePersonnePhysiqueLabs[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.ActiviteProfessionnellePersonnePhysiqueLabs[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">\
            <input name="DossierLabPersonnePhysiques[' + personnePhysiqueOrder + '].PersonnePhysiqueLab.ActiviteProfessionnellePersonnePhysiqueLabs[' + i + '].ActiviteProfessionnelle" value="' + activiteProfessionnelle + '" type="hidden">');
    }
}

function validateDossierLabAttachmentForm(index) {
    var $documentNameElem = $("#selected-document-name-container" + index);

    $documentNameElem.html('');

    var btnValidateAddAttachment = $("#btnValidateAddAttachment" + index).dxButton("instance");

    return isPieceJointeValid(index)
        .then(function () {
            displayAttachmentFileName(index);
            btnValidateAddAttachment.option("disabled", false);
        })
        .catch(function (message) {
            btnValidateAddAttachment.option("disabled", true);
            if (message) {
                $documentNameElem.html('<div class="alert alert-danger p-1" role="alert">' + message + '</div>');
            }
        });
}

function isPieceJointeValid(index) {
    var uploadedFiles = $("#FileUploader" + index).prop('files');

    var newFileUploaded = uploadedFiles.length > 0;

    var newAttachment = isNewAttachment(index);

    if (newAttachment && !newFileUploaded) // New attachment without uploading a new file
    {
        return Promise.reject();
    }

    if (newFileUploaded) // a new file was uploaded
    {
        var newFile = uploadedFiles[0];

        if (newFile.size > _labTailleMaxPieceJointeOctets) {
            return Promise.reject(_labTranslatableMessageMaxFileSizeExceededError);
        }

        var fileExtension = getFileExtension(newFile.name.toUpperCase());
        if (_labWhiteListExtensionValue && !_labWhiteListExtensionValue.includes(fileExtension)) {
            return Promise.reject(_whiteListExtensionError);
        }
    }

    var CategorieDocumentId = $("#CategorieDocumentId" + index).dxSelectBox("instance").option("value");
    if (CategorieDocumentId == _labEnvoiTracfinCategorieDocumentId) {
        return validatePieceJointeDossierLabEnvoiTracfin(newAttachment, index);
    }

    return Promise.resolve(); // Uploading a new file is not necessary for existing attachment
}

function isNewAttachment(index) {
    var latestUploadedFileName = $("#" + _labFormAttachmentsCollectionItemFieldIdPrefix + "_FileName" + index).val();

    return isUndefinedOrWhiteSpaces(latestUploadedFileName);
}

function validateAttachmentForm(index) {
    $("#selected-document-name-container" + index).html('');
    var btnValidateAddAttachment = $("#btnValidateAddAttachment" + index).dxButton("instance");
    var isValid = true;

    if ($("#FileUploader" + index).prop('files').length > 0) {
        var newFile = $("#FileUploader" + index).prop('files')[0];

        if (newFile.size > 10485760) {
            $("#selected-document-name-container" + index).html('<div class="alert alert-danger p-1" role="alert">' + _labTranslatableMessageMaxFileSizeExceededError + '</div>');
            isValid = false;
        }
        else {
            var CategorieDocumentId = $("#CategorieDocumentId" + index).dxSelectBox("instance").option("value");
            var error = {};
            if (CategorieDocumentId == _labEnvoiTracfinCategorieDocumentId &&
                !validatePiecesJointesEnvoiTracfin(newFile, error)) {
                $("#selected-document-name-container" + index).html('<div class="alert alert-danger p-1" role="alert">' + error.message + '</div>');
                isValid = false;

            }
            else if (CategorieDocumentId != _labEnvoiTracfinCategorieDocumentId &&
                _whiteListExtensionValue != null &&
                _whiteListExtensionValue != "" &&
                !_whiteListExtensionValue.includes(getFileExtension(newFile.name.toUpperCase()))) {
                $("#selected-document-name-container" + index).html('<div class="alert alert-danger p-1" role="alert">' + _whiteListExtensionError + '</div>');
                isValid = false;
            }
            else {
                $('#selected-document-name-container' + index).html(htmlEncode(newFile.name));
                $("#partialAttachment_fileSize" + index).dxTextBox("instance").option("value", newFile.size);
            }
        }
    }
    else {
        isValid = false;
    }
    btnValidateAddAttachment.option("disabled", !isValid);
}
function generateFileUploader(id) {
    $(function () {
        $("#FileUploader" + id).dxFileUploader({
            labelText: _labTranslatableMessageOrDrogFile,
            selectButtonText: _labTranslatableMessageSelectFile,
            multiple: false,
            showFileList: false,
            allowedFileExtensions: [".pdf"],
            maxFileSize: 2000000,
            name: "Attachments[" + id + "].File",
            onValueChanged: function (e) {
                if (e.value[0].size <= 2000000) {
                    $("#FileUploader" + id).dxFileUploader({
                        labelText: e.value[0].name
                    });
                    validateAttachmentForm(id);
                }
            }
        });
    });
}
function deleteNewAttachment(e) {

    var suffix = e.row.rowIndex;
    var confirmationDeleteFile = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatableMessageDeleteAttachmentConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });

    confirmationDeleteFile.show().done(function (dialogResult) {
        if (dialogResult) {
            $('#IsDeletedNew' + suffix).val('true');
            $("#inputFile" + suffix).remove();
            disableButton("btnDeleteNewAttachment" + suffix);
            $("#trNewAttachment" + suffix).animate({ backgroundColor: jQuery.Color("#fceca8") }, 2000, null, function () {
                $("#trNewAttachment" + suffix).children('td')
                    .animate({ 'padding-top': 0, 'padding-bottom': 0 })
                    .wrapInner('<div />')
                    .children()
                    .slideUp(function () {
                        $(this).closest('tr').hide();
                    });
            });
        }
    });
}

function deleteExistAttachment(index) {
    var confirmationDeleteFile = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatableMessageDeleteAttachmentConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });

    confirmationDeleteFile.show().done(function (dialogResult) {
        if (dialogResult) {
            $('#IsDeleted' + index).val('true');
            disableButton("btnDeleteAttachment" + index);
            disableButton("btnDownloadFile" + index);
            $("#trAttachment" + index).animate({ backgroundColor: jQuery.Color("#fceca8") }, 2000, null, function () {
                $("#trAttachment" + index).children('td')
                    .animate({ 'padding-top': 0, 'padding-bottom': 0 })
                    .wrapInner('<div />')
                    .children()
                    .slideUp(function () {
                        $(this).closest('tr').hide();
                    });
            });
        }
    });
}

function showFormAttachments(estTransmissionArNew) {
    showLoadPanel(_commonTranslatableMessageLoading);
    var order = $('.attachmentRow').length;
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        url: '/' + culture + '/Lab/ServiceLab/AddAttachmentForm',
        data: { order: order, estTransmissionArNew: estTransmissionArNew }
    }).done(function (data) {
        var popup = $("#modalAttachments").dxPopup('instance');
        popup.registerKeyHandler("escape", function (arg) {
            $("#attachment" + order).remove();
            $("#modalAttachments").dxPopup("hide");
        });
        popup.show();

        $("#attachments-container").append(data);
        var validationGroup = getAttachmentsPopupFormValidationGroup(order);
        DevExpress.validationEngine.addGroup(validationGroup);

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function openDialogFileXMLDeclarationTracfin() {
    $("#XMLDeclarationTracfin").trigger('click');
}

function openDialogFileXMLDeclarationTracfinNew() {
    $("#XMLDeclarationTracfinNew").trigger('click');
}

function importDeclarationTracfinXml() {
    var form = document.forms[0];
    form.method = 'post';
    form.action = window.location.href;
    form.submit();
    DevExpress.ui.notify(_labTranslatableMessageLoadingCases, "success");
    // this.post(window.location.href, document.forms[0]);
}

function importDeclarationTracfinNewXml() {
    var form = document.forms[0];
    var isNewDeclarationTracfin = true;
    form.method = 'post';
    form.action = window.location.href + '?isNewDeclarationTracfin=' + isNewDeclarationTracfin;
    form.submit();
    DevExpress.ui.notify(_labTranslatableMessageLoadingCases, "success");
    // this.post(window.location.href, document.forms[0]);
}

function computePersonnePhysiqueDS() {
    var countPhysicalPerson = $('.physicalPerson').length;
    var physicalSoupsonCounter = 0;

    // Compter les personnes physiques avec déclaration Tracfin
    for (let i = 0; i < countPhysicalPerson; i++) {
        var checkBoxInstance = $("#PersonnePhysiqueLabIsDeclarationTracfin" + i).dxCheckBox('instance');

        if (checkBoxInstance && checkBoxInstance.option('value')) {
            physicalSoupsonCounter++;
        }
    }

    // Mettre à jour le nombre de personnes physiques
    var textBoxInstance = $("#NombrePersonnePhysiques0").dxTextBox("instance");
    if (textBoxInstance != null) {
        textBoxInstance.option("value", physicalSoupsonCounter);
    }
}
function computePersonneMoraleDS() {
    var countMoralPerson = $('.moralPerson').length;
    var moralSoupsonCounter = 0;

    // Compter les personnes morales avec déclaration Tracfin
    for (let i = 0; i < countMoralPerson; i++) {
        var checkBoxInstance = $("#DossierLabPersonneMoralesIsDeclarationTracfin" + i).dxCheckBox('instance');

        if (checkBoxInstance && checkBoxInstance.option('value')) {
            moralSoupsonCounter++;
        }
    }

    // Mettre à jour le nombre de personnes morales
    var textBoxInstance = $("#NombrePersonneMorales0").dxTextBox("instance");
    if (textBoxInstance != null) {
        textBoxInstance.option("value", moralSoupsonCounter);
    }
}

function initbuttonAddManager() {
    $("#divAddSelected").css("display", _iswritelab ? "block" : "none");
}

/*****************************************************/
function showValidationRefusDossierLabModalPreview(contentHtml) {
    var popup = $("#ValidationRefusDossierLabModalPreview").dxPopup('instance');
    popup.option('contentTemplate', function (contentTemplate) {
        contentTemplate.append(contentHtml);
    });
    popup.show();
}

function CancelValidationRefusDossierLabModalPreview() {
    if ($("#ValidationRefusDossierLabModalPreview")) {
        $("#ValidationRefusDossierLabModalPreview").dxPopup('instance').content().empty();
        $("#ValidationRefusDossierLabModalPreview").dxPopup('instance').hide();

    }
}

function validationRefusSelected_OnClick() {
    var dataGrid = null;
    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }

    var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;
    $.ajax({
        method: "GET",
        data: { cryptedId: cryptedId },
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/ValidationRefusDossierLab',
    }).done(function (data) {
        showValidationRefusDossierLabModalPreview(data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });
}

function btnConfirmValidationRefus_click() {

    var dataGrid = null;

    var statutId = 0;
    var motifValid = null;

    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }

    var valid = true;

    if ($("#StatutValidRefusId").length > 0 && $("#StatutValidRefusId").dxSelectBox('instance'))
        statutId = $("#StatutValidRefusId").dxSelectBox('instance').option('value');

    if ($("#MotifValidationId").length > 0 && $("#MotifValidationId").dxTextBox('instance'))
        motifValid = $("#MotifValidationId").dxTextBox('instance').option('value');


    if (statutId == 4 && (motifValid == null || (motifValid != null && motifValid.trim() === ''))) {
        valid = false;
    }
    if (valid) {
        $.ajax({
            method: "POST",
            data: $("#saveValidationRefusForm").serialize(),
            dataType: "json",
            url: '/' + culture + '/Lab/ServiceLab/ConfirmValidationRefusDossierLab',
        }).done(function (data) {

            if (data.success) {
                if (statutId == 2) {
                    location.reload();
                }
                else {
                    var keys = dataGrid.getSelectedRowKeys();
                    $.each(keys, function (value) {
                        this.Statut = data.statusDossier;
                        this.Modificateur = data.modificateur;
                        this.DateModification = data.dateModification;
                        this.Datecloture = data.dateCloture;
                        this.Id = data.id;
                        dataGrid.refresh();
                    });
                    $("#divValidationRefusSelected").css("display", "none");
                }
                DevExpress.ui.notify(_commonTranslatableMessageValidationRefusLab, "success");
            }
            else {
                DevExpress.ui.notify(data.errorMessage, "error");
            }
            CancelValidationRefusDossierLabModalPreview();

        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageValidationRefusLabError, "error");
            CancelValidationRefusDossierLabModalPreview();
        }).always(function (e) {
            hideLoadPanel();
        });

    } else {
        DevExpress.ui.notify(_labTranslatableMessageMotifValidRefusLabObligatoire, "error");
    }

}

function btnCancelValidRefus_click() {
    CancelValidationRefusDossierLabModalPreview();
}

function gridRemoveDossierLab(dossierId) {
    var DossierDataGrid = null;

    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        DossierDataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        DossierDataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }
    var datasourceDossierArray = [];
    var datasource = DossierDataGrid.option('dataSource');
    if (datasource._store != undefined && datasource._store.__rawData != undefined) {
        datasourceDossierArray = datasource._store.__rawData;
    }
    if (datasourceDossierArray.length == 0 && datasource.length > 0) {
        datasourceDossierArray = datasource;
    }
    var DossierRowDataResult = [];

    datasourceDossierArray.filter(function (ent) {
        if (ent != undefined && ent.CryptedId != dossierId) {
            DossierRowDataResult.push(ent);
        };
    });

    DossierDataGrid.option('dataSource', DossierRowDataResult);
    DossierDataGrid.refresh();
}


function direction_criteria_required_lab() {

    var isvalid = true;

    var mode = isManagementMode === true ? 2 : 1;

    var directionValue = $("#DirectionId").dxSelectBox("instance").option('value');

    var codeUniqueValue = $.trim($("#CodeUnique").dxTextBox('instance').option('value'));

    var nomRaisonSociale = $.trim($("#NomRaisonSociale").dxTextBox('instance').option('value'));

    var numeroImmatriculationValue = $.trim($("#personneNumeroImmatriculation").dxTextBox('instance').option('value'));

    var numerocompteValue = $.trim($("#personneNumeroCompte").dxTextBox('instance').option('value'));



    if (mode != 2 && (directionValue == undefined || directionValue == null)) {

        if ((codeUniqueValue.length >= 8)
            || (nomRaisonSociale.length > 1)
            || (numeroImmatriculationValue.length > 1)
            || (numerocompteValue.length > 1)
            || (nomRaisonSociale.length > 1)
            || (isFlge == "True")
        ) {
            isvalid = true;
        } else {
            isvalid = false;
        }


    }
    return isvalid;
}


function initParamRechercheTiers() {

    var isTiers = false;
    isSearchTiers = false;

    var numerocompte = '';
    var nomRaisonSociale = '';
    var prenom = '';
    var numeroImmatriculation = '';
    var dateNaissance = '';

    if ($("#personneNumeroCompte").dxTextBox('instance') && $("#personneNumeroCompte").length > 0) {
        numerocompte = $("#personneNumeroCompte").dxTextBox('instance').option('value');
    }


    if ($("#NomRaisonSociale").dxTextBox('instance') && $("#NomRaisonSociale").length > 0) {
        nomRaisonSociale = $("#NomRaisonSociale").dxTextBox('instance').option('value');
    }

    if ($("#Prenom").length > 0) {
        prenom = $("#Prenom").dxTextBox('instance').option('value');

    }


    if ($("#personneNumeroImmatriculation").dxTextBox('instance') && $("#personneNumeroImmatriculation").length > 0) {
        numeroImmatriculation = $("#personneNumeroImmatriculation").dxTextBox('instance').option('value');
    }

    if ($("#dateNaissance").dxDateBox('instance') && $("#dateNaissance").dxDateBox('instance').option('value'))
        dateNaissance = $.trim($("#dateNaissance").dxDateBox('instance').option('value'));

    if (numerocompte && numerocompte.trim()) { isTiers = true; isSearchTiers = true; }
    if (nomRaisonSociale && nomRaisonSociale.trim()) { isTiers = true; isSearchTiers = true; }
    if (prenom && prenom.trim()) { isTiers = true; isSearchTiers = true; }
    if (numeroImmatriculation && numeroImmatriculation.trim()) { isTiers = true; isSearchTiers = true; }
    if (dateNaissance && dateNaissance.trim()) { isTiers = true; isSearchTiers = true; }

    if ($("#divVwDossiersLabTiersId").length > 0 || $("#divVwDossiersLabNoTiersId").length > 0) {
        if (isTiers) {
            $("#divVwDossiersLabTiersId").css("display", "block");
            $("#divVwDossiersLabNoTiersId").css("display", "none");
        } else {
            $("#divVwDossiersLabTiersId").css("display", "none");
            $("#divVwDossiersLabNoTiersId").css("display", "block");
        }
    }

    if ($("#divMngDossiersLabTiersId").length > 0 || $("#divMngDossiersLabNoTiersId").length > 0) {
        if (isTiers) {
            $("#divMngDossiersLabTiersId").css("display", "block");

            $("#divMngDossiersLabNoTiersId").css("display", "none");
        } else {
            $("#divMngDossiersLabTiersId").css("display", "none");

            $("#divMngDossiersLabNoTiersId").css("display", "block");
        }
    }

}

function gridDossiersLabWithTiers_OnRowPrepared(e) {
    if (e.rowType !== "data")
        return;

    if (e.data.IsDeclarationSoupcon == true) {
        e.rowElement.css("background-color", "#a0ed9f");
    }
    if (e.data.Confidentiel == true) {
        e.rowElement.css("background-color", "#e0fffa");
    }

    if (e.data.IsSuivi == true) {
        e.rowElement.css("background-color", "#FF5E4D");
    }
    if (e.data.IsNoResponse == true) {
        e.rowElement.css("background-color", "#ffffe0");
    }

    if (e.data.IsWaitingRead == true) {
        e.rowElement.css("background-color", "#fba120");
    }
    if (e.data.IsDgt == true) {
        e.rowElement.css("background-color", "#a0ed9f");
    }

}


function gridDossiersLabWithNoTiers_OnRowPrepared(e) {

    if (e.rowType !== "data")
        return;

    if (e.data.IsDeclarationSoupcon == true) {
        e.rowElement.css("background-color", "#a0ed9f");
    }
    if (e.data.Confidentiel == true) {
        e.rowElement.css("background-color", "#e0fffa");
    }

    if (e.data.IsSuivi == true) {
        e.rowElement.css("background-color", "#FF5E4D");
    }
    if (e.data.IsNoResponse == true) {
        e.rowElement.css("background-color", "#ffffe0");
    }

    if (e.data.IsWaitingRead == true) {
        e.rowElement.css("background-color", "#fba120");
    }
    if (e.data.IsDgt == true) {
        e.rowElement.css("background-color", "#a0ed9f");
    }
}

function getInformationRequestStatusLabel(data) {
    if (data.IsWaitingRead) {
        return _informationRequestStatusLabelResponded;
    }
    if (data.IsNoResponse) {
        return _informationRequestStatusLabelCreated;
    }
    return "";
}
function DossierLab_direction_OnValueChanged_OrganismeLabView() {
    $("#createOrUpdateDossierLab_CodeProfession").val("");

    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');

    if (directionId == null || !directionId) {
        filterAllDataSourcesTypesReferencesFinanciere();
        return;
    }

    var isDeclarationTracfin = $("#IsDeclarationSoupcon").dxCheckBox("instance").option("value");

    if (isDeclarationTracfin) {
        if ($("#Organisme_Libelle").dxTextBox('instance')) $("#Organisme_Libelle").dxTextBox('instance').option('value', '');
        if ($("#Organisme_TypeVoieId").dxTextBox('instance')) $("#Organisme_TypeVoieId").dxTextBox('instance').option('value', '');
        if ($("#Organisme_ComplementVoieId").dxTextBox('instance')) $("#Organisme_ComplementVoieId").dxTextBox('instance').option('value', '');
        if ($("#Organisme_PaysId").dxTextBox('instance')) $("#Organisme_PaysId").dxTextBox('instance').option('value', '');
        if ($("#OrganismeId")) $("#OrganismeId").val('');
        if ($("#Organisme_NumeroVoie").dxTextBox('instance')) $("#Organisme_NumeroVoie").dxTextBox('instance').option('value', '');
        if ($("#Organisme_Complement").dxTextBox('instance')) $("#Organisme_Complement").dxTextBox('instance').option('value', '');
        if ($("#Organisme_Voie").dxTextBox('instance')) $("#Organisme_Voie").dxTextBox('instance').option('value', '');
        if ($("#Organisme_Ville").dxTextBox('instance')) $("#Organisme_Ville").dxTextBox('instance').option('value', '');
        if ($("#Organisme_CodePostale").dxTextBox('instance')) $("#Organisme_CodePostale").dxTextBox('instance').option('value', '');
        if ($("#Organisme_Telephone").dxTextBox('instance')) $("#Organisme_Telephone").dxTextBox('instance').option('value', '');
        if ($("#Organisme_Fax").dxTextBox('instance')) $("#Organisme_Fax").dxTextBox('instance').option('value', '');
        if ($("#Organisme_ProfessionId").dxTextBox('instance')) $("#Organisme_ProfessionId").dxTextBox('instance').option('value', '');
        if ($("#Organisme_NumeroIdentifiantProfessionnel").dxTextBox('instance')) $("#Organisme_NumeroIdentifiantProfessionnel").dxTextBox('instance').option('value', '');
    }

    $.ajax({
        method: "GET", cache: false,
        dataType: 'json',
        data: { directionId: directionId },
        url: '/' + culture + '/Configuration/AdministrationService/GetOrganismeLabByDirectionInclude',
    }).done(function (data) {
        if (!data) return;

        if (data.Profession) {
            $("#createOrUpdateDossierLab_CodeProfession").val(data.Profession.Code);
            filterAllDataSourcesTypesReferencesFinanciere();
        }

        if (!isDeclarationTracfin) return;

        $("#OrganismeId").val(data.Id);
        if ($("#Organisme_Libelle").dxTextBox('instance')) $("#Organisme_Libelle").dxTextBox('instance').option('value', data.Libelle);
        if ($("#Organisme_NumeroVoie").dxTextBox('instance')) $("#Organisme_NumeroVoie").dxTextBox('instance').option('value', data.NumeroVoie);
        if (culture == "fr") {
            if ($("#Organisme_TypeVoieId").dxTextBox('instance')) if (data.TypeVoie != undefined) $("#Organisme_TypeVoieId").dxTextBox('instance').option('value', data.TypeVoie.NameFr);
            if ($("#Organisme_ComplementVoieId").dxTextBox('instance')) if (data.ComplementVoie != undefined) $("#Organisme_ComplementVoieId").dxTextBox('instance').option('value', data.ComplementVoie.NameFr);
            if ($("#Organisme_PaysId").dxTextBox('instance')) if (data.Pays != undefined) $("#Organisme_PaysId").dxTextBox('instance').option('value', data.Pays.NameFr);
            if ($("#Organisme_ProfessionId").dxTextBox('instance')) if (data.Profession != undefined) $("#Organisme_ProfessionId").dxTextBox('instance').option('value', data.Profession.NameFr);
        }
        else if (culture == "en") {
            if ($("#Organisme_TypeVoieId").dxTextBox('instance')) if (data.TypeVoie != undefined) $("#Organisme_TypeVoieId").dxTextBox('instance').option('value', data.TypeVoie.NameEn);
            if ($("#Organisme_ComplementVoieId").dxTextBox('instance')) if (data.ComplementVoie != undefined) $("#Organisme_ComplementVoieId").dxTextBox('instance').option('value', data.ComplementVoie.NameEn);
            if ($("#Organisme_PaysId").dxTextBox('instance')) if (data.Pays != undefined) $("#Organisme_PaysId").dxTextBox('instance').option('value', data.Pays.NameEn);
            if ($("#Organisme_ProfessionId").dxTextBox('instance')) if (data.Profession != undefined) $("#Organisme_ProfessionId").dxTextBox('instance').option('value', data.Profession.NameEn);
        }
        if ($("#Organisme_Complement").dxTextBox('instance')) $("#Organisme_Complement").dxTextBox('instance').option('value', data.Complement);
        if ($("#Organisme_Voie").dxTextBox('instance')) $("#Organisme_Voie").dxTextBox('instance').option('value', data.Voie);
        if ($("#Organisme_Ville").dxTextBox('instance')) $("#Organisme_Ville").dxTextBox('instance').option('value', data.Ville);
        if ($("#Organisme_CodePostale").dxTextBox('instance')) $("#Organisme_CodePostale").dxTextBox('instance').option('value', data.CodePostal);
        if ($("#Organisme_Telephone").dxTextBox('instance')) $("#Organisme_Telephone").dxTextBox('instance').option('value', data.TelephoneFixe);
        if ($("#Organisme_Fax").dxTextBox('instance')) $("#Organisme_Fax").dxTextBox('instance').option('value', data.Fax);
        if ($("#Organisme_NumeroIdentifiantProfessionnel").dxTextBox('instance')) $("#Organisme_NumeroIdentifiantProfessionnel").dxTextBox('instance').option('value', data.NumeroIdentifiantProfessionnel);

    }).fail(function (e) {
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}
function filterAllDataSourcesTypesReferencesFinanciere() {
    $("div[id^='gridSupportFinancier'").each(function () {
        var gridInstance = $(this).dxDataGrid("instance");

        if (!gridInstance) return;

        filterDataSourceTypesReferences(gridInstance);
    });

}


function _panelPersonneOnContentReady() {
    console.log('_panelPersonneOnContentReady');
    $('#panelPersonnes').dxTabPanel('instance').option('selectedIndex', 0);
}

function statutId_OnSelectionChanged() {

    var statutId = $("#StatutValidRefusId").dxSelectBox("instance").option("value");

    if (statutId && statutId == 4) {
        $("#motifValidationDivLbl").css("display", "block");
        $("#motifValidationDivTbx").css("display", "block");

    } else {
        $("#motifValidationDivLbl").css("display", "none");
        $("#motifValidationDivTbx").css("display", "none");
    }
}

function numeroPrecedenteDeclarationCheckBox_OnValueChanged(index) {
    verifCheckedCategorieGroupeDeclaration(index);
    var result = $("#NumeroPrecedenteDeclarationCheckBox" + index).dxCheckBox("instance").option("value");
    $("#IsDsComplementaire" + index).dxCheckBox("instance").option("value", result);
    if (result) {
        $("#numero-precedente-declaration-container").removeClass("dx-hidden");
    }
    else {
        $("#numero-precedente-declaration-container").addClass("dx-hidden");
        $("#NumeroPrecedenteDeclaration" + index).dxTextBox("instance").option("value", null);
    }
}

function isDsComplementaire_OnValueChanged(index) {

    verifCheckedCategorieGroupeDeclaration(index);
    var result = $("#IsDsComplementaire" + index).dxCheckBox("instance").option("value");
    $("#NumeroPrecedenteDeclarationCheckBox" + index).dxCheckBox("instance").option("value", result);
}

function btnSearchReportings_onClick(anneeEncours, anneePrecedente) {
    var isAnneeEnCours = anneeEncours == 'True';
    var isAnneePrecedente = anneePrecedente == 'True';
    let today = new Date();
    // showLoadPanel(_dossierTranslatableMessageLoadingDossiers);
    $("#gridDossiersLabQLBReporting").dxDataGrid({ dataSource: [] });
    $.ajax({
        method: "POST",
        data: $("#formSearchReportingDossierLabQLB").serialize(),
        dataType: "json",
        url: '/' + culture + '/lab/ServiceLab/SearchReportings/?isAnneeEnCours=' + isAnneeEnCours + ' &isAnneePrecedente=' + isAnneePrecedente,
    }).done(function (data) {
        if (isAnneeEnCours) {           
            $("#startDateReporting").dxDateBox({ value: new Date(today.getFullYear(),0,1)});
            $("#endDateReporting").dxDateBox({ value: new Date(today)});
        }
        if (isAnneePrecedente) {
            $("#startDateReporting").dxDateBox({ value: new Date(today.getFullYear() - 1, 0, 1) });
            $("#endDateReporting").dxDateBox({ value: new Date(today.getFullYear()- 1, 11,31) });
        }
        var pivotGridDossiersLabQLBReporting = $("#pivotGridDossiersLabQLBReporting").dxPivotGrid("instance");
        if (pivotGridDossiersLabQLBReporting) {

            var pivotGridDataSource = new DevExpress.data.PivotGridDataSource({
                store: data.result,
                fields: [
                    {
                        area: "row",
                        dataField: "Code",
                        areaIndex: 0,
                        expanded: true,
                        width: 130
                    },
                    {
                        area: "row",
                        dataField: "Caption",
                        areaIndex: 1,
                        width: 350
                    },
                    {
                        area: "column",
                        caption: "Année",
                        dataField: "Year",
                        expanded: true,
                    },
                    {
                        area: "column",
                        caption: "Quarter",
                        dataField: "Quarter",
                        expanded: true,
                    },
                    {
                        area: "data",
                        caption: "Value",
                        dataField: "Value",
                        summaryType: 'sum',
                        format: '#0.##',
                    }],
                fieldChooser: {
                    height: 500,
                },
                showBorders: true,
                height: 440,

            });

            var selectedDepartment = "";
            if ($("#DefaultDirectionId") && $("#DefaultDirectionId").dxSelectBox("instance"))
                selectedDepartment = $("#DefaultDirectionId").dxSelectBox("instance").option('selectedItem');

            var departmentName = "";
            if (selectedDepartment)
                departmentName = culture == "fr" ? selectedDepartment.NameFr : selectedDepartment.NameEn;

            var startDateYear = "";
            if ($("#startDateReporting") && $("#startDateReporting").dxDateBox("instance"))
                startDateYear = $("#startDateReporting").dxDateBox("instance").option('value').getFullYear().toString();

            var exportFileName = "QLB " + startDateYear + " " + departmentName;

            $("#pivotGridDossiersLabQLBReporting").dxPivotGrid({
                allowSortingBySummary: true,
                allowSorting: true,
                allowFiltering: true,
                allowExpandAll: true,
                showColumnTotals: false,
                showRowTotals: false,
                showRowGrandTotals: false,
                showTotalsPrior: 'rows',
                showBorders: true,
                "export": {
                    enabled: true,
                    fileName: exportFileName
                },
                fieldChooser: {
                    enabled: true,
                },
                dataSource: pivotGridDataSource
            });
        }
        else {
            var gridLienPersonnePhysique = $("#gridLienPersonnePhysique").dxDataGrid("instance");
            var gridLienPersonneMorale = $("#gridLienPersonneMorale").dxDataGrid("instance");

        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify('error', "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function isDsRuptureRelation_OnValueChanged(index) {

    var result = $("#IsDsRuptureRelation" + index).dxCheckBox("instance").option("value");
    verifCheckedCategorieGroupeDeclaration(index);
    if (result) {
        $("#date-rupture-relation-container").removeClass("dx-hidden");
    }
    else {
        $("#date-rupture-relation-container").addClass("dx-hidden");
        $("#DateRuptureRelation" + index).dxDateBox("instance").option("value", null);
    }
}
function directionView_Initialize() {
    if ($("#DefaultDirectionId")) {
        var insDirection = $("#DefaultDirectionId").dxSelectBox("instance");
        if (insDirection != undefined) {
            var data = insDirection.option("dataSource");
            if (data != undefined) {
                insDirection.option("value", data.store._array[0].Id);
                if (data.store._array.length == 1) {
                    insDirection.option("readOnly", true);
                } else {
                    insDirection.option("readOnly", false);
                }
            }
        }
    }
}

function showTransmissionDossierLabModalPreview(contentHtml) {
    var popup = $("#TransmissionDossierLabModalPreview").dxPopup('instance');
    popup.option("contentTemplate", function () {
        return $("<div id='transmissionDossierLabModalContentScrollView'>").append(contentHtml);
    });
    popup.option('onShown', function () {
        $("#transmissionDossierLabModalContentScrollView").dxScrollView({
            scrollByContent: true,
            scrollByThumb: true,
            showScrollbar: "always",
        });
    });

    popup.show();
}

function showTransmissionDossierLabModalPreviewNew(contentHtml) {
    var popup = $("#TransmissionDossierLabModalPreview").dxPopup('instance');
    popup.option("contentTemplate", function () {
        return $("<div id='transmissionDossierLabModalContentScrollView'>").append(contentHtml);
    });
    popup.option('onShown', function () {
        $("#transmissionDossierLabModalContentScrollView").dxScrollView({
            scrollByContent: true,
            scrollByThumb: true,
            showScrollbar: "always",
        });
    });

    popup.show();
}

function transmissionDossierLabModalPreviewNew_OnShowing() {
    $("#TransmissionDossierLabModalPreview").dxPopup({
        width: "95%"
    });
}

function CancelTransmissionDossierLabModalPreview() {
    if ($("#TransmissionDossierLabModalPreview")) {
        $("#TransmissionDossierLabModalPreview").dxPopup('instance').content().empty();
        $("#TransmissionDossierLabModalPreview").dxPopup('instance').hide();

    }
}

function gridContacterSelected_Lab_OnClick() {

    var rowSelected = getSelectedRowDossier();
    if (rowSelected) {

        var cryptedId = rowSelected.CryptedId;
        $.ajax({
            method: "GET",
            cache: false,
            data: { cryptedId: cryptedId },
            url: '/' + culture + '/Lab/ServiceLab/ShowContacterPopupDossierLab',
        }).done(function (data) {
            if (data.status == false) {
                DevExpress.ui.notify(data.message, "error");
            }
            else {
                showContacterDossierLabModalPreview(data);
            }
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }

        }).always(function (e) {
        });

    }
}

function envoieMessageDossierLab_OnClick() {
    var validationResult = DevExpress.validationEngine.validateGroup("contactResponsableLabFormValidationGroup");
    if (!validationResult.isValid) {
        return false;
    }
    var form = $("#ContactResponsableLabForm").closest("form");
    var formData = new FormData(form[0]);
    if (formData != null) {
        $.ajax({
            method: "POST",
            data: formData,
            dataType: "json",
            url: '/' + culture + '/Lab/ServiceLab/EnvoieMessageDossierLab',
            processData: false,
            contentType: false
        }).done(function (data) {
            if (data.status) {
                DevExpress.ui.notify(data.message, "success");
                CancelContacterDossierLabModalPreview();
            } else {
                DevExpress.ui.notify(data.message, "error");
            }
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify('error', "error");
        }).always(function (e) {
            hideLoadPanel();
        });
    }

}

function showContacterDossierLabModalPreview(contentHtml) {
    var popup = $("#ContacterDossierLabModalPreview").dxPopup('instance');
    popup.option("contentTemplate", function () {
        return $("<div id='contacterDossierLabModalContentScrollView'>").append(contentHtml);
    });
    popup.option('onShown', function () {
        $("#contacterDossierLabModalContentScrollView").dxScrollView({
            scrollByContent: true,
            scrollByThumb: true,
            showScrollbar: "always",
        });
    });

    popup.show();
}

function CancelContacterDossierLabModalPreview() {
    if ($("#ContacterDossierLabModalPreview")) {
        $("#ContacterDossierLabModalPreview").dxPopup('instance').content().empty();
        $("#ContacterDossierLabModalPreview").dxPopup('instance').hide();

    }
}

function gridTransmissionUpdateARSelected_Lab_OnClick() {
    var dataGrid = null;
    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }

    var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;
    $.ajax({
        method: "GET",
        data: { cryptedId: cryptedId },
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/TransmissionDossierLab',
    }).done(function (data) {
        showTransmissionDossierLabModalPreview(data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });
}

function gridTransmissionUpdateNewARSelected_Lab_OnClick() {
    var dataGrid = null;
    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }

    var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;
    $.ajax({
        method: "GET",
        data: { cryptedId: cryptedId },
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/TransmissionDossierLabNew',
    }).done(function (data) {
        showTransmissionDossierLabModalPreviewNew(data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });
}


function gridAddFile_Lab_OnClick() {
    var dataGrid = null;
    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }

    var cryptedId = dataGrid.getSelectedRowsData()[0].CryptedId;
    $.ajax({
        method: "GET",
        data: { cryptedId: cryptedId },
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/TransmissionDossierLab',
    }).done(function (data) {
        showTransmissionDossierLabModalPreview(data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });
}


function btnConfirmTransmission_click() {

    var dataGrid = null;

    var isTransmission = false;
    var numeroAccuseReception = null;

    if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }

    if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }
    var clotureTracfin = "Closed TRACFIN";
    if (culture == "fr") {
        clotureTracfin = "Cloturé TRACFIN";
    }
    var form = $("#saveTransmissionForm").closest("form");
    var formData = new FormData(form[0]);

    $.ajax({
        method: "POST",
        data: formData,
        dataType: "json",
        url: '/' + culture + '/Lab/ServiceLab/ConfirmTransmissionDossierLab',
        processData: false,
        contentType: false
    }).done(function (data) {

        if (data.success) {
            var keys = dataGrid.getSelectedRowKeys();
            $.each(keys, function (value) {
                this.NumeroAccuseReception = data.numeroAccuseReception;
                this.IsTransmissionParquet = data.isTransmissionParquet;
                this.IsTransmissionAutorite = data.isTransmissionAutorite;
                this.IsSuivi = data.isSuivi;
                this.Modificateur = data.modificateur;
                this.DateModification = data.dateModification;
                this.DateDeclaration = data.dateDeclaration;
                this.DateReception = data.dateDetection;
                if (data.statutDossier == 9) {
                    this.Statut = clotureTracfin;
                }

                dataGrid.refresh();
            });
            $("#divValidationRefusSelected").css("display", "none");
            DevExpress.ui.notify(_labTranslatableMessageSavingCase, "success");
        }
        else {
            DevExpress.ui.notify(data.errorMessage, "error");
        }
        CancelTransmissionDossierLabModalPreview();

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageValidationRefusLabError, "error");
        CancelValidationRefusDossierLabModalPreview();
    }).always(function (e) {
        hideLoadPanel();
    });
}
function updateListPersonnePhysiquesForm() {
    $("#personne-physique-relation-form-container").html("");
    var data = $("#gridPpRelation").dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonnePhysiqueLabId = (data[i].CryptedPersonnePhysiqueLabId != null) ? data[i].CryptedPersonnePhysiqueLabId : "";
        var id = data[i].Id;
        var nomNaissance = data[i].NomNaissance;
        var prenoms = data[i].Prenoms;
        var dateNaissance = formatDateGrid(data[i].DateNaissance);
        var typeRelationAffaireLabId = data[i].TypeRelationAffaireLabId;
        var dateCessationRelations = formatDateGrid(data[i].DateCessationRelations);

        $("#personne-physique-relation-form-container").append('<input name="DossierLabPersonnePhysiques[' + 0 + '].PersonnePhysiqueLab.CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="PersonnePhysiqueLabs[' + i + '].CryptedPersonnePhysiqueLabId" value="' + cryptedPersonnePhysiqueLabId + '" type="hidden">\
            <input name="PersonnePhysiqueLabs[' + i + '].Id" value="' + id + '" type="hidden">\
            <input name="PersonnePhysiqueLabs[' + i + '].NomNaissance" value="' + nomNaissance + '" type="hidden">\
            <input name="PersonnePhysiqueLabs[' + i + '].Prenoms" value="' + prenoms + '" type="hidden">\
            <input name="PersonnePhysiqueLabs[' + i + '].TypeRelationAffaireLabId" value="' + typeRelationAffaireLabId + '" type="hidden">\
            <input name="PersonnePhysiqueLabs[' + i + '].DateCessationRelations" value="' + dateCessationRelations + '" type="hidden">\
            <input name="PersonnePhysiqueLabs[' + i + '].DateNaissance" value="' + dateNaissance + '" type="hidden">')
    }
}

function updateListPersonneMoralesForm() {
    $("#personne-morale-relation-form-container").html("");
    var data = $("#gridPersonneMorale").dxDataGrid("instance").getDataSource().items();

    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var cryptedPersonneMoraleLabId = (data[i].CryptedPersonneMoraleLabId != null) ? data[i].CryptedPersonneMoraleLabId : "";
        var id = data[i].Id;
        var raisonSociale = data[i].RaisonSociale;
        var numeroImmatriculation = data[i].NumeroImmatriculation;
        var typeRelationAffaireLabId = data[i].TypeRelationAffaireLabId;
        var dateCessationRelations = formatDateGrid(data[i].DateCessationRelations);

        $("#personne-morale-relation-form-container").append('<input name="DossierLabPersonneMorales[' + 0 + '].PersonneMoraleLab.CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="PersonneMoraleLabs[' + i + '].CryptedPersonneMoraleLabId" value="' + cryptedPersonneMoraleLabId + '" type="hidden">\
            <input name="PersonneMoraleLabs[' + i + '].Id" value="' + id + '" type="hidden">\
            <input name="PersonneMoraleLabs[' + i + '].RaisonSociale" value="' + raisonSociale + '" type="hidden">\
            <input name="PersonneMoraleLabs[' + i + '].NumeroImmatriculation" value="' + numeroImmatriculation + '" type="hidden">\
            <input name="PersonneMoraleLabs[' + i + '].TypeRelationAffaireLabId" value="' + typeRelationAffaireLabId + '" type="hidden">\
            <input name="PersonneMoraleLabs[' + i + '].DateCessationRelations" value = "' + dateCessationRelations + '" type = "hidden" >')
    }
}

function btnCancelTransmission_click() {
    CancelTransmissionDossierLabModalPreview();
}

function getCookie(name) {
    function escape(s) { return s.replace(/([.*+?\^$(){}|\[\]\/\\])/g, '\\$1'); }
    var match = document.cookie.match(RegExp('(?:^|;\\s*)' + escape(name) + '=([^;]*)'));
    return match ? match[1] : null;
}

function Show_LabDisclaimer(e) {
    if (getCookie('flagDesclaimerLab') === 'true') {
        if (!isRobot && !isAdminGlobal && !isAuditGlobal) searchDossiersLab();
    }
    else {
        $('#disclaimerLabModal').modal({
            backdrop: 'static',
            keyboard: false
        })
        $('#disclaimerLabModal').modal('show');
    }
}

function setCookie(cName, cValue, expDays) {
    let date = new Date();
    date.setTime(date.getTime() + (expDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();
    document.cookie = cName + "=" + cValue + "; " + expires + "; path=/";
}

function accept_LabDisclaimer(e) {
    $('#disclaimerLabModal').modal('hide');
    setCookie('flagDesclaimerLab', true, 7);
    if (!isRobot && !isAdminGlobal && !isAuditGlobal) searchDossiersLab();
}
function notAccept_LabDisclaimer(e) {
    document.location.href = "/";
}

function getFileExtension(filename) {
    return filename.substring(filename.lastIndexOf('.') + 1, filename.length) || filename;
}

function criblagePersonnePhysiqueLabModalPreview_OnShowing() {
    $("#CriblagePersonnePhysiqueLabModalPreview").dxPopup({
        width: "90%"
    });
}

function showCriblagePersonnePhysiqueLabModalPreview(contentHtml) {
    var popup = $("#CriblagePersonnePhysiqueLabModalPreview").dxPopup('instance');
    popup.content().css("overflow", "auto");
    popup.option('contentTemplate', function (contentTemplate) {
        contentTemplate.append(contentHtml);
    });
    popup.show();
}


function closeCriblagePersonnePhysiqueLab_onClick() {

    if ($("#CriblagePersonnePhysiqueLabModalPreview")) {
        $("#CriblagePersonnePhysiqueLabModalPreview").dxPopup('instance').content().empty();
        $("#CriblagePersonnePhysiqueLabModalPreview").dxPopup('instance').hide();
    }
}


function showCriblagePersonnePhysiqueLab_onClick(order) {
    var personnePhysiqueNomNaissance = $("#PersonnePhysiqueLabNomNaissance" + order).dxTextBox("instance");
    var personnePhysiquePrenoms = $("#PersonnePhysiqueLabPrenoms" + order).dxTextBox("instance");
    var personnePhysiqueDateNaissance = $("#PersonnePhysiqueLabDateNaissance" + order).dxDateBox("instance");
    var name = null;
    var firstname = null;
    var dateNaissance = null;
    if (personnePhysiqueNomNaissance != null) {
        name = personnePhysiqueNomNaissance.option('value');
    }
    if (personnePhysiquePrenoms != null) {
        firstname = personnePhysiquePrenoms.option('value');
    }
    if (personnePhysiqueDateNaissance != null) {
        var dateNaissanceValue = personnePhysiqueDateNaissance.option('value');
        if (dateNaissanceValue != null) {
            if (typeof dateNaissanceValue != "string") {
                dateNaissance = dateNaissanceValue.toJSON();
            }
            else {
                dateNaissance = new Date(dateNaissanceValue).toJSON();
            }
        }
    }

    if ((name != null && name != "") || (firstname != null && firstname != "") || dateNaissance != null) {

        $.ajax({
            method: "GET",
            cache: false,
            data: { name: name, firstname: firstname, dateNaissance: dateNaissance },
            url: '/' + culture + '/Lab/ServiceLab/GetCriblagePersonnePhysiqueLabPartial',
        }).done(function (data) {
            showCriblagePersonnePhysiqueLabModalPreview(data);
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }

        }).always(function (e) {
        });
    }
}

function criblagePersonneMoraleLabModalPreview_OnShowing() {
    $("#CriblagePersonneMoraleLabModalPreview").dxPopup({
        width: "90%"
    });
}

function natureRelationClientPersonneMoraleLabModalPreview_OnShowing() {
    $("#NatureRelationClientPersonneMoraleLabModalPreview").dxPopup({
        width: "90%"
    });
}

function natureRelationClientPersonnePhysiqueLabModalPreview_OnShowing() {
    $("#NatureRelationClientPersonnePhysiqueLabModalPreview").dxPopup({
        width: "90%"
    });
}

function ppePersonnePhysiqueLabModalPreview_OnShowing() {
    $("#PpePersonnePhysiqueLabModalPreview").dxPopup({
        width: "90%"
    });
}

function showCriblagePersonneMoraleLabModalPreview(contentHtml) {
    var popup = $("#CriblagePersonneMoraleLabModalPreview").dxPopup('instance');
    popup.content().css("overflow", "auto");
    popup.option('contentTemplate', function (contentTemplate) {
        contentTemplate.append(contentHtml);
    });
    popup.show();
}



function showNatureRelationClientPersonneMoraleLabModalPreview(contentHtml) {
    var popup = $("#NatureRelationClientPersonneMoraleLabModalPreview").dxPopup('instance');
    popup.content().css("overflow", "auto");
    popup.option('contentTemplate', function (contentTemplate) {
        contentTemplate.append(contentHtml);
    });
    popup.show();
}

function showNatureRelationClientPersonnePhysiqueLabModalPreview(contentHtml) {
    var popup = $("#NatureRelationClientPersonnePhysiqueLabModalPreview").dxPopup('instance');
    popup.content().css("overflow", "auto");
    popup.option('contentTemplate', function (contentTemplate) {
        contentTemplate.append(contentHtml);
    });
    popup.show();
}


function showPpePersonnePhysiqueLabModalPreview(contentHtml) {
    var popup = $("#PpePersonnePhysiqueLabModalPreview").dxPopup('instance');
    popup.content().css("overflow", "auto");
    popup.option('contentTemplate', function (contentTemplate) {
        contentTemplate.append(contentHtml);
    });
    popup.show();
}



function closeCriblagePersonneMoraleLab_onClick() {

    if ($("#CriblagePersonneMoraleLabModalPreview")) {
        $("#CriblagePersonneMoraleLabModalPreview").dxPopup('instance').content().empty();
        $("#CriblagePersonneMoraleLabModalPreview").dxPopup('instance').hide();
    }
}

function closeNatureRelationClientPersonneMoraleLab_onClick() {

    if ($("#NatureRelationClientPersonneMoraleLabModalPreview").dxPopup('instance')) {
        $("#NatureRelationClientPersonneMoraleLabModalPreview").dxPopup('instance').content().empty();
        $("#NatureRelationClientPersonneMoraleLabModalPreview").dxPopup('instance').hide();
    }
    if ($("#NatureRelationClientPersonnePhysiqueLabModalPreview").dxPopup('instance')) {
        $("#NatureRelationClientPersonnePhysiqueLabModalPreview").dxPopup('instance').content().empty();
        $("#NatureRelationClientPersonnePhysiqueLabModalPreview").dxPopup('instance').hide();
    }
}

function closePpePersonnePhysiqueLab_onClick() {

    if ($("#PpePersonnePhysiqueLabModalPreview").dxPopup('instance')) {
        $("#PpePersonnePhysiqueLabModalPreview").dxPopup('instance').content().empty();
        $("#PpePersonnePhysiqueLabModalPreview").dxPopup('instance').hide();
    }

}



function showCriblagePersonneMoraleLab_onClick(order) {
    var personneMoraleRaisonSociale = $("#PersonneMoraleLabRaisonSociale" + order).dxTextBox("instance");
    var personneMoraleNumeroImmatriculation = $("#PersonneMoraleLabNumeroImmatriculation" + order).dxTextBox("instance");
    var name = null;
    var immatriculation = null;
    if (personneMoraleRaisonSociale != null) {
        name = personneMoraleRaisonSociale.option('value');
    }
    if (personneMoraleNumeroImmatriculation != null) {
        immatriculation = personneMoraleNumeroImmatriculation.option('value');
    }
    if ((name != null && name != "") || (immatriculation != null && immatriculation != "")) {

        $.ajax({
            method: "GET",
            cache: false,
            data: { name: name, immatriculation: immatriculation },
            url: '/' + culture + '/Lab/ServiceLab/GetCriblagePersonneMoraleLabPartial',
        }).done(function (data) {
            showCriblagePersonneMoraleLabModalPreview(data);
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }

        }).always(function (e) {
        });
    }
}

function showNatureRelationClientPmPopupLab_onClick() {

    $.ajax({
        method: "GET",
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/GetNatureRelationClientPartialPopup',
    }).done(function (data) {
        showNatureRelationClientPersonneMoraleLabModalPreview(data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });

}

function showNatureRelationClientPpPopupLab_onClick() {

    $.ajax({
        method: "GET",
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/GetNatureRelationClientPartialPopup',
    }).done(function (data) {
        showNatureRelationClientPersonnePhysiqueLabModalPreview(data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });

}

function showPpePpPopupLab_onClick() {

    $.ajax({
        method: "GET",
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/GetPpePartialPopup',
    }).done(function (data) {
        showPpePersonnePhysiqueLabModalPreview(data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });

}

function gridDuplicateSelected_Lab_OnClick(e) {
    var rowSelected = getSelectedRowDossier();
    if (rowSelected) {
        location = '/' + culture + '/lab/dossier/duplicate/' + rowSelected.CryptedId;
    }
}


function blocage_Lab_OnChanged(id) {

    var blocage = $("#DossierLabOperationsBlocage" + id).dxCheckBox("instance").option("value");
    if (blocage) {
        $("#DossierLabOperationsGel" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsAnnulation" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsLibere" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsRejetSanction" + id).dxCheckBox("instance").option("value", false);
    }

}

function gel_Lab_OnChanged(id) {

    var gel = $("#DossierLabOperationsGel" + id).dxCheckBox("instance").option("value");
    if (gel) {
        $("#DossierLabOperationsBlocage" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsAnnulation" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsLibere" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsRejetSanction" + id).dxCheckBox("instance").option("value", false);
    }
}

function annulation_Lab_OnChanged(id) {

    var annulation = $("#DossierLabOperationsAnnulation" + id).dxCheckBox("instance").option("value");
    if (annulation) {
        $("#DossierLabOperationsGel" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsBlocage" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsLibere" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsRejetSanction" + id).dxCheckBox("instance").option("value", false);
    }
}

function libere_Lab_OnChanged(id) {

    var libere = $("#DossierLabOperationsLibere" + id).dxCheckBox("instance").option("value");
    if (libere) {
        $("#DossierLabOperationsGel" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsAnnulation" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsBlocage" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsRejetSanction" + id).dxCheckBox("instance").option("value", false);
    }
}

function rejetSanction_Lab_OnChanged(id) {

    var rejet = $("#DossierLabOperationsRejetSanction" + id).dxCheckBox("instance").option("value");
    if (rejet) {
        $("#DossierLabOperationsGel" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsAnnulation" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsBlocage" + id).dxCheckBox("instance").option("value", false);
        $("#DossierLabOperationsLibere" + id).dxCheckBox("instance").option("value", false);
    }
}

function set_scenario_by_application() {
    var data = $("#DossierLabScenariosScenarioLabId0").dxSelectBox("instance");
    var value = $("#DossierLabScenariosScenarioLabId0ApplicationScenarioLabId").dxSelectBox("instance").option("value");

    var scenarioLabs = [];
    data.option("value", null);
    if (data && value != undefined) {
        var scenarioLabs = data.getDataSource();
        scenarioLabs.filter(["ApplicationScenarioLabId", "=", value]);
        scenarioLabs.load();
    }
    else {
        var scenarioLabs = data.getDataSource();
        scenarioLabs.filter(["ApplicationScenarioLabId", ">", 0]);
        scenarioLabs.load();
    }

}



function set_scenario_by_application_on_init() {
    var data = $("#DossierLabScenariosScenarioLabId0").dxSelectBox("instance");
    var value = $("#DossierLabScenariosScenarioLabId0ApplicationScenarioLabId").dxSelectBox("instance").option("value");

    var scenarioLabs = [];
    if (data && value != undefined) {
        var scenarioLabs = data.getDataSource();
        scenarioLabs.filter(["ApplicationScenarioLabId", "=", value]);
        scenarioLabs.load();
    }
    else {
        var scenarioLabs = data.getDataSource();
        scenarioLabs.filter(["ApplicationScenarioLabId", ">", 0]);
        scenarioLabs.load();
    }

}

function showListMail() {

    var list = document.getElementById("divListMail");
    if (list.style.display === "none") {
        list.style.display = "block";
    }
    else {
        list.style.display = "none";
    }
}

function showEmailTemplateList() {

    var list = document.getElementById("emailTemplateLabList");
    if (list.style.display === "none") {
        list.style.display = "block";
    }
    else {
        list.style.display = "none";
    }
}


function selection_changed(selectedItems) {
    var data = selectedItems.selectedRowsData;

    if (data.length > 0) {
        $("#DemandeInformationLabMailDestinataire").dxTextBox({
            value: data.map((x) => `${x.Mail}`).join(";")
        });
    } else {
        $("#DemandeInformationLabMailDestinataire").dxTextBox({
            value: ""
        });
    }

}

function selection_changed_Mail_template() {
    var selectedItems = $("#grideMailTemplateList").dxDataGrid("instance");
    var list = document.getElementById("emailTemplateLabList");
    var mailbody = "";
    if (culture === "fr") {
        mailbody = selectedItems.getSelectedRowsData()[0].FrenchDescription;
    } else {
        mailbody = selectedItems.getSelectedRowsData()[0].EnglishDescription;
    }


    if (mailbody.length > 0) {
        $("#DemandeInformationLabDemandeEdit").dxHtmlEditor({
            value: mailbody
        });
    } else {
        $("#DemandeInformationLabDemandeEdit").dxHtmlEditor({
            value: ""
        });
    }
    list.style.display = "none";
}

function btnExportDossierLabNewDS_OnClick() {
    showLoadPanel(_labTranslatableMessageLoadExportDossier);
    var rowSelected = getSelectedRowDossier();
    var cryptedId = rowSelected ? rowSelected.CryptedId : null;
    window.location.href = '/' + culture + '/Lab/ServiceLab/GetPartialExportDossierLabNew/?cryptedId=' + cryptedId;
    hideLoadPanel();
}
function btnExportDossierLabPdf_OnClick() {
    var rowSelected = getSelectedRowDossier();
    var dosId = rowSelected ? rowSelected.CodeUnique : null;
    if (rowSelected.IsNewDeclarationTracfin != null && rowSelected.IsNewDeclarationTracfin == true) {
        showLoadPanel(_labTranslatableMessageLoadExportDossier);
        var rowSelected = getSelectedRowDossier();
        var cryptedId = rowSelected ? rowSelected.CryptedId : null;
        window.location.href = '/' + culture + '/Lab/ServiceLab/GetPartialExportDossierLabNew/?cryptedId=' + cryptedId;
        hideLoadPanel();
    }
    else {
        showLoadPanel(_labTranslatableMessageLoadExportDossier);

        $.ajax({
            method: "GET",
            cache: false,
            url: '/' + culture + '/Lab/ServiceLab/GetPartialExportTypeDossierLab',
            data: { codeUniqueDossier: dosId }
        }).done(function (data) {
            showAddExportDossierLabTypeModalPreview(data);
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
    }
}
function showAddExportDossierLabTypeModalPreview(contentHtml) {

    var popup = $("#ExportDossierLabTypeModalPreview").dxPopup('instance');
    popup.option('contentTemplate', '<div id="exportContentScrollView">' + contentHtml + '</div>');
    popup.option('onShown', function () {
        $("#exportContentScrollView").dxScrollView({
            scrollByContent: true,
            scrollByThumb: true,
            showScrollbar: "always",
        });
    });
    popup.show();
}
function cancelAddExportDossierLabTypeModalPreview() {
    $("#ExportDossierLabTypeModalPreview").dxPopup('instance').hide();
}
function btnConfirmeExportDossierLab_OnClick() {
    hideLoadPanel();
    var typeEnvoie = $("#TypeExport").dxSelectBox("instance").option('value');
    var rowSelected = getSelectedRowDossier();
    var cryptedId = rowSelected ? rowSelected.CryptedId : null;
    if (cryptedId != null && typeEnvoie == 1) {
        window.location.href = '/' + culture + '/Lab/ServiceLab/GetPartialExportDossierLab/?cryptedId=' + cryptedId;
    }
    else if (cryptedId != null && typeEnvoie == 2) {
        gridExtraireDetailsSelected_onClick(cryptedId);
    }
}
function searchPpPm_OnClick(a, b, c, directionId) {
    var order = a;
    var isPp = b;
    var isPm = c;
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');
    $.ajax({
        method: "GET",
        cache: false,
        data: { order, isPp, isPm, directionId },
        url: '/' + culture + '/lab/dossier/SearchPopUp/',
    }).done(function (data) {
        var popup = $("#searchThirdModal").dxPopup('instance');
        popup.option('contentTemplate', function (contentTemplate) {
            contentTemplate.append(data);
        });
        popup.show();
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
    });
}


function closeSearchThird_OnClick() {
    closeSearchThird_OnClickPreview();
}

function closeSearchThird_OnClickPreview() {
    if ($("#searchThirdModal")) {
        $("#searchThirdModal").dxPopup('instance').content().empty();
        $("#searchThirdModal").dxPopup('instance').hide();

    }
}


function bunitCustomer_OnValueChanged() {
    var bunit = $("#BUNIT").dxSelectBox("instance") ? $("#BUNIT").dxSelectBox("instance").option('value') : null;
    var customerId = $("#CustomerIdentifier").dxTextBox("instance") ? $("#CustomerIdentifier").dxTextBox("instance").option('value') : null;

    $.ajax({
        method: "GET",
        data: { bunit, customerId },
        cache: false,
        dataType: "json",
        url: '/' + culture + '/lab/dossier/GetCustomersByBunitAndIdentifier/'
    }).done(function (data) {
        $("#gridThird").dxDataGrid("instance").option("dataSource", data);
        $("#gridThird").dxDataGrid("instance").refresh();
        $("#gridThird").dxDataGrid("instance").option("selectedRowKeys", null);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_searchThirdTranslatableMessageLoadingUsersError, "error");
    }).always(function (e) {
    });
}


function gridThird_OnSelectionChanged(data) {


    if (data.selectedRowsData.length >= 1) {
        $("#addTiersLabLabDiv").css("display", "block");
    } else {
        $("#addTiersLabLabDiv").css("display", "none");
    }


}

function fillingThirds(code, originCode, order, isPp, isPm, selectedrowdata, selectedgrid) {
    var orderAdd = order;
    var origineCode = originCode;
    var pP = isPp;
    var pM = isPm;
    var selectedRowData = selectedrowdata;
    var SelectedGrid = selectedgrid;
    if (pP == "True") {
        $("#btnAddNewCoordonneePhysicalPerson" + orderAdd).click();

    }
    if (pM == "True") {
        $("#btnAddNewCoordonneeMoralePerson" + orderAdd).click();
    }
    $.ajax({
        method: "GET",
        cache: false,
        data: { code },
        url: '/' + culture + '/lab/servicelab/GetPaysByIsoCode/',
    }).done(function (data) {
        var paysId = data;
        var adresse = '';
        var codePostale = '';
        const nomdenaiassance = selectedRowData[0].NAME_1.substring(0, selectedRowData[0].NAME_1.indexOf(","));
        const prenoms = selectedRowData[0].NAME_1.substring(selectedRowData[0].NAME_1.indexOf(",") + 1);
        const gender = selectedRowData[0].GENDER == "M" ? 1 : 2;
        if (selectedRowData[0].ADDRESS.indexOf(',') > -1) {
            adresse = selectedRowData[0].ADDRESS.substring(0, selectedRowData[0].ADDRESS.indexOf(","));
            codePostale = selectedRowData[0].ADDRESS.substring(selectedRowData[0].ADDRESS.indexOf(",") + 1);
        }
        else {
            adresse = selectedRowData[0].ADDRESS;
            codePostale = '';
        }


        if (SelectedGrid.getSelectedRowsData().length > 0) {

            if (pP == "True") {
                $("#PersonnePhysiqueLabIdentifiantClient" + orderAdd).dxTextBox("instance").option("value", selectedRowData[0].RECORD_ID.length != 0 ? selectedRowData[0].RECORD_ID : null);
                $("#PersonnePhysiqueLabNomNaissance" + orderAdd).dxTextBox("instance").option("value", nomdenaiassance != null ? nomdenaiassance : null);
                $("#PersonnePhysiqueLabPrenoms" + orderAdd).dxTextBox("instance").option("value", prenoms != null ? prenoms : null);
                $("#PersonnePhysiqueLabDateNaissance" + orderAdd).dxDateBox("instance").option("value", selectedRowData[0].DOB.length != 0 ? selectedRowData[0].DOB : null);
                $("#PersonnePhysiqueLabCiviliteId" + orderAdd).dxSelectBox("instance").option("value", gender);
                $("#PersonnePhysiqueLabDateEntreeEnRelation" + orderAdd).dxDateBox("instance").option("value", selectedRowData[0].REE.length != 0 ? selectedRowData[0].REE : null);
                $("#btnAddNewCoordonneePhysicalPerson" + orderAdd).click();
                setTimeout(function () {
                    $("#PersonnePhysiqueLabCoordonneesCoordonneeVoie" + orderAdd + '-0').dxTextBox("instance").option("value", adresse != null ? adresse : null);
                    $("#PersonnePhysiqueLabCoordonneesCoordonneeCodePostal" + orderAdd + '-0').dxTextBox("instance").option("value", codePostale != null ? codePostale : null);
                    $("#PersonnePhysiqueLabLieuNaissance" + orderAdd).dxTextBox("instance").option("value", selectedRowData[0].COB.length != 0 ? selectedRowData[0].COB : null);
                    $("#PersonnePhysiqueLabCoordonneesCoordonneePaysId" + orderAdd + '-0').dxSelectBox("instance").option("value", paysId != null ? paysId : null);
                    /*                    $("#PersonnePhysiqueLabNationaliteId" + orderAdd).dxSelectBox("instance").option("value", paysId != null ? paysId : null);*/
                    $("#PersonnePhysiqueLabCoordonneesCoordonneeVille" + orderAdd + '-0').dxTextBox("instance").option("value", selectedRowData[0].CITY.length != 0 ? selectedRowData[0].CITY : null);
                }, 3000);
            }
            if (pM == "True") {
                $("#PersonneMoraleLabIdentifiantClient" + orderAdd).dxTextBox("instance").option("value", selectedRowData[0].RECORD_ID.length != 0 ? selectedRowData[0].RECORD_ID : null);
                $("#PersonneMoraleLabRaisonSociale" + orderAdd).dxTextBox("instance").option("value", selectedRowData[0].NAME_1.length != 0 ? selectedRowData[0].NAME_1 : null);
                $("#PersonneMoraleLabDateImmatriculation" + orderAdd).dxDateBox("instance").option("value", selectedRowData[0].DOB.length != 0 ? selectedRowData[0].DOB : null);
                $("#PersonneMoraleLabNumeroImmatriculation" + orderAdd).dxTextBox("instance").option("value", selectedRowData[0].NATIONAL_ID.length != 0 ? selectedRowData[0].NATIONAL_ID : null);
                $("#PersonneMoraleLabDateEntreeEnRelation" + orderAdd).dxDateBox("instance").option("value", selectedRowData[0].REE.length != 0 ? selectedRowData[0].REE : null);
                setTimeout(function () {
                    $("#PersonneMoraleLabCoordonneeVoie" + orderAdd).dxTextBox("instance").option("value", adresse != null ? adresse : null);
                    $("#PersonneMoraleLabCoordonneeCodePostal" + orderAdd).dxTextBox("instance").option("value", codePostale != null ? codePostale : null);
                    $("#PersonneMoraleLabCoordonneePaysId" + orderAdd).dxSelectBox("instance").option("value", paysId != null ? paysId : null);
                    /*                    $("#PersonneMoraleLabPaysId" + orderAdd).dxSelectBox("instance").option("value", paysId != null ? paysId : null);*/
                    $("#PersonneMoraleLabCoordonneeVille" + orderAdd).dxTextBox("instance").option("value", selectedRowData[0].CITY.length != 0 ? selectedRowData[0].CITY : null);
                }, 3000);

            }

        }
    }).fail(function (e) {
        if (e.status == 401) {

            return false;
        }
    });
    code = origineCode;
    $.ajax({
        method: "GET",
        cache: false,
        data: { code },
        url: '/' + culture + '/lab/servicelab/GetPaysByIsoCode/',
    }).done(function (data) {
        var originePaysId = data;
        if (pP == "True") {
            $("#PersonnePhysiqueLabPaysNaissanceId" + orderAdd).dxSelectBox("instance").option("value", originePaysId);
        }


        closeSearchThird_OnClickPreview();
    }).fail(function (e) {
        if (e.status == 401) {

            return false;
        }
    });
}

function addTiersLab_onClick(order, isPp, isPm) {

    var SelectedGrid = $("#gridThird").dxDataGrid("instance");
    var selectedRowData = SelectedGrid.getSelectedRowsData();
    var payscode = selectedRowData[0].COUNTRY.substring(0, 3);
    var paysOrigineCode = selectedRowData[0].POB.substring(0, 3);
    var orders = order;
    var isPps = isPp;
    var isPms = isPm;

    fillingThirds(payscode, paysOrigineCode, orders, isPps, isPms, selectedRowData, SelectedGrid);

}


function bunit_OnContentReady() {

    var insBunit = $("#BUNIT").dxSelectBox("instance");

    if (insBunit != undefined) {

        var data = insBunit.option("dataSource");

        if (data.store._array.length == 1) {
            insBunit.option("value", data.store._array[0].Code);

            insBunit.option("readOnly", true);

        } else {
            insBunit.option("readOnly", false);
        }

    }
}


function enableSearchThird() {

    var bunit = $("#BUNIT").dxSelectBox("instance").option("value");
    var customerId = $("#CustomerIdentifier").dxTextBox("instance").option("text");
    if (bunit != '' && bunit != null && customerId != '') {
        $("#buttonSearchDiv").css("display", "block");
    } else {
        $("#buttonSearchDiv").css("display", "none");
    }

}

function SaveDossierLab(fromDuplicateButton, isDetailsView) {
    if (isDetailsView != 'True') {
        var form = $("#dossierLabForm").closest("form");
        var formData = new FormData(form[0]);
        $.ajax({
            method: "POST",
            data: formData,
            dataType: "json",
            url: '/' + culture + '/Lab/ServiceLab/CreateOrUpdateDossierLab',
            processData: false,
            contentType: false,
        }).done(function (data) {
            if (data.status) {
                DevExpress.ui.notify(_labTranslatableMessageCaseSavedSuccessfully, "success", 3000);

                if (data.hasOwnProperty('exitDossier') && data.exitDossier) {
                    location = '/' + culture + '/lab/dossiers/management';
                }

                else if (fromDuplicateButton && _codeUnique != '' && _codeUnique != null) {
                    location = '/' + culture + '/fraude/dossier/DossierFraudeDuplicate/?codeUnique=' + _codeUnique
                }

                else if (data.hasOwnProperty('cryptedId') && data.cryptedId != null) {
                    location = '/' + culture + '/lab/dossier/edit/' + data.cryptedId;
                }

                else {
                    location = '/' + culture + '/lab/dossiers/management';
                }
            }
            else {
                if (data.hasOwnProperty('message')) {
                    DevExpress.ui.notify(data.message, "error", 3000);
                }
                else {
                    DevExpress.ui.notify(_labTranslatableMessageCaseSavingErrorOccuredWithoutMessage, "error", 3000);
                }
            }
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            if (e.data.hasOwnProperty('message'))
                DevExpress.ui.notify(e.message, "error", 3000);
            else
                DevExpress.ui.notify(_labTranslatableMessageCaseSavingErrorOccured, "error", 3000);
        }).always(function (e) {
            hideLoadPanel();
            enableButton('buttonInitialize');
            enableButton('buttonSearch');
            isSubmitting = false;
        });
    } else {
        isSubmitting = false;
        if (fromDuplicateButton && _codeUnique != '' && _codeUnique != null) {
            location = '/' + culture + '/fraude/dossier/DossierFraudeDuplicate/?codeUnique=' + _codeUnique
        }
    }

}

function btnDupliquerDossierLab_Onclick(e, isDetailsView) {
    updateInfoDsftFormData(true);
    SaveDossierLab(true, isDetailsView);
}


function GetDossierFraude(statutDossierFraudeId, isEditDossierlab) {
    //si statut dossier cloture =3.
    if (statutDossierFraudeId === 3 || isEditDossierlab === "False") {
        window.location.href = '/' + culture + '/fraude/dossier/details/' + _dossierFraudeCryptedId;
    }
    else {
        window.location.href = '/' + culture + '/fraude/dossier/edit/' + _dossierFraudeCryptedId;
    }

}

function GetDossierFraudeSource(statutDossierFraudeId) {
    if (statutDossierFraudeId === 3) {
        window.location.href = '/' + culture + '/fraude/dossier/details/' + _dossierFraudeCryptedId;
    } else {
        window.location.href = '/' + culture + '/fraude/dossier/edit/' + _dossierFraudeCryptedId;
    }

}


function ppeId_lab_OnValueChanged(id) {
    var ppeSelectedItem = $("#PersonnePhysiqueLabPpeId" + id).dxSelectBox("instance").option('selectedItem');

    var fonctionRelationContainerSelector = "#FonctionRelationDiv" + id;
    var fonctionRelationSelector = "#PpeTypes" + id;
    try {
        disableFieldValidators(fonctionRelationSelector);
    }
    catch (e) {

    }

    if (ppeSelectedItem && ppeSelectedItem.Id == 1) {
        showRequiredField(fonctionRelationContainerSelector, fonctionRelationSelector);
        $(fonctionRelationSelector).addClass('mandatory');
    }
    else {
        $(fonctionRelationSelector).dxTagBox("instance").option("value", null);
        $(fonctionRelationContainerSelector).addClass("dx-hidden");
    }
}


function IsUtilisationAutreIdentite_OnValueChanged(id) {
    var isUtilisationAutreIdentite = $("#PersonnePhysiqueLabIsUtilisationAutreIdentite" + id).dxSelectBox("instance").option('value');

    if (isUtilisationAutreIdentite == 1) {
        $("#informationAutreIdentite1" + id).removeClass("dx-hidden");
        $("#informationAutreIdentite2" + id).removeClass("dx-hidden");
    }
    else {
        $("#informationAutreIdentite1" + id).addClass("dx-hidden");
        $("#informationAutreIdentite2" + id).addClass("dx-hidden");
    }
}


function identificationProfessionnelle_OnValueChanged(index) {

    var numeroImmatriculationContainerSelector = "#PersonnePhysiqueLabNumeroImmatriculationDiv" + index;
    var numeroImmatriculationId = "PersonnePhysiqueLabNumeroImmatriculation" + index;
    var numeroImmatriculationSelector = "#" + numeroImmatriculationId;
    var numeroImmatriculationAsterisk = getRequiredFieldAsterisk(numeroImmatriculationId);

    var denominationSocialeContainerSelector = "#PersonnePhysiqueLabDenominationSocialeDiv" + index;
    var denominationSocialeId = "PersonnePhysiqueLabDenominationSociale" + index;
    var denominationSocialeSelector = "#" + denominationSocialeId;
    var denominationSocialeAsterisk = getRequiredFieldAsterisk(denominationSocialeId);

    var numeroImmatriculationValidationRules = [];
    var denominationSocialeValidationRules = [];

    denominationSocialeValidationRules.push(requiredValidationRule);

    try {
        disableFieldValidators(numeroImmatriculationSelector, numeroImmatriculationAsterisk);
        disableFieldValidators(denominationSocialeSelector, denominationSocialeAsterisk);
    }
    catch (e) { }

    var result = $("#PersonnePhysiqueLabIdentificationProfessionnelleId" + index).dxSelectBox("instance").option("value");

    $("#PersonnePhysiqueLabPaysDeRegistreDiv" + index).toggle(_labIdEtranger && result == _labIdEtranger);

    if (result == 8) {
        $("#AutreIdentificationProfessionnelleDiv" + index).removeClass("dx-hidden");
        $("#PersonnePhysiqueLabNumeroImmatriculationDiv" + index).removeClass("dx-hidden");

        showFieldWithValidationRules(denominationSocialeContainerSelector,
            denominationSocialeSelector,
            null,
            denominationSocialeValidationRules,
            denominationSocialeAsterisk);

        $("#PersonnePhysiqueLabFormeJuridiqueIdDiv" + index).removeClass("dx-hidden");
        $("#PersonnePhysiqueLabActivitePrincipaleDiv" + index).removeClass("dx-hidden");
        $("#PersonnePhysiqueLabSecteurProfessionnelIdDiv" + index).removeClass("dx-hidden");
        $("#PartialActiviteCoordonneePersonnePhysiqueLabDiv" + index).removeClass("dx-hidden");
    }
    else {
        $("#AutreIdentificationProfessionnelleDiv" + index).addClass("dx-hidden");
        if (result != 7)
            $("#PersonnePhysiqueLabAutreIdentificationProfessionnelle" + index).dxTextBox("instance").option("value", "");
        if (result == 9 || result == 10) {
            $("#PersonnePhysiqueLabNumeroImmatriculationDiv" + index).addClass("dx-hidden");
            $("#PersonnePhysiqueLabNumeroImmatriculation" + index).dxTextBox("instance").option("value", "");
            $("#PersonnePhysiqueLabDenominationSocialeDiv" + index).addClass("dx-hidden");
            $("#PersonnePhysiqueLabDenominationSociale" + index).dxTextBox("instance").option("value", "");
            $("#PersonnePhysiqueLabFormeJuridiqueIdDiv" + index).addClass("dx-hidden");
            $("#PersonnePhysiqueLabFormeJuridiqueId" + index).dxSelectBox("instance").option("value", null);
            $("#PersonnePhysiqueLabActivitePrincipaleDiv" + index).addClass("dx-hidden");
            $("#PersonnePhysiqueLabActivitePrincipale" + index).dxTextBox("instance").option("value", "");
            $("#PersonnePhysiqueLabSecteurProfessionnelIdDiv" + index).addClass("dx-hidden");
            $("#PersonnePhysiqueLabSecteurProfessionnelId" + index).dxSelectBox("instance").option("value", null);
            $("#PartialActiviteCoordonneePersonnePhysiqueLabDiv" + index).addClass("dx-hidden");
        }
        else {
            if (result == 7) {
                $("#AutreIdentificationProfessionnelleDiv" + index).removeClass("dx-hidden");
            }

            if (result == 1) {
                numeroImmatriculationValidationRules.push(requiredValidationRule);
            }

            showFieldWithValidationRules(numeroImmatriculationContainerSelector,
                numeroImmatriculationSelector,
                null,
                numeroImmatriculationValidationRules,
                numeroImmatriculationAsterisk);

            showFieldWithValidationRules(denominationSocialeContainerSelector,
                denominationSocialeSelector,
                null,
                denominationSocialeValidationRules,
                denominationSocialeAsterisk);

            $("#PersonnePhysiqueLabFormeJuridiqueIdDiv" + index).removeClass("dx-hidden");
            $("#PersonnePhysiqueLabActivitePrincipaleDiv" + index).removeClass("dx-hidden");
            $("#PersonnePhysiqueLabSecteurProfessionnelIdDiv" + index).removeClass("dx-hidden");
            $("#PartialActiviteCoordonneePersonnePhysiqueLabDiv" + index).removeClass("dx-hidden");
        }
    }
}

function identiteDirigeant_OnValueChanged(e) {

    var result = e.value;
    var name = e.element[0].id.replace("PersonneMoraleLabDirigeantsIdentiteDirigeant", "");
    if (result == -1) {
        $("#TypeDirigeantDiv" + name).removeClass("dx-hidden");
    }
    else {
        $("#TypeDirigeantDiv" + name).addClass("dx-hidden");
        $("#PersonneMoraleLabDirigeantsTypeDirigeant" + name).dxSelectBox("instance").option("value", null);
        $("#PersonneMoraleLabDirigeantsBeneficiaireEffectif" + name).dxSelectBox("instance").option("value", null);
    }
}

function addDirigeantMoralPerson(orderPersonneMorale) {
    showLoadPanel(_labTranslatableMessageAddingNewDirigeant);
    var countDirigeantMoralPerson = $('#dirigeants-personnemorale-' + orderPersonneMorale + ' > .dirigeantMoralPerson').length;
    var cryptedDossierLabId = htmlEncode($('#DossierLabPersonneMorales_' + orderPersonneMorale + '__CryptedDossierLabId').val());
    var cryptedPersonneMoraleLabId = htmlEncode($('#CryptedPersonneMoraleLabId' + orderPersonneMorale).val());

    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        url: '/' + culture + '/Lab/ServiceLab/AddDirigeantPersonneMoraleForm?orderPersonneMorale=' + orderPersonneMorale + '&order=' + countDirigeantMoralPerson + '&cryptedDossierLabId=' + cryptedDossierLabId + '&cryptedPersonneMoraleLabId=' + cryptedPersonneMoraleLabId,
    }).done(function (data) {
        $("#dirigeants-personnemorale-" + orderPersonneMorale).append(data);

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
        hideLoadPanel();
    });
}

function removeDirigeantMoralPerson(orderPersonneMorale, orderDirigeant) {
    $("#moralPersonDirigeant" + orderPersonneMorale + '-' + orderDirigeant).html('<input name="DossierLabPersonneMorales[' + orderPersonneMorale + '].PersonneMoraleLab.DirigeantPersonneMoraleLabs[' + orderDirigeant + '].ToDelete" value="True" id="DirigeantPersonneMoraleLabsToDelete' + orderPersonneMorale + '-' + orderDirigeant + '" type="hidden">');
    $("#moralPersonDirigeant" + orderPersonneMorale + '-' + orderDirigeant).hide();
}

function fonctionDirigeant_OnValueChanged(e) {
    var result = e.value;
    var name = e.element[0].id.replace("PersonneMoraleLabDirigeantsFonctionDirigeant", "");
    if (result == 14) {
        $("#AutreFonctionDirigeantDiv" + name).removeClass("dx-hidden");
    }
    else {
        $("#AutreFonctionDirigeantDiv" + name).addClass("dx-hidden");
        $("#PersonneMoraleLabAutreFonctionDirigeant" + name).dxTextBox("instance").option("value", "");
    }
}

function typeDirigeant_OnValueChanged(e) {
    var result = e.value;
    var name = e.element[0].id.replace("PersonneMoraleLabDirigeantsTypeDirigeant", "");
    if (result == 1) {
        $("#PpDirigeant01Div" + name).removeClass("dx-hidden");
        $("#PpDirigeant02Div" + name).removeClass("dx-hidden");
        $("#PmDirigeant01Div" + name).addClass("dx-hidden");
        $("#coordonnees-personneMorale-dirigeant-" + name).removeClass("dx-hidden");
    }
    else if (result == 2) {
        $("#PpDirigeant01Div" + name).addClass("dx-hidden");
        $("#PpDirigeant02Div" + name).removeClass("dx-hidden");
        $("#PmDirigeant01Div" + name).removeClass("dx-hidden");
        $("#coordonnees-personneMorale-dirigeant-" + name).removeClass("dx-hidden");
    }
    else {
        $("#PpDirigeant01Div" + name).addClass("dx-hidden");
        $("#PpDirigeant02Div" + name).addClass("dx-hidden");
        $("#PmDirigeant01Div" + name).addClass("dx-hidden");
        $("#coordonnees-personneMorale-dirigeant-" + name).addClass("dx-hidden");
        $("#PersonneMoraleLabDirigeantsBeneficiaireEffectif" + name).dxSelectBox("instance").option("value", null);
    }
}

function IdentificationProfessionelDirigeant_OnValueChanged(id) {
    var name = id.replace("PersonneMoraleLabDirigeantProfessionalIdentificationId", "");

    var numeroImmatriculationContainerSelector = "#PersonneMoraleLabDirigeantNumeroImmatriculationDiv" + name;
    var numeroImmatriculationId = "PersonneMoraleLabDirigeantNumeroImmatriculation" + name;
    var numeroImmatriculationSelector = "#" + numeroImmatriculationId;
    var numeroImmatriculationAsterisk = getRequiredFieldAsterisk(numeroImmatriculationId);

    var raisonSocialeContainerSelector = "#PersonneMoraleLabDirigeantRaisonSocialeDiv" + name;
    var raisonSocialeId = "PersonneMoraleLabDirigeantRaisonSociale" + name;
    var raisonSocialeSelector = "#" + raisonSocialeId;
    var raisonSocialeAsterisk = getRequiredFieldAsterisk(raisonSocialeId);

    $(numeroImmatriculationContainerSelector).removeClass("dx-hidden");

    disableFieldValidators(numeroImmatriculationSelector, numeroImmatriculationAsterisk);
    disableFieldValidators(raisonSocialeSelector, raisonSocialeAsterisk);

    var result = $("#" + id).dxSelectBox("instance").option("value");

    $("#PersonneMoraleLabDirigeantPaysDeRegistreDiv" + name).toggle(_labIdEtranger && result == _labIdEtranger);

    if (result == 8 || result == 7) {
        $("#OtherIdProDirigeantDiv" + name).removeClass("dx-hidden");
        return;
    }

    $("#OtherIdProDirigeantDiv" + name).addClass("dx-hidden");
    $("#PersonneMoraleLabDirigeantOtherIdPro" + name).dxTextBox("instance").option("value", "");

    if (result == _labIdIdentificationProfessionnelleSiren) {
        showFieldWithValidationRules(numeroImmatriculationContainerSelector,
            numeroImmatriculationSelector,
            null,
            [requiredValidationRule],
            numeroImmatriculationAsterisk);

        showFieldWithValidationRules(raisonSocialeContainerSelector,
            raisonSocialeSelector,
            null,
            [requiredValidationRule],
            raisonSocialeAsterisk);
        return;
    }

    if (result == _labIdIdentificationProfessionnelleAucune) {
        $(numeroImmatriculationContainerSelector).addClass("dx-hidden");
    }
}

var requiredValidationRule = {
    type: "required"
};
function ProfessionalIdentificationId_OnValueChanged(id) {
    var name = id.replace("PersonneMoraleLabProfessionalIdentificationId", "");
    if ($("#PersonneMoraleLabRaisonSociale" + name).dxTextBox("instance").option("value") == "_") {
        $("#PersonneMoraleLabRaisonSociale" + name).dxTextBox("instance").option("value", "");
    }

    var numeroImmatriculationContainerSelector = "#PersonneMoraleLabNumeroImmatriculationDiv" + name;
    var numeroImmatriculationId = "PersonneMoraleLabNumeroImmatriculation" + name;
    var numeroImmatriculationSelector = "#" + numeroImmatriculationId;
    var numeroImmatriculationAsterisk = getRequiredFieldAsterisk(numeroImmatriculationId);

    var raisonSocialeContainerSelector = "#PersonneMoraleLabRaisonSocialeDiv" + name;
    var raisonSocialeId = "PersonneMoraleLabRaisonSociale" + name;
    var raisonSocialeSelector = "#" + raisonSocialeId;
    var raisonSocialeAsterisk = getRequiredFieldAsterisk(raisonSocialeId);

    var otherRaisonContainerSelector = "#PersonneMoraleLabOtherRaisonIdProDiv" + name;
    var otherRaisonId = "OtherRaisonIdPro" + name;
    var otherRaisonSelector = "#" + otherRaisonId;
    var otherRaisonAsterisk = getRequiredFieldAsterisk(otherRaisonId);

    var numeroImmatriculationValidationRules = [];
    numeroImmatriculationValidationRules.push(requiredValidationRule);

    var raisonSocialeValidationRules = [];
    raisonSocialeValidationRules.push(requiredValidationRule);

    var otherRaisonValidationRules = [];
    otherRaisonValidationRules.push(requiredValidationRule);

    try {
        disableFieldValidators(numeroImmatriculationSelector, numeroImmatriculationAsterisk);
        disableFieldValidators(raisonSocialeSelector, raisonSocialeAsterisk);
        disableFieldValidators(otherRaisonSelector, otherRaisonAsterisk);
    }
    catch (e) { }

    var result = $("#" + id).dxSelectBox("instance").option("value");

    $("#PersonneMoraleLabPaysDeRegistreDiv" + name).toggle(_labIdEtranger && result == _labIdEtranger);

    if (result == 9) {
        $("#PersonneMoraleLabOtherRaisonIdProDiv" + name).addClass("dx-hidden");
        $("#OtherRaisonIdPro" + name).dxTextBox("instance").option("value", "");
        $("#PersonneMoraleLabNumeroImmatriculation" + name).dxTextBox("instance").option("value", "");
        $("#PersonneMoraleLabNumeroImmatriculationDiv" + name).addClass("dx-hidden");
        $("#PersonneMoraleLabDateImmatriculation" + name).dxDateBox("instance").option("value", null);
        $("#PersonneMoraleLabDateImmatriculationDiv" + name).addClass("dx-hidden");

        $("#PersonneMoraleLabSigleDiv" + name).removeClass("dx-hidden");
        $("#PersonneMoraleLabRaisonSocialeDiv" + name).removeClass("dx-hidden");

        showFieldWithValidationRules(raisonSocialeContainerSelector,
            raisonSocialeSelector,
            null,
            raisonSocialeValidationRules,
            raisonSocialeAsterisk);

        return;
    }

    if (result == 10) {
        $("#PersonneMoraleLabOtherRaisonIdProDiv" + name).addClass("dx-hidden");
        $("#OtherRaisonIdPro" + name).dxTextBox("instance").option("value", "");
        $("#PersonneMoraleLabNumeroImmatriculation" + name).dxTextBox("instance").option("value", "");
        $("#PersonneMoraleLabNumeroImmatriculationDiv" + name).addClass("dx-hidden");
        $("#PersonneMoraleLabDateImmatriculation" + name).dxDateBox("instance").option("value", null);
        $("#PersonneMoraleLabDateImmatriculationDiv" + name).addClass("dx-hidden");


        $("#PersonneMoraleLabSigle" + name).dxTextBox("instance").option("value", "");
        $("#PersonneMoraleLabSigleDiv" + name).addClass("dx-hidden");

        $("#PersonneMoraleLabRaisonSociale" + name).dxTextBox("instance").option("value", "_");
        $("#PersonneMoraleLabRaisonSocialeDiv" + name).addClass("dx-hidden");
        return;
    }

    showFieldWithValidationRules(numeroImmatriculationContainerSelector,
        numeroImmatriculationSelector,
        null,
        numeroImmatriculationValidationRules,
        numeroImmatriculationAsterisk);

    showFieldWithValidationRules(raisonSocialeContainerSelector,
        raisonSocialeSelector,
        null,
        raisonSocialeValidationRules,
        raisonSocialeAsterisk);

    if (result == 8) {
        $("#PersonneMoraleLabSigleDiv" + name).removeClass("dx-hidden");
    }

    if (result == 7 || result == 8) {
        $("#PersonneMoraleLabDateImmatriculationDiv" + name).removeClass("dx-hidden");

        showFieldWithValidationRules(otherRaisonContainerSelector,
            otherRaisonSelector,
            null,
            otherRaisonValidationRules,
            otherRaisonAsterisk);

        return;
    }

    $("#PersonneMoraleLabOtherRaisonIdProDiv" + name).addClass("dx-hidden");
    $("#OtherRaisonIdPro" + name).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDateImmatriculationDiv" + name).removeClass("dx-hidden");
    $("#PersonneMoraleLabSigleDiv" + name).removeClass("dx-hidden");
}

function beneficiaireEffectif_OnValueChanged(e) {
    var result = e.value;
    var name = e.element[0].id.replace("PersonneMoraleLabDirigeantsBeneficiaireEffectif", "");
    if (result == 1) {
        $("#BeDirigeantDiv" + name).addClass("dx-hidden");
        $("#nav-coordonnees-dirigeant-be-pm" + name).addClass("dx-hidden");
    }
    else if (result == 2) {
        $("#BeDirigeantDiv" + name).removeClass("dx-hidden");
        $("#nav-coordonnees-dirigeant-be-pm" + name).removeClass("dx-hidden");
    }
    else {
        $("#BeDirigeantDiv" + name).addClass("dx-hidden");
        $("#nav-coordonnees-dirigeant-be-pm" + name).addClass("dx-hidden");
    }

}
function identiteDirigeant_lab_OnInitialized(e) {
    var name = e.element[0].id.replace("PersonneMoraleLabDirigeantsIdentiteDirigeant", "");
    var cryptedDossierId = $("#dirigeantPersonneMoraleLabsCryptedDossierId" + name).val();
    var cryptedPersonneMoraleLabId = $("#dirigeantPersonneMoraleLabsCryptedPersonneMoraleLabId" + name).val();

    $.ajax({
        method: "GET",
        dataType: 'json',
        cache: false,
        data: { cryptedDossierId: cryptedDossierId, cryptedPersonneMoraleLabId: cryptedPersonneMoraleLabId },
        url: '/' + culture + '/Lab/ServiceLab/GetIdentiteDirigeantByDossierId',
    }).done(function (data) {
        $("#" + e.element[0].id).dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    loadMode: "raw",
                    load: function () {
                        return data;
                    }
                })
            })
        });

    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }

    }).always(function (e) {
    });

}


function addCoordonneeDirigeantMoralePerson(e) {
    var name = e.element[0].id.replace("btnAddNewCoordonneeDirigeantMoralePerson", "");
    if ($("#moralPersonDirigeantCoordonnee" + name).hasClass("dx-hidden")) {
        $("#moralPersonDirigeantCoordonnee" + name).show();
    }
    else {
        showLoadPanel(_labTranslatableMessageAddingNewCoordonnee);
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            contentType: "application/json; charset=utf-8",
            url: '/' + culture + '/Lab/ServiceLab/AddCoordonneeDirigeantPersonneMoraleForm?index=' + name,
        }).done(function (data) {
            if ($("#moralPersonDirigeantCoordonnee" + name)) $("#moralPersonDirigeantCoordonnee" + name).remove();
            $("#coordonnees-personneMorale-dirigeant-" + name).append(data);
            if ($("#btnAddNewCoordonneeDirigeantMoralePerson" + name)) $("#btnAddNewCoordonneeDirigeantMoralePerson" + name).addClass("dx-hidden");
            if ($("#remRowCoordonneeDirigeantPersonneMoraleLab" + name)) $("#remRowCoordonneeDirigeantPersonneMoraleLab" + name).show();

        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
    }
}

function addCoordonneeDirigeantBeMoralePerson(e) {
    var name = e.element[0].id.replace("btnAddNewCoordonneeDirigeantBeMoralePerson", "");
    if ($("#moralPersonBeCoordonnee" + name).hasClass("dx-hidden")) {
        $("#moralPersonBeCoordonnee" + name).show();
    }
    else {
        showLoadPanel(_labTranslatableMessageAddingNewCoordonnee);
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            contentType: "application/json; charset=utf-8",
            url: '/' + culture + '/Lab/ServiceLab/AddCoordonneeDirigeantBePersonneMoraleForm?index=' + name,
        }).done(function (data) {
            if ($("#moralPersonBeCoordonnee" + name)) $("#moralPersonBeCoordonnee" + name).remove();
            $("#coordonnees-personneMorale-dirigeant-be-" + name).append(data);
            if ($("#btnAddNewCoordonneeDirigeantBeMoralePerson" + name)) $("#btnAddNewCoordonneeDirigeantBeMoralePerson" + name).addClass("dx-hidden");
            if ($("#remRowCoordonneeDirigeantBePersonneMoraleLab" + name)) $("#remRowCoordonneeDirigeantBePersonneMoraleLab" + name).show();

        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
            hideLoadPanel();
        });
    }


}


function removeCoordonneeDirigeantMoralePerson(orderPersonneMorale, orderCoordonnee) {
    $("#moralPersonDirigeantCoordonnee" + orderPersonneMorale + '-' + orderCoordonnee).hide();
    $("#PersonneMoraleLabDirigeantCoordonneesNumeroVoie" + orderPersonneMorale + '-' + orderCoordonnee).val("");
    $("#PersonneMoraleLabDirigeantCoordonneeComplementVoieId" + orderPersonneMorale + '-' + orderCoordonnee).dxSelectBox("instance").option("value", null);
    $("#PersonneMoraleLabDirigeantCoordonneeTypeVoieId" + orderPersonneMorale + '-' + orderCoordonnee).dxSelectBox("instance").option("value", null);
    $("#PersonneMoraleLabDirigeantCoordonneeVoie" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneeComplement" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneeCodePostal" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneeVille" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneePaysId" + orderPersonneMorale + '-' + orderCoordonnee).dxSelectBox("instance").option("value", null);
    if ($("#btnAddNewCoordonneeDirigeantMoralePerson" + orderPersonneMorale + '-' + orderCoordonnee)) $("#btnAddNewCoordonneeDirigeantMoralePerson" + orderPersonneMorale + '-' + orderCoordonnee).removeClass("dx-hidden");
}
function removeCoordonneeDirigeantBeMoralePerson(orderPersonneMorale, orderCoordonnee) {
    $("#moralPersonBeCoordonnee" + orderPersonneMorale + '-' + orderCoordonnee).hide();
    $("#PersonneMoraleLabDirigeantCoordonneesBeNumeroVoie" + orderPersonneMorale + '-' + orderCoordonnee).val("");
    $("#PersonneMoraleLabDirigeantCoordonneeBeComplementVoieId" + orderPersonneMorale + '-' + orderCoordonnee).dxSelectBox("instance").option("value", null);
    $("#PersonneMoraleLabDirigeantCoordonneeBeTypeVoieId" + orderPersonneMorale + '-' + orderCoordonnee).dxSelectBox("instance").option("value", null);
    $("#PersonneMoraleLabDirigeantCoordonneeBeVoie" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneeBeComplement" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneeBeCodePostal" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneeBeVille" + orderPersonneMorale + '-' + orderCoordonnee).dxTextBox("instance").option("value", "");
    $("#PersonneMoraleLabDirigeantCoordonneeBePaysId" + orderPersonneMorale + '-' + orderCoordonnee).dxSelectBox("instance").option("value", null);
    if ($("#btnAddNewCoordonneeDirigeantBeMoralePerson" + orderPersonneMorale + '-' + orderCoordonnee)) $("#btnAddNewCoordonneeDirigeantBeMoralePerson" + orderPersonneMorale + '-' + orderCoordonnee).removeClass("dx-hidden");
}

function removeCoordonneeActivitePersonnePhysique(orderPersonnePhysique) {
    $("#physicalPersonActivityCoordonnee" + orderPersonnePhysique).hide();
    $("#PersonnePhysiqueLabCoordonneesNumeroVoie" + orderPersonnePhysique).val("");
    $("#PersonnePhysiqueLabCoordonneeComplementVoieId" + orderPersonnePhysique).dxSelectBox("instance").option("value", null);
    $("#PersonnePhysiqueLabCoordonneeTypeVoieId" + orderPersonnePhysique).dxSelectBox("instance").option("value", null);
    $("#PersonnePhysiqueLabCoordonneeVoie" + orderPersonnePhysique).dxTextBox("instance").option("value", "");
    $("#PersonnePhysiqueLabCoordonneeComplement" + orderPersonnePhysique).dxTextBox("instance").option("value", "");
    $("#PersonnePhysiqueLabCoordonneeCodePostal" + orderPersonnePhysique).dxTextBox("instance").option("value", "");
    $("#PersonnePhysiqueLabCoordonneeVille" + orderPersonnePhysique).dxTextBox("instance").option("value", "");
    $("#PersonnePhysiqueLabCoordonneePaysId" + orderPersonnePhysique).dxSelectBox("instance").option("value", null);
    try {
        disableFieldValidators("PersonnePhysiqueLabCoordonneesNumeroVoie" + orderPersonnePhysique);
        disableFieldValidators("PersonnePhysiqueLabCoordonneeVille" + orderPersonnePhysique);
    }
    catch (e) { }


    if ($("#btnAddNewCoordonneeActivite" + orderPersonnePhysique)) {
        $("#btnAddNewCoordonneeActivite" + orderPersonnePhysique).show();
        $("#btnAddNewCoordonneeActivite" + orderPersonnePhysique).removeClass("dx-state-invisible");
    }
}
function IsSoupconNonFinanciers_OnValueChanged(e) {
    if (e.value == true) {
        $("#MontantTotal").dxNumberBox("instance").option('value', 0);
        $("#MontantTotal").dxNumberBox("instance").option("disabled", true);
        $("#IsOperationCoursExecution").dxCheckBox("instance").option("disabled", true);
        $("#IsOperationCoursExecution").dxCheckBox("instance").option("value", false);
        $("#OperationSuspectDeclarationTracfinDiv").addClass("dx-hidden");
        $("#OperationCoursExecutionDeclarationTracfinDiv").addClass("dx-hidden");



    }
    else {
        $("#MontantTotal").dxNumberBox("instance").option("disabled", false);
        $("#IsOperationCoursExecution").dxCheckBox("instance").option("disabled", false);
        $("#OperationSuspectDeclarationTracfinDiv").removeClass("dx-hidden");
        $("#OperationCoursExecutionDeclarationTracfinDiv").removeClass("dx-hidden");
    }
}
function IsOperationCoursExecution_OnValueChanged(e) {
    if (e.value == true) {
        $("#gridOperationEnCoursDeclarationTracfinDiv").removeClass("dx-hidden");
    }
    else {
        $("#gridOperationEnCoursDeclarationTracfinDiv").addClass("dx-hidden");
    }
}
function updateOperationSuspectDeclarationTracfinForm(declarationTracfinOrder) {
    $("#operation-suspect-declaration-tracfin-form-container" + declarationTracfinOrder).html("");
    var data = $("#gridOperationSuspectDeclarationTracfin" + declarationTracfinOrder).dxDataGrid("instance").getDataSource().items();
    for (let i = 0; i < data.length; i++) {
        var cryptedId = (data[i].CryptedId != null) ? data[i].CryptedId : "";
        var typeOperationId = data[i].TypeOperationId;
        var autreTypeOperation = data[i].AutreTypeOperation;
        var emetteurId = data[i].EmetteurId;
        var paysOrigineId = data[i].PaysOrigineId;
        var referenceFinanciereEmissionId = data[i].ReferenceFinanciereEmissionId;
        var beneficiareId = data[i].BeneficiareId;
        var paysArriveeId = data[i].PaysArriveeId;
        var referenceFinanciereReceptionId = data[i].ReferenceFinanciereReceptionId;

        $("#operation-suspect-declaration-tracfin-form-container" + declarationTracfinOrder).append('<input name="DeclarationTracfins[' + declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].CryptedId" value="' + cryptedId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].TypeOperationId" value="' + typeOperationId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].AutreTypeOperation" value="' + autreTypeOperation + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].EmetteurId" value="' + emetteurId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].PaysOrigineId" value="' + paysOrigineId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].TypeOperationId" value="' + typeOperationId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].ReferenceFinanciereEmissionId" value="' + referenceFinanciereEmissionId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].BeneficiareId" value="' + beneficiareId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].PaysArriveeId" value="' + paysArriveeId + '" type="hidden">\
            <input name="DeclarationTracfins['+ declarationTracfinOrder + '].OperationSuspectDeclarationTracfins[' + i + '].ReferenceFinanciereReceptionId" value="' + referenceFinanciereReceptionId + '" type="hidden">')
    }
}


//OperationEnCours
function updateFormOperationEnCours(orderAction) {
    var validationResult = DevExpress.validationEngine.validateGroup(
        _labOperationEnCoursValidationGroup);

    if (!validationResult.isValid) {
        return;
    }

    $("#OperationCoursExecutionDeclarationTracfinDiv").removeClass('required-date');

    var emetteurId = $("#EmetteurIdOperationEnCoursEdit").dxSelectBox("instance").option("value");
    var emetteur = $("#EmetteurIdOperationEnCoursEdit").dxSelectBox("instance").option("text");
    var referenceFinanciereId = $("#ReferenceFinanciereIdOperationEnCoursEdit").dxSelectBox("instance").option("value");
    var referenceFinanciere = $("#ReferenceFinanciereIdOperationEnCoursEdit").dxSelectBox("instance").option("text");

    var typeOperationId = $("#TypeOperationIdOperationEnCoursEdit").dxSelectBox("instance").option("value");

    var typeOperation = $("#TypeOperationIdOperationEnCoursEdit").dxSelectBox("instance").option("text");

    var precisionAutreTypeOperation = "";

    if (typeOperationId == _labIdTypeOperationAutre) {
        var precisionAutreTypeOperation = $("#AutreTypeOperationEnCoursEdit").dxTextBox("instance").option("value");

        var precisionAutreTypeOperation = (precisionAutreTypeOperation == null || !precisionAutreTypeOperation) ?
            "" : precisionAutreTypeOperation.replace(/<[^>]*>?/gm, '');
    }
    var montant = $("#MontantTotalOperationEnCoursEdit").dxNumberBox("instance").option("value") != null ? $("#MontantTotalOperationEnCoursEdit").dxNumberBox("instance").option("value") : "";
    var dateLimiteExecution = $("#DateLimiteExecutionOperationEnCoursEdit").dxDateBox("instance").option("value");
    var formattedDateLimiteExecution = $("#DateLimiteExecutionOperationEnCoursEdit").dxDateBox("instance").option("text");
    var heureLimiteExecution = $("#HeureLimiteExecutionOperationEnCoursEdit").dxTextBox("instance").option("value") != null ? $("#HeureLimiteExecutionOperationEnCoursEdit").dxTextBox("instance").option("value") : "";

    $("#EmetteurIdOperationEnCours" + orderAction).val(emetteurId);

    $("#ReferenceFinanciereIdOperationEnCours" + orderAction).dxTextBox("instance").option("value", referenceFinanciereId);

    $("#TypeOperationIdOperationEnCours" + orderAction).dxSelectBox("instance").option("value", typeOperationId);

    $("#AutreTypeOperationEnCours" + orderAction).dxTextBox("instance").option("value", precisionAutreTypeOperation);

    $("#MontantTotalOperationEnCours" + orderAction).dxNumberBox("instance").option("value", montant);
    $("#HeureLimiteExecutionOperationEnCours" + orderAction).dxTextBox("instance").option("value", heureLimiteExecution);
    $("#DateLimiteExecutionOperationEnCours" + orderAction).dxDateBox("instance").option("value", dateLimiteExecution);

    var libelleTypeOperationEnCours = typeOperation + ' ' + precisionAutreTypeOperation;

    $("#cellEmetteurIdOperationEnCours" + orderAction).html(emetteur);
    $("#cellReferenceFinanciereIdOperationEnCours" + orderAction).html(referenceFinanciere);
    $("#cellTypeOperationIdOperationEnCours" + orderAction).html(libelleTypeOperationEnCours);
    $("#cellMontantTotalOperationEnCours" + orderAction).html(montant);
    $("#cellDateLimiteExecutionOperationEnCours" + orderAction).html(formattedDateLimiteExecution);
    $("#cellHeureLimiteExecutionOperationEnCours" + orderAction).html(heureLimiteExecution);

    if ($("#rowOperationEnCours" + orderAction).hasClass("isNewAction")) {
        $("#operationEnCoursTable").append('<tr id="trOperationEnCours' + orderAction + '">\
        <td id = "cellEmetteurIdOperationEnCours'+ orderAction + '" >' + emetteur + '</td >\
        <td id = "cellReferenceFinanciereIdOperationEnCours'+ orderAction + '" >' + referenceFinanciere + '</td >\
        <td id = "cellTypeOperationIdOperationEnCours'+ orderAction + '" >' + libelleTypeOperationEnCours + '</td>\
        <td id = "cellMontantTotalOperationEnCours'+ orderAction + '" >' + montant + '</td>\
        <td id = "cellDateLimiteExecutionOperationEnCours'+ orderAction + '" >' + formattedDateLimiteExecution + '</td>\
        <td id = "cellHeureLimiteExecutionOperationEnCours'+ orderAction + '" >' + heureLimiteExecution + '</td>\
        <td>\
            <div id="btnEditOperationEnCoursForm'+ orderAction + '"></div>\
        </td>\
        <td>\
            <div id="btnDeleteOperationEnCoursForm'+ orderAction + '"></div>\
        </td>\
        </tr>');

        $("#DateLimiteExecutionOperationEnCours" + orderAction).dxDateBox("instance").option("value", $("#DateLimiteExecutionOperationEnCoursEdit").dxDateBox("instance").option("value"));
        $("#btnEditOperationEnCoursForm" + orderAction).dxButton({
            type: "normal",
            icon: "edit",
            elementAttr:
            {
                class: "color-blue"
            },
            onClick: function () {
                showFormOperationEnCours(orderAction);
            }
        });
        $("#btnDeleteOperationEnCoursForm" + orderAction).dxButton({
            type: "normal",
            icon: "trash",
            elementAttr:
            {
                class: "color-red"
            },
            onClick: function () {
                showFormConfirmationDeleteOperationEnCours(orderAction);
            }
        });

        $("#rowOperationEnCours" + orderAction).removeClass("isNewAction");
        $("#operationEnCoursToDelete" + orderAction).remove();

    }

    closeFormOperationEnCours();
}
function closeFormOperationEnCours() {
    $('#modalOperationEnCours').dxPopup("instance").hide().done(function (e) {

        $("#EmetteurIdOperationEnCoursEdit").dxSelectBox("instance").option("value", "");
        $("#ReferenceFinanciereIdOperationEnCoursEdit").dxSelectBox("instance").option("value", "");
        $("#TypeOperationIdOperationEnCoursEdit").dxSelectBox("instance").option("value", "");
        $("#AutreTypeOperationEnCoursEdit").dxTextBox("instance").reset();
        $("#MontantTotalOperationEnCoursEdit").dxNumberBox("instance").option("value", "");
        $("#DateLimiteExecutionOperationEnCoursEdit").dxDateBox("instance").option("value", "");
        $("#HeureLimiteExecutionOperationEnCoursEdit").dxTextBox("instance").option("value", "");

        $('#modalOperationEnCours').dxPopup("instance").option("onHiding", null);
    });
}
function showFormOperationEnCours(orderAction) {
    if (orderAction !== null) {
        var popup = $("#modalOperationEnCours").dxPopup("instance");
        var isNewAction = $("#rowOperationEnCours" + orderAction).hasClass("isNewAction");

        popup.show().done(function () {
            setOperationEnCoursPopupValidators();

            populateTiersOperationEnCoursSelectBox();

            $("#EmetteurIdOperationEnCoursEdit").dxSelectBox("instance").option("value", $("#EmetteurIdOperationEnCours" + orderAction).val());
            $("#ReferenceFinanciereIdOperationEnCoursEdit").dxSelectBox("instance").option("value", $("#ReferenceFinanciereIdOperationEnCours" + orderAction).dxTextBox("instance").option("value"));
            $("#TypeOperationIdOperationEnCoursEdit").dxSelectBox("instance").option("value", $("#TypeOperationIdOperationEnCours" + orderAction).dxSelectBox("instance").option("value"));

            $("#AutreTypeOperationEnCoursEdit").dxTextBox("instance").option("value",
                $("#AutreTypeOperationEnCours" + orderAction).dxTextBox("instance").option("value"));

            $("#MontantTotalOperationEnCoursEdit").dxNumberBox("instance").option("value", $("#MontantTotalOperationEnCours" + orderAction).dxNumberBox("instance").option("value"));
            $("#DateLimiteExecutionOperationEnCoursEdit").dxDateBox("instance").option("value", $("#DateLimiteExecutionOperationEnCours" + orderAction).dxDateBox("instance").option("value"));
            $("#HeureLimiteExecutionOperationEnCoursEdit").dxTextBox("instance").option("value", $("#HeureLimiteExecutionOperationEnCours" + orderAction).dxTextBox("instance").option("value"));
            var isView = $("#EmetteurIdOperationEnCoursEdit").dxSelectBox("instance").option("disabled") == true;
            var titleTemplate = isView ? _labTranslatableMessageViewOperation : (isNewAction ? _labTranslatableMessageAddOperation : _labTranslatableMessageEditOperation);
            popup.option('titleTemplate', '<div class="d-flex flex-row align-items-center justify-content-between">\
            <div class= "font-size-16px smallCaps text-green"> <i class="fas fa-plus mr-2"></i>'+ titleTemplate + '</div>\
                                </div>');

            $("#btnCloseOperationEnCoursForm").dxButton("instance").option("onClick", function () {
                if (isNewAction) {
                    popup.option('onHiding', function () {
                        $('#rowOperationEnCours' + orderAction).remove();
                    });
                }
                closeFormOperationEnCours();
            });

            if ($("#btnValidateOperationEnCoursForm").dxButton("instance") != undefined) {
                $("#btnValidateOperationEnCoursForm").dxButton("instance").option("onClick", function () { updateFormOperationEnCours(orderAction); });
            }
        });
    }
    else {
        showLoadPanel(_commonTranslatableMessageLoading);
        var countActions = $('.operationEnCoursRow').length;
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            url: '/' + culture + '/Lab/ServiceLab/AddOperationEnCoursForm?Order=' + countActions,
        }).done(function (data) {
            $("#actionsContainerOperationEnCours").append(data);
            $("#rowOperationEnCours" + countActions).append('<input id="operationEnCoursToDelete' + countActions + '" type="hidden" name="DeclarationTracfins[0].OperationEnCoursDeclarationTracfins[' + countActions + '].ToDelete" value="true">');
            showFormOperationEnCours(countActions);
            hideLoadPanel();
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
        });
    }
}
function setOperationEnCoursPopupValidators() {
    $("#DateLimiteExecutionOperationEnCoursEdit").dxValidator(
        {
            validationGroup: _labOperationEnCoursValidationGroup,
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageDateOperationObligatoire,
            }]
        });
    $("#MontantTotalOperationEnCoursEdit").dxValidator(
        {
            validationGroup: _labOperationEnCoursValidationGroup,
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageMontantOperationObligatoire
            },
            {
                type: 'range',
                min: 0,
                message: _labTranslatableMessageMontantOperationMinValue.format(0)
            }]
        });

    DevExpress.validationEngine.resetGroup(_labOperationEnCoursValidationGroup);
}
function showFormConfirmationDeleteOperationEnCours(id) {
    var confirmDeleteActionDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _commonTranslatableMessageDeleteActionConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });
    confirmDeleteActionDialog.show().done(function (dialogResult) {
        if (dialogResult) {
            $("#rowOperationEnCours" + id).html('<input type="hidden" name="DeclarationTracfins[0].OperationEnCoursDeclarationTracfins[' + id + '].ToDelete" value="true">');
            $("#trOperationEnCours" + id).addClass("dx-hidden");
        }
    });
}
//OperationSuspect
function PersonnesSelectedItemList_OnInitialized(e) {
    var cryptedDossierId = $("#CryptedId").val();
    $.ajax({
        method: "GET",
        dataType: "json",
        cache: false,
        url: '/Lab/ServiceLab/GetPersonnesSelectedItemList?cryptedDossierId=' + cryptedDossierId
    }).done(function (data) {

        if (data) {
            e.component.option("dataSource", data);
        }
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
    });
}
function updateFormOperationSuspect(orderAction) {
    var validationResult = DevExpress.validationEngine.validateGroup("OperationSuspectPopupValidationGroup");
    if (!validationResult.isValid) {
        return;
    }

    var selectedTypeOperation = $("#TypeOperationIdOperationSuspectEdit").dxSelectBox("instance").option("selectedItem");
    var typeOperationId;
    var typeOperation = "";
    var typeOperationCode;
    if (selectedTypeOperation) {
        typeOperationId = selectedTypeOperation.Id;
        typeOperation = culture == "fr" ? selectedTypeOperation.NameFr : selectedTypeOperation.NameEn;
        typeOperationCode = selectedTypeOperation.Code;
    }

    var emetteurId;
    var emetteur = "";
    var referenceFinanciereEmiId;
    var referenceFinanciereEmi = "";
    var paysOrigineId;
    var paysOrigine = "";

    if (typeOperationCode != _labCodeOperationEspecesVersement) {
        var selectBoxEmetteurs = $("#EmetteurIdOperationSuspectEdit").dxSelectBox("instance");
        emetteurId = selectBoxEmetteurs.option("value");
        emetteur = emetteurId == _labAutreTier.value ?
            $("#AutreEmetteurOperationSuspecteEdit").dxTextBox("instance").option("value").trim() :
            selectBoxEmetteurs.option("text");

        var selectBoxRefEmission = $("#ReferenceFinanciereEmissionIdOperationSuspectEdit").dxSelectBox("instance");
        var referenceFinanciereEmiId = selectBoxRefEmission.option("value");
        var referenceFinanciereEmi = referenceFinanciereEmiId == _labAutreReferenceFinanciere.value ?
            $("#AutreRefFinanciereEmissionOperationSuspecteEdit").dxTextBox("instance").option("value").trim() :
            selectBoxRefEmission.option("text");

        paysOrigineId = $("#PaysOrigineIdOperationSuspectEdit").dxSelectBox("instance").option("value");
        paysOrigine = $("#PaysOrigineIdOperationSuspectEdit").dxSelectBox("instance").option("text");
    }

    var beneficiareId;
    var beneficiare = "";
    var referenceFinanciereRecId;
    var referenceFinanciereRec = "";
    var paysArriveeId;
    var paysArrivee = "";

    if (typeOperationCode != _labCodeOperationEspecesRetrait) {
        var selectBoxBeneficiaires = $("#BeneficiareIdOperationSuspectEdit").dxSelectBox("instance");
        beneficiareId = selectBoxBeneficiaires.option("value");
        beneficiare = beneficiareId == _labAutreTier.value ?
            $("#AutreBeneficiaireOperationSuspecteEdit").dxTextBox("instance").option("value").trim() :
            selectBoxBeneficiaires.option("text");

        var selectBoxRefReception = $("#ReferenceFinanciereReceptionIdOperationSuspectEdit").dxSelectBox("instance");
        referenceFinanciereRecId = selectBoxRefReception.option("value");
        referenceFinanciereRec = referenceFinanciereRecId == _labAutreReferenceFinanciere.value ?
            $("#AutreRefFinanciereReceptionOperationSuspecteEdit").dxTextBox("instance").option("value").trim() :
            selectBoxRefReception.option("text");

        paysArriveeId = $("#PaysArriveeIdOperationSuspectEdit").dxSelectBox("instance").option("value");
        paysArrivee = $("#PaysArriveeIdOperationSuspectEdit").dxSelectBox("instance").option("text");
    }

    var montantTotal = $("#MontantTotalOperationSuspectEdit").dxNumberBox("instance").option("value");

    var nombreOperation = $("#NombreOperationOperationSuspectEdit").dxNumberBox("instance").option("value");

    if (isNullOrUndefined(nombreOperation)) {
        nombreOperation = "";
    }

    var nomAgent = "";
    if ($("#NomAgentOperationSuspectEdit").dxTextBox("instance") !== undefined) {
        nomAgent = $("#NomAgentOperationSuspectEdit").dxTextBox("instance").option("value") === null ? "" : $("#NomAgentOperationSuspectEdit").dxTextBox("instance").option("value").replace(/<[^>]*>?/gm, '');
    }
    var sirenAgent = "";
    if ($("#SirenAgentOperationSuspectEdit").dxTextBox("instance") !== undefined) {
        sirenAgent = $("#SirenAgentOperationSuspectEdit").dxTextBox("instance").option("value") === null ? "" : $("#SirenAgentOperationSuspectEdit").dxTextBox("instance").option("value").replace(/<[^>]*>?/gm, '');
    }
    var autreTypeOperation = $("#AutreTypeOperationOperationSuspectEdit").dxTextBox("instance").option("value") === null ? "" : $("#AutreTypeOperationOperationSuspectEdit").dxTextBox("instance").option("value").replace(/<[^>]*>?/gm, '');

    $("#EmetteurIdOperationSuspect" + orderAction).val(emetteurId);

    if (emetteurId == _labAutreTier.value) {
        $("#AutreEmetteurOperationSuspecte" + orderAction).dxTextBox("instance").option("value", emetteur);
    }

    $("#ReferenceFinanciereEmissionIdOperationSuspect" + orderAction).dxTextBox("instance").option("value", referenceFinanciereEmiId);
    if (referenceFinanciereEmiId == _labAutreReferenceFinanciere.value) {
        $("#AutreRefFinanciereEmissionOperationSuspecte" + orderAction).dxTextBox("instance").option("value", referenceFinanciereEmi);
    }

    $("#TypeOperationIdOperationSuspect" + orderAction).dxSelectBox("instance").option("value", typeOperationId);

    $("#BeneficiareIdOperationSuspect" + orderAction).val(beneficiareId);

    if (beneficiareId == _labAutreTier.value) {
        $("#AutreBeneficiaireOperationSuspecte" + orderAction).dxTextBox("instance").option("value", beneficiare);
    }

    $("#ReferenceFinanciereReceptionIdOperationSuspect" + orderAction).dxTextBox("instance").option("value", referenceFinanciereRecId);
    if (referenceFinanciereRecId == _labAutreReferenceFinanciere.value) {
        $("#AutreRefFinanciereReceptionOperationSuspecte" + orderAction).dxTextBox("instance").option("value", referenceFinanciereRec);
    }

    $("#PaysOrigineIdOperationSuspect" + orderAction).dxSelectBox("instance").option("value", paysOrigineId);
    $("#PaysArriveeIdOperationSuspect" + orderAction).dxSelectBox("instance").option("value", paysArriveeId);
    $("#MontantTotalOperationSuspect" + orderAction).dxNumberBox("instance").option("value", montantTotal);
    $("#NombreOperationOperationSuspect" + orderAction).dxNumberBox("instance").option("value", nombreOperation);

    if ($("#NomAgentOperationSuspect" + orderAction).dxTextBox("instance") !== undefined) {
        $("#NomAgentOperationSuspect" + orderAction).dxTextBox("instance").option("value", nomAgent);
    }

    if ($("#SirenAgentOperationSuspect" + orderAction).dxTextBox("instance") !== undefined) {
        $("#SirenAgentOperationSuspect" + orderAction).dxTextBox("instance").option("value", sirenAgent);
    }
    $("#AutreTypeOperationOperationSuspect" + orderAction).dxTextBox("instance").option("value", autreTypeOperation);

    $("#cellEmetteurIdOperationSuspect" + orderAction).html(emetteur);
    $("#cellReferenceFinanciereEmissionIdOperationSuspect" + orderAction).html(referenceFinanciereEmi);
    $("#cellPaysOrigineIdOperationSuspect" + orderAction).html(paysOrigine);
    $("#cellTypeOperationIdOperationSuspect" + orderAction).html(typeOperation + ' ' + autreTypeOperation);
    $("#cellBeneficiareIdOperationSuspect" + orderAction).html(beneficiare);
    $("#cellReferenceFinanciereReceptionIdOperationSuspect" + orderAction).html(referenceFinanciereRec);
    $("#cellPaysArriveeIdOperationSuspect" + orderAction).html(paysArrivee);
    $("#cellMontantTotalOperationSuspect" + orderAction).html(montantTotal);
    $("#cellNombreOperationOperationSuspect" + orderAction).html(nombreOperation);

    if ($("#cellNomAgentOperationSuspect" + orderAction)) {
        $("#cellNomAgentOperationSuspect" + orderAction).html(nomAgent);
    }
    if ($("#cellSirenAgentOperationSuspect" + orderAction)) {
        $("#cellSirenAgentOperationSuspect" + orderAction).html(sirenAgent);
    }

    if ($("#rowOperationSuspect" + orderAction).hasClass("isNewAction")) {

        if ($("#SirenAgentOperationSuspect" + orderAction).dxTextBox("instance") !== undefined) {
            $("#operationSuspectTable").append('<tr id="trOperationSuspect' + orderAction + '">\
            <td id = "cellTypeOperationIdOperationSuspect'+ orderAction + '" >' + typeOperation + ' ' + autreTypeOperation + '</td>\
            <td id = "cellEmetteurIdOperationSuspect'+ orderAction + '" >' + emetteur + '</td >\
            <td id = "cellPaysOrigineIdOperationSuspect'+ orderAction + '" >' + paysOrigine + '</td>\
            <td id = "cellReferenceFinanciereEmissionIdOperationSuspect'+ orderAction + '" >' + referenceFinanciereEmi + '</td >\
            <td id = "cellBeneficiareIdOperationSuspect'+ orderAction + '" >' + beneficiare + '</td>\
            <td id = "cellPaysArriveeIdOperationSuspect'+ orderAction + '" >' + paysArrivee + '</td>\
            <td id = "cellReferenceFinanciereReceptionIdOperationSuspect'+ orderAction + '" >' + referenceFinanciereRec + '</td>\
            <td id = "cellMontantTotalOperationSuspect'+ orderAction + '" >' + montantTotal + '</td>\
            <td id = "cellNombreOperationOperationSuspect'+ orderAction + '" >' + nombreOperation + '</td>\
            <td id = "cellNomAgentOperationSuspect'+ orderAction + '" >' + nomAgent + '</td>\
            <td id = "cellSirenAgentOperationSuspect'+ orderAction + '" >' + sirenAgent + '</td>\
            <td>\
                <div id="btnEditOperationSuspectForm'+ orderAction + '"></div>\
            </td>\
            <td>\
                <div id="btnDeleteOperationSuspectForm'+ orderAction + '"></div>\
            </td>\
            </tr>');
        }
        else {
            $("#operationSuspectTable").append('<tr id="trOperationSuspect' + orderAction + '">\
            <td id ="cellTypeOperationIdOperationSuspect'+ orderAction + '" >' + typeOperation + ' ' + autreTypeOperation + '</td>\
            <td id="cellEmetteurIdOperationSuspect'+ orderAction + '" >' + emetteur + '</td >\
            <td id="cellPaysOrigineIdOperationSuspect'+ orderAction + '" >' + paysOrigine + '</td>\
            <td id="cellReferenceFinanciereEmissionIdOperationSuspect'+ orderAction + '" >' + referenceFinanciereEmi + '</td >\
            <td id="cellBeneficiareIdOperationSuspect'+ orderAction + '" >' + beneficiare + '</td>\
            <td id="cellPaysArriveeIdOperationSuspect'+ orderAction + '" >' + paysArrivee + '</td>\
            <td id="cellReferenceFinanciereReceptionIdOperationSuspect'+ orderAction + '" >' + referenceFinanciereRec + '</td>\
            <td id="cellMontantTotalOperationSuspect'+ orderAction + '" >' + montantTotal + '</td>\
            <td id="cellNombreOperationOperationSuspect'+ orderAction + '" >' + nombreOperation + '</td>\
            <td>\
                <div id="btnEditOperationSuspectForm'+ orderAction + '"></div>\
            </td>\
            <td>\
                <div id="btnDeleteOperationSuspectForm'+ orderAction + '"></div>\
            </td>\
            </tr > ');
        }


        $("#btnEditOperationSuspectForm" + orderAction).dxButton({
            type: "normal",
            icon: "edit",
            elementAttr:
            {
                class: "color-blue"
            },
            onClick: function () {
                showFormOperationSuspect(orderAction, null);
            }
        });
        $("#btnDeleteOperationSuspectForm" + orderAction).dxButton({
            type: "normal",
            icon: "trash",
            elementAttr:
            {
                class: "color-red"
            },
            onClick: function () {
                showFormConfirmationDeleteOperationSuspect(orderAction);
            }
        });

        $("#rowOperationSuspect" + orderAction).removeClass("isNewAction");
        $("#operationSuspectToDelete" + orderAction).remove();

    }

    closeFormOperationSuspect();
}
function closeFormOperationSuspect() {
    $('#modalOperationSuspect').dxPopup("instance").hide().done(function (e) {

        $("#EmetteurIdOperationSuspectEdit").dxSelectBox("instance").option("value", "");
        $("#ReferenceFinanciereEmissionIdOperationSuspectEdit").dxSelectBox("instance").option("value", "");
        $("#TypeOperationIdOperationSuspectEdit").dxSelectBox("instance").option("value", "");
        $("#PaysOrigineIdOperationSuspectEdit").dxSelectBox("instance").option("value", "");
        $("#BeneficiareIdOperationSuspectEdit").dxSelectBox("instance").option("value", "");
        $("#ReferenceFinanciereReceptionIdOperationSuspectEdit").dxSelectBox("instance").option("value", "");
        $("#PaysArriveeIdOperationSuspectEdit").dxSelectBox("instance").option("value", "");
        $("#MontantTotalOperationSuspectEdit").dxNumberBox("instance").option("value", "");
        $("#NombreOperationOperationSuspectEdit").dxNumberBox("instance").option("value", "");
        if ($("#NomAgentOperationSuspectEdit").dxTextBox("instance") !== undefined) {
            $("#NomAgentOperationSuspectEdit").dxTextBox("instance").option("value", "");
        }
        if ($("#SirenAgentOperationSuspectEdit").dxTextBox("instance") !== undefined) {
            $("#SirenAgentOperationSuspectEdit").dxTextBox("instance").option("value", "");
        }
        if ($("#AutreTypeOperationOperationSuspectEdit").dxTextBox("instance") !== undefined) {
            $("#AutreTypeOperationOperationSuspectEdit").dxTextBox("instance").option("value", "");
        }
        $('#modalOperationSuspect').dxPopup("instance").option("onHiding", null);
    });
}
function showFormOperationSuspect(orderAction, professionId) {
    if (orderAction !== null) {
        var popup = $("#modalOperationSuspect").dxPopup("instance");
        var isNewAction = $("#rowOperationSuspect" + orderAction).hasClass("isNewAction");

        popup.show().done(function () {

            populateTiersOperationSuspectesSelectBoxes();
            setOperationSuspectPopupValidators();

            $("#EmetteurIdOperationSuspectEdit").dxSelectBox("instance").option("value", $("#EmetteurIdOperationSuspect" + orderAction).val());
            $("#AutreEmetteurOperationSuspecteEdit").dxTextBox("instance").option("value", $("#AutreEmetteurOperationSuspecte" + orderAction).dxTextBox("instance").option("value"));

            $("#ReferenceFinanciereEmissionIdOperationSuspectEdit").dxSelectBox("instance").option("value", $("#ReferenceFinanciereEmissionIdOperationSuspect" + orderAction).dxTextBox("instance").option("value"));
            $("#AutreRefFinanciereEmissionOperationSuspecteEdit").dxTextBox("instance").option("value", $("#AutreRefFinanciereEmissionOperationSuspecte" + orderAction).dxTextBox("instance").option("value"));

            $("#TypeOperationIdOperationSuspectEdit").dxSelectBox("instance").option("value", $("#TypeOperationIdOperationSuspect" + orderAction).dxSelectBox("instance").option("value"));
            $("#PaysOrigineIdOperationSuspectEdit").dxSelectBox("instance").option("value", $("#PaysOrigineIdOperationSuspect" + orderAction).dxSelectBox("instance").option("value"));

            $("#BeneficiareIdOperationSuspectEdit").dxSelectBox("instance").option("value", $("#BeneficiareIdOperationSuspect" + orderAction).val());
            $("#AutreBeneficiaireOperationSuspecteEdit").dxTextBox("instance").option("value", $("#AutreBeneficiaireOperationSuspecte" + orderAction).dxTextBox("instance").option("value"));

            $("#ReferenceFinanciereReceptionIdOperationSuspectEdit").dxSelectBox("instance").option("value", $("#ReferenceFinanciereReceptionIdOperationSuspect" + orderAction).dxTextBox("instance").option("value"));
            $("#AutreRefFinanciereReceptionOperationSuspecteEdit").dxTextBox("instance").option("value", $("#AutreRefFinanciereReceptionOperationSuspecte" + orderAction).dxTextBox("instance").option("value"));

            $("#PaysArriveeIdOperationSuspectEdit").dxSelectBox("instance").option("value", $("#PaysArriveeIdOperationSuspect" + orderAction).dxSelectBox("instance").option("value"));
            $("#MontantTotalOperationSuspectEdit").dxNumberBox("instance").option("value", $("#MontantTotalOperationSuspect" + orderAction).dxNumberBox("instance").option("value"));
            $("#NombreOperationOperationSuspectEdit").dxNumberBox("instance").option("value", $("#NombreOperationOperationSuspect" + orderAction).dxNumberBox("instance").option("value"));

            if ($("#NomAgentOperationSuspectEdit").dxTextBox("instance") != undefined) {
                $("#NomAgentOperationSuspectEdit").dxTextBox("instance").option("value", $("#NomAgentOperationSuspect" + orderAction).dxTextBox("instance").option("value"));
            }

            if ($("#SirenAgentOperationSuspectEdit").dxTextBox("instance") != undefined) {
                $("#SirenAgentOperationSuspectEdit").dxTextBox("instance").option("value", $("#SirenAgentOperationSuspect" + orderAction).dxTextBox("instance").option("value"));
            }

            if ($("#AutreTypeOperationOperationSuspectEdit").dxTextBox("instance") != undefined) {
                $("#AutreTypeOperationOperationSuspectEdit").dxTextBox("instance").option("value", $("#AutreTypeOperationOperationSuspect" + orderAction).dxTextBox("instance").option("value"));
            }

            var isView = $("#EmetteurIdOperationSuspectEdit").dxSelectBox("instance").option("disabled") == true;
            var titleTemplate = isView ? _labTranslatableMessageViewOperation : (isNewAction ? _labTranslatableMessageAddOperation : _labTranslatableMessageEditOperation);
            popup.option('titleTemplate', '<div class="d-flex flex-row align-items-center justify-content-between">\
            <div class= "font-size-16px smallCaps text-green"> <i class="fas fa-plus mr-2"></i>'+ titleTemplate + '</div>\
                                </div>');

            $("#btnCloseOperationSuspectForm").dxButton("instance").option("onClick", function () {
                if (isNewAction) {
                    popup.option('onHiding', function () {
                        $('#rowOperationSuspect' + orderAction).remove();
                    });
                }
                closeFormOperationSuspect();
            });

            if ($("#btnValidateOperationSuspectForm").dxButton("instance") != undefined) {
                $("#btnValidateOperationSuspectForm").dxButton("instance").option("onClick", function () { updateFormOperationSuspect(orderAction); });
            }
        });
    }
    else {
        showLoadPanel(_commonTranslatableMessageLoading);
        var countActions = $('.operationSuspectRow').length;
        $.ajax({
            method: "GET",
            cache: false,
            dataType: "html",
            url: '/' + culture + '/Lab/ServiceLab/AddOperationSuspectForm?Order=' + countActions + "&ProfessionId=" + professionId,
        }).done(function (data) {
            $("#actionsContainerOperationSuspect").append(data);
            $("#rowOperationSuspect" + countActions).append('<input id="operationSuspectToDelete' + countActions + '" type="hidden" name="DeclarationTracfins[0].OperationSuspectDeclarationTracfins[' + countActions + '].ToDelete" value="true">');
            showFormOperationSuspect(countActions, null);
            hideLoadPanel();
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        }).always(function (e) {
        });
    }
}
function resetTiersReferencesFinancieres() {
    var divs = ["#divAutreEmetteur", "#divAutreBeneficiaire", "#divAutreRefFinanciereEmission", "#divAutreRefFinanciereReception"];
    divs.forEach(function (div) {
        toggleContainerIncludingValidators(div, false, "OperationSuspectPopupValidationGroup");
    });
}
function populateTiersSelectBoxesImmobilier(orderAction) {
    var tiers = getTiersOperationsImmobilieres();
    $("#partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee" + orderAction).dxSelectBox("instance").option("dataSource", tiers);
    $("#partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee" + orderAction).dxSelectBox("instance").option("dataSource", tiers);
}
function populateTiersOperationSuspectesSelectBoxes() {
    var tiers = getTiersOperationSuspecte();
    $("#EmetteurIdOperationSuspectEdit").dxSelectBox("instance").option("dataSource", tiers);
    $("#BeneficiareIdOperationSuspectEdit").dxSelectBox("instance").option("dataSource", tiers);
}
function populateTiersOperationEnCoursSelectBox() {

    var tiers = getTiersOperationEnCours();

    $("#EmetteurIdOperationEnCoursEdit")
        .dxSelectBox("instance")
        .option("dataSource", tiers);
}
function setOperationSuspectPopupValidators() {

    var initialVisibleFieldsContainersSelectors = ["#divSelectEmetteur", "#divSelectBeneficiaire", "#divSelectRefFinanciereEmission", "#divSelectRefFinanciereReception", "#divPaysFonds", "#divMontantTotalNombreOperations"];
    initialVisibleFieldsContainersSelectors.forEach(function (item, i) {
        toggleContainerIncludingValidators(item, true, "OperationSuspectPopupValidationGroup");
    });
    var initialHiddenFieldsContainersSelectors = ["#divAutreTypeOperation", "#divAutreEmetteur", "#divAutreBeneficiaire", "#divAutreRefFinanciereEmission", "#divAutreRefFinanciereReception"];
    initialHiddenFieldsContainersSelectors.forEach(function (item, i) {
        toggleContainerIncludingValidators(item, false, "OperationSuspectPopupValidationGroup");
    });
    DevExpress.validationEngine.resetGroup("OperationSuspectPopupValidationGroup");
}

//////US 1003: Evolution des champs tiers; remettre l'onglet représentant légaux.
//function showRepresentantsLegaux(i) {
//    if (_IsNewDeclarationTracfin == true)
//        $("#nav-representantslegaux-pm-tab" + i).addClass("dx-hidden");
//    else
//        $("#nav-representantslegaux-pm-tab" + i).removeClass("dx-hidden");
//}
function showFormConfirmationDeleteOperationSuspect(id) {
    var confirmDeleteActionDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _commonTranslatableMessageDeleteActionConfirmation,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });
    confirmDeleteActionDialog.show().done(function (dialogResult) {
        if (dialogResult) {
            $("#rowOperationSuspect" + id).html('<input type="hidden" name="DeclarationTracfins[0].OperationSuspectDeclarationTracfins[' + id + '].ToDelete" value="true">');
            $("#trOperationSuspect" + id).addClass("dx-hidden");
        }
    });
}
function EmetteurIdOperationEnCoursEdit_OnSelectionChanged(e) {
    var selectBoxReferencesFinancieres = $("#ReferenceFinanciereIdOperationEnCoursEdit").dxSelectBox("instance");

    refreshReferencesFinancieresOperationEnCours(
        e.selectedItem,
        selectBoxReferencesFinancieres);
}

function refreshReferencesFinancieresOperationEnCours(selectedTier, selectBoxReferencesFinancieres) {
    refreshReferencesFinancieresWithoutAddingItemAutre(selectedTier, selectBoxReferencesFinancieres);
}
function refreshReferencesFinancieresWithoutAddingItemAutre(selectedTier, selectBoxReferencesFinancieres) {
    var referencesFinancieres = (selectedTier != null && selectedTier) ?
        getReferencesFinancieres(selectedTier.value) :
        [];

    selectBoxReferencesFinancieres.option("dataSource", referencesFinancieres);

    if (selectedTier == null || !selectedTier) {
        selectBoxReferencesFinancieres.option("value", undefined);
    }
}
function emetteurOperationSuspectEdit_OnSelectionChanged(e) {
    var selectBoxReferencesFinancieres = $("#ReferenceFinanciereEmissionIdOperationSuspectEdit").dxSelectBox("instance");
    var selectedTier = e.selectedItem;
    refreshReferencesFinancieres(selectedTier, selectBoxReferencesFinancieres);
    toggleContainerIncludingValidators("#divAutreEmetteur",
        selectedTier && selectedTier != null && selectedTier.value == _labAutreTier.value,
        "OperationSuspectPopupValidationGroup");
}
function beneficiaireOperationSuspectEdit_OnSelectionChanged(e) {
    var selectBoxReferencesFinancieres = $("#ReferenceFinanciereReceptionIdOperationSuspectEdit").dxSelectBox("instance");
    var selectedTier = e.selectedItem;
    refreshReferencesFinancieres(selectedTier, selectBoxReferencesFinancieres);
    toggleContainerIncludingValidators("#divAutreBeneficiaire",
        selectedTier && selectedTier != null && selectedTier.value == _labAutreTier.value,
        "OperationSuspectPopupValidationGroup");
}
function emetteurOperationImmobilierEdit_OnSelectionChanged(order) {
    onSelectedTierChange("partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee",
        order,
        "partialOperationsCompaniesImmobiliers_ReferenceFinanciereEmettriceConcernee",
        "divSelectRefFinanciereEmission",
        "divAutreEmetteurOperationImmobiliere",
        "partialOperationsCompanieImmobilier_TypeEmettrice",
        "PpTypeEmettrice01Div",
        "PmTypeEmettrice01Div"
    );
}
function onSelectedTierChange(tierSelectBoxIdPrefix,
    order,
    refFinancieresSelectBoxIdPrefix,
    selectRefFinancieresDivIdPrefix,
    autreTierDivIdPrefix,
    typeAutreTierSelectBoxIdPrefix,
    personnePhysiqueDivIdPrefix,
    personneMoraleDivIdPrefix) {
    var selectedTier = $("#" + tierSelectBoxIdPrefix + order)
        .dxSelectBox("instance")
        .option("selectedItem");

    $("#" + tierSelectBoxIdPrefix + "_hidden_" + order).val(
        selectedTier != null && selectedTier ? selectedTier.text : "");

    var referencesFinancieresSelectBoxSelector = "#" + refFinancieresSelectBoxIdPrefix + order;

    var typesAutreTierSelectBoxSelector = "#" + typeAutreTierSelectBoxIdPrefix + order;

    if ($(referencesFinancieresSelectBoxSelector).dxSelectBox("instance") != undefined &&
        $(referencesFinancieresSelectBoxSelector).dxSelectBox("instance") != null) {

        var showRefFinancieres = selectedTier != null &&
            selectedTier &&
            selectedTier.value != _labAutreTier.value;

        toggleContainerIncludingValidators(
            "#" + selectRefFinancieresDivIdPrefix + order,
            showRefFinancieres);

        if (showRefFinancieres) {
            var referencesFinancieresSelectBox = $(referencesFinancieresSelectBoxSelector)
                .dxSelectBox("instance");

            refreshReferencesFinancieresWithoutAddingItemAutre(selectedTier,
                referencesFinancieresSelectBox);
        }

        var showAutreTier = selectedTier != null &&
            selectedTier &&
            selectedTier.value == _labAutreTier.value;

        toggleContainerIncludingValidators("#" + autreTierDivIdPrefix + order,
            showAutreTier);

        var typeAutreTier = $(typesAutreTierSelectBoxSelector)
            .dxSelectBox("instance")
            .option("value");

        displayAutreTierTransaction(
            showAutreTier,
            typeAutreTier,
            "#" + personnePhysiqueDivIdPrefix + order,
            "#" + personneMoraleDivIdPrefix + order);
    }

    if ($(typesAutreTierSelectBoxSelector).dxSelectBox("instance") != undefined &&
        $(typesAutreTierSelectBoxSelector).dxSelectBox("instance") != null) {
        if (selectedTier != null && selectedTier && selectedTier.value != _labAutreTier.value) {
            $(typesAutreTierSelectBoxSelector).dxSelectBox("instance").reset();
        }
    }
}
function beneficiaireOperationImmobilierEdit_OnSelectionChanged(order) {
    onSelectedTierChange("partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee",
        order,
        "partialOperationsCompaniesImmobiliers_ReferenceFinanciereDestinataireConcernee",
        "divSelectRefFinanciereReception",
        "divAutreBeneficiaireOperationImmobiliere",
        "partialOperationsCompanieImmobilier_TypeBeneficiaire",
        "PpTypeDestinataire01Div",
        "PmTypeDestinataire01Div"
    );
}


function referenceFinanciereEmettriceDataSource() {
    $("#partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee0").dxSelectBox();
    if ($("#partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee0").dxSelectBox("instance") != undefined &&
        $("#partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee0").dxSelectBox("instance") != null) {
        var selectedTier = $("#partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee0").dxSelectBox("instance").option('selectedItem');
        refreshReferencesFinancieres(selectedTier, $("#partialOperationsCompaniesImmobiliers_ReferenceFinanciereEmettriceConcernee0").dxSelectBox("instance"));
    }
}

function referenceFinanciereDestinataireDataSource() {
    $("#partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee0").dxSelectBox();
    if ($("#partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee0").dxSelectBox("instance") != undefined &&
        $("#partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee0").dxSelectBox("instance") != null) {
        var selectedTier = $("#partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee0").dxSelectBox("instance").option('selectedItem');
        refreshReferencesFinancieres(selectedTier, $("#partialOperationsCompaniesImmobiliers_ReferenceFinanciereDestinataireConcernee0").dxSelectBox("instance"));
    }
}

function refreshReferencesFinancieres(selectedTier, selectBoxReferencesFinancieres) {
    var referencesFinancieres = [];
    if (selectedTier && selectedTier.value != _labAutreTier.value) {
        referencesFinancieres = getReferencesFinancieres(selectedTier.value)
    }

    referencesFinancieres.push({
        value: _labAutreReferenceFinanciere.value,
        text: _labAutreReferenceFinanciere.text
    });

    selectBoxReferencesFinancieres.option("dataSource", referencesFinancieres);
    if (!selectedTier || selectedTier == null)
        selectBoxReferencesFinancieres.option("value", undefined);
    else if (selectedTier.value == _labAutreTier.value) {
        selectBoxReferencesFinancieres.option("value", _labAutreReferenceFinanciere.value);
    }
}
function referenceFinanciereEmissionIdOperationSuspectEdit_SelectionChanged(e) {
    toggleContainerIncludingValidators("#divAutreRefFinanciereEmission",
        e.selectedItem && e.selectedItem.value == _labAutreReferenceFinanciere.value,
        "OperationSuspectPopupValidationGroup");

    $("#AutreRefFinanciereEmissionOperationSuspecteEdit").dxTextBox();
    if ($("#AutreRefFinanciereEmissionOperationSuspecteEdit").dxTextBox("instance") != undefined &&
        $("#AutreRefFinanciereEmissionOperationSuspecteEdit").dxTextBox("instance") != null) {
        if (e.selectedItem && e.selectedItem.value != _labAutreReferenceFinanciere.value) {
            $("#AutreRefFinanciereEmissionOperationSuspecteEdit").dxTextBox("instance").reset();
        }
    }
}
function referenceFinanciereReceptionIdOperationSuspectEdit_SelectionChanged(e) {
    toggleContainerIncludingValidators("#divAutreRefFinanciereReception",
        e.selectedItem && e.selectedItem.value == _labAutreReferenceFinanciere.value,
        "OperationSuspectPopupValidationGroup");

    $("#AutreRefFinanciereReceptionOperationSuspecteEdit").dxTextBox();
    if ($("#AutreRefFinanciereReceptionOperationSuspecteEdit").dxTextBox("instance") != undefined &&
        $("#AutreRefFinanciereReceptionOperationSuspecteEdit").dxTextBox("instance") != null) {
        if (e.selectedItem && e.selectedItem.value != _labAutreReferenceFinanciere.value) {
            $("#AutreRefFinanciereReceptionOperationSuspecteEdit").dxTextBox("instance").reset();
        }
    }
}
function BeneficiareIdOperationSuspectEdit_OnValueChanged(e) {
    var cryptedDossierId = $("#CryptedId").val();
    $.ajax({
        method: "GET",
        dataType: "json",
        cache: false,
        url: '/Lab/ServiceLab/GetReferenceFinanciereIdOperationEnCoursEdit?personneId=' + e.value + '&cryptedDossierId=' + cryptedDossierId
    }).done(function (data) {
        $("#ReferenceFinanciereReceptionIdOperationSuspectEdit").dxSelectBox("instance").option("dataSource", data);
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
    }).always(function (e) {
    });
}
function showCorrespondantDeclarant(e) {

    var estCorrespondantDeclarant = $("#EstCorrespondantDeclarant").dxCheckBox("instance").option('value');
    if (estCorrespondantDeclarant) {
        $.ajax({
            method: "GET",
            cache: false,
            dataType: 'json',
            url: '/' + culture + '/Lab/ServiceLab/GetConnectedUser',
        }).done(function (data) {
            if (data != undefined) {
                $("#PrenomCorrespondant").dxTextBox("instance").option("value", data.Prenom);
                $("#NomCorrespondant").dxTextBox("instance").option("value", data.Nom);
                $("#MailCorrespondant").dxTextBox("instance").option("value", data.Email);
                $("#TelephoneCorrespondant").dxTextBox("instance").option("value", data.Telephone);
            }
        }).fail(function (e) {
            if (e.status == 401) {
                var popup = $("#modalReconnect").dxPopup('instance');
                popup.show();
                return;
            }
        })
        $('#detailCorrespondantContainer').slideToggle(200);
        $("#PrenomCorrespondant").dxValidator({
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessagePrenomCorrespondantObligatoire
            }]
        });
        $("#NomCorrespondant").dxValidator({
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageNomCorrespondantObligatoire
            }]
        });
        $("#MailCorrespondant").dxValidator({
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageMailCorrespondantObligatoire
            }]
        });
        $("#TelephoneCorrespondant").dxValidator({
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageTelephoneCorrespondantObligatoire
            }]
        });
    }
    else {
        disableFieldValidators("#PrenomCorrespondant");
        disableFieldValidators("#NomCorrespondant");
        disableFieldValidators("#MailCorrespondant");
        disableFieldValidators("#TelephoneCorrespondant");
        $('#detailCorrespondantContainer').slideToggle(200);

        $("#PrenomCorrespondant").dxTextBox("instance").option("value", null);
        $("#NomCorrespondant").dxTextBox("instance").option("value", null);
        $("#MailCorrespondant").dxTextBox("instance").option("value", null);
        $("#TelephoneCorrespondant").dxTextBox("instance").option("value", null);
        $("#AutreNumeroCorrespondant").dxTextBox("instance").option("value", null);
    }

}

function verfiChampObligatoire(e) {
    $("#PrenomCorrespondant").dxValidator({
        validationRules: [{
            type: 'required',
            message: _labTranslatableMessagePrenomCorrespondantObligatoire
        }]
    });
    $("#NomCorrespondant").dxValidator({
        validationRules: [{
            type: 'required',
            message: _labTranslatableMessageNomCorrespondantObligatoire
        }]
    });
    $("#MailCorrespondant").dxValidator({
        validationRules: [{
            type: 'required',
            message: _labTranslatableMessageMailCorrespondantObligatoire
        }]
    });
    $("#TelephoneCorrespondant").dxValidator({
        validationRules: [{
            type: 'required',
            message: _labTranslatableMessageTelephoneCorrespondantObligatoire
        }]
    });
}

function typeOperationIdOperationSuspect_OnSelectionChanged(e) {
    toggleOperationSuspectePopupFormControls(e.selectedItem != null && e.selectedItem && e.selectedItem.Code);
}
function toggleOperationSuspectePopupFormControls(operationType) {
    var isOperationRetrait = operationType == _labCodeOperationEspecesRetrait;
    toggleBeneficiaireOperationSuspecte(!isOperationRetrait);
    toggleReferenceFinanciereReceptionOperationSuspecte(!isOperationRetrait);
    toggleContainerIncludingValidators("#divSelectPaysArriveeFonds", !isOperationRetrait, _labOperationSuspectesValidationGroup);

    var isOperationVersement = operationType == _labCodeOperationEspecesVersement;
    toggleEmetteurOperationSuspecte(!isOperationVersement);
    toggleReferenceFinanciereEmissionOperationSuspecte(!isOperationVersement);
    toggleContainerIncludingValidators("#divSelectPaysOrigineFonds", !isOperationVersement, _labOperationSuspectesValidationGroup);

    var isOperationAutre = operationType == _labCodeOperationAutre;
    toggleContainerIncludingValidators("#divAutreTypeOperation", isOperationAutre, _labOperationSuspectesValidationGroup);
}

function toggleEmetteurOperationSuspecte(isVisible) {
    toggleSelectBoxHavingFieldAutre(isVisible,
        "OperationSuspectPopupValidationGroup",
        "#divSelectEmetteur",
        $("#EmetteurIdOperationSuspectEdit").dxSelectBox("instance"),
        "#divAutreEmetteur",
        _labAutreTier.value);
}
function toggleBeneficiaireOperationSuspecte(isVisible) {
    toggleSelectBoxHavingFieldAutre(isVisible,
        "OperationSuspectPopupValidationGroup",
        "#divSelectBeneficiaire",
        $("#BeneficiareIdOperationSuspectEdit").dxSelectBox("instance"),
        "#divAutreBeneficiaire",
        _labAutreTier.value);
}
function toggleReferenceFinanciereEmissionOperationSuspecte(isVisible) {
    toggleSelectBoxHavingFieldAutre(isVisible,
        "OperationSuspectPopupValidationGroup",
        "#divSelectRefFinanciereEmission",
        $("#ReferenceFinanciereEmissionIdOperationSuspectEdit").dxSelectBox("instance"),
        "#divAutreRefFinanciereEmission",
        _labAutreReferenceFinanciere.value);
}
function toggleReferenceFinanciereReceptionOperationSuspecte(isVisible) {
    toggleSelectBoxHavingFieldAutre(isVisible,
        "OperationSuspectPopupValidationGroup",
        "#divSelectRefFinanciereReception",
        $("#ReferenceFinanciereReceptionIdOperationSuspectEdit").dxSelectBox("instance"),
        "#divAutreRefFinanciereReception",
        _labAutreReferenceFinanciere.value);
}
function envoiTracfin_onClick() {

    var confirmationEnvoiTracfinDossiersLabErrorNumTelDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatbaleMessageErrorNumTelEnvoieTracfin,
        buttons: [{ text: _labTranslatbaleMessageJaiCompris, onClick: function () { return true; } }]
    });

    $.ajax({
        method: "GET",
        dataType: "json",
        cache: false,
        url: '/' + culture + '/Lab/ServiceLab/HasTelephoneNumber',
    }).done(function (data) {

        if (data.status == true) {
            envoiTracfin_onClickDossiersLabDialog();
        } else {
            confirmationEnvoiTracfinDossiersLabErrorNumTelDialog.show().done();
        }
    }).always();

}

function envoiTracfin_onClickDossiersLabDialog() {

    var confirmationEnvoiTracfinDossiersLabDialog = DevExpress.ui.dialog.custom({
        title: _labTranslatableMessageWarning,
        message: _labTranslatbaleMessageEnvoieTracfin,
        buttons: [{ text: _commonTranslatableMessageYes, onClick: function () { return true; } }, { text: _commonTranslatableMessageNo, onClick: function () { return false; } }]
    });

    confirmationEnvoiTracfinDossiersLabDialog.show().done(function (dialogResult) {
        if (dialogResult) {
            showLoadPanel(_labTranslatableMessageEnCoursEnvoiTracfin);
            var rowSelected = getSelectedRowDossier();
            var cryptedDossierId = rowSelected.CryptedId;

            if (rowSelected) {
                $.ajax({
                    method: "GET",
                    dataType: "json",
                    cache: false,
                    data: { cryptedDossierId },
                    url: '/' + culture + '/Lab/ServiceLab/EnvoieTracfin',
                }).done(function (data) {

                    if (data.status == true) {

                        var dataGrid = null;
                        if ($("#gridDossiersLabWithTiers").length > 0 && isSearchTiers) {
                            dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
                        }

                        if ($("#gridDossiersLabWithNoTiers").length > 0 && !isSearchTiers) {
                            dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
                        }
                        var keys = dataGrid.getSelectedRowKeys();
                        $.each(keys, function (value) {
                            this.Statut = data.statusDossier;
                            this.Id = data.id;
                            dataGrid.refresh();
                        });
                        DevExpress.ui.notify(data.message, "success");
                    } else {
                        DevExpress.ui.notify(data.message, "error");
                    }
                }).always(function (e) { hideLoadPanel(); });
            }
        }

    });



}

function getTiersOperationSuspecte() {
    return getAllTiersWithAutre();
}
function getTiersOperationEnCours() {
    return getTiersIntegresDS();
}
function getTiersOperationsImmobilieres() {
    return getAllTiersWithAutre();
}
function getAllTiersWithAutre() {
    var tiers = getTiersIntegresDS();
    tiers.push({
        text: _labAutreTier.text,
        value: _labAutreTier.value
    });
    return tiers;
}
function getPersonnesIntegreesDs() {
    var personnesPhysiquesIntegreesDs = getPersonnesPhysiquesIntegreesDs();
    var personnesMoralesIntegreesDs = getPersonnesMoralesIntegreesDs();
    var personnes = personnesPhysiquesIntegreesDs.concat(personnesMoralesIntegreesDs);
    return sortTiers(personnes);
}
function getTiersIntegresDS() {
    var personnesPhysiquesIntegreesDs = getPersonnesPhysiquesIntegreesDs();
    var personnesMoralesIntegreesDs = getPersonnesMoralesIntegreesDs();
    var nonConnusIntegresDS = getNonConnusIntegresDs();
    var tiers = personnesPhysiquesIntegreesDs
        .concat(personnesMoralesIntegreesDs)
        .concat(nonConnusIntegresDS);
    return sortTiers(tiers);
}
function sortTiers(tiersItemsArray) {
    return sortItemsByField(tiersItemsArray, function (tier) { return tier.text; });
}
function getPersonnesPhysiquesIntegreesDs() {
    var list = [];
    $(".physicalPerson:visible").each(function () {
        var integreeDs = $(this)
            .find('[id^="PersonnePhysiqueLabIsDeclarationTracfin"]')
            .dxCheckBox("instance")
            .option("value");

        if (!integreeDs)
            return;

        var nom = $(this).find('[id^="PersonnePhysiqueLabNomNaissance"].dx-textbox').dxTextBox("instance").option("value");
        var prenom = $(this).find('[id^="PersonnePhysiqueLabPrenoms"].dx-textbox').dxTextBox("instance").option("value");
        if (nom && nom.trim()) {
            var id = $(this).find('[id^="PersonnePhysiqueLabIdPersonne"]').val();
            var trimmedNom = nom.trim();
            var trimmedPrenom = prenom && prenom.trim();
            list.push({ value: id, text: trimmedPrenom ? trimmedNom + ' ' + trimmedPrenom : trimmedNom });
        }
    });
    return list;
}
function getPersonnesMoralesIntegreesDs() {
    var list = [];
    $(".moralPerson:visible").each(function () {
        var integreeDs = $(this)
            .find('[id^="DossierLabPersonneMoralesIsDeclarationTracfin"]')
            .dxCheckBox("instance")
            .option("value");

        if (!integreeDs)
            return;

        var raisonSociale = $(this).find('[id^="PersonneMoraleLabRaisonSociale"].dx-textbox').dxTextBox("instance").option("value");
        if (raisonSociale && raisonSociale.trim()) {
            var id = $(this).find('[id^="PersonneMoraleLabIdPersonne"]').val();
            list.push({ value: id, text: raisonSociale.trim() });
        }
    });
    return list;
}
function getNonConnusIntegresDs() {
    var list = [];
    $(".nonConnu:visible").each(function () {
        var integreeDs = $(this)
            .find('[id^="DossierLabNonConnusIsDeclarationTracfin"]')
            .dxCheckBox("instance")
            .option("value");

        if (!integreeDs)
            return;

        var elementsIdentification = $(this).find('[id^="DossierLabNonConnuElementsIdentification"]').dxHtmlEditor("instance").option("value");
        if (elementsIdentification) {
            var elementsIdentificationText = $("<div>").html(elementsIdentification).text();
            if (!elementsIdentificationText || !elementsIdentificationText.trim())
                return;
            var id = $(this).find('[id^="NonConnuLabIdPersonne"]').val();
            list.push({ value: id, text: elementsIdentificationText.trim().substring(0, 20) });
        }
    });
    return list;
}
function getReferencesFinancieres(idPersonne) {
    var result = [];
    var personne = $('input[id*="LabIdPersonne"][value="' + idPersonne + '"]').parent();
    var dataGridReferencesFinancieres = personne.find("div[id^='gridSupportFinancier']").dxDataGrid("instance");
    var dataGridReferencesFinancieresItems = dataGridReferencesFinancieres.getDataSource().items();
    $.each(dataGridReferencesFinancieresItems, function (index, item) {
        if (!item.Iban || !item.Iban.trim())
            return;

        var id = item.Id ? item.Id : item.__KEY__;
        var trimmedIban = item.Iban.trim();
        result.push({
            value: id,
            text: trimmedIban
        });
    });
    result = sortItemsByField(result, function (ref) { return ref.text; });
    return result;
}

function limitTagBoxSelectedItemsNumber(tagBoxInstance, max) {
    var selectedItems = tagBoxInstance.option("value");
    if (selectedItems && selectedItems.length > max)
        tagBoxInstance.option("value", selectedItems.slice(0, max));
}
function toggleDivPrecisionAutre(tagBoxInstance, idAutre, divSelector) {
    var selectedItems = tagBoxInstance.option("value");
    var autreSelected = selectedItems != null && selectedItems && selectedItems.includes(idAutre);
    $(divSelector).toggle(autreSelected);
    toggleAllFieldValidators(divSelector, autreSelected);
}
function toggleDivPrecisionAutreSelectBoxItem(selectBoxSelector, autre, divAutreSelector, validationGroup) {
    var autreSelected = $(selectBoxSelector).dxSelectBox("instance").option("value") == autre;
    $(divAutreSelector).toggle(autreSelected);
    toggleAllFieldValidators(divAutreSelector, autreSelected, validationGroup);
}
function refreshAllOperationsAssurancesConcernedPersonsLists() {
    var prefixIdDivOperations = "divPartialOperationsCompanieAssurance";
    var prefixIdSelectBox = "partialOperationsCompanieAssurance_PersonneConcernee";
    var persons = getPersonnesIntegreesDs();
    refreshAllOperationsConcernedPersonsLists(prefixIdDivOperations, prefixIdSelectBox, persons);
}
function refreshAllOperationsImmobilieresConcernedPersonsLists() {
    var prefixIdDivOperations = "divPartialOperationsCompanieImmobilier";
    var prefixIdSelectBoxArray = ["partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee",
        "partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee"];
    var persons = getTiersOperationsImmobilieres();
    prefixIdSelectBoxArray.forEach(function (prefixIdSelectBox) {
        refreshAllOperationsConcernedPersonsLists(prefixIdDivOperations, prefixIdSelectBox, persons);
    });
}
function refreshAllOperationsConcernedPersonsLists(prefixIdDivOperations, prefixIdSelectBox, persons) {
    $("div[id^='" + prefixIdDivOperations + "']").each(
        function () {
            var order = htmlEncode($(this).attr('order'));
            if (!isStringInteger(order))
                return;
            $("#" + prefixIdSelectBox + order).dxSelectBox("instance").option("dataSource", persons);
        }
    );
}

function decodeHtmlEntities(str) {
    var txt = document.createElement("textarea");
    txt.innerHTML = str;
    return txt.value;
}

function setSelectedPersonneConcerneeValue(text, order) {
    var selectBoxPersonneConcernee = $("#partialOperationsCompanieAssurance_PersonneConcernee" + order).dxSelectBox("instance");
    var items = selectBoxPersonneConcernee.option("items");
    var itemToSelect = items.filter(function (item) {
        var decodedName = decodeHtmlEntities(text);
        return item.text == decodedName;
    });
    if (itemToSelect.length == 0)
        return;
    selectBoxPersonneConcernee.option("value", itemToSelect[0].value);
}
function resetAllHiddenOperationsAssurancesFieldsValues(formId) {
    $("#" + formId + " div[id^='divPartialOperationsCompanieAssurance']").each(
        function () {
            var order = htmlEncode($(this).attr('order'));
            if (!isStringInteger(order))
                return;
            resetHiddenOperationsAssurancesFieldsValues(order, formId);
        }
    );
}
function resetAllHiddenOperationImmobilieresFieldsValues(formId) {
    $("#" + formId + " div[id^='divPartialOperationsCompanieImmobilier']").each(
        function () {
            var order = htmlEncode($(this).attr('order'));
            if (!isStringInteger(order))
                return;
            resetHiddenOperationsImmobilieresFieldsValues(order);
        }
    );
}
function resetHiddenOperationsImmobilieresFieldsValues(order) {
    if ($("#divAutreEmetteurOperationImmobiliere" + order).is(":hidden")) {
        $("#partialOperationsCompanieImmobilier_TypeEmettrice" + order).dxSelectBox("instance").reset();
    }

    if ($("#PmTypeEmettrice01Div" + order).is(":hidden")) {
        $("#partialOperationsCompanieImmobilier_DenominationEmettrice" + order).dxTextBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_NumeroIdentificationProfessionnelleEmettrice" + order).dxTextBox("instance").reset();
    }

    if ($("#PpTypeEmettrice01Div" + order).is(":hidden")) {
        $("#partialOperationsCompanieImmobilier_NomNaissanceEmettrice" + order).dxTextBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_PrenomsEmettrice" + order).dxTextBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_DateNaissanceEmettrice" + order).dxDateBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_LieuNaissanceEmettrice" + order).dxTextBox("instance").reset();
    }

    if ($("#divAutreBeneficiaireOperationImmobiliere" + order).is(":hidden")) {
        $("#partialOperationsCompanieImmobilier_TypeBeneficiaire" + order).dxSelectBox("instance").reset();
    }

    if ($("#PmTypeDestinataire01Div" + order).is(":hidden")) {
        $("#partialOperationsCompanieImmobilier_DenominationDestinataire" + order).dxTextBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_NumeroIdentificationProfessionnelleDestinataire" + order).dxTextBox("instance").reset();
    }

    if ($("#PpTypeDestinataire01Div" + order).is(":hidden")) {
        $("#partialOperationsCompanieImmobilier_NomNaissanceDestinataire" + order).dxTextBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_PrenomsDestinataire" + order).dxTextBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_DateNaissanceDestinataire" + order).dxDateBox("instance").reset();
        $("#partialOperationsCompanieImmobilier_LieuNaissanceDestinataire" + order).dxTextBox("instance").reset();
    }

    var divAutreRefFinanciereEmissionOperationImmobiliere = $("#divPartialOperationsCompanieImmobilier" + order + " div[id='divAutreRefFinanciereEmission']");
    if (divAutreRefFinanciereEmissionOperationImmobiliere.is(":hidden")) {
        divAutreRefFinanciereEmissionOperationImmobiliere.find("#AutreRefFinanciereEmissionOperationSuspecteEdit").dxTextBox("instance").reset();
    }

    var divAutreRefFinanciereReceptionOperationImmobiliere = $("#divPartialOperationsCompanieImmobilier" + order + " div[id='divAutreRefFinanciereReception']");
    if (divAutreRefFinanciereEmissionOperationImmobiliere.is(":hidden")) {
        divAutreRefFinanciereReceptionOperationImmobiliere.find("#AutreRefFinanciereReceptionOperationSuspecteEdit").dxTextBox("instance").reset();
    }
}
function resetAllHiddenOperationSuspectesFieldsValues() {
    $(".operationSuspectRow input[name*='IdPersonneEmetteur'").each(function () {
        if ($(this).val() == _labAutreTier.value)
            return;
        $(this).parent().parent().find('[id^="AutreEmetteurOperationSuspecte"]').dxTextBox("instance").reset();
    });

    $(".operationSuspectRow input[name*='IdPersonneBeneficiaire'").each(function () {
        if ($(this).val() == _labAutreTier.value)
            return;
        $(this).parent().parent().find('[id^="AutreBeneficiaireOperationSuspecte"]').dxTextBox("instance").reset();
    });

    $(".operationSuspectRow [name*='ReferenceFinanciereEmissionClientId'").each(function () {
        var textBoxReferenceFinanciereEmissionId = $(this).closest('[id*="ReferenceFinanciereEmissionIdOperationSuspect"]');
        if (textBoxReferenceFinanciereEmissionId.dxTextBox("instance").option("value") == _labAutreReferenceFinanciere.value)
            return;
        textBoxReferenceFinanciereEmissionId.parent().parent().find('div[id^="AutreRefFinanciereEmissionOperationSuspecte"]').dxTextBox("instance").reset();
    });

    $(".operationSuspectRow [name*='ReferenceFinanciereReceptionClientId'").each(function () {
        var textBoxReferenceFinanciereReceptionId = $(this).closest('[id*="ReferenceFinanciereReceptionIdOperationSuspect"]');
        if (textBoxReferenceFinanciereReceptionId.dxTextBox("instance").option("value") == _labAutreReferenceFinanciere.value)
            return;
        textBoxReferenceFinanciereReceptionId.parent().parent().find('div[id^="AutreRefFinanciereReceptionOperationSuspecte"]').dxTextBox("instance").reset();
    });
}
function resetAllHiddenTiersFieldsValues() {
    resetAllHiddenPersonnesPhysiquesFieldsValues();

    resetAllHiddenPersonnesMoralesFieldsValues();
}
function resetAllHiddenPersonnesPhysiquesFieldsValues() {
    $("div[id^='PersonnePhysiqueLabIdentificationProfessionnelleId']").each(function () {
        if ($(this).dxSelectBox("instance").option("value") == _labIdEtranger) return;

        $(this).parent()
            .parent()
            .find("div[id^='partialPersonnePhysiqueLab_PaysDeRegistreId']")
            .dxSelectBox("instance").reset();
    });
}
function resetAllHiddenPersonnesMoralesFieldsValues() {
    resetAllHiddenPersonnesMoralesIdentificationFieldsValues();

    resetAllHiddenPersonnesMoralesDirigeantsFieldsValues();
}
function resetAllHiddenPersonnesMoralesIdentificationFieldsValues() {
    $("div[id^='PersonneMoraleLabProfessionalIdentificationId']").each(function () {
        if ($(this).dxSelectBox("instance").option("value") == _labIdEtranger) return;

        $(this).parent()
            .parent()
            .find("div[id^='partialPersonneMoraleLab_PaysDeRegistreId']")
            .dxSelectBox("instance").reset();
    });
}
function resetAllHiddenPersonnesMoralesDirigeantsFieldsValues() {
    $("div[id^='PersonneMoraleLabDirigeantProfessionalIdentificationId']").each(function () {
        if ($(this).dxSelectBox("instance").option("value") == _labIdEtranger) return;

        $(this).parent()
            .parent()
            .find("div[id^='partialDirigeantPersonneMoraleLab_PaysDeRegistreId']")
            .dxSelectBox("instance").reset();
    });
}

function resetHiddenOperationsAssurancesFieldsValues(order, formId) {
    if ($("#divOperationsSuspectesMultiplesOui" + order).is(":hidden")) {
        $("#partialOperationsCompanieAssurance_DateDebutOperationsMultiples" + order).dxDateBox("instance").reset();
        $("#partialOperationsCompanieAssurance_DateFinOperationsMultiples" + order).dxDateBox("instance").reset();
        $("#partialOperationsCompanieAssurance_NombreOperations" + order).dxNumberBox("instance").reset();
    }

    if ($("#divOperationsSuspectesMultiplesNon" + order).is(":hidden")) {
        $("#partialOperationsCompanieAssurance_DateExecutionOperationUnique" + order).dxDateBox("instance").reset();
    }

    if ($("#divOperationUniquementEnFranceMetroNon" + order).is(":hidden")) {
        $("#partialOperationsCompanieAssurance_PaysDepartId" + order).dxTagBox("instance").reset();
        $("#partialOperationsCompanieAssurance_AutrePaysDepart" + order).dxTextBox("instance").reset();
        $("#partialOperationsCompanieAssurance_PaysArriveeId" + order).dxTagBox("instance").reset();
        $("#partialOperationsCompanieAssurance_AutrePaysArrivee" + order).dxTextBox("instance").reset();
    }
    else {
        if ($("#divPrecisionAutrePaysDepart" + order).is(":hidden")) {
            $("#partialOperationsCompanieAssurance_AutrePaysDepart" + order).dxTextBox("instance").reset();
        }

        if ($("#divPrecisionAutrePaysArrivee" + order).is(":hidden")) {
            $("#partialOperationsCompanieAssurance_AutrePaysArrivee" + order).dxTextBox("instance").reset();
        }
    }

    if ($("#divAutreTypeGarantie" + order).is(":hidden")) {
        $("#partialOperationsCompanieAssurance_AutreTypeDeGarantie" + order).dxTextBox("instance").reset();
    }

    if ($("#divCheckboxOperationsEnCoursChecked" + order).is(":hidden")) {
        var operationsSusceptiblesOppositionsDataGrid = $("#partialOperationsCompanieAssurance_OperationsSusceptiblesOppositionDatagrid" + order).dxDataGrid("instance");
        operationsSusceptiblesOppositionsDataGrid.option("dataSource", []);
        removeFormData("DeclarationTracfins[" + order + "].OperationsCompaniesAssurances.OperationsSusceptiblesOpposition", formId);
    }
}
function getAttachmentsPopupFormValidationGroup(index) {
    return _attachmentsPopupFormValidationGroupPrefix + index;
}
function ensureNombreReferencesFinancieresTiersIntegresDsBelowMaximumNumberOnRowInserting(e) {
    var checkboxIntegreeDs = e.element.closest(".tab-content").find("div[id*='IsDeclarationTracfin']");
    if (!checkboxIntegreeDs || checkboxIntegreeDs.length == 0) {
        return;
    }

    var isCurrentPersonneIntegreeDs = checkboxIntegreeDs.dxCheckBox("instance").option("value");
    if (!isCurrentPersonneIntegreeDs) {
        return;
    }

    if (getCountReferencesFinancieresTiersIntegresDs() >= _labNombreMaximumReferencesFinancieresTiersIntegreDsParDossier) {
        e.cancel = true;
        DevExpress.ui.notify(getErrorMessageNombreMaxReferencesFinancieresAtteint(), "error");
    }
}
function ensureNombreReferencesFinancieresTiersIntegresDsBelowMaximumNumber(e) {
    var newValue = e.component.option("value");

    if (newValue == null || !newValue || !doesNbrReferencesFinancieresTiersIntegresDsExceedMaxNumber()) {
        return true;
    }

    e.component.option("value", false);

    DevExpress.ui.notify(getErrorMessageNombreMaxReferencesFinancieresAtteint(), "error");

    return false;
}

function getCountReferencesFinancieresTiersIntegresDs() {
    var result = 0;
    $(".physicalPerson:visible [id^='PersonnePhysiqueLabIsDeclarationTracfin']").each(function () {
        result += $(this).dxCheckBox("instance").option("value") ? getCountReferencesFinancieresTiers($(this).closest(".physicalPerson"), "gridSupportFinancier") : 0;
    });

    $(".moralPerson:visible [id^='DossierLabPersonneMoralesIsDeclarationTracfin']").each(function () {
        result += $(this).dxCheckBox("instance").option("value") ? getCountReferencesFinancieresTiers($(this).closest(".moralPerson"), "gridSupportFinancier") : 0;
    });
    $(".nonConnu:visible [id^='DossierLabNonConnusIsDeclarationTracfin']").each(function () {
        result += $(this).dxCheckBox("instance").option("value") ? getCountReferencesFinancieresTiers($(this).closest(".nonConnu"), "gridSupportFinancier") : 0;
    });
    return result;
}
function getCountReferencesFinancieresTiers(person, prefixIdGridReferencesFinancieres) {
    var count = 0;
    person.find("div[id^='" + prefixIdGridReferencesFinancieres + "']").each(function () {
        count += $(this).dxDataGrid("instance").getDataSource().items().length;
    });
    return count;
}
function ensureNombreTiersIntegresDsBelowMaximumNumber(e) {
    var newValue = e.component.option("value");

    if (newValue == null ||
        !newValue ||
        !doesNombreTiersIntegresDsExceedMaximum(getCountTiersIntegresDs())) {
        return true;
    }

    e.component.option("value", false);

    DevExpress.ui.notify(getErrorMessageNombreMaxTiersAtteint(), "error");

    return false;
}
function getCountTiersIntegresDs() {
    var result = 0;
    $(".physicalPerson").each(function () {
        if ($(this).css('display') == 'none') return;

        result += $(this).find("[id^='PersonnePhysiqueLabIsDeclarationTracfin']")
            .dxCheckBox("instance")
            .option("value") ? 1 : 0;
    });

    $(".moralPerson").each(function () {
        if ($(this).css('display') == 'none') return;

        result += $(this).find("[id^='DossierLabPersonneMoralesIsDeclarationTracfin']")
            .dxCheckBox("instance")
            .option("value") ? 1 : 0;
    });
    $(".nonConnu").each(function () {
        if ($(this).css('display') == 'none') return;

        result += $(this).find("[id^='DossierLabNonConnusIsDeclarationTracfin']")
            .dxCheckBox("instance")
            .option("value") ? 1 : 0;
    });
    return result;
}
function ensureNombreLiensPersonnesIntegreesDsParDossierBelowMaxNumberOnRowInserting(e) {
    var checkboxIntegreeDs = e.element.closest(".tab-content").parent().closest(".tab-content").find("div[id*='IsDeclarationTracfin']");
    if (!checkboxIntegreeDs || checkboxIntegreeDs.length == 0) {
        return;
    }

    var isCurrentPersonneIntegreeDs = checkboxIntegreeDs.dxCheckBox("instance").option("value");
    if (!isCurrentPersonneIntegreeDs) {
        return;
    }

    if (getCountLiensPersonnesIntegreesDsParDossier() >= _labNombreMaximumLiensPersonnesIntegreesDsParDossier) {
        e.cancel = true;
        DevExpress.ui.notify(getErrorMessageNombreMaxLiensPersonnesIntegreesDsParDossierAtteint(), "error");
    }
}
function ensureNombreLiensPersonnesIntegreesDsParDossierBelowMaxNumber(e) {
    var newValue = e.component.option("value");

    if (newValue == null || !newValue || !doesNombreLiensPersonnesIntegreesDsParDossierExceedsMaxNumber()) {
        return true;
    }

    e.component.option("value", false);

    DevExpress.ui.notify(getErrorMessageNombreMaxLiensPersonnesIntegreesDsParDossierAtteint(), "error");

    return false;
}

function getCountLiensPersonnesIntegreesDsParDossier() {
    var result = 0;
    $(".physicalPerson").each(function () {
        if ($(this).css('display') == 'none') return;

        var checkboxDeclarationTracfin = $(this).find("[id^='PersonnePhysiqueLabIsDeclarationTracfin']");

        result += checkboxDeclarationTracfin
            .dxCheckBox("instance")
            .option("value") ?
            getCountLiensPersonne(checkboxDeclarationTracfin.closest(".physicalPerson"), "gridLien") : 0;
    });

    $(".moralPerson").each(function () {
        if ($(this).css('display') == 'none') return;

        var checkboxDeclarationTracfin = $(this).find("[id^='DossierLabPersonneMoralesIsDeclarationTracfin']");

        result += checkboxDeclarationTracfin
            .dxCheckBox("instance")
            .option("value") ?
            getCountLiensPersonne(checkboxDeclarationTracfin.closest(".moralPerson"), "gridLien") : 0;
    });
    $(".nonConnu").each(function () {
        if ($(this).css('display') == 'none') return;

        var checkboxDeclarationTracfin = $(this).find("[id^='DossierLabNonConnusIsDeclarationTracfin']");

        result += checkboxDeclarationTracfin
            .dxCheckBox("instance").option("value") ?
            getCountLiensPersonne(checkboxDeclarationTracfin.closest(".nonConnu"), "gridNonConnuLien") : 0;
    });
    return result;
}
function getCountLiensPersonne(person, prefixIdGridLiens) {
    var count = 0;
    person.find("div[id^='" + prefixIdGridLiens + "']").each(function () {
        count += $(this).dxDataGrid("instance").getDataSource().items().length;
    });
    return count;
}

function ensureNombreCoordonneesBelowMaximumNumber(index) {
    return getNombreCoordonneesPersonnePhysique(index) < _labNombreMaximumCoordonneesPersonnesPhysiques;
}

function getErrorMessageNombreMaxCoordonneesPersonnesPhysiquesAtteint() {
    return _labTranslatableMessageErrorNombreMaxCoordonneesPersonnesPhysiquesAtteint
        .format(_labNombreMaximumCoordonneesPersonnesPhysiques);
}
function doesNombreCoordonneesPersonnesPhysiquesExceedsMaxNumber(index) {
    return getNombreCoordonneesPersonnePhysique(index) > _labNombreMaximumCoordonneesPersonnesPhysiques;
}
function getNombreCoordonneesPersonnePhysique(index) {
    return $("div[id^='physicalPersonCoordonnee" + index + "-']")
        .toArray()
        .reduce(function (acc, current) {
            if ($(current).css('display') != 'none') {
                return ++acc;
            }
            return acc;
        }, 0);
}
function ensureNombreOperationsSuspectesBelowMaxNumberOnAddNewOperation() {
    return ensureNombreOperationsBelowMaxNumberOnAddNewOperation(_labNombreMaximumOperationsSuspectes,
        "operationSuspectTable");
}
function ensureNombreOperationsEnCoursExecutionBelowMaxNumberOnAddNewOperation() {
    return ensureNombreOperationsBelowMaxNumberOnAddNewOperation(_labNombreMaximumOperationsEnCours,
        "operationEnCoursTable");
}
function ensureNombreOperationsBelowMaxNumberOnAddNewOperation(arrayNombreMax, operationsTableId) {
    var max = getNombreMaximumOperations(arrayNombreMax);

    if (!max) {
        return true;
    }

    if ($("#" + operationsTableId + " tbody tr").length < max) {
        return true;
    }

    DevExpress.ui.notify(getErrorMessageNombreMaxOperationsAtteint(max), "error");

    return false;
}
function getErrorMessageNombreMaxOperationsAtteint(max) {
    return _labTranslatableMessageErrorNombreMaxOperationsSuspectesAtteint.format(max);
}
function ensureGridLengthBelowMaxRowsNumberOnInserting(e, max, errorMessage) {
    if (e.component.getDataSource().items().length < max)
        return true;

    DevExpress.ui.notify(errorMessage, "error");
    e.cancel = true;
    return false;
}
function ensureNombreContractsConcernesBelowMaxNumberOnInserting(e) {
    return ensureGridLengthBelowMaxRowsNumberOnInserting(e,
        _labNombreMaximumContratsConcernes,
        getErrorMessageNombreMaximumContratsConcernesAtteint(),
    );
}
function getErrorMessageNombreMaximumContratsConcernesAtteint() {
    return _labTranslatableMessageErrorNombreMaxContratsConcernesAtteint
        .format(_labNombreMaximumContratsConcernes);
}
function ensureNombreOperationsSusceptiblesOppositionBelowMaxNumberOnInserting(e) {
    return ensureGridLengthBelowMaxRowsNumberOnInserting(e,
        _labNombreMaximumOperationsSusceptiblesOpposition,
        _labTranslatableMessageErrorNombreMaxOperationsSuspectesAtteint.format(_labNombreMaximumOperationsSusceptiblesOpposition),
    );
}

function validatePieceJointeDossierLabEnvoiTracfin(isNewAttachment, index) {
    var piecesJointesEnvoiTracfin = getPiecesJointes(_labEnvoiTracfinCategorieDocumentId);

    if (!isNombrePiecesJointesEnvoiTracfinValid(isNewAttachment, piecesJointesEnvoiTracfin, index)) {
        return Promise.reject(getErrorMessageNombreMaximumPiecesJointesEnvoiTracfinParDossierAtteint());
    }

    var info = getAttachmentFileInfo(index);

    var fileName = info.fileName;

    if (!isTracfinValidFileName(fileName)) {
        return Promise.reject(_labTranslatableMessageInvalidTracfinFileName);
    }

    if (!checkFileExtension(fileName, _labExtensionsPiecesJointesDeclarationTracfin)) {
        return Promise.reject(getErrorMessageInvalidExtensionPieceJointeEnvoiTracfin());
    }

    if (!isPiecesJointesEnvoiTracfinTotalSizeValid(isNewAttachment, piecesJointesEnvoiTracfin, index, info.fileSize)) {
        return Promise.reject(getErrorMessageTailleMaximalePiecesJointesEnvoiTracfinAtteinte());
    }

    return assertFileNotEncrypted(index, isNewAttachment);
}

function assertFileNotEncrypted(index, isNewAttachment) {
    var file = getUploadedFile(index);

    if (file) {
        return isUploadedFileEncrypted(file).then(function (isEncrypted) {
            if (isEncrypted) {
                return Promise.reject(_labTranslatableMessageFichierCrypte);
            }
        });
    }

    if (!isNewAttachment) {
        return isSavedFileEncrypted(index)
            .then(function (isEncrypted) {
                if (isEncrypted) {
                    return Promise.reject(_labTranslatableMessageFichierCrypte);
                }
            })
            .catch(function (message) {
                return Promise.reject(message);
            });
    }

    return Promise.resolve();
}

function isSavedFileEncrypted(index) {
    var cryptedDocumentId = $("input[id$='_CryptedDocumentLabId" + index + "']").val();
    var categorieDocumentId = $("input[id$='_CategorieDocumentId" + index + "']").val();

    return $.ajax({
        url: "/" + culture + "/Lab/ServiceLab/IsFileEncrypted",
        method: "GET",
        dataType: "json",
        data: { cryptedDocumentId, categorieDocumentId }
    }).then(function (response) {
        if (response && response.isEncrypted) {
            return Promise.resolve(true);
        }
        return Promise.resolve(false);
    }).fail(function () {
        return Promise.reject(_labTranslatableMessageErrorCheckingCryptedFile);
    });
}

function getUploadedFile(i) {
    var $fileUploader = $("#FileUploader" + i);

    if ($fileUploader.length > 0) {
        var uploadedFiles = $("#FileUploader" + i).prop('files');

        if (uploadedFiles && uploadedFiles.length > 0) {
            return uploadedFiles[0];
        }
    }
}

function isTracfinValidFileName(name) {
    return /^[^.]+\.[^.]+$/.test(name);
}

function isPiecesJointesEnvoiTracfinTotalSizeValid(isNewAttachment, piecesJointesEnvoiTracfin, index, fileSize) {
    var autresPiecesJointes = isNewAttachment ?
        piecesJointesEnvoiTracfin :
        piecesJointesEnvoiTracfin.filter(pj => pj.index != index);

    var tailleAutresPiecesJointes = getTaillePiecesJointes(autresPiecesJointes);

    if (tailleAutresPiecesJointes + fileSize > _labTailleMaxPiecesJointesDeclarationTracfinMo * 1024 * 1024) {
        return false;
    }
    return true;
}

function isNombrePiecesJointesEnvoiTracfinValid(isNewAttachment, piecesJointesEnvoiTracfin, index) {
    var autresPiecesJointes = isNewAttachment ?
        piecesJointesEnvoiTracfin :
        piecesJointesEnvoiTracfin.filter(pj => pj.index != index);

    if (autresPiecesJointes.length == _labNombreMaximumPiecesJointesCatgorieTracfin) {
        return false;
    }
    return true;
}

function validatePiecesJointesEnvoiTracfin(newFile, errorObj) {
    var piecesJointesEnvoiTracfin = getPiecesJointes(_labEnvoiTracfinCategorieDocumentId);

    if (piecesJointesEnvoiTracfin.length >= _labNombreMaximumPiecesJointesCatgorieTracfin) {
        errorObj.message = getErrorMessageNombreMaximumPiecesJointesEnvoiTracfinParDossierAtteint();
        return false;
    }

    if (!checkFileExtension(newFile.name, _labExtensionsPiecesJointesDeclarationTracfin)) {
        errorObj.message = getErrorMessageInvalidExtensionPieceJointeEnvoiTracfin();
        return false;
    }

    var taillePiecesJointesEnvoiTracfin = getTaillePiecesJointes(piecesJointesEnvoiTracfin);

    if (taillePiecesJointesEnvoiTracfin + newFile.size > _labTailleMaxPiecesJointesDeclarationTracfinMo * 1024 * 1024) {
        errorObj.message = getErrorMessageTailleMaximalePiecesJointesEnvoiTracfinAtteinte();
        return false;
    }
    return true;
}

function isUploadedFileEncrypted(file) {
    return new Promise(function (resolve) {
        var ext = (file.name.split('.').pop() || '').toLowerCase();
        var r = new FileReader();

        r.onload = function (e) {
            var buf = e.target.result;

            // PDFs
            if (ext === "pdf") {
                pdfjsLib.getDocument({ data: buf }).promise
                    .then(function () { resolve(false); }) // opened => not encrypted
                    .catch(function () { resolve(true); }); // any failure => encrypted per policy
                return;
            }

            // OOXML (docx/xlsx/pptx)
            if (ext === "docx" || ext === "xlsx" || ext === "pptx") {
                JSZip.loadAsync(buf).then(function (zip) {
                    // IRM/AIP quick check: any entry under "drm/"?
                    for (var name in zip.files) {
                        if (zip.files.hasOwnProperty(name) && name.toLowerCase().indexOf("drm/") === 0) {
                            resolve(true); return;
                        }
                    }
                    resolve(false); // ZIP loaded and no /drm/ => not encrypted
                }).catch(function () {
                    resolve(true); // ZIP failed => treat as encrypted
                });
                return;
            }

            resolve(false); // others not checked here
        };

        r.onerror = function () { resolve(true); }; // safe default
        r.readAsArrayBuffer(file);
    });
}

function getTaillePiecesJointes(piecesJointes) {
    return piecesJointes.map(pj => pj.FileSize || 0)
        .reduce(function (accumulator, currentValue) {
            return accumulator + currentValue;
        }, 0);
}
function getErrorMessageNombreMaximumPiecesJointesEnvoiTracfinParDossierAtteint() {
    return _labTranslatableMessageErrorNombreMaxPiecesJointesAtteint
        .format(_labNombreMaximumPiecesJointesCatgorieTracfin);
}
function getErrorMessageInvalidExtensionPieceJointeEnvoiTracfin() {
    return _labTranslatableMessageErrorExtensionPiecesJointesInvalide
        .format(_labExtensionsPiecesJointesDeclarationTracfin.join(", "));
}
function getErrorMessageTailleMaximalePiecesJointesEnvoiTracfinAtteinte() {
    return _labTranslatableMessageErrorTailleMaxPiecesJointesAtteinte
        .format(_labTailleMaxPiecesJointesDeclarationTracfinMo);
}
function getPiecesJointes(catgeorieDocumentId) {
    var piecesjointes = [];
    var gridAttachmentsDataSource = $("#gridAttachmentsLab").dxDataGrid("instance").getDataSource();
    if (gridAttachmentsDataSource) {
        piecesjointes = gridAttachmentsDataSource.items();
    }
    return piecesjointes.filter(pj => pj.CategorieDocumentId == catgeorieDocumentId && !pj.IsDeletedNew);
}
function gridSupportFinancier_OnEditorPreparing(e) {
    if (e.parentType != "dataRow") {
        return;
    }

    if (e.dataField == 'TypeReferenceLabId') {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);

        if (e.editorOptions.value == _labIdTypeReferenceNumContrat &&
            _labLastSelectedProfessionCode[e.element.attr('id')] != _labCodeProfessionOrganismeAssurance) {
            e.editorOptions.value = "";
            e.setValue("");
        }

        filterDataSourceTypesReferences(e.component);

        return;
    }

    if (e.dataField == 'Iban' || e.dataField == 'CodeBic' || e.dataField == 'CodeCib') {
        e.editorOptions.onValueChanged = function (args) {
            e.setValue(args.value.removeAllWhiteSpaces());
        }
    }

    var readOnlyRuleEntry = getEntry(gridSupportFinancierRulesDictionary, e.row.data.TypeReferenceLabId, e.dataField);
    if (!readOnlyRuleEntry)
        return;

    if (e.editorOptions.readOnly == readOnlyRuleEntry.readOnly)
        return;

    e.editorOptions.readOnly = readOnlyRuleEntry.readOnly;

    var cssClass = e.editorOptions.readOnly ? 'read-only-cell' : '';
    e.editorOptions.inputAttr = { class: cssClass };

    var currentFieldNotEmpty = notEmptyValidationCallback(e.value);
    if (e.editorOptions.readOnly && currentFieldNotEmpty) {
        e.editorOptions.value = "";
        e.setValue("");
    }
}
function filterDataSourceTypesReferences(gridInstance) {
    var professionCode = $("#createOrUpdateDossierLab_CodeProfession").val();

    var gridId = gridInstance.element().attr('id');

    if (professionCode == _labLastSelectedProfessionCode[gridId]) {
        return;
    }

    _labLastSelectedProfessionCode[gridId] = professionCode;

    var store = gridInstance.columnOption("TypeReferenceLabId").lookup.dataSource.store;

    gridInstance.columnOption("TypeReferenceLabId", "editorOptions", {
        dataSource: new DevExpress.data.DataSource(
            {
                store,
                filter: professionCode == _labCodeProfessionOrganismeAssurance ?
                    null :
                    ["Code", "<>", _labCodeTypeReferenceNumContrat]
            })
    });
}
function gridSupportFinancier_columnValidationCallback(e) {
    var dataField = e.column.dataField;
    var typeReferenceLabId = e.data.TypeReferenceLabId;

    if (!dataField || !typeReferenceLabId)
        return true;

    var ruleEntry = getEntry(gridSupportFinancierRulesDictionary, typeReferenceLabId, dataField);
    if (!ruleEntry || !ruleEntry.validationCallBack) {
        return true;
    }

    e.rule.message = ruleEntry.message;
    return ruleEntry.validationCallBack(e.value);
}
function setReferencesFinancieresCustomValidationRules(gridReferencesFinanciereSelector) {
    var dataGridInstance = $(gridReferencesFinanciereSelector).dxDataGrid("instance");
    dataGridInstance.option("columns").forEach(function (column, index) {
        if (!column.dataField || column.dataField == "TypeReferenceLabId" || !dataGridInstance.columnOption(index, "visible"))
            return;

        dataGridInstance.columnOption(index, "validationRules", [{ type: 'custom', validationCallback: gridSupportFinancier_columnValidationCallback }])
    });
    dataGridInstance.repaint();
}
function tryInitInfoContratConcerneOperationImmobiliere(currentNombreDataGridReferencesFinancieresReady) {
    if (currentNombreDataGridReferencesFinancieresReady < _nombreInitialTiers) {
        return;
    }
    var selectBoxesIdsPrefixesArray = [
        "partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee",
        "partialOperationsCompaniesImmobiliers_ReferenceFinanciereEmettriceConcernee",
        "partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee",
        "partialOperationsCompaniesImmobiliers_ReferenceFinanciereDestinataireConcernee"
    ];
    selectBoxesIdsPrefixesArray.forEach(function (selectBoxIdPrefix) {
        var hiddenFieldIdPrefix = selectBoxIdPrefix + _labHiddenSuffix;
        setSelectBoxesSelectedItemsFromHiddenFields(hiddenFieldIdPrefix, selectBoxIdPrefix, item => item.text);
    });
}
function setSelectBoxesSelectedItemsFromHiddenFields(hiddenFieldPrefix, selectBoxIdPrefix, comparisonFieldFunc) {
    $("input[type='hidden'][id^='" + hiddenFieldPrefix + "']").each(function () {
        var value = $(this).val();
        var order = $(this).attr('id').substring(hiddenFieldPrefix.length);
        if (isNaN(order)) {
            return;
        }
        var selectBoxInstance = $("#" + selectBoxIdPrefix + order).dxSelectBox("instance");
        setSelectBoxSelectedItem(value, selectBoxInstance, comparisonFieldFunc);
    });
}
function setSelectBoxSelectedItem(value, selectBoxInstance, comparisonFieldFunc) {
    var items = selectBoxInstance.option("items");
    var itemToSelect = items.filter(function (item) {
        return comparisonFieldFunc(item) == value;
    });
    if (itemToSelect.length == 0) {
        selectBoxInstance.option("selectedItem", undefined);
    }
    else {
        selectBoxInstance.option("value", itemToSelect[0].value);
    }
}
function refreshAllReferencesFinancieresOperationsImmobilieres() {
    $("div[id^='divPartialOperationsCompanieImmobilier']").each(function () {

        var order = htmlEncode($(this).attr('order'));

        refreshReferencesFinancieresTier(
            "#partialOperationsCompaniesImmobiliers_PersonneEmettriceConcernee" + order,
            "#partialOperationsCompaniesImmobiliers_ReferenceFinanciereEmettriceConcernee" + order
        );

        refreshReferencesFinancieresTier(
            "#partialOperationsCompaniesImmobiliers_PersonneDestinataireConcernee" + order,
            "#partialOperationsCompaniesImmobiliers_ReferenceFinanciereDestinataireConcernee" + order
        );
    });
}
function refreshReferencesFinancieresTier(selectBoxTierSelector, selectBoxReferencesFinancieresSelector) {
    var selectedTier = $(selectBoxTierSelector).dxSelectBox("instance").option("selectedItem");
    var selectBoxReferencesFinancieres = $(selectBoxReferencesFinancieresSelector).dxSelectBox("instance");
    refreshReferencesFinancieres(selectedTier, selectBoxReferencesFinancieres);
}
function loadTypeLienPersonnePhysiquePhysiquesById(e) {
    if (e.parentType != "dataRow") return;

    if (e.dataField == "CategorieLienPersonnePhysiquePhysiqueId" ||
        e.dataField == "TypeLienPersonnePhysiquePhysiqueId") {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);
    }

    if (e.dataField == "TypeLienPersonnePhysiquePhysiqueId") {
        refreshTypesLiens(e,
            data => data.CategorieLienPersonnePhysiquePhysiqueId,
            _labGetTypesLiensPpPpUrl,
            'TypeLienPersonnePhysiquePhysiqueId');
    }
}
function precisionsValidationCallback(e, idTypeLienFunc, idTypeLienAutre) {
    var idTypeLien = idTypeLienFunc(e.data);

    if (idTypeLien == null || !idTypeLien) return true;

    if (idTypeLien != idTypeLienAutre) return true;

    return e.value != null && e.value && e.value.trim();
}
function loadTypeLienPersonnePhysiqueMoraleById(e) {
    if (e.parentType != "dataRow") return;

    if (e.dataField == "CategorieLienPersonneMoralePhysiqueId" ||
        e.dataField == "TypeLienPersonneMoralePhysiqueId") {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);
    }

    if (e.dataField == "TypeLienPersonneMoralePhysiqueId") {
        refreshTypesLiens(e,
            data => data.CategorieLienPersonneMoralePhysiqueId,
            _labGetTypesLiensPmPpUrl,
            'TypeLienPersonneMoralePhysiqueId');
    }
}

function loadTypeLienPersonneMoralePhysiqueById(e) {

    if (e.parentType != "dataRow") return;

    if (e.dataField == "CategorieLienPersonnePhysiqueMoraleId" ||
        e.dataField == "TypeLienPersonnePhysiqueMoraleId") {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);
    }

    if (e.dataField == "TypeLienPersonnePhysiqueMoraleId") {
        refreshTypesLiens(e,
            data => data.CategorieLienPersonnePhysiqueMoraleId,
            _labGetTypesLiensPpPmUrl,
            'TypeLienPersonnePhysiqueMoraleId');
    }
}
function refreshTypesLiens(e, categoryFieldFunc, url, typeLienField) {

    var categorieId = categoryFieldFunc(e.row.data);

    $.ajax({
        method: "GET",
        data: { categorieId },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        cache: false,
        url
    }).done(function (data) {

        refreshDataGridSelectBoxField(e,
            data,
            typeLienField,
            'Id',
            culture == 'fr' ? 'NameFr' : 'NameEn');
    });
}
function loadTypeLienPersonneMoraleMoraleById(e) {

    if (e.parentType != "dataRow") return;

    if (e.dataField == "CategorieLienPersonneMoraleMoraleId" ||
        e.dataField == "TypeLienPersonneMoraleMoraleId") {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);
    }

    if (e.dataField == "TypeLienPersonneMoraleMoraleId") {
        refreshTypesLiens(e,
            data => data.CategorieLienPersonneMoraleMoraleId,
            _labGetTypesLiensPmPmUrl,
            'TypeLienPersonneMoraleMoraleId');
    }
}

function loadTypeLienPersonneInconnuPhysiquesById(e) {
    if (e.parentType != "dataRow") return;

    if (e.dataField == "CategorieLienPersonnePhysiquePhysiqueId" ||
        e.dataField == "TypeLienPersonnePhysiquePhysiqueId") {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);
    }

    if (e.dataField == "TypeLienPersonnePhysiquePhysiqueId") {
        refreshTypesLiens(e,
            data => data.CategorieLienPersonnePhysiquePhysiqueId,
            _labGetTypesLiensPpPncUrl,
            'TypeLienPersonnePhysiquePhysiqueId');
    }
}

function loadTypeLienPersonneNonConnueMoraleById(e) {
    if (e.parentType != "dataRow") return;

    if (e.dataField == "CategorieLienPersonneMoralePhysiqueId" ||
        e.dataField == "TypeLienPersonneMoralePhysiqueId") {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);
    }

    if (e.dataField == "TypeLienPersonneMoralePhysiqueId") {
        refreshTypesLiens(e,
            data => data.CategorieLienPersonneMoralePhysiqueId,
            _labGetTypesLiensPmPncUrl,
            'TypeLienPersonneMoralePhysiqueId');
    }
}

function showDateDerniereDeclarationDgt() {
    var isDgt = $("#IsDgt").dxCheckBox("instance").option('value');
    if (isDgt) {
        $("#date-derniere-declaration-dgt").removeClass("dx-hidden");
    }
    else {
        $("#date-derniere-declaration-dgt").addClass("dx-hidden");
        $("#DateDerniereDeclarationDgt").dxDateBox("instance").option("value", null);
    }
}

function typeOperationEnCours_OnSelectionChanged(e) {
    var typeOperationEnCoursSelectBoxSelector = "#" + e.element.attr('id');

    toggleDivPrecisionAutreSelectBoxItem(typeOperationEnCoursSelectBoxSelector,
        _labIdTypeOperationAutre,
        "#divOperationEnCours_AutreTypeOperation",
        _labOperationEnCoursValidationGroup
    );
}


function setDefaultValueCertitudeLien(e) {
    if (e.parentType == 'dataRow' && e.dataField == 'CertitudeLien') {
        e.editorElement.dxSelectBox('instance').option('value', null);
    }
}
function customRequiredReferenceFinanciereFieldValidationCallback(e) {
    var validationRulesEntry = getEntry(gridSupportFinancierRulesDictionary, e.data.TypeReferenceLabId, e.column.dataField)

    if (!validationRulesEntry || !validationRulesEntry.required)
        return true;

    return notEmptyValidationCallback(e.value);
}
function customReferenceFinanciereFieldPatternValidationCallback(e) {
    var validationRulesEntry = getEntry(gridSupportFinancierRulesDictionary, e.data.TypeReferenceLabId, e.column.dataField)

    if (!validationRulesEntry || !validationRulesEntry.pattern)
        return true;

    return validationRulesEntry.pattern.test(e.value);
}
function customReferenceFinanciereFieldMinLengthValidationCallback(e) {
    var validationRulesEntry = getEntry(gridSupportFinancierRulesDictionary, e.data.TypeReferenceLabId, e.column.dataField)

    if (!validationRulesEntry || !validationRulesEntry.minLength)
        return true;

    return e.value && e.value.length >= validationRulesEntry.minLength;
}
function customReferenceFinanciereFieldMaxLengthValidationCallback(e) {
    var validationRulesEntry = getEntry(gridSupportFinancierRulesDictionary, e.data.TypeReferenceLabId, e.column.dataField)

    if (!validationRulesEntry || !validationRulesEntry.maxLength)
        return true;

    return e.value && e.value.length <= validationRulesEntry.maxLength;
}
function MajStatutDossier_OnClick(e) {
    var form = $("#dossierLabForm").closest("form");
    var formData = new FormData(form[0]);
    var declarantId = $("#DeclarantId").dxSelectBox("instance").option("value");
    if (_codeUnique != "") {
        if (declarantId > 0) {
            $.ajax({
                method: "POST",
                data: formData,
                dataType: "json",
                url: '/' + culture + '/Lab/ServiceLab/MajStatutDossierClotureTracfin?declarantId=' + declarantId,
                processData: false,
                contentType: false,
            }).done(function (data) {
                if (data) {
                    DevExpress.ui.notify(_labTranslatableMessageMajStatutTracfinSuccess, "success");
                    location = '/' + culture + '/lab/dossiers/management';
                } else {
                    DevExpress.ui.notify(_labTranslatableMessageMajStatutTracfinError, "error");
                }
            }).fail(function (e) {
                if (e.status == 401) {
                    var popup = $("#modalReconnect").dxPopup('instance');
                    popup.show();
                    return;
                };
            });
        } else {
            DevExpress.ui.notify(_labTranslatableMessageErrorDeclarantObligatoire, "error");
        }
    } else {
        DevExpress.ui.notify(_labTranslatableMessageErrorSaveCase, "error");
    }


}

function customRequiredTypeLienPersonnePhysiquePhysiqueIdValidationCallback(e) {
    return customRequiredTypeLienValidationCallBack(e.data.CategorieLienPersonnePhysiquePhysiqueId, e.value);
}
function customRequiredTypeLienPersonneMoralePhysiqueIdValidationCallback(e) {
    return customRequiredTypeLienValidationCallBack(e.data.CategorieLienPersonneMoralePhysiqueId, e.value);
}
function customRequiredTypeLienPersonnePhysiqueMoraleIdValidationCallback(e) {
    return customRequiredTypeLienValidationCallBack(e.data.CategorieLienPersonnePhysiqueMoraleId, e.value);
}
function customRequiredTypeLienPersonneMoraleMoralesValidationCallback(e) {
    return customRequiredTypeLienValidationCallBack(e.data.CategorieLienPersonneMoraleMoraleId, e.value);
}
function customRequiredTypeLienValidationCallBack(category, value) {
    if (category == null || !category)
        return true;

    return value != null && value;
}

function verifNumeroFiscal(e, order) {
    $("#PersonnePhysiqueLabIdFiscal" + order).dxValidator({
        validationRules: [
            {
                type: 'pattern',
                pattern: '^[0-9]{13}$',
                message: _labTranslatableMessageFormatFiscalIncorrect,
            }
        ]
    });
}

function numeroDeVoieNePasEtre0(e, id, order) {
    $("#" + id + order).dxValidator({
        validationRules: [
            {
                type: 'custom',
                validationCallback: function (e) {
                    return e.value !== 0
                },
                message: _labTranslatableMessageNumeroDeVoieNePasEtre0,
            }
        ]
    });
}



function toggleSectionAssurence(isVisible) {

    toggleContainerIncludingValidators("#OperationsAssurance", isVisible);
    if (isVisible) {
        initSectionAssurance();
    }

}

function banqueValidation(isVisible) {

    toggleContainerIncludingValidators("#OperationsBanque", isVisible);


}

function immobilereValidation(isVisible) {
    if (isVisible) {
        initSectionImmobilier();
    }
}
function DateCessationRelations_ValidationCallback(e) {
    if (e.data.TypeRelationAffaireLabId == _labIdRelationAffaireRelationTerminee ||
        e.data.TypeRelationAffaireLabId == _labIdRelationAffairesCessationAvecBlocageVente) {
        return e.value != null && e.value;
    }
    return true;
}
function partialConfirmationTransmissionDossierLab_gridPpPm_OnEditorPreparing(e) {
    if (e.parentType != "dataRow") {
        return;
    }

    if (e.dataField == 'TypeRelationAffaireLabId') {
        setEditedRowToBeRepaintedAfterSelectionChange(e, item => item.Id);
        return;
    }
}

function setPartialPersonnePhysiqueLabAdditionalValidationRules(order) {
    $("#PersonnePhysiqueLabDateCessationRelation" + order).dxValidator({
        validationRules: [
            {
                type: 'required',
                message: _labTranslatableMessageDateCessationRelationObligatoire
            }
        ]
    });
}

function setPaysIdPersonnePhysiqueLabAdditionalValidationRules(order) {
    $("#PersonnePhysiqueLabCoordonneesCoordonneePaysId" + order).dxValidator({
        validationRules: [
            {
                type: 'required',
                message: _labTranslatableMessagePaysIsRequired
            }
        ]
    });
}

function setPaysIdPersonneMoraleLabAdditionalValidationRules(order) {
    $("#PersonneMoraleLabCoordonneePaysId" + order).dxValidator({
        validationRules: [
            {
                type: 'required',
                message: _labTranslatableMessagePaysIsRequired
            }
        ]
    });
}


function checkExistDs() {
    var isDs = false;
    var form = $("#dossierLabForm").closest("form");
    var formData = new FormData(form[0]);
    $.ajax({
        method: "POST",
        data: formData,
        dataType: "json",
        url: '/' + culture + '/Lab/ServiceLab/VerifDeclarationTracfin',
        processData: false,
        contentType: false,
        async: false
    }).done(function (data) {
        isDs = data;
    });
    return isDs;
}

function showRequiredFieldWithAsterisk(fieldId, validationGroup) {
    var asterisk = getRequiredFieldAsterisk(fieldId);
    var fieldSelector = "#" + fieldId;
    showFieldWithValidationRules(fieldSelector,
        fieldSelector,
        validationGroup,
        [requiredValidationRule],
        asterisk);
}

function disableEnableRequiredFiledOnAddPpPm() {
    var nbPp = $("div[class*='card physicalPerson']").length;
    var nbPm = $("div[class*='card moralPerson']").length;
    // donnée personne physique.
    var personnePhysiqueLabTypeImplicationId = 'PersonnePhysiqueLabTypeImplicationId';
    var personnePhysiqueLabPpeId = 'PersonnePhysiqueLabPpeId';
    var personnePhysiqueLabDateNaissance = 'PersonnePhysiqueLabDateNaissance';

    // données personne morle.
    var personneMoraleLabTypeImplicationId = 'PersonneMoraleLabTypeImplicationId';
    var personneMoraleLabNumeroImmatriculation = 'PersonneMoraleLabNumeroImmatriculation';
    var personneMoraleLabProfessionalIdentificationId = 'PersonneMoraleLabProfessionalIdentificationId';
    var personneMoraleLabRaisonSociale = 'PersonneMoraleLabRaisonSociale';

    var isDs = checkExistDs();
    if (isDs) {
        if (nbPp > 0) {
            for (let i = 0; i < nbPp; i++) {
                showRequiredFieldWithAsterisk(personnePhysiqueLabPpeId + i);
                showRequiredFieldWithAsterisk(personnePhysiqueLabTypeImplicationId + i);
            }
        }

        if (nbPm > 0) {
            for (let i = 0; i < nbPm; i++) {
                showRequiredFieldWithAsterisk(personneMoraleLabTypeImplicationId + i);

                var identificationProfessionnelle = $("#PersonneMoraleLabProfessionalIdentificationId" + i).dxSelectBox("instance").option("value");

                if (identificationProfessionnelle != _labIdIdentificationProfessionnelleAucune &&
                    identificationProfessionnelle != _labIdIdentificationProfessionnelleInconnue) {
                    showRequiredFieldWithAsterisk(personneMoraleLabNumeroImmatriculation + i);
                }

                showRequiredFieldWithAsterisk(personneMoraleLabProfessionalIdentificationId + i);

            }
        }
    }
    else {
        if (nbPp > 0) {
            for (let i = 0; i < nbPp; i++) {

                var personnePhysiqueLabPpeIdIdAsterisk = getRequiredFieldAsterisk(personnePhysiqueLabPpeId + i);
                disableFieldValidators(personnePhysiqueLabPpeId + i, personnePhysiqueLabPpeIdIdAsterisk);

                var personnePhysiqueLabTypeImplicationIdAsterisk = getRequiredFieldAsterisk(personnePhysiqueLabTypeImplicationId + i);
                disableFieldValidators(personnePhysiqueLabTypeImplicationId + i, personnePhysiqueLabTypeImplicationIdAsterisk);
            }
        }
        if (nbPm > 0) {
            for (let i = 0; i < nbPm; i++) {

                showRequiredFieldWithAsterisk(personneMoraleLabRaisonSociale + i);

                var personneMoraleLabTypeImplicationIdAsterisk = getRequiredFieldAsterisk(personneMoraleLabTypeImplicationId + i);
                disableFieldValidators(personneMoraleLabTypeImplicationId + i, personneMoraleLabTypeImplicationIdAsterisk);

                var personneMoraleLabNumeroImmatriculationAsterisk = getRequiredFieldAsterisk(personneMoraleLabNumeroImmatriculation + i);
                disableFieldValidators(personneMoraleLabNumeroImmatriculation + i, personneMoraleLabNumeroImmatriculationAsterisk);

                var personneMoraleLabProfessionalIdentificationIdAsterisk = getRequiredFieldAsterisk(personneMoraleLabProfessionalIdentificationId + i);
                disableFieldValidators(personneMoraleLabProfessionalIdentificationId + i, personneMoraleLabProfessionalIdentificationIdAsterisk);
            }

        }
    }
}

function btnClotureTracfin_OnClick() {
    if (shouldAlertUserToEmptyPiecesJointesEnvoiTracfin()) {
        showConfirmPopup(_labTranslatableMessageWarning,
            _labTranslatableWarningMessageNoPiecesJointesEnvoiTracfin,
            showClotureTracfinPopup);

        return;
    }
    showClotureTracfinPopup();
}
function showClotureTracfinPopup() {
    var directionId = $("#DirectionId").dxSelectBox("instance").option('value');
    $.ajax({
        method: "GET",
        cache: false,
        data: directionId,
        url: '/' + culture + '/Lab/ServiceLab/GetClotureTracfinPartialPopup?directionId=' + directionId,
    }).done(function (data) {
        var popup = $("#clotureTracfinModal").dxPopup('instance');
        popup.option('contentTemplate', function (contentTemplate) {
            contentTemplate.append(data);
        });
        popup.show();
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
    });
}

function fermerClotureTracfin_OnClick() {
    fermerClotureTracfin_OnClickPreview();
}

function fermerClotureTracfin_OnClickPreview() {
    if ($("#clotureTracfinModal")) {
        $("#clotureTracfinModal").dxPopup('instance').content().empty();
        $("#clotureTracfinModal").dxPopup('instance').hide();

    }
}

function validatePaysByCategory(fieldId) {
    var categorieId = "";
    if ($("#CategorieId").dxSelectBox("instance").option('selectedItem') != null) {
        categorieId = $("#CategorieId").dxSelectBox("instance").option("selectedItem").Code;
    }
    if (categorieId != "" && (categorieId == "140" || categorieId == "148")) {
        showRequiredFieldWithAsterisk(fieldId);
    }
    else {
        disableFieldValidators("#PaysId");
    }
}

function onCategorieContributeurValueChange($element, initialLoad) {
    var selectedItem = $element.dxSelectBox("instance").option("selectedItem");

    if (isNullOrUndefined(selectedItem)) {
        return;
    }

    var suffix = $element.attr("id").replace(_labSelectBoxCategorieContributeurPrefix, "");

    $("#" + _labDivSelectContributeurPrefix + suffix).show();

    var categoryCode = selectedItem.Code;

    var isCategorieAutreSelected = categoryCode == _labCodeCategorieContributeurAutre;

    toggleContainerIncludingValidators("#" + _labDivSelectBoxContributeurPrefix + suffix, !isCategorieAutreSelected);

    toggleContainerIncludingValidators("#" + _labDivAutreContributeurPrefix + suffix, isCategorieAutreSelected);

    var selectBoxContributeurs = $("#" + _labSelectBoxContributeurPrefix + suffix).dxSelectBox("instance");

    if (categoryCode != _labCodeCategorieContributeurAutre) {
        var dataSourceContributeurs = selectBoxContributeurs.getDataSource();

        dataSourceContributeurs.filter(["Categorie.Code", "=", categoryCode]);

        dataSourceContributeurs.load();
    }

    if (!initialLoad) {
        selectBoxContributeurs.reset();
    }
}

var lastValueChangedPromise = $.Deferred().resolve().promise();

function onContributeurChange(e) {
    if (isSubmitting) return;

    var deferred = $.Deferred();

    lastValueChangedPromise = deferred.promise();

    toggleEnableSectionInfosDSFTInputs(e.element, false);

    tryEnableAddNewContributeur(e.element.attr("id"), e.value);

    deferred.resolve();
}
function tryEnableAddNewContributeur(id, value) {
    var currentContributeurElement = $("#" + id);

    if (isNullOrWhiteSpace(id) ||
        (typeof value === "string" && isNullOrWhiteSpace(value)) ||
        (typeof value != "string" && isNullOrUndefined(value))) {
        toggleEnableSectionInfosDSFTInputs(currentContributeurElement, true);
        return;
    }

    var indexDeclarationTracfin = Number(id.split("_")[2])

    var $allContributeursItems = $("#" + _labDivPartialInformationInternesDSFTPrefix + indexDeclarationTracfin + " .contributor-item");

    var $visibleContributeursItems = $allContributeursItems.filter(":visible");

    var countVisibleContributeurs = $visibleContributeursItems.length;

    var indexContributeur = $visibleContributeursItems.index(currentContributeurElement.closest(".contributor-item"));

    if ((indexContributeur < countVisibleContributeurs - 1) || (countVisibleContributeurs == _labMaxNumberOfContributeurs)) {
        toggleEnableSectionInfosDSFTInputs(currentContributeurElement, true);
        return;
    }

    enableAddNewContributeur(indexDeclarationTracfin, $allContributeursItems.length);
}
function tryEnableAddNewContributeurNextTo(lastContributeurItem) {
    var selectedCategorieContributeur = lastContributeurItem
        .find("div[id^='" + _labSelectBoxCategorieContributeurPrefix + "']")
        .dxSelectBox("instance")
        .option("selectedItem");

    if (isNullOrUndefined(selectedCategorieContributeur)) {
        toggleEnableSectionInfosDSFTInputs(lastContributeurItem, true);
        return;
    }

    var isCategorieAutreSelected = selectedCategorieContributeur.Code == _labCodeCategorieContributeurAutre;

    var value;
    var prefix;

    if (isCategorieAutreSelected) {
        prefix = _labTextBoxAutreContributeurPrefix;

        value = lastContributeurItem
            .find("div[id^='" + prefix + "']")
            .dxTextBox("instance")
            .option("value")
    }
    else {
        prefix = _labSelectBoxContributeurPrefix;

        value = lastContributeurItem
            .find("div[id^='" + prefix + "']")
            .dxSelectBox("instance")
            .option("value");
    }

    var id = lastContributeurItem
        .find("div[id^='" + prefix + "']")
        .attr("id");

    tryEnableAddNewContributeur(id, value);
}
function enableAddNewContributeur(indexDeclarationTracfin, indexContributeur) {
    _labIsNewContributeur = true;
    var suffix = indexDeclarationTracfin + "_" + indexContributeur;
    var $divPartialInformationInternesDSFTElement = $("#" + _labDivPartialInformationInternesDSFTPrefix + indexDeclarationTracfin);
    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        url: '/' + culture + '/Lab/ServiceLab/EnableAddNewContributeur',
        data: { indexDeclarationTracfin, indexContributeur }
    }).done(function (data) {
        $divPartialInformationInternesDSFTElement.append(data);
        $("#" + _labDivAddContributeurPrefix + suffix).show();
        refreshIndexes($divPartialInformationInternesDSFTElement.attr("id"));
    }).fail(function (e) {
        if (e.status == 401) {
            var popup = $("#modalReconnect").dxPopup('instance');
            popup.show();
            return;
        }
        DevExpress.ui.notify(_labTranslatableMessageErrorAddContributeur, "error");
    }).always(function () {
        toggleEnableSectionInfosDSFTInputs($divPartialInformationInternesDSFTElement, true);
    });
}
function addNewContributeur(e) {
    var suffix = e.element.attr("id").replace(_labButtonAddContributorPrefix, "");

    $("#" + _labDivAddContributeurPrefix + suffix).hide();

    resetContributeurValidators(suffix);

    $("#" + _labDivDeleteCategoriePrefix + suffix).show();
}

function initContributeurValidators(suffix, isNewRequiredContributeur) {
    if (isNewRequiredContributeur) {
        resetContributeurValidators(suffix);
    }
    else if (_labIsNewContributeur) {
        toggleContainerIncludingValidators("#" + _labDivContributeurPrefix + suffix, false);
    }
}
function resetContributeurValidators(suffix) {
    toggleContainerIncludingValidators("#" + _labDivContributeurPrefix + suffix, true);

    toggleContainerIncludingValidators("#" + _labDivSelectContributeurPrefix + suffix, false);
}
function onBtnDeleteCategorieClick(e) {
    toggleEnableSectionInfosDSFTInputs(e.element, false);

    var currentContributeurItem = e.element.closest(".contributor-item");

    removeContributeur(currentContributeurItem);

    var $divPartialInformationInternesDSFTElem = currentContributeurItem
        .closest("[id^='" + _labDivPartialInformationInternesDSFTPrefix + "']");

    refreshIndexes($divPartialInformationInternesDSFTElem.attr("id"));

    var lastContributeurItem = $divPartialInformationInternesDSFTElem
        .find(".contributor-item:visible")
        .last();

    tryEnableAddNewContributeurNextTo(lastContributeurItem);
}
function removeContributeur(contributeurItemElem) {
    contributeurItemElem.addClass("hidden");

    toggleAllFieldValidators("#" + contributeurItemElem.attr("id"), false);

    contributeurItemElem.find("[id^='" + _labHiddenIsDeletedContributeurPrefix + "']").val(true);
}
function refreshIndexes(id) {
    $("#" + id + " .contributor-item:visible").each(function (index) {
        $(this).find(".index").text((index + 1).toString());
    });
}

function getNombreCategorieSelectedInOtherSelectboxes(selectBoxElement, code) {
    var $categoriesContributeurs = selectBoxElement
        .closest("[id^='" + _labDivPartialInformationInternesDSFTPrefix + "']")
        .find(".contributor-item:visible div[id^='" + _labSelectBoxCategorieContributeurPrefix + "']");

    var nombreCategorieSelected = 0;

    $categoriesContributeurs.each(function () {
        var suffix = $(this).attr("id").replace(_labSelectBoxCategorieContributeurPrefix, "");
        if (suffix == selectBoxElement.attr("id").replace(_labSelectBoxCategorieContributeurPrefix, "")) {
            return;
        }
        var selectedCategorie = $(this).dxSelectBox("instance").option("selectedItem");

        if (!isNullOrUndefined(selectedCategorie) && selectedCategorie.Code == code) {
            nombreCategorieSelected++;
        }
    });
    return nombreCategorieSelected;
}

function toggleEnableSectionInfosDSFTInputs(childElement, isEnabled) {
    var $divPartialInformationInternesDSFTElem = childElement
        .closest("[id^='" + _labDivPartialInformationInternesDSFTPrefix + "']");

    var dxButtons = $divPartialInformationInternesDSFTElem
        .find(".dx-button:visible");

    dxButtons.each(function () {
        const buttonInstance = DevExpress.ui.dxButton.getInstance(this);
        if (buttonInstance) {
            buttonInstance.option("disabled", !isEnabled);
        }
    });

    var autresContributeursInputs = $divPartialInformationInternesDSFTElem.find("div[id^='" + _labTextBoxAutreContributeurPrefix + "']");
    autresContributeursInputs.each(function () {
        const textBoxInstance = DevExpress.ui.dxTextBox.getInstance(this);
        if (textBoxInstance) {
            textBoxInstance.option("disabled", !isEnabled);
        }
    });
}

function enableDeleteExistingOptionalContributeurs(id) {
    $("#" + id).find(".contributor-item:visible")
        .slice(_labMinNumberOfContributeurs)
        .find("div[id^='" + _labDivDeleteCategoriePrefix + "']")
        .show();
}
function toggleSectionInformationInternesDsFt(isVisible) {
    var divInfoDsftElemSelector = "div[id^='" + _labDivPartialInformationInternesDSFTPrefix + "']";
    if (isVisible) {
        $(divInfoDsftElemSelector).show();

        resetNatureDsValidator();

        resetAllContributeurValidators();
        $(divInfoDsftElemSelector).each(function () {
            onAllCategorieContributeursValuesChange($(this).attr("id"));
        });
    }
    else {
        toggleContainerIncludingValidators(divInfoDsftElemSelector, false);
    }
}
function resetAllContributeurValidators() {
    $(".contributor-item:visible").each(function () {
        if ($(this).find("div[id^='" + _labDivAddContributeurPrefix + "']").is(":visible"))
            return;

        var suffix = $(this).attr("id").replace(_labDivContributeurItemPrefix, "");
        resetContributeurValidators(suffix);
    });
}
function resetNatureDsValidator() {
    toggleContainerIncludingValidators("div[id^='" + _labDivNatureDsPrefix + "']", true);
}

function hideAllEditContributeursDsftButtons() {
    $("div[id^='" + _labDivAddContributeurPrefix + "']").hide();
    $("div[id^='" + _labDivDeleteCategoriePrefix + "']").hide();
}

function setFieldsValidators() {
    setRelationAffaireValidator();
    setMotifSoupconValidator();
    setDateDeclarationLocaleValidator();

}

function setRelationAffaireValidator() {

    $('.PersonneMoraleLabTypeRelationAffaireLabId').each(function () {
        var natureRelationClient = $(this).closest('.tab-pane').find('div[id^="PersonneMoraleLabNatureRelationClientId"]');
        if (!natureRelationClient.length) return;
        var natureRelationClientSelectBox = natureRelationClient.dxSelectBox('instance');
        if (isNullOrUndefined(natureRelationClientSelectBox)) return;
        var validationRules;
        if (natureRelationClientSelectBox.option('value') == 1 && _labIsDeclarationTracfin) {
            validationRules = [{
                type: 'required',
                message: _labTranslatableMessageTypeRelationAffaireObligatoire
            }];

        } else {
            validationRules = [];
        }
        $(this).dxValidator({
            validationRules
        });

    });
    $('.PersonnePhysiqueLabTypeRelationAffaireLabId').each(function () {
        var natureRelationClient = $(this).closest('.tab-pane').find('div[id^="PersonnePhysiqueLabNatureRelationClientId"]');
        if (!natureRelationClient.length) return;
        var natureRelationClientSelectBox = natureRelationClient.dxSelectBox('instance');
        if (isNullOrUndefined(natureRelationClientSelectBox)) return;
        var validationRules;
        if (natureRelationClientSelectBox.option('value') == 1 && _labIsDeclarationTracfin) {
            validationRules = [{
                type: 'required',
                message: _labTranslatableMessageTypeRelationAffaireObligatoire
            }];

        } else {
            validationRules = [];
        }
        $(this).dxValidator({
            validationRules
        });
    });

}

function setMotifSoupconValidator() {
    $("#MotifsSoupcons").dxValidator({
        validationRules: [{
            type: 'custom',
            message: _labTranslatableMessageMotifsSoupconsRequired,
            validationCallback: validateMotifSoupcon
        }]
    });
}

function setDateDeclarationLocaleValidator() {
    $("#DateDeclarationLocale").dxValidator({
        validationRules: [{
            type: 'custom',
            message: _labTranslatableMessageDateDeclarationLocaleRequired,
            validationCallback: validateDateDeclarationLocale
        }]
    });
}

function validateMotifSoupcon() {
    var statutDossierId = $("#StatutDossierId")
        .dxSelectBox("instance")
        .option("value");

    if ($("div[id='MotifsSoupcons']").hasClass("mandatory") && statutDossierId == 2) {
        return !isHtmlEditorEmpty("MotifsSoupcons");
    }
    return true;
}

function validateDateDeclarationLocale() {
    var statutDossierId = $("#StatutDossierId")
        .dxSelectBox("instance")
        .option("value");

    if ($("div[id='DateDeclarationLocale']").hasClass("mandatory") && statutDossierId == 2) {
        return !isNullOrUndefined($("#DateDeclarationLocale").dxDateBox("instance").option("value"));
    }
    return true;
}

function dateDebutFinAnalyseDeclarationTracfinRequired(order) {
    var statutDossierText = $("#StatutDossierId").dxSelectBox("instance").option("text");
    var statutDossierValue = $("#StatutDossierId").dxSelectBox("instance").option("value");
    var isStatutAttenteEnvoieTracfin = _Lang == "Fr" ? statutDossierText == "Attente d'envoi TRACFIN"
        : statutDossierText == "Wating for sending to TRACFIN";
    var isStatutAttenteValidation = statutDossierValue == 6;

    if (isStatutAttenteEnvoieTracfin || isStatutAttenteValidation) {
        $("#DebutPeriodeFaitsConsideres" + order).addClass('mandatory');
        $("#debutPeriodeFaitsConsideresLabel").append('<span style="color:red">*</span>');
        $("#DebutPeriodeFaitsConsideres" + order).dxValidator({
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageDateDebutRequied
            }]
        });

        $("#FinPeriodeFaitsConsideres" + order).addClass('mandatory');
        $("#finPeriodeFaitsConsideresLabel").append('<span style="color:red">*</span>');
        $("#FinPeriodeFaitsConsideres" + order).dxValidator({
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageDateFinRequied
            }]
        });

        $("#DeclarationTracfinsAnalyses" + order).addClass('mandatory');
        $("#declarationTracfinsAnalysesLabel").append('<span style="color:red">*</span>');
        $("#DeclarationTracfinsAnalyses" + order).dxValidator({
            validationRules: [{
                type: 'required',
                message: _labTranslatableMessageAnalyseRequied
            }]
        });
    } else {
        $("#DebutPeriodeFaitsConsideres" + order).removeClass('mandatory');
        $("#debutPeriodeFaitsConsideresLabel span").remove();
        disableFieldValidators("#DebutPeriodeFaitsConsideres" + order);

        $("#FinPeriodeFaitsConsideres" + order).removeClass('mandatory');
        $("#finPeriodeFaitsConsideresLabel span").remove();
        disableFieldValidators("#FinPeriodeFaitsConsideres" + order);

        $("#DeclarationTracfinsAnalyses" + order).removeClass('mandatory');
        $("#declarationTracfinsAnalysesLabel span").remove();
        disableFieldValidators("#DeclarationTracfinsAnalyses" + order);

    }
}

function btnViewContentSelected_Lab_OnClick() {
    var dataGrid = null;

    if ($("#gridDossiersLabWithTiers").is(":visible")) {
        dataGrid = $("#gridDossiersLabWithTiers").dxDataGrid("instance");
    }
    else if ($("#gridDossiersLabWithNoTiers").is(":visible")) {
        dataGrid = $("#gridDossiersLabWithNoTiers").dxDataGrid("instance");
    }

    var cryptedDossierId = dataGrid.getSelectedRowsData()[0].CryptedId;

    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        url: '/' + culture + '/Lab/ServiceLab/ViewContent?cryptedDossierId=' + cryptedDossierId
    })
        .done(function (htmlContent) {
            showScrollablePopup(_labViewContentPopupId, htmlContent);
        })
        .fail(function (e) {
            if (e.status == 401) {
                DevExpress.ui.notify(_commonTranslatableMessageOperationNonAutorise, "error");
                return;
            }
            DevExpress.ui.notify(_commonTranslatableMessageErrorLoadForm, "error");
        });
}




//function addTooltipIconOnCellPrepared(e) {
//    if (!listeQlb) {
//        console.warn("listeQlb n'est pas défini");
//        return;
//    }

//    if (e.area === "row" && e.cellElement) {
//        for (const [key, value] of Object.entries(listeQlb)) {
//            var text = e.cell.text;

//            // Vérifier si le texte correspond exactement à la clé
//            if (typeof text === "string" && text.trim() === key.trim()) {
//                console.log("Match trouvé pour la clé:", key, "valeur:", value);

//                // Créer un ID unique pour cette tooltip
//                const tooltipId = "tooltip-" + (tooltipCounter++);
//                tooltipStorage[tooltipId] = value;

//                const icon = $("<i>")
//                    .addClass("fas fa-info-circle infoTooltipTarget")
//                    .attr("data-tooltip-id", tooltipId)
//                    .css({
//                        marginLeft: "3px",
//                        cursor: "pointer",
//                        color: "#177775",
//                        display: "inline-block",
//                        fontSize: "16px"
//                    });
//                e.cellElement.append(icon);
//            }
//        }
//    }
//}

function addTooltipIconOnCellPrepared(e) {
    if (!listeQlb) {
        return;
    }

    if (e.area === "row" && e.cellElement) {
        for (const [key, value] of Object.entries(listeQlb)) {
            var text = e.cell.text;

            // Vérifier si le texte correspond exactement à la clé
            if (typeof text === "string" && text.trim() === key.trim()) {
                const icon = document.createElement("i");
                icon.className = "fas fa-info-circle infoTooltipTarget";
                icon.style.marginLeft = "3px";
                icon.style.cursor = "pointer";
                icon.style.color = "#177775";
                icon.style.display = "inline-block";
                icon.style.fontSize = "16px";
                icon.setAttribute("data-tooltip-value", value);

                // Convertir e.cellElement en élément DOM s'il est jQuery
                const cellElement = e.cellElement instanceof jQuery ? e.cellElement[0] : e.cellElement;
                cellElement.appendChild(icon);
            }
        }
    }
}


let currentTooltip = null;
let scrollTimeout = null;

function showTooltipPopover(element, content) {
    hideTooltipPopover();

    // Créer la tooltip
    const tooltip = $("<div class='custom-tooltip'>");

    // Utiliser text() au lieu de .text(content) pour éviter les injections XSS
    tooltip.text(content);

    tooltip.css({
        position: "fixed",
        backgroundColor: "#fff",
        border: "2px solid #177775",
        borderRadius: "6px",
        padding: "12px 16px",
        boxShadow: "0 4px 12px rgba(0,0,0,0.2)",
        zIndex: 10000,
        maxWidth: "400px",
        wordWrap: "break-word",
        lineHeight: "1.5",
        fontSize: "14px",
        color: "#333",
        pointerEvents: "auto"
    });

    $("body").append(tooltip);
    currentTooltip = tooltip;

    // Fonction pour positionner la tooltip
    function positionTooltip() {
        const elementOffset = $(element).offset();
        const tooltipWidth = tooltip.outerWidth();
        const tooltipHeight = tooltip.outerHeight();
        const windowWidth = $(window).width();
        const padding = 10;

        // Convertir en coordonnées viewport (soustraire le scroll)
        const scrollTop = $(window).scrollTop();
        const scrollLeft = $(window).scrollLeft();

        // Calculer la position horizontale
        let left = elementOffset.left - scrollLeft - tooltipWidth / 2 + 10;

        // Vérifier si la tooltip dépasse à gauche
        if (left < padding) {
            left = padding;
        }

        // Vérifier si la tooltip dépasse à droite
        if (left + tooltipWidth > windowWidth - padding) {
            left = windowWidth - tooltipWidth - padding;
        }

        // Calculer la position verticale
        let top = elementOffset.top - scrollTop - tooltipHeight - 10;

        // Vérifier si la tooltip dépasse en haut
        if (top < padding) {
            top = elementOffset.top - scrollTop + 30;
        }

        tooltip.css({
            top: top + "px",
            left: left + "px"
        });
    }

    // Fonction debounce pour le scroll
    function onScroll() {
        clearTimeout(scrollTimeout);
        scrollTimeout = setTimeout(function () {
            if (currentTooltip) {
                positionTooltip();
            }
        }, 50);
    }

    // Positionner la tooltip initialement
    positionTooltip();

    // Écouter les événements scroll avec debounce
    $(window).on("scroll.tooltip", onScroll);
    $("#pivotGridDossiersLabQLBReporting").on("scroll.tooltip", onScroll);

    // Garder la tooltip visible au survol
    tooltip.hover(
        function () {
            clearTimeout(tooltip.data("hideTimeout"));
        },
        function () {
            hideTooltipPopover();
        }
    );
}

function hideTooltipPopover() {
    if (currentTooltip) {
        console.log("Masquage de la tooltip");
        // Retirer les événements scroll
        $(window).off("scroll.tooltip");
        $("#pivotGridDossiersLabQLBReporting").off("scroll.tooltip");
        clearTimeout(scrollTimeout);
        currentTooltip.fadeOut(200, function () {
            $(this).remove();
        });
        currentTooltip = null;
    }
}

var listeQlb = null;
function chargeListeQlb() {
    return $.ajax({
        method: "GET",
        cache: false,
        dataType: "json",
        url: '/' + culture + '/Lab/ServiceLab/GetQlbTraductionTemplates',
        success: function (response) {
            listeQlb = response;
        }
    });
}

