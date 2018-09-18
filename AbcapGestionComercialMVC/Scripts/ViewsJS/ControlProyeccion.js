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
    $(xTd).html("<input id='txttemporal' onkeypress='return validaLetras(event);' onkeydown='return evaluaLetra(this);' type='text' value='" + xValorAnterior + "' class='form-control form-control-sm'>")
    $('#txttemporal').focus();
    $('#txttemporal').select();
    setTimeout(seleccionarTexto, 200);
}

/**
 * Valida que solo se ingresen letras
 * @param {any} e
 */
function validaLetras(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
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
        if (anteriorTD != null) {
            console.log($('#txttemporal').val());
            $(anteriorTD).html($('#txttemporal').val());
        }
        iniciaEdicion(this);
        anteriorTD = this;
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
        deselectAllText: "Deseleccionar",
        noneSelectedText: "Clientes",
        selectAllText: "Todo seleccionado"
    });
    $('#DromnDownClientes').on('hidden.bs.select', function (e) {

    });


    $('#DromnDownProductos').selectpicker({
        liveSearch: true,
        deselectAllText: "Deseleccionar",
        noneSelectedText: "Productos",
        selectAllText: "Todo seleccionado"
    });

    $('#DromnDownPresentacion').selectpicker({
        liveSearch: true,
        deselectAllText: "Deseleccionar",
        noneSelectedText: "Presentación",
        selectAllText: "Todo seleccionado"
    });

    $('#DromnDownTemporada').selectpicker({
        liveSearch: true,
        deselectAllText: "Deseleccionar",
        noneSelectedText: "Temporada",
        selectAllText: "Todo seleccionado"
    });

    eventosGrilla();
}

/**
 * Evalua cada letra que se pulsa sobre la tabla de la proyección
 * @param {any} xTxt
 */
function evaluaLetra(xTxt) {

    var decValorAnterior = parseFloat('0');
    decValorAnterior = parseFloat($('#txttemporal').val());
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
        if (anteriorFila.length > 0) {
            if (anteriorTD != null) {
                $(anteriorTD).html(number_format(xValorActual, 2));
                _BDDetalle[xFila].MatSemanas[xColumna] = decValorAnterior;
                xInsertarMovimiento = 1;
            }
            calcularTotalProducto(anteriorTD);
            iniciaEdicion(anteriorFila.find('td')[xIndex]);
            anteriorTD = anteriorFila.find('td')[xIndex];
        } else {
            xIndex = xIndex - 1;
            if (xIndex > -1) {
                if (anteriorTD != null) {
                    $(anteriorTD).html(number_format(xValorActual, 2));
                    _BDDetalle[xFila].MatSemanas[xColumna] = decValorAnterior;
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
                $(anteriorTD).html(number_format(xValorActual, 2));
                _BDDetalle[xFila].MatSemanas[xColumna] = decValorAnterior;
                xInsertarMovimiento = 1;
            }
            calcularTotalProducto(anteriorTD);
            iniciaEdicion(anteriorFila.find('td')[xIndex]);
            anteriorTD = anteriorFila.find('td')[xIndex];
        } else {
            xIndex = xIndex + 1;
            if (xIndex < $($($(anteriorTD).parent()).prev()).find('td').length) {
                if (anteriorTD != null) {
                    $(anteriorTD).html(number_format(xValorActual, 2));
                    _BDDetalle[xFila].MatSemanas[xColumna] = decValorAnterior;
                    xInsertarMovimiento = 1;
                }
                calcularTotalProducto(anteriorTD);
                var xFilas = $($($(anteriorTD).parent()).parent()).find('tr');
                anteriorFila = xFilas[0];
                iniciaEdicion($(anteriorFila).find('td')[xIndex]);
                anteriorTD = $(anteriorFila).find('td')[xIndex];
            }
        }
    }
    if (event.keyCode == 37) { /*IZQUIERDA*/
        var xCadenaSeleccion = getSelected();
        if (xCadenaSeleccion[0] == "") {
            var iIndex = $(anteriorTD).index();
            if (iIndex - 1 >= 0) {
                if (anteriorTD != null) {
                    $(anteriorTD).html(number_format(xValorActual, 2));
                    _BDDetalle[xFila].MatSemanas[xColumna] = decValorAnterior;
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
                        $(anteriorTD).html(number_format(xValorActual, 2));
                        _BDDetalle[xFila].MatSemanas[xColumna] = decValorAnterior;
                        xInsertarMovimiento = 1;
                    }
                    calcularTotalProducto(anteriorTD);
                    anteriorTD = $(ifilaSiguiente).find('td')[0];
                    iniciaEdicion(anteriorTD);
                }
            } else {
                if (anteriorTD != null) {
                    $(anteriorTD).html(number_format(xValorActual, 2));
                    _BDDetalle[xFila].MatSemanas[xColumna] = decValorAnterior;
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
        objMovimiento.SetAnioSemana = $(lstColumnas[xColumna + 8]).html();
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
function actualizaGrid(xIndexSeleccionado) {
    var xInicio = parseInt('0');
    var xFinal = parseInt('0');

    if (this._BDDetalle.length > 0) {
        var xCantidaRegistros = parseInt(20);

        xInicio = xIndexSeleccionado - parseInt((xCantidaRegistros / 2) - 1);
        xFinal = xIndexSeleccionado + parseInt(xCantidaRegistros / 2) + 1;

        if (xIndexSeleccionado <= xCantidaRegistros) {
            if (_RegistroGuia < 9) {
                xInicio = 0;
                xFinal = xCantidaRegistros;
                _RegistroGuia = 9;
            }
        }
        if (xIndexSeleccionado >= (_BDDetalle.length - xCantidaRegistros)) {
            if (xIndexSeleccionado >= _BDDetalle.length - parseInt((xCantidaRegistros / 2) - 1)) {
                _RegistroGuia = (_BDDetalle.length - 1) - parseInt((xCantidaRegistros / 2) - 1);
                xInicio = _RegistroGuia - parseInt((xCantidaRegistros / 2) - 1);
                xFinal = _RegistroGuia + parseInt(xCantidaRegistros / 2);
            }
        }

        /*Detalle proyeccion*/
        var strFilasProyeccionResumenHTML = '';
        var strFilasProyeccionResumenHTMLFlotante = '<tr class=""><td class="columna-conWidth" colSpam="4"></td></tr>';
        strFilasProyeccionResumenHTML = this._cabeceraHTML;
        for (var intColumna = xInicio; intColumna < xFinal; intColumna++) {

            strFilasProyeccionResumenHTML +=
                '<tr idFila="' + this._BDDetalle[intColumna].ID +
                '" IdUniversal="' + intColumna +
                '" Id="' + this._BDDetalle[intColumna].ID +
                '" Canal="' + this._BDDetalle[intColumna].Canal +
                '" CodigoPedido="' + this._BDDetalle[intColumna].CodigoPedido +
                '" IDCliente="' + this._BDDetalle[intColumna].IDcliente +
                '" IDProductos="' + this._BDDetalle[intColumna].IDProductos +
                '" IDPresentaciones="' + this._BDDetalle[intColumna].IDPresentaciones +
                '" IDTemporadaBase="' + this._BDDetalle[intColumna].IDTemporadasBase +
                '" IDPeriodo="' + this._BDDetalle[intColumna].IDPeriodo +
                '" >' +
                '<td tabIndex="' + intColumna + '" class="column_' + getColumna(17).ID + '" style="max-width:' + getColumna(17).WidthInicial + 'px" >' + this._BDDetalle[intColumna].ID + '</td>' +
                '<td tabIndex="' + intColumna + '" class="column_' + getColumna(18).ID + '" style="max-width:' + getColumna(18).WidthInicial + 'px" >' + this._BDDetalle[intColumna].Canal + '</td>' +
                '<td tabIndex="' + intColumna + '" class="column_' + getColumna(19).ID + '"  style="max-width:' + getColumna(19).WidthInicial + 'px" title="' + getCliente(this._BDDetalle[intColumna].IDcliente).Nombre + '">' + getCliente(this._BDDetalle[intColumna].IDcliente).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" class="column_' + getColumna(21).ID + '"  style="max-width:' + getColumna(21).WidthInicial + 'px" title="' + getProducto(this._BDDetalle[intColumna].IDProductos).Nombre + '">' + getProducto(this._BDDetalle[intColumna].IDProductos).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" class="column_' + getColumna(22).ID + '"  style="max-width:' + getColumna(22).WidthInicial + 'px">' + getPresentacion(this._BDDetalle[intColumna].IDPresentaciones).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" class="column_' + getColumna(23).ID + ' ultimaColumna"  style="max-width:' + getColumna(23).WidthInicial + 'px">' + getTemporada(this._BDDetalle[intColumna].IDTemporadasBase).Nombre + '</td>';

            strFilasProyeccionResumenHTMLFlotante +=
                '<tr>' +
                '<td tabIndex="' + intColumna + '" id="col_' + this._BDDetalle[intColumna].ID + '"  style="display:none" class="columnF columnF_' + getColumna(19).ID + '">' + getCliente(this._BDDetalle[intColumna].IDcliente).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" id="col_' + this._BDDetalle[intColumna].ID + '"  style="display:none" class=" columnF columnF_' + getColumna(21).ID + '">' + getProducto(this._BDDetalle[intColumna].IDProductos).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" id="col_' + this._BDDetalle[intColumna].ID + '"  style="display:none"  class=" columnF columnF_' + getColumna(22).ID + '">' + getPresentacion(this._BDDetalle[intColumna].IDPresentaciones).Nombre + '</td>' +
                '<td tabIndex="' + intColumna + '" id="col_' + this._BDDetalle[intColumna].ID + '"  style="display:none" class="columnF columnF_' + getColumna(23).ID + '">' + getTemporada(this._BDDetalle[intColumna].IDTemporadasBase).Nombre + '</td></tr>';

            /*CARGA SEMANAS*/
            for (var intSemanas = 0; intSemanas < this._BDDetalle[intColumna].MatSemanas.length; intSemanas++) {
                strFilasProyeccionResumenHTML += '<td  tabIndex="' + intSemanas + '" IDColuma = "' + intSemanas + '" class="celda-modifica ' + (this._BDDetalle[intColumna].MatSemanas[intSemanas] != 0 ? "cajaConDato" : "") + ' datoSemana column_' + (intSemanas + 8) + '" >' + number_format(this._BDDetalle[intColumna].MatSemanas[intSemanas], 2) + '</td>';
            }

            strFilasProyeccionResumenHTML += '</tr>';
        }
        $('#tblDatosProyeccion').html(strFilasProyeccionResumenHTML);
        $('.tbl-Cuerpop-Flotante').html(strFilasProyeccionResumenHTMLFlotante);

        eventosGridProyeccionDetalle();
        controlScrollPersonalizado();
        $('#lblTotalResumen').html((xInicio + 1) + " al " + (xFinal) + " registros de " + _BDDetalle.length);
    } else {
        $('#tblDatosProyeccion').html("");
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
                _RegistroGuia -= 3;
                actualizaGrid(_RegistroGuia);
            }
        }
        else {
            if (_RegistroGuia < (_BDDetalle.length - 2)) {
                _RegistroGuia += 3;
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
    setInterval(sincronizacionBD, 10000)

});

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
            var _IDModificacion = "1000";
            $('#CapEstado').html("Sincronizando...");
            _enSincronizacion = 1;
            $.ajax({
                url: '/ControlProyeccion/SincronizarBDTemporal/',
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
    $.ajax({
        url: "/ControlProyeccion/consultarProyecciones",
        type: "POST",
        //data: _data,
        dataType: "html",
        success: function (data) {
            $('#cuerpoPopupConsultaProyecciones').html(data);
            $('#popupConsultarProyecciones').modal("show").on("shown.bs.modal", function () {
                $('#ddlProyeccionVenta').val($('#hdfIDProyeccionVenta').val());
            });
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