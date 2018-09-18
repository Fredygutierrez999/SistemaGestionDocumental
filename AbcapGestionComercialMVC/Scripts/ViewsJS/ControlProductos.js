/**
 * Carga listado de productos a sincronizar
 */
function buscarProductosASincronizar() {
    $.ajax({
        url: "/Productos/ListadoProductosASincronizar",
        type: "POST",
        data: $('#fmrBuscarProductosASincronizar').serialize(),
        dataType: "html",
        success: function (data) {
            $('#divDetalleProductosSincronizados').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}


/**
 * Carga listado de productos a sincronizar
 */
function buscarProductos() {
    $.ajax({
        url: "/Productos/ListadoProductos",
        type: "POST",
        data: $('#fmrBuscarProductosASincronizar').serialize(),
        dataType: "html",
        success: function (data) {
            $('#divDetalleProductosSincronizados').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * Producto a Descargar desde produccion
 * @param {any} xIdProducto
 */
function DescargarProducto(xIdProducto) {
    $.ajax({
        url: "/Productos/SincronizarProductoProduccion",
        type: "POST",
        data: "xIDProducto=" + xIdProducto,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                mostrarMensaje(1, "Producto descargado correctamente.");
                buscarProductosASincronizar();
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
 * Producto a Descargar desde produccion
 * @param {any} xIdProducto
 */
function ActualizarProducto(xIdProducto) {
    $.ajax({
        url: "/Productos/ActualizarProductoProduccion",
        type: "POST",
        data: "xIDProducto=" + xIdProducto,
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso) {
                mostrarMensaje(1, "Producto actulizado correctamente.");
                buscarProductos();
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
 * Producto a Descargar desde produccion
 * @param {any} xIdProducto
 */
function VerRecetaProducto(xIdProducto) {
    $('#popupVerRecetaProducto').modal('show');
}