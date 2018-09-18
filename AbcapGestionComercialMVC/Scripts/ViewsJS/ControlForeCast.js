var eventoControlado = false;
var controlCambia = "";
var ItemSeleccionado = null;
var lstItemsSeleccionados = [];
var cantidadRegistrosGlobal = 20;
var conFocoAparte = false;
/**
 * CONSULTA ROLES
 */
function ConsultaForeCast() {
    $.ajax({
        url: "/ForeCast/ConsultaListadoForeCast",
        type: "POST",
        data: $('#fmrConsultaForeCast').serialize(),
        dataType: "html",
        success: function (data) {
            $('#DetalleForeCast').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * CONSULTA ROL SI EXISTE SI NO LLENA LA ESTRUCTURA CON INFORMACIÓN BÁSICA
 * @param {any} xIDForeCast
 */
function CreaModificaForeCast(xIDForeCast) {
    $.ajax({
        url: "/ForeCast/AdministrarForeCast",
        type: "POST",
        data: "xIDForeCast=" + xIDForeCast,
        dataType: "html",
        success: function (data) {
            $('#cuerpoAdministraForeCast').html(data);
            $('#popupAdministrarForeCast').modal("show");
            cambiEstadoAntriorForeCast();
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Cambia semana inicial
 */
function cargaSemanaPorAnio(obj, xcontrolCambia) {
    controlCambia = xcontrolCambia;
    var xAnio = $(obj).val();
    $.ajax({
        url: "/ForeCast/consultaSemanaPorAnio",
        type: "POST",
        data: "xAnio=" + xAnio,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                var strHtml = "";
                for (var i = 0; i < data.objResultado.length; i++) {
                    var _item = data.objResultado[i];
                    strHtml += '<option value="' + _item.ID + '">' + _item.Nombre + '</option>';
                }
                $(controlCambia).html(strHtml);
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
 * Genera nombre autormatico
 */
function cargaNombreAutomaticoForeCast() {
    var xbasTipoDato = $('#Bas_TipoDatoPYG_ID').val();
    var xbasSemanaInicial = $('#AnioSemanaInicial_ID').val();
    var xbasSemanaFinal = $('#AnioSemanaFinal_ID').val();

    $.ajax({
        url: "/ForeCast/crearNombreAutomaticoFcts",
        type: "POST",
        data: "idBasTipoDatoPyG=" + xbasTipoDato + "&xSemanaInicia=" + xbasSemanaInicial + "&xSemanaFinal=" + xbasSemanaFinal,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                $('#Nombre').val(data.MensajeProceso);
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
 * Funcion utilizada para iniciar importacion de datos del foreCast
 */
function iniciaImportacionDatos() {
    var frmSerializado = $('#fmrIniciaImportacionDatos').serialize();

    $.ajax({
        url: "/ForeCast/procesarImportacionDatos",
        type: "POST",
        data: frmSerializado,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                var lstArray = new Array();
                lstArray = data.objResultado;
                for (var i = 0; i < lstArray.length; i++) {

                    var _item = lstArray[i];
                    var _itemBD = _BDDetalle.find(m =>
                        m.IDcliente == _item.IDCliente &&
                        m.IDProductos == _item.IDProductos &&
                        m.IDPresentaciones == _item.IDPresentaciones
                        //  && m.IDTemporadasBase == _item.IDTemporadasBase
                    );


                    if (_itemBD != null) {
                        var _indexSemana = _columnas.findIndex(m => m.CampoNoDinamico == _item.NombreSemana) - 6;
                        _itemBD.MatSemanas[_indexSemana] = _item.CantidadCajas;
                    } else {
                        console.log(_item);
                    }
                }

                _IDUniversal = parseInt(data.MensajeProceso);

                actualizaGrid(_ultimoIndexSeleccionado);
                $('#popupImportarDatos').modal('hide');
                mostrarMensaje(1, "Importación exitosa.");

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
 * Cambia estado anterior forcas
 */
function cambiEstadoAntriorForeCast() {
    if ($('#CargarAnteriorForeCast').prop('checked')) {
        $('#IDForeCastAnterior').removeAttr('disabled');
    } else {
        $('#IDForeCastAnterior').attr('disabled', 'disabled');
    }
}

/**
 * GUARDA CAMBIOS SOBRE ROL
 */
function guardarForeCast() {

    $.ajax({
        url: "/ForeCast/guardarForeCast",
        type: "POST",
        data: $('#fmrGuardarForeCast').serialize(),
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso == true) {
                $('#popupAdministrarForeCast').modal("hide");
                ConsultaForeCast();
                mostrarMensaje(1, data.MensajeProceso);
            } else {
                mostrarMensaje(2, data.objResultado);
            }
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });

}



/**
 * GUARDA CAMBIOS SOBRE ROL
 */
function consultaMovimientosForeCast(IDForeCast) {
    $.ajax({
        url: "/ForeCast/consultaMovimientosForeCast",
        type: "POST",
        data: "IDForeCast=" + IDForeCast,
        dataType: "HTML",
        success: function (data) {
            $('#cuerpoContenidoMovimientos').html(data);
            $('#popupMoviemtosForeCast').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });

}

/**
 * Carga listado de archivos utilizados en la proyección
 * @param {any} IDForeCast
 */
var xVentanaMostrarventanaAdministradorArchivos = false;
function MostrarventanaAdministradorArchivos(IDForeCast) {
    $.ajax({
        url: "/ForeCast/administrarArchivosProyeccion",
        type: "POST",
        data: "xIDForeCast=" + IDForeCast,
        dataType: "HTML",
        success: function (data) {
            $('#ContenidoAdministracionArchivos').html(data);
            $('#popupArchivosForeCast').modal("show");
            if (xVentanaMostrarventanaAdministradorArchivos == false) {
                $('#popupSubirArchivoNuevo').on("show.bs.modal", function () {
                    $("#fileArchivos").fileinput({
                        uploadUrl: "/ForeCast/SubirArchivosForeCast?IDForeCast=" + IDForeCast,
                        uploadAsync: true,
                        previewFileIcon: '<i class="fa fa-file"></i>',
                        allowedPreviewTypes: null, // set to empty, null or false to disable preview for all types
                        previewFileIconSettings: {
                            'txt': '<span class="glyphicon glyphicon-file" aria-hidden="true"></span>',
                        },
                        previewFileExtSettings: {
                            'txt': function (ext) {
                                return ext.match(/(txt|ini|md)$/i);
                            },
                        },
                        minFileCount: 1,
                        maxFileCount: 1,
                        validateInitialCount: true,
                        language: 'es',
                        showClose: false,
                        tActionUpload: '',
                        maxImageHeight: 100,
                        minImageHeight: 100
                    }
                    ).on('fileuploaded', function (event, files, extra) {
                        MostrarventanaAdministradorArchivos($('#hdfForeCast').val());
                        $('#popupSubirArchivoNuevo').modal('hide');
                    });;
                }).on('filepreupload', function (event, data, previewId, index) {
                    data.form.append('ddlTipoArchivo', $("#ddlTipoArchivo").val());
                    data.form.append('txtObservaciones', $("#txtObservaciones").val());
                    data.form.append('txtFechaArchivo', $("#txtFechaArchivo").val());
                });;
                xVentanaMostrarventanaAdministradorArchivos = true;
            }
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Consolidar archivos
 */
function eliminarArchivos() {
    var lstItemseSeleccionados = $('.chkSeleccionarItems');
    var strCadena = ",";
    for (var i = 0; i < lstItemseSeleccionados.length; i++) {
        if ($(lstItemseSeleccionados[i]).prop("checked")) {
            strCadena += $(lstItemseSeleccionados[i]).val() + ",";
        }
    }
    if (strCadena != ",") {

        $.ajax({
            url: "/ForeCast/eliminarArchivo",
            type: "POST",
            data: "strIDArchivos=" + strCadena,
            dataType: "json",
            success: function (data) {
                if (data.ResultadoProceso == true) {
                    MostrarventanaAdministradorArchivos($('#hdfForeCast').val());
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
        mostrarMensaje(2, "Debe seleccionar un item de la lista");
    }
}


/**
 * Inicia validación archivo
 * @param {any} IDArchivo
 */
function ValidarArchivo(IDArchivo) {
    $.ajax({
        url: "/ForeCast/ValidarArchivo",
        type: "POST",
        data: "xIDArchivo=" + IDArchivo,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso == true) {
                MostrarventanaAdministradorArchivos($('#hdfForeCast').val());
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
 * Funcion utilizada para mostrar archivos
 * @param {any} xIDarchivo
 */
function mostrarErroresCargaArchivo(xIDarchivo) {
    $.ajax({
        url: "/ForeCast/ConsultaListadoValidacionArchivo",
        type: "POST",
        data: "xIDArchivoForeCast=" + xIDarchivo,
        dataType: "HTML",
        success: function (data) {
            $('#cuerpoValidacionesArchivo').html(data);
            $('#popupValidacionesArchivo').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}


/**
 * Carga listado de archivos utilizados en la proyección
 * @param {any} IDForeCast
 */
function subirNuevosArchivos() {
    $.ajax({
        url: "/ForeCast/iniciarNuevaSubida",
        type: "POST",
        dataType: "HTML",
        success: function (data) {
            $('#SubirArchivonuevo').html(data);
            $('#popupSubirArchivoNuevo').modal("show");

            $('#txtFechaArchivo').datetimepicker({
                viewMode: 'days',
                format: 'MM/DD/YYYY'
            });

        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}


/**
 * Inicia consolidado de datos para los archivos
 * @param {any} xIdForeCast
 */
function consolidarArchivos(xIdForeCast) {
    bloquear(true);
    $.ajax({
        url: "/ForeCast/validaSiProcesaWC",
        type: "POST",
        data: "xIDForeCast=" + xIdForeCast,
        dataType: "json",
        success: function (data) {

            if (data.ResultadoProceso) {
                $.ajax({
                    url: "/ForeCast/consolidarArchivos",
                    type: "POST",
                    data: "xIDForeCast=" + xIdForeCast,
                    dataType: "html",
                    success: function (data) {
                        bloquear(false);
                        $('#SeleccionarPrincipalArchivoWC').html(data);
                        $('#popupSeleccionarArchivoPrincipalWC').modal('show');
                    },
                    error: function (jqXHR, textStatus, error) {
                        bloquear(false);
                        mostrarMensaje(2, error);
                    }
                });
            } else {
                bloquear(false);
                generarConsolidadoArchivosSoloGG(data.objResultado);
            }

        },
        error: function (jqXHR, textStatus, error) {
            bloquear(false);
            mostrarMensaje(2, error);
        }
    });


}

/**
 * Generar consolidado de archivos
 */
function generarConsolidadoArchivosSoloGG(xIdForeCast) {

    bloquear(true);
    xIdArchivoPrincipal = -1;

    $.ajax({
        url: "/ForeCast/generarConsolidadoArchivos",
        type: "POST",
        data: "xIDForeCast=" + xIdForeCast + "&xIDArchivoPrincipal=" + xIdArchivoPrincipal,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                bloquear(false);
                $('#popupSeleccionarArchivoPrincipalWC').modal('hide');
                MostrarventanaAdministradorArchivos(data.objResultado);
            } else {
                bloquear(false);
                mostrarMensaje(2, data.MensajeProceso);
            }
        },
        error: function (jqXHR, textStatus, error) {
            bloquear(false);
            mostrarMensaje(2, error);
        }
    });

}

/**
 * Generar consolidado de archivos
 */
function generarConsolidadoArchivos() {

    xIdForeCast = $('#hdfXIDForeCast').val();
    xIdArchivoPrincipal = $('#ddlArchivoPrincipalWc').val();

    $.ajax({
        url: "/ForeCast/generarConsolidadoArchivos",
        type: "POST",
        data: "xIDForeCast=" + xIdForeCast + "&xIDArchivoPrincipal=" + xIdArchivoPrincipal,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                $('#popupSeleccionarArchivoPrincipalWC').modal('hide');
                MostrarventanaAdministradorArchivos(data.objResultado);
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
 * Muestra popup de validacion de eliminacion
 */
function validacionEliminacionConsolidacion() {
    $('#popupvalidacionEliminacion').modal('show');
}

/**
 * Ejecuta proceso de eliminacion de forecast
 */
function eliminarConsolidacionArchivo() {

    var xIDForeCast = $('#hdfForeCast').val();
    $.ajax({
        url: "/ForeCast/eliminarConsolidacionArchivo",
        type: "POST",
        data: "xIDForeCast=" + xIDForeCast,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                MostrarventanaAdministradorArchivos($('#hdfForeCast').val());
            } else {
                mostrarMensaje(2, data.MensajeProceso);
            }
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });

}



var anteriorTD = null;
var _columnas = new Array();
var _BDDetalle = new Array();
var _Clientes = new Array();
var _Productos = new Array();
var _Presentaciones = new Array();
var _Temporadas = new Array();
var _Periodos = new Array();
var _RegistroGuia = parseInt('0');
var _cabeceraHTML = '';
var _clickSostenido = false;
var _lstMovimiento = new Array();
var _IDUniversal = 0;
var xValorAnterior;
var _enSincronizacion = 0;
var _listadoSincronizado = Array();


/**
 * metodo encargado de calcular total por producto
 * @param {any} xTd
 */
function calcularTotalProducto(xTd) {
    var fila = $(xTd).parent();
    var iIndexFila = $(fila).index();
    var decTotal = parseFloat('0');
    $(fila).find('td').each(function () {
        decTotal += parseFloat($(this).html());
    });
    var iFila = $('#tblProyeccionVentaTotalesPRDO').find('tr')[iIndexFila + 1];
    $($(iFila).find('td')[0]).html(decTotal);
}

/**
 * Cambia el control de la celda de una tabla para empezar la edición de forma dinamica
 * @param {any} xTd
 */
function iniciaEdicion(xTd) {
    xValorAnterior = $(xTd).html();
    $(xTd).html("<input id='txttemporal'  onkeypress='return validaLetras(event);'  onkeydown='return evaluaLetra(this);' type='text' value='" + xValorAnterior + "' class='form-control form-control-sm'>")
    //$('#txttemporal').number(true, 4);
    $('#txttemporal').focus();
    $('#txttemporal').select();
    setTimeout(seleccionarTexto, 100);
}

/**
 * Valida que solo se ingresen letras
 * @param {any} e
 */
function validaLetras(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8 || tecla == 13 || tecla == 46) {
        return true;
    }
    // Patron de entrada, en este caso solo acepta numeros
    patron = /[0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

/**
 * Selecciona el texto del control txttemporal
 */
function seleccionarTexto() {
    $('#txttemporal').select();
}

/**
 * Optiene el texto seleccionado
 */
function getSelected() {
    var u = $('#txttemporal').val();
    var start = $('#txttemporal').get(0).selectionStart;
    var end = $('#txttemporal').get(0).selectionEnd;
    return [u.substring(0, start), u.substring(end), u.substring(start, end)];
}

/**
 * Retorna el obj Cliente
 */
function getCliente(IDCliente) {
    return this._Clientes.find(Item => Item.ID == IDCliente);
}

/*Retorna el obj Producto*/
function getProducto(IDProducto) {
    return this._Productos.find(Item => Item.ID == IDProducto);
}

/*Retorna el obj Presentacion */
function getPresentacion(IDPresentacion) {
    return this._Presentaciones.find(Item => Item.ID == IDPresentacion);
}

/*Retorna el obj Producto*/
function getTemporada(IDTemporada) {
    return this._Temporadas.find(Item => Item.ID == IDTemporada);
}

/*Retorna el obj Producto*/
function getPeriodo(IDPeriodo) {
    return this._Periodos.find(Item => Item.ID == IDPeriodo);
}

/*Retorna width columna*/
function getColumna(IDColumna) {
    return this._columnas.find(Item => Item.ID == IDColumna);
}

/*Funcion encargadda de cargar proyeccion detalle*/
function eventosGridProyeccionDetalle() {
    /*evento clicl sobre un td*/
    $('#tblProyeccionVentaDetalle tr .datoSemana').on('click', function (e) {
        if ($(this).attr('class').indexOf('datoSemana') != -1) {
            if (anteriorTD != null) {
                console.log($('#txttemporal').val());
                $(anteriorTD).html($('#txttemporal').val());
            }
            iniciaEdicion(this);
            anteriorTD = this;
        }
    });



}

/**
 * Carga listado de columnas
 */
function cargarListados() {
    /*CARGAR DATOS A CACHE*/
    if ($('#hdfDetalleProyeccion').val() != "") {
        this._columnas = JSON.parse($('#hdfColumnas').val());
        this._BDDetalle = JSON.parse($('#hdfDetalleProyeccion').val());
        this._Clientes = JSON.parse($('#hdfClientes').val());
        this._Productos = JSON.parse($('#hdfProductos').val());
        this._Presentaciones = JSON.parse($('#hdfPresentaciones').val());
        this._Temporadas = JSON.parse($('#hdfTemporadas').val());
        this._Periodos = JSON.parse($('#hdfPeriodos').val());

        /*CARGA CLIENTES*/
        var strOpciones = "";
        for (var intCliente = 0; intCliente < this._Clientes.length; intCliente++) {
            strOpciones += '<option value="' + this._Clientes[intCliente].ID + '">' + this._Clientes[intCliente].Nombre + '</option>';
        }
        $('#DromnDownClientes').html(strOpciones);

        /*CARGA PRODUCTOS*/
        strOpciones = "";
        for (var intProductos = 0; intProductos < this._Productos.length; intProductos++) {
            strOpciones += '<option value="' + this._Productos[intProductos].ID + '">' + this._Productos[intProductos].Nombre + '</option>';
        }
        $('#DromnDownProductos').html(strOpciones);

        /*CARGA PRESENTACIONES*/
        strOpciones = "";
        for (var intPresentacion = 0; intPresentacion < this._Presentaciones.length; intPresentacion++) {
            strOpciones += '<option value="' + this._Presentaciones[intPresentacion].ID + '">' + this._Presentaciones[intPresentacion].Nombre + '</option>';
        }
        $('#DromnDownPresentacion').html(strOpciones);

        /*CARGA TEMPORADA*/
        strOpciones = "";
        for (var intTemporada = 0; intTemporada < this._Temporadas.length; intTemporada++) {
            strOpciones += '<option value="' + this._Temporadas[intTemporada].ID + '">' + this._Temporadas[intTemporada].Nombre + '</option>';
        }
        $('#DromnDownTemporada').html(strOpciones);


        /*Columas */
        var strColumnasHTML = '';
        var strColumnasHTMLStatica = '';
        var strColumnasHTMLWIdth = '';
        var strFilasProyeccionResumenHTML = '';

        for (var intColumna = 0; intColumna < this._columnas.length; intColumna++) {

            strColumnasHTML +=
                '<td class="text-center" style="min-width:' +
                this._columnas[intColumna].WidthInicial + 'px" id="col_' +
                this._columnas[intColumna].ID + '" >' +
                this._columnas[intColumna].Nombre +
                (this._columnas[intColumna].Dinamico == true ?
                    '<span class="glyphicon glyphicon-filter pointer filtro_Columna " indexColumna="' + intColumna + '" aria-hidden="true"></span>' +
                    '<span class="glyphicon glyphicon-sort-by-attributes pointer " indexColumna="' + intColumna + '" aria-hidden="true"></span>'
                    : '') +
                '</td>';

            if (this._columnas[intColumna].CargaDinamica) {
                strColumnasHTMLStatica +=
                    '<td class="text-center columnF columnF_' + this._columnas[intColumna].ID + '" style="display:none;min-width:' +
                    this._columnas[intColumna].WidthInicial + 'px" id="col_' +
                    this._columnas[intColumna].ID + '" >' +
                    this._columnas[intColumna].Nombre +
                    '</td>';
            }

            strColumnasHTMLWIdth += '<td class="columna-conWidth column_' + intColumna + '" style="min-width:' + this._columnas[intColumna].WidthInicial + 'px" id="col_' + this._columnas[intColumna].ID + '" ></td>';
        }
        $('#tblColumnas').html('<tr>' + strColumnasHTML + '</tr>');
        $('.tbl-Cabeza-Flotante').html('<tr>' + strColumnasHTMLStatica + '</tr>');


        /*Detalle proyeccion*/
        this._cabeceraHTML = '<tr>' + strColumnasHTMLWIdth + '</tr>';
        actualizaGrid(0);

        calculatTotalCajas();

        /*LIBERA CODIGO HTML*/
        $('#hdfColumnas').val("");
        $('#hdfDetalleProyeccion').val("");
        $('#hdfClientes').val("");
        $('#hdfProductos').val("");
        $('#hdfPresentaciones').val("");
        $('#hdfTemporadas').val("");
        $('#hdfPeriodos').val("");
    }


    $('#DromnDownClientes').selectpicker({
        liveSearch: true,
        deselectAllText: "Deselec.",
        noneSelectedText: "Clientes",
        selectAllText: "Selec.",
        actionsBox: true
    });
    $('#DromnDownClientes').on('hidden.bs.select', function (e) {
        cambiaEstadoFiltro($('#DromnDownClientes'));
        filtrarDatosBD();
    });
    $('#DromnDownProductos').on('hidden.bs.select', function (e) {
        cambiaEstadoFiltro($('#DromnDownProductos'));
        filtrarDatosBD();
    });
    $('#DromnDownPresentacion').on('hidden.bs.select', function (e) {
        cambiaEstadoFiltro($('#DromnDownPresentacion'));
        filtrarDatosBD();
    });
    $('#DromnDownTemporada').on('hidden.bs.select', function (e) {
        cambiaEstadoFiltro($('#DromnDownTemporada'));
        filtrarDatosBD();
    });


    $('#DromnDownProductos').selectpicker({
        liveSearch: true,
        deselectAllText: "Deselec.",
        noneSelectedText: "Productos",
        selectAllText: "Selec.",
        actionsBox: true
    });

    $('#DromnDownPresentacion').selectpicker({
        liveSearch: true,
        deselectAllText: "Deselec.",
        noneSelectedText: "Presentación",
        selectAllText: "Selec.",
        actionsBox: true
    });

    $('#DromnDownTemporada').selectpicker({
        liveSearch: true,
        deselectAllText: "Deselec.",
        noneSelectedText: "Temporada",
        selectAllText: "Selec.",
        actionsBox: true
    });

    eventosGrilla();
}

/**
 * Funcion general que asigna estado a boton del filtro
 * @param {any} ctrFiltro
 */
function cambiaEstadoFiltro(ctrFiltro) {
    var lstItems = $(ctrFiltro).val();
    var ctrBtnFiltro = $('#' + $(ctrFiltro).attr("btnAccionFiltro"));
    if (ctrBtnFiltro.attr("class").indexOf("filtro-activado") != -1) {
        ctrBtnFiltro.attr("class", ctrBtnFiltro.attr("class").replace("filtro-activado", ""))
    }
    if (lstItems != null) { /*SIN VALORES*/
        ctrBtnFiltro.attr("class", ctrBtnFiltro.attr("class") + " filtro-activado");
    }
}

/**
 * Remueve todos los filtros seleccionados
 * @param {any} cmbFiltro
 */
function removerFiltrosSeleccionados(cmbFiltro) {

}

/**
 * Carga valores a celda
 * @param {any} anteriorTD
 * @param {any} xValorActual
 * @param {any} xFila
 * @param {any} xColumna
 */
function cargaValorTxt(anteriorTD, xValorActual, xFila, xColumna) {
    $(anteriorTD).html(number_format(xValorActual, 3));
    if (xValorActual != 0) {
        var strClases = $(anteriorTD).attr('class');
        if (strClases.indexOf('cajaConDato') == -1) {
            $(anteriorTD).attr('class', strClases + ' cajaConDato');
        }
    } else {
        var strClases = $(anteriorTD).attr('class');
        if (strClases.indexOf('cajaConDato') != -1) {
            $(anteriorTD).attr('class', strClases.replace('cajaConDato', ''));
        }
    }
    _BDDetalle[xFila].MatSemanas[xColumna] = xValorActual;
    var xTotalTallos = _BDDetalle[xFila].Tallos * xValorActual;
    $(anteriorTD).attr("title", "Tallos: " + xTotalTallos);
    calculatTotalCajas();
}

/**
 * Evalua cada letra que se pulsa sobre la tabla de la proyección
 * @param {any} xTxt
 */
function evaluaLetra(xTxt) {

    var decValorAnterior = parseFloat('0');
    decValorAnterior = parseFloat($('#txttemporal').val().replace(/,/gi,''));
    var xColumna = parseInt($($('#txttemporal').parent()).attr("IDColuma"));
    var xFila = parseInt($($($('#txttemporal').parent()).parent()).attr("idFila"));
    xFila = xFila - 1;
    var lstColumnas = $('#tblColumnas tr td');
    var xValorActual;
    var xValorAnteriorActual;
    var xInsertarMovimiento = 0;
    var xRowAnterior = null;

    xValorActual = parseFloat($('#txttemporal').val());
    xValorAnteriorActual = xValorAnterior;
    xRowAnterior = $($($('#txttemporal').parent()).parent());

    if (event.keyCode == 38) { /*ARRIBA*/
        var xIndex = $(anteriorTD).index();
        var anteriorFila = $($(anteriorTD).parent()).prev();
        if (anteriorFila.length > 0 && $(anteriorFila).index() != 0) {
            if (anteriorTD != null) {
                cargaValorTxt(anteriorTD, decValorAnterior, xFila, xColumna);
                xInsertarMovimiento = 1;
            }
            calcularTotalProducto(anteriorTD);
            iniciaEdicion(anteriorFila.find('td')[xIndex]);
            anteriorTD = anteriorFila.find('td')[xIndex];
        } else {
            xIndex = xIndex - 1;
            if (xIndex >= 6) {
                if (anteriorTD != null) {
                    cargaValorTxt(anteriorTD, decValorAnterior, xFila, xColumna);
                    xInsertarMovimiento = 1;
                }
                calcularTotalProducto(anteriorTD);
                var xFilas = $($($(anteriorTD).parent()).parent()).find('tr');
                anteriorFila = xFilas[xFilas.length - 1];
                iniciaEdicion($(anteriorFila).find('td')[xIndex]);
                anteriorTD = $(anteriorFila).find('td')[xIndex];
            }
        }
    }
    if (event.keyCode == 40) { /*ABAJO*/
        var xIndex = $(anteriorTD).index();
        var anteriorFila = $($(anteriorTD).parent()).next();
        if (anteriorFila.length > 0) {
            if (anteriorTD != null) {
                cargaValorTxt(anteriorTD, decValorAnterior, xFila, xColumna);
                xInsertarMovimiento = 1;
            }
            calcularTotalProducto(anteriorTD);
            iniciaEdicion(anteriorFila.find('td')[xIndex]);
            anteriorTD = anteriorFila.find('td')[xIndex];
        } else {
            xIndex = xIndex + 1;
            if (xIndex < $($($(anteriorTD).parent()).prev()).find('td').length) {
                if (anteriorTD != null) {
                    cargaValorTxt(anteriorTD, decValorAnterior, xFila, xColumna);
                    xInsertarMovimiento = 1;
                }
                calcularTotalProducto(anteriorTD);
                var xFilas = $($($(anteriorTD).parent()).parent()).find('tr');
                anteriorFila = xFilas[1];
                iniciaEdicion($(anteriorFila).find('td')[xIndex]);
                anteriorTD = $(anteriorFila).find('td')[xIndex];
            }
        }
    }
    if (event.keyCode == 37) { /*IZQUIERDA*/
        var xCadenaSeleccion = getSelected();
        if (xCadenaSeleccion[0] == "") {
            var iIndex = $(anteriorTD).index();
            if (iIndex - 1 >= 6) {
                if (anteriorTD != null) {
                    cargaValorTxt(anteriorTD, decValorAnterior, xFila, xColumna);
                    xInsertarMovimiento = 1;
                }
                calcularTotalProducto(anteriorTD);
                var anteriorTDAtras = $(anteriorTD).prev();
                iniciaEdicion(anteriorTDAtras);
                anteriorTD = anteriorTDAtras;
            }
        }
    }
    if (event.keyCode == 39 || event.keyCode == 13) { /*DERECHA*/
        var xCadenaSeleccion = getSelected();
        if ((xCadenaSeleccion[xCadenaSeleccion.length - 1] == "" && xCadenaSeleccion[xCadenaSeleccion.length - 2] == "") || event.keyCode == 13) {
            var iCantidadColumnas = $($(anteriorTD).parent()).find('td').length;
            var iColumnaActual = $(anteriorTD).index();
            if (iCantidadColumnas - 1 == iColumnaActual) {
                var ifilaSiguiente = $($(anteriorTD).parent()).next();
                if (ifilaSiguiente.length > 0) {
                    if (anteriorTD != null) {
                        cargaValorTxt(anteriorTD, decValorAnterior, xFila, xColumna);
                        xInsertarMovimiento = 1;
                    }
                    calcularTotalProducto(anteriorTD);
                    anteriorTD = $(ifilaSiguiente).find('td')[6];
                    iniciaEdicion(anteriorTD);
                }
            } else {
                if (anteriorTD != null) {
                    cargaValorTxt(anteriorTD, decValorAnterior, xFila, xColumna);
                    xInsertarMovimiento = 1;
                }
                calcularTotalProducto(anteriorTD);
                anteriorTD = $(anteriorTD).next();
                iniciaEdicion(anteriorTD);
            }
        }
    }

    /*Inserta movimiento*/
    if (xInsertarMovimiento == 1 && parseFloat(xValorAnteriorActual) != parseFloat(xValorActual)) {

        _IDUniversal++;
        var objMovimiento = new movimiento(_IDUniversal);
        objMovimiento.SetID = 0;
        objMovimiento.SetAnteriorValor = parseFloat(xValorAnteriorActual);
        objMovimiento.SetNuevoValor = parseFloat(xValorActual);
        objMovimiento.SetAnioSemana = $(lstColumnas[xColumna + 6]).html();
        objMovimiento.SetIDUniversal = xRowAnterior.attr('IdUniversal');
        objMovimiento.SetID = xRowAnterior.attr('Id');
        objMovimiento.SetCanal = xRowAnterior.attr('Canal');
        objMovimiento.SetCodigoPedido = xRowAnterior.attr('CodigoPedido');
        objMovimiento.SetIDCliente = xRowAnterior.attr('IDCliente');
        objMovimiento.SetIDProductos = xRowAnterior.attr('IDProductos');
        objMovimiento.SetIDPresentaciones = xRowAnterior.attr('IDPresentaciones');
        objMovimiento.SetIDTemporadaBase = xRowAnterior.attr('IDTemporadaBase');
        objMovimiento.SetIDPeriodo = xRowAnterior.attr('IDPeriodo');
        _lstMovimiento.push(objMovimiento);
    }

}

function number_format(amount, decimals) {

    amount += ''; // por si pasan un numero en vez de un string
    amount = parseFloat(amount.replace(/[^0-9\.]/g, '')); // elimino cualquier cosa que no sea numero o punto

    decimals = decimals || 0; // por si la variable no fue fue pasada

    // si no es un numero o es igual a cero retorno el mismo cero
    if (isNaN(amount) || amount === 0)
        return parseFloat(0).toFixed(decimals);

    // si es mayor o menor que cero retorno el valor formateado como numero
    amount = '' + amount.toFixed(decimals);

    var amount_parts = amount.split('.'),
        regexp = /(\d+)(\d{3})/;

    while (regexp.test(amount_parts[0]))
        amount_parts[0] = amount_parts[0].replace(regexp, '$1' + ',' + '$2');

    return amount_parts.join('.');
}

/**
 * Actualiza grid de acuerdo al index indicado
 */
var _ultimoIndexSeleccionado = 0;
function actualizaGrid(xIndexSeleccionado) {
    var xInicio = parseInt('0');
    var xFinal = parseInt('0');
    _ultimoIndexSeleccionado = xIndexSeleccionado;


    var lstClientes = $('#DromnDownClientes').val();
    var lstProductos = $('#DromnDownProductos').val();
    var lstPresentacion = $('#DromnDownPresentacion').val();
    var lstTemporada = $('#DromnDownTemporada').val();

    /**
     * Carga arreeglo con filtros indicados
     */
    var lstItemsFiltros = [];
    for (var i = 0; i < this._BDDetalle.length; i++) {
        if (
            (lstClientes == null || lstClientes.findIndex(m => parseInt(m) == this._BDDetalle[i].IDcliente) != -1) &&
            (lstProductos == null || lstProductos.findIndex(m => parseInt(m) == this._BDDetalle[i].IDProductos) != -1) &&
            (lstPresentacion == null || lstPresentacion.findIndex(m => parseInt(m) == this._BDDetalle[i].IDPresentaciones) != -1) &&
            (lstTemporada == null || lstTemporada.findIndex(m => parseInt(m) == this._BDDetalle[i].IDTemporadasBase) != -1)
        ) {
            lstItemsFiltros.push(this._BDDetalle[i]);
        }
    }

    if (lstItemsFiltros.length > 0) {

        var xCantidaRegistros = parseInt(cantidadRegistrosGlobal);
        if (xCantidaRegistros > lstItemsFiltros.length) {
            xCantidaRegistros = lstItemsFiltros.length;
        }

        xInicio = xIndexSeleccionado - parseInt((xCantidaRegistros / 2) - 1);
        xFinal = xIndexSeleccionado + parseInt(xCantidaRegistros / 2) + 1;

        if (xIndexSeleccionado <= xCantidaRegistros) {
            if (_RegistroGuia <= 9) {
                xInicio = 0;
                xFinal = xCantidaRegistros;
                _RegistroGuia = 9;
            }
        }
        if (xIndexSeleccionado >= (lstItemsFiltros.length - xCantidaRegistros)) {
            if (xIndexSeleccionado >= lstItemsFiltros.length - parseInt((xCantidaRegistros / 2) - 1)) {
                if (lstItemsFiltros.length <= parseInt(cantidadRegistrosGlobal)) {
                    _RegistroGuia = (lstItemsFiltros.length - 1) - parseInt((xCantidaRegistros / 2) - 1);
                    xInicio = 0;
                    xFinal = lstItemsFiltros.length;
                } else {
                    _RegistroGuia = (lstItemsFiltros.length - 1) - parseInt((xCantidaRegistros / 2) - 1);
                    xInicio = _RegistroGuia - parseInt((xCantidaRegistros / 2) - 1);
                    xFinal = (_RegistroGuia == 0 ? 1 : _RegistroGuia) + parseInt(xCantidaRegistros / 2);
                }
            }
        }

        /*Detalle proyeccion*/
        var strFilasProyeccionResumenHTML = '';
        var strFilasProyeccionResumenHTMLFlotante = '<tr class=""><td class="columna-conWidth" colSpam="4"></td></tr>';
        strFilasProyeccionResumenHTML = this._cabeceraHTML;
        for (var intColumna = xInicio; intColumna < xFinal; intColumna++) {

            strFilasProyeccionResumenHTML +=
                '<tr  onmousedown=\"menuContextualMouse(event,this);\" class="context-Item-producto-Proyectado" idFila="' + lstItemsFiltros[intColumna].ID +
                '" IdUniversal="' + lstItemsFiltros[intColumna].IdUniversal +
                '" Id="' + lstItemsFiltros[intColumna].ID +
                '" Canal="' + lstItemsFiltros[intColumna].Canal +
                '" CodigoPedido="' + lstItemsFiltros[intColumna].CodigoPedido +
                '" IDCliente="' + lstItemsFiltros[intColumna].IDcliente +
                '" IDProductos="' + lstItemsFiltros[intColumna].IDProductos +
                '" IDPresentaciones="' + lstItemsFiltros[intColumna].IDPresentaciones +
                '" IDTemporadaBase="' + lstItemsFiltros[intColumna].IDTemporadasBase +
                '" IDPeriodo="' + lstItemsFiltros[intColumna].IDPeriodo +
                '" >' +
                '<td tabIndex="' + intColumna + '" class="cancel column_' + getColumna(17).ID + '" style="max-width:' + getColumna(17).WidthInicial + 'px" >' + lstItemsFiltros[intColumna].ID + '</td>' +
                '<td tabIndex="' + intColumna + '" class=" cancel column_' + getColumna(18).ID + '" style="max-width:' + getColumna(18).WidthInicial + 'px" >' + lstItemsFiltros[intColumna].Canal + '</td>' +
                '<td tabIndex="' + intColumna + '" class=" cancel column_' + getColumna(19).ID + '"  style="max-width:' + getColumna(19).WidthInicial + 'px" title="' + getCliente(lstItemsFiltros[intColumna].IDcliente).Nombre + '">' + getCliente(lstItemsFiltros[intColumna].IDcliente).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" class=" cancel column_' + getColumna(21).ID + '"  style="max-width:' + getColumna(21).WidthInicial + 'px" title="' + getProducto(lstItemsFiltros[intColumna].IDProductos).Nombre + '">' + getProducto(lstItemsFiltros[intColumna].IDProductos).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" class=" cancel column_' + getColumna(22).ID + '"  style="max-width:' + getColumna(22).WidthInicial + 'px">' + getPresentacion(lstItemsFiltros[intColumna].IDPresentaciones).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" class=" cancel column_' + getColumna(23).ID + ' ultimaColumna"  style="max-width:' + getColumna(23).WidthInicial + 'px">' + getTemporada(lstItemsFiltros[intColumna].IDTemporadasBase).Nombre + '</td>';

            strFilasProyeccionResumenHTMLFlotante +=
                '<tr>' +
                '<td tabIndex="' + intColumna + '" id="col_' + lstItemsFiltros[intColumna].ID + '"  style="display:none" class="columnF columnF_' + getColumna(19).ID + '">' + getCliente(lstItemsFiltros[intColumna].IDcliente).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" id="col_' + lstItemsFiltros[intColumna].ID + '"  style="display:none" class=" columnF columnF_' + getColumna(21).ID + '">' + getProducto(lstItemsFiltros[intColumna].IDProductos).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" id="col_' + lstItemsFiltros[intColumna].ID + '"  style="display:none"  class=" columnF columnF_' + getColumna(22).ID + '">' + getPresentacion(lstItemsFiltros[intColumna].IDPresentaciones).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" id="col_' + lstItemsFiltros[intColumna].ID + '"  style="display:none" class="columnF columnF_' + getColumna(23).ID + '">' + getTemporada(lstItemsFiltros[intColumna].IDTemporadasBase).Nombre + '</td></tr>';

            /*CARGA SEMANAS*/
            for (var intSemanas = 0; intSemanas < lstItemsFiltros[intColumna].MatSemanas.length; intSemanas++) {
                var xTallos = lstItemsFiltros[intColumna].Tallos * lstItemsFiltros[intColumna].MatSemanas[intSemanas];
                strFilasProyeccionResumenHTML += '<td title="Tallos: ' + xTallos + '" tabIndex="' + intSemanas + '" IDColuma = "' + intSemanas + '" class="celda-modifica ' + (lstItemsFiltros[intColumna].MatSemanas[intSemanas] != 0 ? "cajaConDato" : "") + ' datoSemana column_' + (intSemanas + 8) + '" >' + number_format(lstItemsFiltros[intColumna].MatSemanas[intSemanas], 3) + '</td>';
            }

            strFilasProyeccionResumenHTML += '</tr>';
        }
        $('#tblDatosProyeccion').html(strFilasProyeccionResumenHTML);
        $('.tbl-Cuerpop-Flotante').html(strFilasProyeccionResumenHTMLFlotante);

        eventosGridProyeccionDetalle();
        controlScrollPersonalizado();
        $('#lblTotalResumen').html((xInicio + 1) + " al " + (xFinal) + " registros de " + lstItemsFiltros.length);
    }
    if (eventoControlado == true) {
        finalizarEdicionDatos();
    }
}

/**
 * Eventos de la grilla
 */
function eventosGrilla() {

    /*BOTON ACCION*/
    $('.btnAccion').click(function () {
        var xAccion = parseInt($(this).attr("accion"));
        if (xAccion == 1) { /*CREAR NUEVA*/

        }
        if (xAccion == 2) { /*CONSULTAR*/
            consultarProyecciones();
        }
        if (xAccion == 3) { /*Modificar productos a proyectar*/
            location.href = "../ForeCast/ConfiguracionClientesProductos?xIDUnico=" + $('#hdfID').val();
        }
        if (xAccion == 4) { /*Modificar productos a proyectar*/

            var xIDForeCast = $('#hdfID').val();
            var xddlCanal = $('#hdfCanal').val();
            var xIDModificacion = $('#hdfIDModificacion').val();
            $.ajax({
                url: "/ForeCast/ImportarDatosAForeCast",
                type: "POST",
                data: "ddlProyeccionVenta=" + xIDForeCast + "&ddlCanal=" + xddlCanal + "&hdfIDModificacion=" + xIDModificacion,
                dataType: "html",
                success: function (data) {
                    $('#popupCuerpoImportarDatos').html(data);
                    $('#popupImportarDatos').modal('show');
                },
                error: function (jqXHR, textStatus, error) {
                    mostrarMensaje(2, error);
                }
            });

        }
        if (xAccion == 5) { /*Modificar productos a proyectar*/
            var lstConsolidadoCajas = [];
            for (var i = 0; i < _BDDetalle.length; i++) {
                for (var j = 0; j < _BDDetalle[i].MatSemanas.length; j++) {
                    if (lstConsolidadoCajas[j] == null) {
                        lstConsolidadoCajas[j] = _BDDetalle[i].MatSemanas[j] * _BDDetalle[i].Tallos;
                    } else {
                        lstConsolidadoCajas[j] += _BDDetalle[i].MatSemanas[j] * _BDDetalle[i].Tallos;
                    }
                }
            }

            $('#divContenedorGraficas').html("");

            var arraySegundo = [];
            for (var i = 6; i < _columnas.length; i++) {
                arraySegundo.push(_columnas[i].Nombre);
            }

            var config = {
                type: 'line',
                data: {
                    labels: arraySegundo,
                    datasets: [{
                        label: "Número de tallos",
                        backgroundColor: window.chartColors.red,
                        borderColor: window.chartColors.black,
                        data: lstConsolidadoCajas,
                        fill: true,
                    }]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Cantidad tallos Vs Semana operacional'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                // the data minimum used for determining the ticks is Math.min(dataMin, suggestedMin)
                                suggestedMin: 10,

                                // the data maximum used for determining the ticks is Math.max(dataMax, suggestedMax)
                                suggestedMax: 50
                            }
                        }]
                    }
                }
            };

            $('#divContenedorGraficas').html("<canvas id=\"canvas\"></canvas>");

            var ctx = document.getElementById('canvas').getContext('2d');
            window.myLine = new Chart(ctx, config);

            $('#popupGraficaProductoComercial').modal('show');



        }
        if (xAccion == 900) { /*GUARDAR CAMBIOS REALIZADOS*/
            sincronizacionBD_Final(0);
        }
        if (xAccion == 901) { /*GUARDAR CAMBIOS REALIZADOS*/
            sincronizacionBD_Final(1);
        }
        if (xAccion == 6) { /*Modificar productos a proyectar*/
            if (eventoControlado == false) {
                iniciaModificacionDatos();
            } else {
                finalizarEdicionDatos();
            }
        }
        if (xAccion == 7) {

            var strEncabezado = "";
            var strNombreArchivo = "";
            var strCuerpo = "";

            strNombreArchivo = ($('#lblCanalArchivo').html().trim() + $('#lblNombreArchivo').html().trim()).replace(/ /g, '');
            /*Crea encabezado*/
            for (var i = 0; i < _columnas.length; i++) {
                if (i >= 2 && i <= 5) {
                    strEncabezado += "ID " + _columnas[i].Nombre + "\t";
                }
                strEncabezado += _columnas[i].Nombre + "\t";
            }
            strEncabezado += "\n";

            /*Crea cuerpo*/
            for (var i = 0; i < _BDDetalle.length; i++) {
                var objCliente = getCliente(_BDDetalle[i].IDcliente);
                var objProducto = getProducto(_BDDetalle[i].IDProductos);
                var objPresentacion = getPresentacion(_BDDetalle[i].IDPresentaciones);
                var objTemporada = getTemporada(_BDDetalle[i].IDTemporadasBase);

                strCuerpo +=
                    _BDDetalle[i].ID +
                    "\t" + _BDDetalle[i].Canal +
                    "\t" + objCliente.ID +
                    "\t" + objCliente.Nombre +
                    "\t" + objProducto.ID +
                    "\t" + objProducto.Nombre +
                    "\t" + objPresentacion.ID +
                    "\t" + objPresentacion.Nombre +
                    "\t" + objTemporada.ID +
                    "\t" + objTemporada.Nombre + 
                    "\t";

                var strSemanas = "";
                /*carga datos de semanas*/
                for (var j = 0; j < _BDDetalle[j].MatSemanas.length; j++) {
                    strSemanas += _BDDetalle[i].MatSemanas[j] + "\t";
                }

                strCuerpo += strSemanas + "\n";

            }

            /**
             * GENERA STREAM DE EXCEL PARA DESCARGAR
             */
            var tmpElemento = document.createElement('a');
            var data_type = 'data:application/vnd.ms-excel';
            var tabla_html = (strEncabezado + strCuerpo).replace(/ /g, '%20');
            tmpElemento.href = data_type + ', ' + tabla_html;
            tmpElemento.download = strNombreArchivo + '.xls';
            tmpElemento.click();

        }
    })

    $(".filtro_Columna").click(function () {
        var xColumna = $(this).attr("indexColumna");
        var xNombreColumna = ".column_" + xColumna;
        $(xNombreColumna).css("display", "none");
        $($(this).parent()).css("display", "none");


    })

    $('.tabla-datos').bind('mousewheel', function (e) {

        var heightbtn = $('.scrroll-top').height() + 2;
        var heightbtnBottom = $('.scrroll-bottom').height() + 2;
        var alto = $('.scroll').height();
        var altoBarra = $('.scrroll-desplazo').height();

        if (e.originalEvent.wheelDelta / 120 > 0) {
            if (_RegistroGuia >= 1) {
                _RegistroGuia -= 2;
                actualizaGrid(_RegistroGuia);
            }
        }
        else {
            if (_RegistroGuia < (_BDDetalle.length - 2)) {
                _RegistroGuia += 2;
                actualizaGrid(_RegistroGuia);
            }
        }

        var cantidadFilas = _BDDetalle.length;
        var heightDisponible = alto - (heightbtn + heightbtnBottom + altoBarra);
        var posicion = heightDisponible * (_RegistroGuia / cantidadFilas);
        $('.scrroll-desplazo').css('top', parseInt(posicion) + heightbtn + 'px');

    });

}

/**
 * Calcula de acuerdo a la posicion de la barra de desplazamiento los registros a mostrar
 * @param {any} ctrDesplazamiento Control que dispara el metodo para optener el top
 * @param {any} uiposition Nueva posiciton teorica
 */
function calcularMuestraRegistros(ctrDesplazamiento, uiposition) {
    var heightbtn = $('.scrroll-top').height() + 2;
    var heightbtnBottom = $('.scrroll-bottom').height() + 2;
    var alto = $('.scroll').height();
    var heightScroll = $(ctrDesplazamiento).height() + 2;
    if (uiposition.top < heightbtn) {
        uiposition.top = heightbtn;
    }
    if ((alto - (heightbtn + heightScroll)) < uiposition.top) {
        uiposition.top = alto - (heightbtn + heightScroll);
    }

    /*REFREZCA TABLA DE DATOS*/
    if (_BDDetalle != null) {
        var xCantidadRegistros = _BDDetalle.length;
        var xAltoCalculado = uiposition.top - heightbtn;
        if (xCantidadRegistros > 0) {
            var xVariacion = parseFloat('0');
            var xTotalAreaDesplazamiento = (alto - (heightbtn + heightbtnBottom + heightScroll));
            xVariacion = xCantidadRegistros / xTotalAreaDesplazamiento;
            _RegistroGuia = parseInt(xVariacion * (uiposition.top - heightbtn));
            actualizaGrid(_RegistroGuia);
        }
    }
}

/**
 * Funcion encargada de llamarse a si misma para simular el click sostenido para el scroll personalizado
 */
function subirScroll() {
    if (_clickSostenido == true) {
        var heightbtn = $('.scrroll-top').height() + 4;
        var _posicion = $('.scrroll-desplazo').position();
        if ((_posicion.top - heightbtn) > 0) {
            $('.scrroll-desplazo').css("top", (_posicion.top - 3) + "px");
        }
    }
}

$('#btnAgregarProducto').on('click', function (e) {
    $('#popupAgregarProducto').modal('show');
});

$(document).ready(function () {

    var heigtPantalla = screen.height;
    var xheigntPrincipal = (heigtPantalla - 200);
    var xheigntTabla = (heigtPantalla - 320);
    var xheigntBarra = (heigtPantalla - 320);

    cantidadRegistrosGlobal = Math.round((heigtPantalla - 345) / 21, 0);

    $('.container-gridEdit').css('height', xheigntPrincipal + 'px');
    $('.tabla-datos').css('max-height', xheigntTabla + 'px');
    $('.tabla-datos').css('min-height', xheigntTabla + 'px');
    $('.scroll').css('height', xheigntBarra + 'px');

    setInterval(subirScroll, 100);

    /*BARRA DESLIZADORA DE LA INTERFAZ*/
    $(".scrroll-desplazo").draggable({
        grid: [0, 1],
        cursor: 'move'
    });

    /*TOP*/
    $(".scrroll-top").on("click", function () {
        if (_RegistroGuia > 0) {
            _RegistroGuia -= 1;
            actualizaGrid(_RegistroGuia);
        }
    })

    /*TOP*/
    $(".scrroll-top").mousedown(function () {
        _clickSostenido = true;
    });

    /* TOP */
    $(".scrroll-top").mouseup(function () {
        if (_RegistroGuia <= 1) {
            _RegistroGuia -= 1;
            actualizaGrid(_RegistroGuia);
        }
    });

    /*BOTTOM*/
    $(".scrroll-bottom").on("click", function () {
        if (_RegistroGuia < (_BDDetalle.length - 2)) {
            _RegistroGuia += 1;
            actualizaGrid(_RegistroGuia);
        }
    })

    /*MAXIMO*/
    $(".scrroll-desplazo").bind("drag", function (evento, ui) {
        calcularMuestraRegistros(this, ui.position);
    });

    /*INICIA SINCRONIZACIÓN BD*/
    setInterval(sincronizacionBD, 5000)

});

var cerrrarVentana = 0;
/**
 * Metodo encargado de sincronizar datos con BD
 */
function sincronizacionBD_Final(xAccion) {
    cerrrarVentana = xAccion;
    if (_enSincronizacion == 0) {
        var _IDProyeccionVenta = parseInt($('#hdfIDProyeccionVenta').val());
        for (var intSincronizado = 0; intSincronizado < _lstMovimiento.length; intSincronizado++) {
            if (_lstMovimiento[intSincronizado].SincronizadaBD == 0) {
                _listadoSincronizado.push(_lstMovimiento[intSincronizado]);
            }
        }
        var _IDModificacion = $('#hdfIDModificacion').val();
        $('#CapEstado').html("Sincronizando y Guardando...");
        _enSincronizacion = 1;
        $.ajax({
            url: '/ForeCast/SincronizarYGuardaDatosFinal/',
            datatType: 'json',
            data: "xIdProvision=" + _IDProyeccionVenta + "&strData=" + JSON.stringify(_listadoSincronizado) + "&IDModificacion=" + _IDModificacion,
            cache: false,
            type: 'POST',
            success: function (data) {
                if (data.ResultadoProceso) {
                    for (var intSincronizado = 0; intSincronizado < _listadoSincronizado.length; intSincronizado++) {
                        var OjbiTem = _lstMovimiento.find(
                            m => (
                                m.IDuniversal == _listadoSincronizado[intSincronizado].IDuniversal &&
                                m.AnioSemana == _listadoSincronizado[intSincronizado].AnioSemana &&
                                m.Orden == _listadoSincronizado[intSincronizado].Orden)
                        );
                        OjbiTem.SetSinCronizadaBD = 1;
                    }
                    _listadoSincronizado = new Array();
                    $('#CapEstado').html("Sincronizado y Guardado");
                    $('#lblSincronizacion').html(data.MensajeTecnicoFormulario);
                    if (cerrrarVentana == 0) {
                        mostrarMensaje(1, data.MensajeProceso);
                    } else {
                        mostrarMensajeUrl(5, data.MensajeProceso, '../ForeCast/Consulta');
                    }
                } else {
                    mostrarMensaje(2, data.MensajeProceso);
                }
                _enSincronizacion = 0;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                showError(xhr.status, xhr.responseText);
                _enSincronizacion = 0;
            }
        });
    }
}

/**
 * Metodo encargado de sincronizar datos con BD
 */
function sincronizacionBD() {
    if (_enSincronizacion == 0) {
        var _IDProyeccionVenta = parseInt($('#hdfIDProyeccionVenta').val());
        for (var intSincronizado = 0; intSincronizado < _lstMovimiento.length; intSincronizado++) {
            if (_lstMovimiento[intSincronizado].SincronizadaBD == 0) {
                _listadoSincronizado.push(_lstMovimiento[intSincronizado]);
            }
        }
        if (_listadoSincronizado.length > 0) {
            var _IDModificacion = $('#hdfIDModificacion').val();
            $('#CapEstado').html("Sincronizando...");
            _enSincronizacion = 1;
            $.ajax({
                url: '/ForeCast/SincronizarBDTemporal/',
                datatType: 'json',
                data: "xIdProvision=" + _IDProyeccionVenta + "&strData=" + JSON.stringify(_listadoSincronizado) + "&IDModificacion=" + _IDModificacion,
                cache: false,
                type: 'POST',
                success: function (data) {
                    if (data.ResultadoProceso) {
                        for (var intSincronizado = 0; intSincronizado < _listadoSincronizado.length; intSincronizado++) {
                            var OjbiTem = _lstMovimiento.find(
                                m => (
                                    m.IDuniversal == _listadoSincronizado[intSincronizado].IDuniversal &&
                                    m.AnioSemana == _listadoSincronizado[intSincronizado].AnioSemana &&
                                    m.Orden == _listadoSincronizado[intSincronizado].Orden)
                            );
                            OjbiTem.SetSinCronizadaBD = 1;
                        }
                        _listadoSincronizado = new Array();
                        $('#CapEstado').html("Sincronizado");
                        $('#lblSincronizacion').html(data.MensajeTecnicoFormulario);
                    } else {
                        alert(data.MensajeProceso);
                    }
                    _enSincronizacion = 0;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    showError(xhr.status, xhr.responseText);
                    _enSincronizacion = 0;
                }
            });
        }
    }
}

function consultarProyecciones() {
    //var _data = $("#fmrValidacionUsuario").serialize();
    var IDUnico = $('#hdfID').val();
    $.ajax({
        url: "/ForeCast/consultarProyecciones",
        type: "POST",
        data: "IDUnico=" + IDUnico,
        dataType: "html",
        success: function (data) {
            $('#cuerpoPopupConsultaProyecciones').html(data);
            $('#popupConsultarProyecciones').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            //mostrarMensaje(2, error);
        }
    });
}

/**
 * Metodo encargado ejecutarse con el scroll
 */
function controlScrollPersonalizado() {
    var acomuladoColumna = parseFloat('0');
    var acomuladoDinamicoAnterior = parseFloat('0');
    var xUbicacion = $('.tabla-datos').scrollLeft();
    var acomuladoDinamico = parseFloat('0');

    for (var intColuma = 0; intColuma < _columnas.length; intColuma++) {
        if (_columnas[intColuma].CargaDinamica == true) {
            if (acomuladoColumna <= (xUbicacion + acomuladoDinamico) && $('#chkColumnasFijas').prop('checked') == true) {
                $('.column' + _columnas[intColuma].ID).css('display', 'table-cell');
                $('.columnF_' + _columnas[intColuma].ID).css('display', 'table-cell');
                acomuladoDinamico += _columnas[intColuma].WidthInicial;
            } else {
                $('.column' + _columnas[intColuma].ID).css('display', 'none');
                $('.columnF_' + _columnas[intColuma].ID).css('display', 'none');
                if (acomuladoDinamico > 0) {
                    acomuladoDinamico -= _columnas[intColuma].WidthInicial;
                }
            }
        }
        acomuladoColumna += _columnas[intColuma].WidthInicial;
    }

}

/**
 * Funcion utilizada para descargar excel con los datos indicados
 * @param {any} XIDForeCast
 */
function seleccionarTipoArchivoExcel(XIDForeCast, tipo) {
    //var _data = $("#fmrValidacionUsuario").serialize();
    $.ajax({
        url: "/ForeCast/seleccionarTipoArchivoExcel",
        type: "POST",
        data: "IDUnico=" + XIDForeCast + "&tipoDocumento=" + tipo,
        dataType: "html",
        success: function (data) {
            $('#cuerpoTipoExportacionExcel').html(data);
            $('#popupTipoExportacionExcel').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            //mostrarMensaje(2, error);
        }
    });
}

/**
 * Funcion utilizada para descargar listado de productos
 */
function DescargarListadoExcelProducto() {
    $('#popupTipoExportacionExcel').modal('hide');
    window.open('../ForeCast/GenerarExcelParametros?' + $('#fmrExportarDatosExcel').serialize(), '_blank');
}

/**
 * Click derecho sobre item
 * @param {any} e
 * @param {any} ctr
 */
function menuContextualMouse(e, ctr) {
    if (e.which == 3) {
        ItemSeleccionado = ctr;
    }
}

$(document).ready(function () {
    $.contextMenu({
        selector: '.context-Item-producto-Proyectado',
        callback: function (key, options) {
            if (key == "EvCo") {
                $("#tblDatosProyeccion tr").selectable("destroy");
                $("#tblDatosProyeccion tr").selectable({
                    cancel: "a,.cancel"
                });
            }
            if (key == "DiSe") {
                $('#hdftipoExportacion').val("Dise");
                $('#lblTituloAsignarValor').html("Asignar valor promedio");
                $('#popupAsignarValor').modal('show');
            }
            if (key == "AsVA") {
                $('#hdftipoExportacion').val("AsVA");
                $('#lblTituloAsignarValor').html("Asignar valor fijo");
                $('#popupAsignarValor').modal('show');
            }
            if (key == "VerPro") {

                $('#divContenedorGraficas').html("");

                var ojbItem = _BDDetalle[parseInt($(ItemSeleccionado).attr('iduniversal'))];
                var arraySegundo = [];
                for (var i = 6; i < _columnas.length; i++) {
                    arraySegundo.push(_columnas[i].Nombre);
                }

                var config = {
                    type: 'line',
                    data: {
                        labels: arraySegundo,
                        datasets: [{
                            label: getProducto(ojbItem.IDProductos).Nombre,
                            backgroundColor: window.chartColors.red,
                            borderColor: window.chartColors.red,
                            data: ojbItem.MatSemanas,
                            fill: false,
                        }]
                    },
                    options: {
                        responsive: true,
                        title: {
                            display: true,
                            text: 'CAJAS - Producto comercial Vs Semana operacional'
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    // the data minimum used for determining the ticks is Math.min(dataMin, suggestedMin)
                                    suggestedMin: 10,

                                    // the data maximum used for determining the ticks is Math.max(dataMax, suggestedMax)
                                    suggestedMax: 50
                                }
                            }]
                        }
                    }
                };

                $('#divContenedorGraficas').html("<canvas id=\"canvas\"></canvas>");

                var ctx = document.getElementById('canvas').getContext('2d');
                window.myLine = new Chart(ctx, config);

                $('#popupGraficaProductoComercial').modal('show');
            }
            if (key == "CatTallo") {
                var ojbItem = _BDDetalle[parseInt($(ItemSeleccionado).attr('iduniversal'))];
                $('#lblCantidadTallosProductos').html(number_format(ojbItem.Tallos));
                $('#popupTallosProductoComercial').modal('show');
            }
            if (key == "VerPrT") {

                $('#divContenedorGraficas').html("");

                var ojbItem = _BDDetalle[parseInt($(ItemSeleccionado).attr('iduniversal'))];
                var arraySegundo = [];
                var valoresTallos = [];
                for (var i = 6; i < _columnas.length; i++) {
                    arraySegundo.push(_columnas[i].Nombre);
                }

                for (var i = 0; i < ojbItem.MatSemanas.length; i++) {
                    valoresTallos.push(ojbItem.MatSemanas[i] * ojbItem.Tallos);
                }

                var config = {
                    type: 'line',
                    data: {
                        labels: arraySegundo,
                        datasets: [{
                            label: getProducto(ojbItem.IDProductos).Nombre,
                            backgroundColor: window.chartColors.red,
                            borderColor: window.chartColors.blue,
                            data: valoresTallos,
                            fill: false,
                        }]
                    },
                    options: {
                        responsive: true,
                        title: {
                            display: true,
                            text: 'TALLOS - Producto comercial Vs Semana operacional'
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    // the data minimum used for determining the ticks is Math.min(dataMin, suggestedMin)
                                    suggestedMin: 10,

                                    // the data maximum used for determining the ticks is Math.max(dataMax, suggestedMax)
                                    suggestedMax: 50
                                }
                            }]
                        }
                    }
                };

                $('#divContenedorGraficas').html("<canvas id=\"canvas\"></canvas>");

                var ctx = document.getElementById('canvas').getContext('2d');
                window.myLine = new Chart(ctx, config);

                $('#popupGraficaProductoComercial').modal('show');
            }

        },
        items: {
            //"Selec": { name: "ASIGNAR VALORES", disabled: true },
            //"DiSe": { name: "Distribuir valor por semana (Promedio)" },
            //"AsVA": { name: "Asignar un mismo valor a cada semana (Valor)" },
            //"sep1": "---------",
            "Grafi": { name: "GRAFICAS", disabled: true },
            "VerPro": { name: "Ver comportamiento de necesidad cajas(Cajas)" },
            "VerPrT": { name: "Ver comportamiento de necesidad cajas(Tallos)" },
            "sep1": "---------",
            "Grafi2": { name: "PRODUCTO", disabled: true },
            "CatTallo": { name: "Cantidad de tallos" },
            "sep1": "---------",
            "EvCo": { name: "Deseleccionar todas las celdas" }
        }
    });



    window.onload = function () {
        //document.onkeypress = mostrarInformacionCaracter;
        document.onkeyup = mostrarInformacionTecla;
        document.onkeydown = mostrarInformacionTeclaDown;
    }
    /*
    function mostrarInformacionCaracter(evObject) {

        var msg = ''; var elCaracter = String.fromCharCode(evObject.which);

        if (evObject.which != 0 && evObject.which != 13) {

            msg = 'Tecla pulsada: ' + elCaracter;

            console.log(msg + '-------------------------');
        }

        else {
            msg = 'Pulsada tecla especial';

            console.log(msg + '----------------------------');
        }

        eventoControlado = true;

    }*/

    function mostrarInformacionTecla(evObject) {
        var teclaPulsada = evObject.keyCode;
        if (teclaPulsada == 27 && eventoControlado == true) {

            finalizarEdicionDatos();

        }
    }

    function mostrarInformacionTeclaDown(evObject) {
        var teclaPulsada = evObject.keyCode;
        if (teclaPulsada == 16 && eventoControlado == false) {

            iniciaModificacionDatos();

        }

        if (teclaPulsada == 13 && eventoControlado == true && conFocoAparte == false) {
            lstItemsSeleccionados = $('.ui-selected');
            if (lstItemsSeleccionados.length != 0) {
                $('#txtValorAAplicarDatos').number(true, 3);
                $('#popupAsignarValorMultriple').modal('show');
                setTimeout(function () { $('#txtValorAAplicarDatos').select(); $('#txtValorAAplicarDatos').focus(); }, 600);
            } else {
                mostrarMensaje(3, "Debe seleccionar una o mas celdas.");
            }
        } else {
            if (conFocoAparte == true) {
                conFocoAparte = false;
            }
        }
    }

});

/**
 * Finaliza edicion de datos
 */
function finalizarEdicionDatos() {
    eventoControlado = false;
    var lstCeldas = $('.celda-modifica');
    for (var i = 0; i < lstCeldas.length; i++) {
        var strClass = $(lstCeldas[i]).attr('class');
        if (strClass.indexOf('habilitaModificacion') != -1) {
            $(lstCeldas[i]).attr('class', strClass.replace('habilitaModificacion', '').replace('ui-widget-content', '').replace('ItemConModificaciones', '').replace('conValores', '') + ' datoSemana');
        }
    }
    try {
        $("#tblDatosProyeccion tr").selectable("destroy");
    } catch (E) { }
    $('#btnEdicionDatos').html("<span class=\"glyphicon glyphicon-edit\"></span> Seleccionar y editar");

}


function iniciaModificacionDatos() {

    $('#btnEdicionDatos').html("<span class=\"glyphicon glyphicon-edit\"></span> En edición...");

    /*SI ESTA TRABAJANDO SOBRE UN ITEM SE INACTIVA*/
    if (anteriorTD != null) {
        $(anteriorTD).html($('#txttemporal').val());
    }

    document.onselectstart = noSeleccionarTexto;

    eventoControlado = true;
    var lstCeldas = $('.celda-modifica');
    for (var i = 0; i < lstCeldas.length; i++) {
        var strClass = $(lstCeldas[i]).attr('class');
        var valor = parseFloat($(lstCeldas[i]).html());
        if (strClass.indexOf('habilitaModificacion') == -1) {
            $(lstCeldas[i]).attr('class', strClass.replace('datoSemana', '') + " habilitaModificacion ui-widget-content " + (valor != 0 ? "conValores" : ""));
        }
    }

    $("#tblDatosProyeccion tr").selectable({
        cancel: "a,.cancel"
    });
}

/**
 * Inhabiltia la seleccion de texto
 */
function noSeleccionarTexto() {
    return false;
}

/**
 * Calcula cantidad de cajas total
 */
function calculatTotalCajas() {
    var TotalCajas = parseInt('0');
    var TotalCajasTallos = parseInt('0');
    for (var i = 0; i < _BDDetalle.length; i++) {
        for (var J = 0; J < _BDDetalle[i].MatSemanas.length; J++) {
            TotalCajas += _BDDetalle[i].MatSemanas[J];
            TotalCajasTallos += _BDDetalle[i].MatSemanas[J] * _BDDetalle[i].Tallos;
        }
    }
    $('#divCantidadTotaCajas').html(number_format(TotalCajas, 2));
    $('#divCantidadTotaTallos').html(number_format(TotalCajasTallos, 2));
}

function determinaColor(xcanal) {
    if (xcanal == 'WC') {
        return 'blue';
    }
    if (xcanal == 'GG') {
        return 'red';
    }
    if (xcanal == 'IG') {
        return 'yellow';
    }
    if (xcanal == 'FZ') {
        return 'green';
    }
    if (xcanal == 'CR') {
        return 'orange';
    }
    if (xcanal == 'CR') {
        return 'orange';
    }
    return window.chartColors.black;
}

/**
 * Metodo utilizado para generar graficas
 * @param {any} xIDForeCast
 */
function generarGraficaDeVentas(xIDForeCast) {
    //var _data = $("#fmrValidacionUsuario").serialize();
    $.ajax({
        url: "/ForeCast/GenerarGraficaConsolidadoPorCanal",
        type: "POST",
        data: "xIDForeCast=" + xIDForeCast,
        dataType: "html",
        success: function (data) {
            $('#cuerpopopupGenerarGrafica').html(data);

            var _lstDatos = JSON.parse($('#dhfDatosGrafica').val());

            $('#divContenedorGraficas').html("");

            var arraySegundo = [];
            for (var i = 1; i < _lstDatos.lstColumnas.length; i++) {
                arraySegundo.push(_lstDatos.lstColumnas[i].Nombre);
            }

            var arrayDatos = [];
            for (var i = 0; i < _lstDatos.lstDetalleProyeccionResumen.length; i++) {
                arrayDatos.push({
                    label: _lstDatos.lstDetalleProyeccionResumen[i].Canal,
                    backgroundColor: _lstDatos.lstDetalleProyeccionResumen[i].Nombre,
                    borderColor: _lstDatos.lstDetalleProyeccionResumen[i].Nombre,//determinaColor(_lstDatos.lstDetalleProyeccionResumen[i].Canal),
                    data: _lstDatos.lstDetalleProyeccionResumen[i].MatSemanas,
                    fill: false,
                });
            }

            var config = {
                type: 'line',
                data: {
                    labels: arraySegundo,
                    datasets: arrayDatos
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Cantidad de Tallos  - Canales Vs Semana operacional'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                // the data minimum used for determining the ticks is Math.min(dataMin, suggestedMin)
                                suggestedMin: 10,

                                // the data maximum used for determining the ticks is Math.max(dataMax, suggestedMax)
                                suggestedMax: 50
                            }
                        }]
                    }
                }
            };

            $('#divContenedorGraficas').html("<canvas style=\"height: 450vh;\" id=\"canvas\"></canvas>");

            var ctx = document.getElementById('canvas').getContext('2d');
            window.myLine = new Chart(ctx, config);

            $('#popupGenerarGrafica').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            //mostrarMensaje(2, error);
        }
    });
}

/**
 * Habilitar caja de texto a cajas
 */
function habilitarCajaADatos() {
    $('#txtValorAAplicarDatos').removeAttr('readonly');
    $('#txtValorAAplicarDatos').val(0);
}

/**
 * Inhabilitar caja de texto a cajas
 */
function inactivarCajaADatos() {
    var xValorSeleccionados = parseFloat('0');
    for (var i = 0; i < lstItemsSeleccionados.length; i++) {
        xValorSeleccionados += parseFloat($(lstItemsSeleccionados[i]).html());
    }
    $('#txtValorAAplicarDatos').attr('readonly', 'readonly');
    $('#txtValorAAplicarDatos').val(xValorSeleccionados);
}

/**
 * Remueve itemse seleccionados
 */
function limpiarItemsSeleccionados() {
    $("#tblDatosProyeccion tr").selectable("destroy");
    $("#tblDatosProyeccion tr").selectable({
        cancel: "a,.cancel"
    });
}

/**
 * Carga al precionar enter
 */
function cargarEnterValores() {
    if (event.keyCode == 13) {
        conFocoAparte = true;
        aplicarValoresADatos();
    }
}

/**
 * Metodo utilizado para asignar valores a cajas seleccionadas
 */
function aplicarValoresADatos() {
    if (lstItemsSeleccionados.length > 0) {
        if ($('#rbtnValorFijo').prop('checked') == true || $('#rbtnValorPromedio').prop('checked') == true || $('#rbtnPromediaDatos').prop('checked') == true) {
            if (parseFloat($('#txtValorAAplicarDatos').val()) != null) {

                var xValorSeleccionados = parseFloat('0');
                if ($('#rbtnValorFijo').prop('checked') == true) {
                    xValorSeleccionados = parseFloat($('#txtValorAAplicarDatos').val()) * lstItemsSeleccionados.length;
                }
                if ($('#rbtnValorPromedio').prop('checked') == true) {
                    xValorSeleccionados = parseFloat($('#txtValorAAplicarDatos').val());
                }
                if ($('#rbtnPromediaDatos').prop('checked') == true) {
                    for (var i = 0; i < lstItemsSeleccionados.length; i++) {
                        xValorSeleccionados += parseFloat($(lstItemsSeleccionados[i]).html());
                    }
                }
                var lstColumnas = $('#tblColumnas tr td');

                /*ASIGNA VALOR CELDA A CELDA*/
                for (var i = 0; i < lstItemsSeleccionados.length; i++) {
                    var xValorAnteriorActual = parseFloat($(lstItemsSeleccionados[i]).html());
                    var xNuevoValor = (xValorSeleccionados / lstItemsSeleccionados.length);
                    var xColumna = parseInt($($(lstItemsSeleccionados[i])).attr("IDColuma"));
                    var xFila = parseInt($($(lstItemsSeleccionados[i]).parent()).attr("idFila"));
                    xFila = xFila - 1;
                    cargaValorTxt(lstItemsSeleccionados[i], xNuevoValor, xFila, xColumna);
                    $(lstItemsSeleccionados[i]).attr("class", $(lstItemsSeleccionados[i]).attr("class") + ' ItemConModificaciones')
                    var xRowAnterior = $(lstItemsSeleccionados[i]).parent();

                    /*Inserta movimiento*/
                    if (parseFloat(xValorAnteriorActual) != parseFloat(xNuevoValor)) {

                        _IDUniversal++;
                        var objMovimiento = new movimiento(_IDUniversal);
                        objMovimiento.SetID = 0;
                        objMovimiento.SetAnteriorValor = parseFloat(xValorAnteriorActual);
                        objMovimiento.SetNuevoValor = parseFloat(xNuevoValor);
                        objMovimiento.SetAnioSemana = $(lstColumnas[xColumna + 6]).html();
                        objMovimiento.SetIDUniversal = xRowAnterior.attr('IdUniversal');
                        objMovimiento.SetID = xRowAnterior.attr('Id');
                        objMovimiento.SetCanal = xRowAnterior.attr('Canal');
                        objMovimiento.SetCodigoPedido = xRowAnterior.attr('CodigoPedido');
                        objMovimiento.SetIDCliente = xRowAnterior.attr('IDCliente');
                        objMovimiento.SetIDProductos = xRowAnterior.attr('IDProductos');
                        objMovimiento.SetIDPresentaciones = xRowAnterior.attr('IDPresentaciones');
                        objMovimiento.SetIDTemporadaBase = xRowAnterior.attr('IDTemporadaBase');
                        objMovimiento.SetIDPeriodo = xRowAnterior.attr('IDPeriodo');
                        _lstMovimiento.push(objMovimiento);
                    }

                }

                limpiarItemsSeleccionados();
                $('#popupAsignarValorMultriple').modal('hide');

            } else {
                mostrarMensaje(2, "Debe indicar un valor diferente a vacio.");
            }
        } else {
            mostrarMensaje(2, "Debe seleccionar la operación que desea realizar.");
        }
    } else {
        mostrarMensaje(2, "Debe seleccionar una o varias celdas.");
    }
}

/**
 * Funcin utilizada para cargar resumen
 * @param {any} xIDForeCast
 */
function generarResumenGeneral(xIDForeCast) {
    //var _data = $("#fmrValidacionUsuario").serialize();
    $.ajax({
        url: "/ForeCast/resumenGenerarForeCast",
        type: "POST",
        data: "xIDForeCast=" + xIDForeCast,
        dataType: "html",
        success: function (data) {
            $('#cuerpoExportacionAbcap').html(data);
            $('#popupExportacionAbcap').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            //mostrarMensaje(2, error);
        }
    });
}

/**
 * Funcion utilizada para mostrar popup de inconsistencias
 * @param {any} Canal
 * @param {any} Anio
 * @param {any} IDForeCast
 */
function consultaInconsistenciasCanalYAnio(Canal, Anio, IDForeCast) {
    //var _data = $("#fmrValidacionUsuario").serialize();
    $.ajax({
        url: "/ForeCast/consultaInconsistenciaCanalAnio",
        type: "POST",
        data: "xCanal=" + Canal + "&xAnio=" + Anio + "&xIDForeCast=" + IDForeCast,
        dataType: "html",
        success: function (data) {
            $('#cuerpoDetalleErroresResumen').html(data);
            $('#popupDetalleErroresResumen').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            //mostrarMensaje(2, error);
        }
    });
}


/**
 * FIltrar datos de combobox
 */
function filtrarDatosBD() {
    actualizaGrid(_ultimoIndexSeleccionado);
}


/**
 * Muestra ventana de validación 
 */
function validacionExportacionConsolidacion(xIDForeCast) {
    $('#hdfIDForeCast').val(xIDForeCast);
    $('#popupValidacionExportacion').modal('show');
}

/**
 * Inicia proceso de exportación de datos a abpca_Gestion_Comercial
 */
function aceptarExportacionAbcapGestionComercial() {
    //var _data = $("#fmrValidacionUsuario").serialize();
    var xIDforeCast = parseInt($('#hdfIDForeCast').val());
    $.ajax({
        url: "/ForeCast/ExportarDatosGestionComercial",
        type: "POST",
        data: "xIDForeCast=" + xIDforeCast,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
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