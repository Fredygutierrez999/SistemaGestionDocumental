var ItemSeleccionado = null;
var lstCanales = new Array();
var lstClientes = new Array();
var lstProductos = new Array();
var lstProductosFiltrados = new Array();
var lstVersion = new Array();
var lstPresentacion = new Array();
var lstTemporadabase = new Array();
var lstResumenBD = new Array();

var colorSeleccionado = "#f9b959";
var colorNoSeleccionado = "#ffffff";

var xAntCanalSelec = null;
var xAntClienteSelec = null;
var xAntProductoSelec = null;
var xAntVersionProductoSelec = null;
var xAntPresentacionSelec = null;
var xAntTemporadaSelec = null;

var xCanalSelec = null;
var xClienteSelec = null;
var xProductoSelec = null;
var xVersionProductoSelec = null;
var xPresentacionSelec = null;
var xTemporadaSelec = null;
var lstProductosTemporal = new Array();
var lstProductosSeleccionados = new Array();

/**
 * retorna cliente por ID
 * @param {any} xIDCliente
 */
function getCliente(xIDCliente) {
    return lstClientes.find(m => m.ID == xIDCliente);
}

/**
 * retorna cliente por ID
 * @param {any} xIDProducto
 */
function getProductos(xIDProducto) {
    return lstProductos.find(m => m.ID == xIDProducto);
}

/**
 * retorna Presentacion por ID
 * @param {any} xIDPresentacion
 */
function getPresentacion(xIDPresentacion) {
    return lstPresentacion.find(m => m.ID == xIDPresentacion);
}

/**
 * retorna temporada base por ID
 * @param {any} xIDTemporada
 */
function getTemporada(xIDTemporada) {
    return lstTemporadabase.find(m => m.ID == xIDTemporada);
}

/**
 * metodo encargado de validar si existen item a un nivel inferiro para seleccionar checkbox
 * @param {any} XiD valor
 * @param {any} Grupo Grupo
 */
function getExisteMarca(XiD, Grupo) {
    switch (Grupo) {
        case 'Cana':
            return lstResumenBD.find(m => m.Canal == XiD && m.Seleccionado == true) != null;
            break;
        case 'Clie':
            return lstResumenBD.find(m => m.Canal == xCanalSelec && m.IDCliente == XiD && m.Seleccionado == true) != null;
            break;
        case 'Prod':
            var arrProductos = new Array();
            lstResumenBD.forEach(function (obj) {
                if (obj.Canal == xCanalSelec &&
                    obj.IDCliente == xClienteSelec &&
                    obj.Seleccionado == true) {
                    arrProductos.push(obj);
                }
            });

            var objProductoSelec = getProductos(XiD);
            return arrProductos.find(
                m => m.Canal == xCanalSelec &&
                    m.IDCliente == xClienteSelec &&
                    getProductos(m.IDProductos).Nombre == objProductoSelec.Nombre &&
                    m.Seleccionado == true) != null;
            break;
        case 'Revi':
            return lstResumenBD.find(
                m => m.Canal == xCanalSelec &&
                    m.IDCliente == xClienteSelec &&
                    m.IDProductos == XiD &&
                    m.Seleccionado == true) != null;
            break;
        case 'Pres':
            return lstResumenBD.find(
                m => m.Canal == xCanalSelec &&
                    m.IDCliente == xClienteSelec &&
                    m.IDProductos == xProductoSelec &&
                    m.IDPresentacion == XiD &&
                    m.Seleccionado == true) != null;
            break;
        case 'Temp':
            return lstResumenBD.find(
                m => m.Canal == xCanalSelec &&
                    m.IDCliente == xClienteSelec &&
                    m.IDProductos == xProductoSelec &&
                    m.IDPresentacion == xPresentacionSelec &&
                    m.IDTemporadaBase == XiD &&
                    m.Seleccionado == true) != null;
            break;
    }
}

/**
 * Retorna estado de controles
 */
function estadoControles() {
    if ($('#hdfEstadoModificacion').val() == "True") {
        return "";
    } else {
        return "disabled=\"disabled\"";
    }
}


/**
 * Carga listao de base de datos
 */
function cargaListadosBD() {
    var resumenBD = JSON.parse($('#hdfData_Canales').val());
    lstCanales = resumenBD.Canales;
    lstClientes = resumenBD.Clientes;
    lstProductos = resumenBD.Productos;
    lstPresentacion = resumenBD.Presentaciones;
    lstTemporadabase = resumenBD.TemporadasBase;
    lstResumenBD = resumenBD.DatosResumen;
    $('#hdfData_Canales').val('');
    /*Carga canales*/
    var strHtml = '';
    for (var i = 0; i < lstCanales.length; i++) {
        strHtml +=
            '<div>' +
            '<input type="CheckBox" ' + (getExisteMarca(lstCanales[i].Canal, 'Cana') ? 'checked="checked"' : '') + ' ' + estadoControles() + '  onChange="marcarCheckBox(\'Cana\',this)" id="chkCanal_' + lstCanales[i].Canal + '" tabindex="' + i + '" value="' + lstCanales[i].Canal + '" /> <small onclick="cargarClientePorCanal(\'' + lstCanales[i].Canal + '\')" id="lblCanal_' + lstCanales[i].Canal + '" tabindex="0" class="itemSeleccionado">' + lstCanales[i].Canal + '</small>' +
            '</div > ';
    }
    $('#lblCantidadCanal').html(lstCanales.length);
    $('#divCanales').html(strHtml);
    cargarClientePorCanal(lstCanales[0].Canal);
}

/**
 * Carga listado de clientes filtrados por el canal
 * @param {any} xCanal
 */
function cargarClientePorCanal(xCanal) {
    xCanalSelec = xCanal;

    /*SELECCION ITEM*/
    if (xAntCanalSelec != null) {
        $('#lblCanal_' + xAntCanalSelec).css("background-color", colorNoSeleccionado);
    }
    $('#lblCanal_' + xCanalSelec).css("background-color", colorSeleccionado);
    xAntCanalSelec = xCanalSelec;

    var strHtml = "";
    var objInicio = null;
    var _clientes = {};
    for (var i = 0; i < lstResumenBD.length; i++) {
        if (lstResumenBD[i].Canal == xCanal) {
            var objCliente = _clientes[lstResumenBD[i].IDCliente];
            if (objCliente == null) {
                _clientes[lstResumenBD[i].IDCliente] = lstResumenBD[i];
            }
        }
    }
    var xCantidadCliente = 0;
    var lstvalores = Object.values(_clientes);

    lstvalores.sort(function (a, b) {
        var xNombreA = getCliente(a.IDCliente).Nombre;
        var xNombreB = getCliente(b.IDCliente).Nombre;
        if (xNombreA > xNombreB) {
            return 1;
        }
        if (xNombreA < xNombreB) {
            return -1;
        }
        return 0;
    });

    /*RECORRE DICCIONARIO*/
    for (var i = 0; i < lstvalores.length; i++) {
        var objCliente = lstvalores[i];
        strHtml +=
            '<div idCliente = "' + objCliente.IDCliente + '">' +
            '<input type="CheckBox" ' + estadoControles() + ' onChange="marcarCheckBox(\'Clie\',this)" ' + (getExisteMarca(objCliente.IDCliente, 'Clie') ? 'checked="checked"' : '') + ' id="chkCliente_' + objCliente.IDCliente + '"  tabindex="' + i + '" value="' + objCliente.IDCliente + '" /> ' +
            '<small tabindex="0" onclick="cargarProductosPorCanalCliente(\'' + objCliente.IDCliente + '\')" id="lblCliente_' + objCliente.IDCliente + '" class="itemSeleccionado">' + getCliente(objCliente.IDCliente).Nombre + '</small>' +
            '</div > ';
        if (objInicio == null) {
            objInicio = objCliente;
        }
        xCantidadCliente += 1;
    }

    $('#lblCantidadCliente').html(xCantidadCliente);

    $('#divClientes').html(strHtml);
    if (objInicio != null) {
        cargarProductosPorCanalCliente(objInicio.IDCliente);
    }
}

/**
 * Carga listado de productos filtrados por el canal y cliente
 * @param {any} xCanal
 */
function cargarProductosPorCanalCliente(xCliente) {
    xClienteSelec = xCliente;

    /*SELECCION ITEM*/
    if (xAntClienteSelec != null) {
        $('#lblCliente_' + xAntClienteSelec).css("background-color", colorNoSeleccionado);
    }
    $('#lblCliente_' + xCliente).css("background-color", colorSeleccionado);
    xAntClienteSelec = xCliente;

    var strHtml = "";
    var xObjProducto = null;
    var _Productos = {};
    lstProductosFiltrados = new Array();
    for (var i = 0; i < lstResumenBD.length; i++) {
        if (lstResumenBD[i].Canal == xCanalSelec && lstResumenBD[i].IDCliente == xClienteSelec) {
            var _objProductoData = getProductos(lstResumenBD[i].IDProductos);
            var _productoItem = _Productos[_objProductoData.Nombre];
            if (_productoItem == null) {
                _Productos[_objProductoData.Nombre] = lstResumenBD[i];
            }
            lstProductosFiltrados.push(lstResumenBD[i]);
        }
    }

    var lstvalores = Object.values(_Productos);
    lstvalores.sort(function (a, b) {
        var xNombreA = getProductos(a.IDProductos).Nombre;
        var xNombreB = getProductos(b.IDProductos).Nombre;
        if (xNombreA > xNombreB) {
            return 1;
        }
        if (xNombreA < xNombreB) {
            return -1;
        }
        return 0;
    });

    /*RECORRE DICCIONARIO*/
    var xCantidadProductos = 0;
    for (var i = 0; i < lstvalores.length; i++) {
        var objProducto = lstvalores[i];
        strHtml +=
            '<div idProducto = "' + objProducto.IDProductos + '">' +
            '<input ' + estadoControles() + ' type="CheckBox" ' + (getExisteMarca(objProducto.IDProductos, 'Prod') ? 'checked="checked"' : '') + ' onChange="marcarCheckBox(\'Prod\',this)" id="chkProducto_' + objProducto.IDProductos + '" tabindex="' + i + '" value="' + objProducto.IDProductos + '" /> ' +
            '<small class=\"context-producto\" tabindex="0" onmousedown=\"menuContextualMouse(event,this);\" onclick="cargarVersionPorPorducto(\'' + objProducto.IDProductos + '\')"  id="lblProducto_' + objProducto.IDProductos + '" selectIndex=0 class="itemSeleccionado">' + getProductos(objProducto.IDProductos).Nombre + '</small>' +
            '</div > ';
        if (xObjProducto == null) {
            xObjProducto = objProducto;
        }
        xCantidadProductos += 1;
    }

    $('#lblCantidadProducto').html(xCantidadProductos);

    $('#divProductos').html(strHtml);
    if (xObjProducto != null) {
        cargarVersionPorPorducto(xObjProducto.IDProductos)
    }
}

/**
 * Carga listado de version filtrados por el canal, cliente, producto y version
 * @param {any} xCanal
 */
function cargarVersionPorPorducto(xProducto) {
    xVersionProductoSelec = xProducto;

    /*SELECCION ITEM*/
    if (xAntVersionProductoSelec != null) {
        $('#lblProducto_' + xAntVersionProductoSelec).css("background-color", colorNoSeleccionado);
    }
    $('#lblProducto_' + xVersionProductoSelec).css("background-color", colorSeleccionado);
    xAntVersionProductoSelec = xVersionProductoSelec;
    var objProductoDataFiltro = getProductos(parseInt(xVersionProductoSelec));

    var strHtml = "";
    var objItem = null;
    var _revisiones = {};
    for (var i = 0; i < lstProductosFiltrados.length; i++) {

        var objProductoData = getProductos(lstProductosFiltrados[i].IDProductos);
        if (lstProductosFiltrados[i].Canal == xCanalSelec &&
            lstProductosFiltrados[i].IDCliente == xClienteSelec &&
            objProductoData.Nombre == objProductoDataFiltro.Nombre) {
            var objtempRevision = _revisiones[objProductoData.Revision];
            if (objtempRevision == null) {
                _revisiones[objProductoData.Revision] = lstProductosFiltrados[i];
            }
        }
    }

    var CantidadRevision = 0;
    /*RECORRE DICCIONARIO*/
    for (key in _revisiones) {
        var objRevision = _revisiones[key];
        strHtml +=
            '<div>' +
            '<input ' + estadoControles() + ' type="CheckBox" ' + (getExisteMarca(objRevision.IDProductos, 'Revi') ? 'checked="checked"' : '') + ' onChange="marcarCheckBox(\'Vers\',this)" id="chkRevision_' + objRevision.IDProductos + '" tabindex="' + i + '" value="' + objRevision.IDProductos + '" /> ' +
            '<small tabindex="0" onclick="cargarPresentacionPorPorducto(\'' + objRevision.IDProductos + '\')" id="lblRevision_' + objRevision.IDProductos + '" selectIndex=0 class="itemSeleccionado">' + getProductos(objRevision.IDProductos).Revision + '</small>' +
            '</div > ';
        if (objItem == null) {
            objItem = objRevision;
        }
        CantidadRevision += 1;
    }

    $('#lblCantidadVersion').html(CantidadRevision);

    $('#divVersiones').html(strHtml);
    if (objItem != null) {
        cargarPresentacionPorPorducto(objItem.IDProductos);
    }
}

/**
 * Carga listado de presentaciones filtrados por el canal, cliente y producto
 * @param {any} xCanal
 */
function cargarPresentacionPorPorducto(xProducto) {
    xProductoSelec = xProducto;

    /*SELECCION ITEM*/
    if (xAntProductoSelec != null) {
        $('#lblRevision_' + xAntProductoSelec).css("background-color", colorNoSeleccionado);
    }
    $('#lblRevision_' + xProductoSelec).css("background-color", colorSeleccionado);
    xAntProductoSelec = xProductoSelec;

    var strHtml = "";
    var objItem = null;
    var _presentaciones = {};
    for (var i = 0; i < lstResumenBD.length; i++) {
        if (lstResumenBD[i].Canal == xCanalSelec &&
            lstResumenBD[i].IDCliente == xClienteSelec &&
            lstResumenBD[i].IDProductos == xProductoSelec) {
            var objTempoPresentaciones = _presentaciones[lstResumenBD[i].IDPresentacion];
            if (objTempoPresentaciones == null) {
                _presentaciones[lstResumenBD[i].IDPresentacion] = lstResumenBD[i];
            }
        }
    }

    var cantidadPresentacion = 0;
    /*RECORRE DICCIONARIO*/
    for (key in _presentaciones) {
        var objPresentacion = _presentaciones[key];
        strHtml +=
            '<div>' +
            '<input ' + estadoControles() + ' type="CheckBox"   ' + (getExisteMarca(objPresentacion.IDPresentacion, 'Pres') ? 'checked="checked"' : '') + '  onChange="marcarCheckBox(\'Pres\',this)" id="chkPresentacion_' + objPresentacion.IDPresentacion + '" tabindex="' + i + '" value="' + objPresentacion.IDPresentacion + '" /> ' +
            '<small tabindex="0" onclick="cargarTemporadasPorPresentacion(\'' + objPresentacion.IDPresentacion + '\')" id="lblPresentacion_' + objPresentacion.IDPresentacion + '" selectIndex=0 class="itemSeleccionado">' + getPresentacion(objPresentacion.IDPresentacion).Nombre + '</small>' +
            '</div > ';
        if (objItem == null) {
            objItem = objPresentacion;
        }
        cantidadPresentacion += 1;
    }

    $('#lblCantidadPresentacion').html(cantidadPresentacion);

    $('#divPresentaciones').html(strHtml);
    if (objItem != null) {
        cargarTemporadasPorPresentacion(objItem.IDPresentacion);
    }
}

/**
 * Carga listado de temporadas filtrados por el canal, cliente,producto y Presentacion
 * @param {any} xCanal
 */
function cargarTemporadasPorPresentacion(xPresentacion) {
    xPresentacionSelec = xPresentacion;

    /*SELECCION ITEM*/
    if (xAntPresentacionSelec != null) {
        $('#lblPresentacion_' + xAntPresentacionSelec).css("background-color", colorNoSeleccionado);
    }
    $('#lblPresentacion_' + xPresentacion).css("background-color", colorSeleccionado);
    xAntPresentacionSelec = xPresentacionSelec;

    var objItem = null;
    var strHtml = "";
    var CantidadTemporada = 0;
    for (var i = 0; i < lstResumenBD.length; i++) {
        if (lstResumenBD[i].Canal == xCanalSelec &&
            lstResumenBD[i].IDCliente == xClienteSelec &&
            lstResumenBD[i].IDProductos == xProductoSelec &&
            lstResumenBD[i].IDPresentacion == xPresentacionSelec) {
            strHtml +=
                '<div>' +
                '<input ' + estadoControles() + ' type="CheckBox"  ' + (getExisteMarca(lstResumenBD[i].IDTemporadaBase, 'Temp') ? 'checked="checked"' : '') + ' onChange="marcarCheckBox(\'Temp\',this)" id="chkTemporadaBase_' + lstResumenBD[i].IDTemporadaBase + '" tabindex="' + i + '" value="' + lstResumenBD[i].IDTemporadaBase + '" /> ' +
                '<small tabindex="0" id="lblTemporadaBase_' + lstResumenBD[i].IDTemporadaBase + '" selectIndex=0 class="itemSeleccionado">' + getTemporada(lstResumenBD[i].IDTemporadaBase).Nombre + '</small>' +
                '</div > ';
            if (objItem == null) {
                objItem = lstResumenBD[i];
            }
            CantidadTemporada += 1;
        }
    }

    $('#lblCantidadTemporada').html(CantidadTemporada);

    $('#divTemporadas').html(strHtml);
    if (objItem != null) {
        cargarTemporadaSeleccionada(objItem.IDTemporadaBase);
    }
}

/**
 * Carga listado de temporadas filtrados por el canal, cliente,producto, Presentacion y temporada
 * @param {any} xCanal
 */
function cargarTemporadaSeleccionada(xTemporada) {
    xTemporadaSelec = xTemporada;

    /*SELECCION ITEM*/
    if (xAntTemporadaSelec != null) {
        $('#lblTemporadaBase_' + xAntTemporadaSelec).css("background-color", colorNoSeleccionado);
    }
    $('#lblTemporadaBase_' + xTemporada).css("background-color", colorSeleccionado);
    xAntTemporadaSelec = xTemporadaSelec;
}






/**
 * Evalua la letra que ingresa el usuario y si es entre salta de item
 * @param {any} key
 */
function evaluaLetrabuscarCliente(key) {
    if (event.keyCode == 13) { /*ARRIBA*/
        buscarCliente($('#txtBusquedaCliente')[0]);
    }
}

/**
 * Buscacr cliente en listado de objetos ya cargados
 * @param {any} txtCliente
 */
var strClienteAnterior = "";
function buscarCliente(txtCliente) {
    var txtBusquedaCliente = $(txtCliente).val();
    if (strClienteAnterior != txtBusquedaCliente) {
        var objItemCliente = $('#divClientes > div')[0];
        cargarProductosPorCanalCliente($(objItemCliente).attr("idCliente"));
    }
    if (txtBusquedaCliente != "") {
        var itemCliente = $('#lblCliente_' + xClienteSelec).parent();
        var burcar = true;
        while (burcar) {
            var itemCliente = $(itemCliente).next();
            if (itemCliente.length > 0) {
                burcar = (itemCliente.find('small').html().toString().toUpperCase().indexOf(txtBusquedaCliente.toUpperCase()) == -1);
            } else {
                burcar = false;
            }
        }
        var IDCliente = 0;
        if (itemCliente.length > 0) {
            IDCliente = $(itemCliente).attr("idCliente");
        } else {
            IDCliente = $($("#divClientes div")[0]).attr("idCliente");
            mostrarMensaje(1, "Sin resultado");
        }
        cargarProductosPorCanalCliente(IDCliente);
        $('#lblCliente_' + IDCliente).focus();
        $(txtCliente).focus();
    }
    strClienteAnterior = txtBusquedaCliente;
}


/**
 * Evalua la letra que ingresa el usuario y si es entre salta de item
 * @param {any} key
 */
function evaluaLetrabuscarProducto(key) {
    if (event.keyCode == 13) { /*ARRIBA*/
        buscarProducto($('#txtBusquedaProducto')[0]);
    }
}

/**
 * Buscacr cliente en listado de objetos ya cargados
 * @param {any} txtProducto
 */
var xCadenaAnterior = "";
function buscarProducto(txtProducto) {
    var txtCadena = $(txtProducto).val();
    if (txtCadena != xCadenaAnterior) {
        var xDivPrimero = $('#divProductos > div')[0];
        cargarVersionPorPorducto($(xDivPrimero).attr("idProducto"));
    }
    if (txtCadena != "") {
        var itemCliente = $('#lblProducto_' + xProductoSelec).parent();
        var burcar = true;
        while (burcar) {
            var itemCliente = $(itemCliente).next();
            if (itemCliente.length > 0) {
                burcar = (itemCliente.find('small').html().toString().toUpperCase().indexOf(txtCadena.toUpperCase()) == -1);
            } else {
                burcar = false;
            }
        }

        var IDCliente = 0;
        if (itemCliente.length > 0) {
            IDCliente = $(itemCliente).attr("idProducto");
        } else {
            IDCliente = $($("#divProductos div")[0]).attr("idProducto");
            mostrarMensaje(1, "Sin resultado");
        }
        cargarVersionPorPorducto(IDCliente);
        $('#lblProducto_' + IDCliente).focus();
        $(txtProducto).focus();
    }
    xCadenaAnterior = txtCadena;
}

/**
 * Marca Item
 * @param {any} Grupo
 */
function marcarCheckBox(Grupo, chk) {
    var _nuevoEstado = $(chk).prop('checked');
    switch (Grupo) {
        case 'Cana':
            var xstrCanal = $(chk).val();
            lstResumenBD.forEach(function (obj) {
                if (obj.Canal == xstrCanal) {
                    obj.Seleccionado = _nuevoEstado;
                    obj.Modificado = true;
                }
            });
            cargarClientePorCanal(xstrCanal);
            break;
        case 'Clie':
            var xstrCliente = parseInt($(chk).val());
            lstResumenBD.forEach(function (obj) {
                if (obj.Canal == xCanalSelec && obj.IDCliente == xstrCliente) {
                    obj.Seleccionado = _nuevoEstado;
                    obj.Modificado = true;
                }
            });
            cargarProductosPorCanalCliente(xstrCliente);
            break;
        case 'Prod':
            var arrProductos = new Array();
            lstResumenBD.forEach(function (obj) {
                if (obj.Canal == xCanalSelec && obj.IDCliente == xClienteSelec) {
                    arrProductos.push(obj);
                }
            });

            var xstrProducto = parseInt($(chk).val());
            var objProductoSelec = getProductos(xstrProducto);
            arrProductos.forEach(function (obj) {
                if (obj.Canal == xCanalSelec &&
                    obj.IDCliente == xClienteSelec &&
                    getProductos(obj.IDProductos).Nombre == objProductoSelec.Nombre) {
                    obj.Seleccionado = _nuevoEstado;
                    obj.Modificado = true;
                }
            });
            cargarVersionPorPorducto(xstrProducto);
            break;
        case 'Revi':
            var xstrRevision = parseInt($(chk).val());
            lstResumenBD.forEach(function (obj) {
                if (obj.Canal == xCanalSelec &&
                    obj.IDCliente == xClienteSelec &&
                    obj.IDProductos == xstrRevision) {
                    obj.Seleccionado = _nuevoEstado;
                    obj.Modificado = true;
                }
            });
            cargarPresentacionPorPorducto(xstrRevision);
            break;
        case 'Pres':
            var xstrPresentacion = parseInt($(chk).val());
            lstResumenBD.forEach(function (obj) {
                if (obj.Canal == xCanalSelec &&
                    obj.IDCliente == xClienteSelec &&
                    obj.IDProductos == xProductoSelec &&
                    obj.IDPresentacion == xstrPresentacion) {
                    obj.Seleccionado = _nuevoEstado;
                    obj.Modificado = true;
                }
            });
            cargarTemporadasPorPresentacion(xstrPresentacion);
            break;
        case 'Temp':
            var xstrTemporada = parseInt($(chk).val());
            lstResumenBD.forEach(function (obj) {
                if (obj.Canal == xCanalSelec &&
                    obj.IDCliente == xClienteSelec &&
                    obj.IDProductos == xProductoSelec &&
                    obj.IDPresentacion == xPresentacionSelec &&
                    obj.IDTemporadaBase == xstrTemporada) {
                    obj.Seleccionado = _nuevoEstado;
                    obj.Modificado = true;
                }
            });
            cargarTemporadaSeleccionada(xstrTemporada);
            break;
    }
}


/**
 * Metodo encargado de guardar datos modificados
 */
function guardarItemModificados() {
    bloquear(true);
    var lstItemsModificados = new Array();
    lstResumenBD.forEach(function (obj) {
        if (obj.Modificado == true) {
            lstItemsModificados.push(obj);
        }
    });

    $('#hdfDataModificada').val(JSON.stringify(lstItemsModificados));
    var fmrData = $('#fmrItemsModificadoAProyectar').serialize();

    $.ajax({
        url: "/ForeCast/GuardarConfiguracionClientesProductos",
        type: "POST",
        data: fmrData,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso == true) {
                location.href = "../ForeCast/?xIDUnico=" + data.objResultado;
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
 * Agregar producto a cliente
 */
function AgregarProducto() {

    var strparametros = "xIntIDUnico=" + $('#ID').val() + "&XCliente=" + xClienteSelec + "&xCanal=" + xCanalSelec;
    $.ajax({
        url: "/ForeCast/ConsultaProductosSinAsignar",
        type: "POST",
        data: strparametros,
        dataType: "html",
        success: function (data) {
            //Remueve items seleccionados   
            lstProductosSeleccionados.splice(0, lstProductosSeleccionados.length);
            actualizaProductosSeleccionados();

            $('#cuerpoAgregarProducto').html(data);
            $('#popupAgregarProducto').modal("show");
            consultaListadoProductosSinAsignar();
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}



/**
 * Agregar producto a cliente
 */
function consultaListadoProductosSinAsignar() {
    var strFiltro = $('#txtFiltro').val();
    var strCanal = $('#ddlCanalFiltro').val();
    var strCliente = $('#ddlCliente').val();
    var strparametros = "xIDUnico=" + $('#ID').val() + "&xCLiente=" + strCliente + "&xCanal=" + strCanal + "&xFiltro=" + strFiltro;
    $.ajax({
        url: "/ForeCast/ConsultaListadoProductosSinAsignar",
        type: "POST",
        data: strparametros,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                var lstListado = new Array();
                lstListado = data.objResultado;
                lstProductosTemporal = data.objResultado;

                var strHtml = "";
                for (var i = 0; i < lstListado.length; i++) {
                    strHtml += "<tr>";
                    strHtml += "<td  style=\"width: 30px\" ><input id=\"chk_" + lstListado[i].ID + "\" onChange=\"seleccionarItem(this)\" " + (lstProductosSeleccionados.find(m => m.ID == lstListado[i].ID) == null ? "" : "checked=\"checked\"") + " type=\"checkbox\" value=\"" + lstListado[i].ID + "\" ></td>";
                    strHtml += "<td  style=\"width: 130px\">" + lstListado[i].Codigo + "</td>";
                    strHtml += "<td>" + lstListado[i].Nombre + "</td>";
                    strHtml += "<td  style=\"width: 60px\">" + lstListado[i].Revision + "</td>";
                    strHtml += "</tr>";
                }

                $('#tblCuerpoProductos').html(strHtml);
                $('#lblCantidadProductos').html(lstListado.length);

            } else {
                mostrarMensaje(2, data.MensajeProceso);
            }
        },
        error: function (jqXHR, textStatus, error) {
            $('#lblCantidadProductos').html('0');
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Funcion al seleccionar item
 * @param {any} chk Objeto Checkbox
 */
function seleccionarItem(chk) {
    var strID = parseInt($(chk).val());
    if ($(chk).prop("checked") == true) { //Selecciona item
        var objIitemBusqueda = lstProductosSeleccionados.find(m => m.ID == strID);
        if (objIitemBusqueda == null) {
            var objIitem = lstProductosTemporal.find(m => m.ID == strID);
            lstProductosSeleccionados.push(objIitem);
        }
    } else { //Deselecciona Item
        var objIitem = lstProductosTemporal.find(m => m.ID == strID);
        var index = lstProductosSeleccionados.indexOf(objIitem);
        lstProductosSeleccionados.splice(index, 1);
    }
    actualizaProductosSeleccionados();
}

/**
 * Funcion utilizada para actualizar control de productos seleccionados
 */
function actualizaProductosSeleccionados() {
    var strHTML = "";
    for (var i = 0; i < lstProductosSeleccionados.length; i++) {
        strHTML += "<tr>";
        strHTML += "<td style=\"cursor:pointer\" title=\"" + lstProductosSeleccionados[i].Nombre + "\"> <span style=\"color:red\" class=\"glyphicon glyphicon-remove-circle\" onclick=\"RemoverItemSeleccionado(" + lstProductosSeleccionados[i].ID + ");\"></span> " + lstProductosSeleccionados[i].Codigo + "</td>";
        strHTML += "</tr>";
    }
    $('#tblProductosSeleccionaos').html(strHTML);
    $('#lblCantidadProductosSeleccionados').html(lstProductosSeleccionados.length);
}

/**
 * Elimina item seleccionado
 * @param {any} xID ID producto
 */
function RemoverItemSeleccionado(strID) {
    var objIitem = lstProductosTemporal.find(m => m.ID == strID);
    var index = lstProductosSeleccionados.indexOf(objIitem);
    lstProductosSeleccionados.splice(index, 1);
    actualizaProductosSeleccionados();
    $('#chk_' + strID).prop("checked", false);
}

/**
 * Actualiza listado de clientes de acuerdo al canal
 */
function seleccionarCanalFiltro() {
    var strCanal = $('#ddlCanalFiltro').val();
    var strparametros = "strCanal=" + strCanal;
    $.ajax({
        url: "/ForeCast/ConsultaListadoClientesXCanaJson",
        type: "POST",
        data: strparametros,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {

                //Remueve items seleccionados   
                lstProductosSeleccionados.splice(0, lstProductosSeleccionados.length);
                actualizaProductosSeleccionados();

                var lstListado = new Array();
                lstListado = data.objResultado;
                var strHtml = "";
                $("#ddlCliente").html("");
                for (var i = 0; i < lstListado.length; i++) {
                    $("#ddlCliente").append('<option value="' + lstListado[i].ID + '" name="' + lstListado[i].ID + '">' + lstListado[i].Nombre + '</option>');
                }
                $('#txtFiltro').val();
                consultaListadoProductosSinAsignar();
            } else {
                mostrarMensaje(2, data.MensajeProceso);
            }
        },
        error: function (jqXHR, textStatus, error) {
            $('#lblCantidadProductos').html('0');
            mostrarMensaje(2, error);
        }
    });
}


/**
 * Metedo utilizado para agregar productos de proyeccion
 */
function agregarProductosAproyectar() {
    if (this.lstProductosSeleccionados.length > 0) {
        var xCliente = $('#ddlCliente').val();
        var xProductos = "";
        this.lstProductosSeleccionados.forEach(function (objItem) {
            xProductos += "," + objItem.ID;
        });
        xProductos += ",";
        var xCanal = $('#ddlCanalFiltro').val();
        var xIntIDUnico = parseInt($('#ID').val());

        //Envia datos al servidor
        $.ajax({
            url: "/ForeCast/cargaNuevosProductosAProyectarCliente",
            type: "POST",
            data: "xIntIDUnico=" + xIntIDUnico + "&xCanal=" + xCanal + "&xCliente=" + xCliente + "&xProductos=" + xProductos,
            dataType: "json",
            success: function (data) {
                if (data.ResultadoProceso) {

                    var JsonDatos = data.objResultado;

                    ///Carga nuevos clientes
                    JsonDatos.Canales.forEach(function (objItem) {
                        var objCanalExiste = lstCanales.find(m => m.Canal == objItem.Canal);
                        if (objCanalExiste == null) {
                            lstCanales.push(objItem);

                            var strHTMLcanal = '<div><input type="CheckBox" checked="checked" onchange="marcarCheckBox(\'Cana\',this)" id="chkCanal_' + objItem.Canal + '" tabindex="0" value="' + objItem.Canal + '"> <small onclick="cargarClientePorCanal(\'' + objItem.Canal + '\')" id="lblCanal_' + objItem.Canal + '" tabindex="0" class="itemSeleccionado" style="background-color: rgb(255, 255, 255);">' + objItem.Canal + '</small></div>';
                            $('#divCanales').html($('#divCanales').html() + strHTMLcanal);

                        }
                    });

                    ///Carga nuevos clientes
                    JsonDatos.Clientes.forEach(function (objItem) {
                        var objClienteExiste = getCliente(objItem.ID);
                        if (objClienteExiste == null) {
                            lstClientes.push(objItem);
                        }
                    });

                    ///Carga nuevos producutos
                    JsonDatos.Productos.forEach(function (objItem) {
                        var objProductoExiste = getProductos(objItem.ID);
                        if (objProductoExiste == null) {
                            lstProductos.push(objItem);
                        }
                    });


                    ///Carga nuevas presentaciones
                    JsonDatos.Presentaciones.forEach(function (objItem) {
                        var objPresentacionExiste = getPresentacion(objItem.ID);
                        if (objPresentacionExiste == null) {
                            lstPresentacion.push(objItem);
                        }
                    });


                    ///Carga nuevas temporadas
                    JsonDatos.TemporadasBase.forEach(function (objItem) {
                        var objTemporadaExiste = getTemporada(objItem.ID);
                        if (objTemporadaExiste == null) {
                            lstTemporadabase.push(objItem);
                        }
                    });

                    ///Carga nuevos Items
                    JsonDatos.DatosResumen.forEach(function (objItem) {

                        var objItemResumen = lstResumenBD.find(m =>
                            m.Canal == objItem.Canal &&
                            m.IDCliente == objItem.IDCliente &&
                            m.IDPresentacion == objItem.IDPresentacion &&
                            m.IDProductos == objItem.IDProductos &&
                            m.IDTemporadaBase == objItem.IDTemporadaBase
                        );
                        if (objItemResumen == null) {
                            lstResumenBD.push(objItem);
                        }
                    });

                    /*SELECCIONA Y MARCA EL CLIENTE*/
                    cargarClientePorCanal(JsonDatos.Canales[0].Canal);
                    cargarProductosPorCanalCliente(JsonDatos.Clientes[0].ID);

                    $('#popupAgregarProducto').modal('hide');

                } else {
                    mostrarMensaje(2, data.mostrarMensaje);
                }
            },
            error: function (jqXHR, textStatus, error) {
                mostrarMensaje(2, error);
            }
        });

    } else {
        mostrarMensaje(2, "Debe selecciona uno o más productos.");
    }
}

/**
 * Funcion utilizada para remover productos
 */
function removerProductoSeleccion() {
    if (ItemSeleccionado != null) {
        var objCheck = $($(ItemSeleccionado).parent()).find('input')[0];
        var xProductos = parseInt($(objCheck).val());
        var xCliente = xClienteSelec;
        var xCanal = xCanalSelec;
        var xIntIDUnico = parseInt($('#ID').val());

        var strparametros = "xIntIDUnico=" + xIntIDUnico + "&xCliente=" + xCliente + "&xCanal=" + xCanal + "&xProductos=" + xProductos;


        $.ajax({
            url: "/ForeCast/eliminarProductoVinculadoProyeccion",
            type: "POST",
            data: strparametros,
            dataType: "json",
            success: function (data) {
                if (data.ResultadoProceso) {

                    var objCheck = $($(ItemSeleccionado).parent()).find('input')[0];
                    var xProductos = parseInt($(objCheck).val());
                    var xCliente = xClienteSelec;
                    var xCanal = xCanalSelec;
                    var xIntIDUnico = parseInt($('#ID').val());

                    var lstItemsAEliminar = new Array();
                    lstResumenBD.forEach(function (objItem) {
                        if (objItem.IDCliente == xCliente &&
                            objItem.IDProductos == xProductos &&
                            objItem.Canal == xCanal) {
                            lstItemsAEliminar.push(objItem);
                        }
                    });

                    ///Elimina item
                    for (var i = 0; i < lstItemsAEliminar.length; i++) {
                        var Index = lstResumenBD.indexOf(lstItemsAEliminar[i]);
                        if (Index != -1) {
                            lstResumenBD.splice(Index, 1);
                        }
                    }

                    cargarClientePorCanal(xCanal);
                    cargarProductosPorCanalCliente(xCliente);

                    $('#popupValidarRemovercionProducto').modal('hide');

                } else {
                    mostrarMensaje(2, data.MensajeProceso);
                }
            },
            error: function (jqXHR, textStatus, error) {
                $('#lblCantidadProductos').html('0');
                mostrarMensaje(2, error);
            }
        });

    }
}


function menuContextualMouse(e, ctr) {
    if (e.which == 3) {
        ItemSeleccionado = ctr;
    }
}


$(document).ready(function () {
    $(function () {
        $.contextMenu({
            selector: '.context-producto',
            callback: function (key, options) {
                if (key == "Selec") {
                    var ojbCkec = $($(ItemSeleccionado).parent()).find('input')[0];
                    $(ojbCkec).prop("checked", true);
                    marcarCheckBox('Prod', ojbCkec);
                }
                if (key == "DeSele") {
                    var ojbCkec = $($(ItemSeleccionado).parent()).find('input')[0];
                    $(ojbCkec).prop("checked", false);
                    marcarCheckBox('Prod', ojbCkec);
                }
                if (key == "VerPro") {
                    mostrarMensaje(1, "Sin información del producto");
                }
                if (key == "RemPro") {
                    $('#popupValidarRemovercionProducto').modal('show');
                }
            },
            items: {
                //"Selec": { name: "Seleccionar producto", icon: "edit" },
                //"DeSele": { name: "Deseleccionar producto", icon: "edit" },
                //"sep1": "---------",
                "VerPro": { name: "Ver información del producto", icon: "glyphicon glyphicon-list" },
                "RemPro": { name: "Remover producto", icon: "delete" }
            }
        });

        $('.context-producto').mousedown(function (e) {

        })
    });
});