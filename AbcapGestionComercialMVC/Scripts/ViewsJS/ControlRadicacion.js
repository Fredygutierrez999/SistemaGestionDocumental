var lstSoportes = new Array();
/**
 * Carga listado de soportes
 */
function cargarListadoSoportes() {
    var conSoporte = false;
    var strHtmlSoportes = "";
    for (var i = 0; i < lstSoportes.length; i++) {
        if (lstSoportes[i].Eliminar == false) {
            strHtmlSoportes +=
                "<tr>" +
                "<td class=\"text-left\">" +
                "<small><a href=\"javascript:VerDocumento(" + lstSoportes[i].ID + "," + lstSoportes[i].Temporal + "," + lstSoportes[i].ContendorImagen + ")\">" + lstSoportes[i].Nombre + "</a></small>" +
                "</td >" +
                "<td class=\"text-center\">" +
                "<small>" + lstSoportes[i].Extension + "</small>" +
                "</td>" +
                "<td class=\"text-center\">" +
                "<small>" + lstSoportes[i].Tamanio + "</small>" +
                "</td>" +
                "<td class=\"text-center\">" +
                "<a href=\"javascript:eliminarDocumento(" + lstSoportes[i].ID + ")\" >Eliminar</a>" +
                "</td>" +
                "</tr >";
            conSoporte = true;
        }
    }
    $('#tblSoportes').html(strHtmlSoportes);
    if (conSoporte == true) {
        $('#tblConSoportes').css("display", "block");
        $('#tblSinDatosSoportes').css("display", "none");
    } else {
        $('#tblConSoportes').css("display", "none");
        $('#tblSinDatosSoportes').css("display", "block");
    }
}

/*
 * Funcion utuizada para guardar documento
 */
function guardarRadicacion() {
    var strHtmlSoportes = ",";
    for (var i = 0; i < lstSoportes.length; i++) {
        if (lstSoportes[i].Eliminar == true) {
            strHtmlSoportes += lstSoportes[i].ID + ",";
        }
    }
    $('#xIDArchivosEliminados').val(strHtmlSoportes);
    var _Data = $('#fmrRadicacion').serialize();

    $.ajax({
        url: "/Radicacion/Guardar",
        type: "POST",
        data: _Data,
        dataType: "JSON",
        success: function (data) {
            if (data.ResultadoProceso) {
                mostrarMensajeUrl(5, data.MensajeProceso, "./MisDocumentos?IDDocumento=" + data.objResultado);
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
 * Carga previsualizacion de archivo
 * @param {any} XIDArchivo ID archivo
 * @param {any} tipoArchivo Tipo archivo (false = Cargado, True = temporal)
 */
function VerDocumento(XIDArchivo, tipoArchivo, xTipoContenedor) {
    if (xTipoContenedor == 1) {
        $('#contenidoArchivo').html('<img src="CargaArchivo?xIDArchivo=' + XIDArchivo + '&xTipoArchivo=' + tipoArchivo + '" width="100%" height="100%" />');
        $('#popupDialogArchivo').css("width", "40%");
    } else {
        $('#contenidoArchivo').html('<iframe src="CargaArchivo?xIDArchivo=' + XIDArchivo + '&xTipoArchivo=' + tipoArchivo + '" width="900px" height="600px"></iframe>');
        $('#popupDialogArchivo').css("width", "950px");
    }
    $('#popupMuestraArchivo').modal('show');
}

/**
 * Eliminar item archivo
 * @param {any} xIDArchivo
 */
function eliminarDocumento(xIDArchivo) {
    $('#hdfArchivoAEliminar').val(xIDArchivo);
    $('#popupValidaEliminacionArchivo').modal("show");
}

/**
 * COnfirma eliminacion de archivo
 * */
function confirmaEliminacion() {
    var xIDArchivo = parseInt($('#hdfArchivoAEliminar').val());
    var objItem = lstSoportes.find(m => m.ID == xIDArchivo);
    objItem.Eliminar = true;
    cargarListadoSoportes();
    $('#popupValidaEliminacionArchivo').modal("hide");
}

/*
 * Busca documentos con filtros indicados
 * */
function consultarDetalle() {
    var xDataFMr = $('#fmrDetalleDocumento').serialize();
    $.ajax({
        url: "/Radicacion/MisDocumentos_Detalle",
        type: "POST",
        data: xDataFMr,
        dataType: "HTML",
        success: function (data) {
            $('#contenidoDetalle').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Muestra modal con dato de soportes abjuntos al documento
 * @param {any} xIDDocumento ID Documento
 * @param {any} data Datos
 */
function verSoportesDocumentos(xIDDocumento, data) {
    var lstData = JSON.parse(data);
    if (lstData.length > 0) {
        var strHtml = '';
        for (var i = 0; i < lstData.length; i++) {
            strHtml += '<tr>';
            strHtml += '<td><a href="javascript:VerDocumento(' + lstData[i].ID + ',' + lstData[i].Temporal + ',' + lstData[i].ContendorImagen + ')">' + lstData[i].Nombre + '</a></td>';
            strHtml += '<td>' + lstData[i].Extension + '</td>';
            strHtml += '<td>' + lstData[i].Tamanio + '</td>';
            strHtml += '</tr >';
        }
        $('#cuerpoSoportes').html(strHtml);
        $('#popupListadoDocumentos').modal('show');
    } else {
        mostrarMensaje(1, "El documento no posee documentos abjuntos.");
    }
}



/*
 * Busca documentos con filtros indicados
 * */
function consultarMovimiento(XIDDocumento) {
    $.ajax({
        url: "/Radicacion/MisDocumentos_Movimientos",
        type: "POST",
        data: "xIDDocumento=" + XIDDocumento,
        dataType: "HTML",
        success: function (data) {
            $('#divCuerpoDocumentoMovimientos').html(data);
            $('#popupListadoMovimientos').modal('show');
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Captura key press tecla
 * @param {any} txt
 */
function buscarEmisorKeyPres(e) {
    if (event.keyCode == 13) {
        var xFiltro = $('#txtBuscaEmisor').val();
        $.ajax({
            url: "/Basico/Emisor_ListadosJson",
            type: "POST",
            data: "xFiltro=" + xFiltro,
            dataType: "JSON",
            success: function (data) {
                if (data.ResultadoProceso) {
                    var strHtml = "";
                    for (var i = 0; i < data.objResultado.length; i++) {
                        strHtml += '<option value="' + data.objResultado[i].ID + '">' + data.objResultado[i].Nombre + '</option>';
                    }
                    $('#IDAppNetEmisor_ID').html(strHtml);
                } else {
                    mostrarMensaje(2, data.MensajeProceso);
                }
            },
            error: function (jqXHR, textStatus, error) {
                mostrarMensaje(2, error);
            }
        });
    }
}