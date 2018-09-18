
function ConsultaConsolidaciones() {

}

/**
 * Funcion utilizada para la creación y modificación del consolidado
 * @param {any} idConsolidacion
 */
function CreaModificaConsolidacion(idConsolidacion) {
    $.ajax({
        url: "/ForeCast/CreaModificaConsolidacion",
        type: "POST",
        data: "IDConsolidacion=" + idConsolidacion,
        dataType: "HTML",
        success: function (data) {
            $('#cuerpoCreaModificaConsolidacion').html(data);
            $('#popupCreaModificaConsolidacion').modal('show');
            if (parseInt($('#ID').val()) == -1) {
                generarNombreConsolidado();
            }
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Metodo encargado de generar nombre del consolidado
 */
function generarNombreConsolidado() {
    var strNombre = "Consolidado ";
    var strItemsSeleccionados = "";
    var strItemsSeleccionadosAnt = "";
    var lstItems = $('.chkSelec');
    for (var i = 0; i < lstItems.length; i++) {
        if ($(lstItems[i]).prop("checked") == true) {
            if (strItemsSeleccionados == "") {
                strItemsSeleccionados = strItemsSeleccionadosAnt;
            } else {
                strItemsSeleccionados += ", " + strItemsSeleccionadosAnt;
            }
            strItemsSeleccionadosAnt = $(lstItems[i]).attr("nombre")
        }
    }
    if (strItemsSeleccionados == "") {
        strItemsSeleccionados = strItemsSeleccionadosAnt;
    } else {
        strItemsSeleccionados += ", " + strItemsSeleccionadosAnt;
    }
    $('#Nombre').val(strNombre + " - " + strItemsSeleccionados);
}

/**
 * Funcion utilizada para guardar consolidacion
 */
function guardarModificarConsolidacion() {
    var objItem = null;
    var lstItems = $('.chkSelec');
    for (var i = 0; i < lstItems.length; i++) {
        if ($(lstItems[i]).prop("checked") == true) {
            objItem = lstItems[i];
        }
    }

    if (objItem != null) {

        $('#HdfForeCastSeleccionado').val($(objItem).attr("Valor"));
        var fmrData = $('#fmrConsolidacionForeCast').serialize();
        $.ajax({
            url: "/ForeCast/GuardarConsolidacion",
            type: "POST",
            data: fmrData,
            dataType: "json",
            success: function (data) {
                if (data.ResultadoProceso) {
                    consultarListadoCOnsolidado();
                    mostrarMensaje(1, data.MensajeProceso);
                    $('#popupCreaModificaConsolidacion').modal('hide');
                } else {
                    mostrarMensaje(2, data.MensajeProceso);
                }
            },
            error: function (jqXHR, textStatus, error) {
                mostrarMensaje(2, error);
            }
        });


    } else {
        mostrarMensaje(2, "Debe seleccionar un item a consolidar.");
    }

}

/**
 * Funcion encargada de consolidar ForeCast
 */
function ProcesarConsolidadoForeCast(xIdProyeccion) {
    $.ajax({
        url: "/ForeCast/iniciaProcesoConsolidacion",
        type: "POST",
        data: "xIDForeCast=" + xIdProyeccion,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                consultarListadoCOnsolidado();
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
 * Metodo utilizado para consultar listado de consolidaciones
 */
function consultarListadoCOnsolidado() {
    var xFiltros = $('#fmrConsultaConsolidacion').serialize();
    $.ajax({
        url: "/ForeCast/consultaListadoConsolidado",
        type: "POST",
        data: xFiltros,
        dataType: "HTML",
        success: function (data) {
            $('#DetalleConsolidacion').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Funcion encargada de habilitar botones de accion
 */
function SeleccionarItem() {
    var lstCheckBox = $('.chkSeleccion');
    var lstSeleccionados = new Array();
    var bolEliminar = true;
    var bolGenerarResumen = true;
    var bolExportar = true;
    for (var i = 0; i < lstCheckBox.length; i++) {
        if ($(lstCheckBox[i]).prop('checked') == true) {
            lstSeleccionados.push(lstCheckBox[i]);
            var IDEstado = parseInt($(lstCheckBox[i]).attr('IDEstado'));
            bolEliminar = bolEliminar == true && IDEstado != 7; //7 es exportado
            bolGenerarResumen = bolGenerarResumen == true && (IDEstado == 6 || IDEstado == 7); //7 es exportado y VALIDADO
            bolExportar = bolExportar == true && IDEstado == 6; //7 es VALIDADO
        }
    }
    if (lstSeleccionados.length > 0) {
        if (bolEliminar == true) {
            $('#btnEliminar').removeAttr("disabled");
        } else {
            $('#btnEliminar').attr("disabled", "disabled");
        }
        if (bolGenerarResumen == true) {
            $('#btnGenerarExcel').removeAttr("disabled");
        } else {
            $('#btnGenerarExcel').attr("disabled", "disabled");
        }
        if (bolExportar == true) {
            $('#btnExportar').removeAttr("disabled");
        } else {
            $('#btnExportar').attr("disabled", "disabled");
        }
    } else {
        $('#btnEliminar').attr("disabled", "disabled");
        $('#btnGenerarExcel').attr("disabled", "disabled");
        $('#btnExportar').attr("disabled", "disabled");
    }
}

/**
 * Funcion utilizada para eliminar consolidaciones seleccionadas
 */
function eliminarConsolidacionesSeleccionadas() {
    var lstCheckBox = $('.chkSeleccion');
    var lstSeleccionados = new Array();
    var strItemSeleccionados = ",";
    for (var i = 0; i < lstCheckBox.length; i++) {
        if ($(lstCheckBox[i]).prop('checked') == true) {
            lstSeleccionados.push(lstCheckBox[i]);
        }
    }

    if (lstSeleccionados.length > 0) {

        for (var i = 0; i < lstSeleccionados.length; i++) {
            strItemSeleccionados += $(lstSeleccionados[i]).attr("value") + ",";
        }

        $.ajax({
            url: "/ForeCast/eliminarConsolidadosDeForeCast",
            type: "POST",
            data: "xItemsSeleccionados=" + strItemSeleccionados,
            dataType: "JSON",
            success: function (data) {
                if (data.ResultadoProceso) {
                    consultarListadoCOnsolidado();
                    mostrarMensaje(1, data.MensajeProceso);
                } else {
                    mostrarMensaje(2, data.MensajeProceso);
                }
            },
            error: function (jqXHR, textStatus, error) {
                mostrarMensaje(2, error);
            }
        });



    } else {
        mostrarMensaje(2, "Debe seleccionar un ítem de la lista.");
    }
}


/**
 * Funcion encargada de generar consolidación de datos
 * @param {any} xIDConsolidacion
 */
function VerErroresConsolidacion(xIDConsolidacion) {
    $.ajax({
        url: "/ForeCast/consultarListadoErroresConsolidacion",
        type: "POST",
        data: "xIDConsolidacion=" + xIDConsolidacion,
        dataType: "html",
        success: function (data) {
            $('#cuerpoValidacionesConsolidado').html(data);
            $('#popupValidacionesConsolidado').modal('show');
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}