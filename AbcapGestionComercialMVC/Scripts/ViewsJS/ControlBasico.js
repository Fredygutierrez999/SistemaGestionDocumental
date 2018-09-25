/**
 * ADMINISTRAR EMISOR
 */
function ConsultaListadoEmisores() {
    $.ajax({
        url: "/Basico/Emisor_Listados",
        type: "POST",
        data: $('#fmrConsultaEmisores').serialize(),
        dataType: "html",
        success: function (data) {
            $('#divDetalleEmisor').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}


/**
 * ADMINISTRAR EMISOR
 */
function AministrarEmisor(XIDEmisor) {
    $.ajax({
        url: "/Basico/AdministrarEmisor",
        type: "POST",
        data: "xID=" + XIDEmisor,
        dataType: "html",
        success: function (data) {
            $('#divCuerpoPopupAdministraEmisor').html(data);
            $('#popupAdministraEmisor').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}


/**
 * GUARDAR MODIFICAR EMISOR
 */
function GuardaModificarEmisor() {
    $.ajax({
        url: "/Basico/GuardaModificaEmisor",
        type: "POST",
        data: $('#fmrAdministrarEmisor').serialize(),
        dataType: "JSON",
        success: function (data) {
            if (data.ResultadoProceso) {
                $('#popupAdministraEmisor').modal('hide');
                ConsultaListadoEmisores();
                mostrarMensaje(1, data.MensajeProceso);
            } else {
                mostrarMensaje(2, data.MensajeProceso);
            }
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}