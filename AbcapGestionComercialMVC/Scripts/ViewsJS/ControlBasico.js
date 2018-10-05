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



/**
 * ADMINISTRAR EMISOR
 */
function ConsultaListadoEstadoFlujo() {
    $.ajax({
        url: "/Basico/FlujoDocumental_Listados",
        type: "POST",
        data: $('#fmrConsultaFlujoDocumental').serialize(),
        dataType: "html",
        success: function (data) {
            $('#divDetalleFlujoDocumental').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Administrar estado
 * @param {any} xID
 */
function AministrarEstado(xID) {
    $.ajax({
        url: "/Basico/FlujoDocumentalAdministrador",
        type: "POST",
        data: "xID=" + xID,
        dataType: "html",
        success: function (data) {
            $('#popupAdministradorFlujo').modal("show");
            $('#divCuerpoPopupAdministraEstado').html(data);
            AministrarEstado_Listado();
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Administrar estado
 * @param {any} xID
 */
function AministrarEstado_Listado() {
    var strGuid = $('#hdfGuid').val();
    $.ajax({
        url: "/Basico/FlujoDocumentalAdministrador_Responsables",
        type: "POST",
        data: "xIDGuid=" + strGuid,
        dataType: "html",
        success: function (data) {
            $('#divAccionesEstado').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}


/**
 * Administrar estado
 * @param {any} xID
 */
function AministrarEstado_Acciones(XID) {
    var strGuid = $('#hdfGuid').val();
    $.ajax({
        url: "/Basico/FlujoDocumentalAdministrador_XAccion",
        type: "POST",
        data: "xIDGuid=" + strGuid + "&xID=" + XID,
        dataType: "html",
        success: function (data) {
            $('#divCuerpoPopupAdministraEstado_Accion').html(data);
            $('#popupAdministradorFlujo_Accion').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}


/**
 * Administrar estado
 * @param {any} xID
 */
function guardarAccionTemporal() {
    var strAccionTemporal = $('#fmrAccionTemporal').serialize();
    $.ajax({
        url: "/Basico/GuardarAccionTemporal",
        type: "POST",
        data: strAccionTemporal,
        dataType: "JSON",
        success: function (data) {
            if (data.ResultadoProceso) {
                $('#popupAdministradorFlujo_Accion').modal('hide');
                AministrarEstado_Listado();
            } else {
                mostrarMensaje(2, data.MensajeProceso);
            }
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}