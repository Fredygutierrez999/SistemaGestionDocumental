/**
 * CONSULTA ROLES
 */
function ConsultaRoles() {
    $.ajax({
        url: "/Seguridad/ConsultDetalleRoles",
        type: "POST",
        data: $('#fmrConsultaRoles').serialize(),
        dataType: "html",
        success: function (data) {
            $('#DetalleRoles').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * CONSULTA ROL SI EXISTE SI NO LLENA LA ESTRUCTURA CON INFORMACIÓN BÁSICA
 * @param {any} xIDRol
 */
function CreaModificaRol(xIDRol) {
    $.ajax({
        url: "/Seguridad/AdministrarRol",
        type: "POST",
        data: "xIDRol=" + xIDRol,
        dataType: "html",
        success: function (data) {
            $('#cuerpoAdministraRol').html(data);
            $('#popupAdministrarRol').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * GUARDA CAMBIOS SOBRE ROL
 */
function guardarRol() {

    var listaMenu = $('.chkOpcionesMenu');
    var strCadenaMenu = "";
    for (var i = 0; i < listaMenu.length; i++) {
        if ($(listaMenu[i]).prop('checked') == true) {
            strCadenaMenu += "," + $(listaMenu[i]).val();
        }
    }
    strCadenaMenu += ",";
    $('#hdfSeleccionMenuPopup').val(strCadenaMenu);

    $.ajax({
        url: "/Seguridad/guardarRol",
        type: "POST",
        data: $('#fmrGuardarRol').serialize(),
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso == true) {
                $('#popupAdministrarRol').modal("hide");
                ConsultaRoles();
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
 * Cambiar tipo de rol
 */
function cambiaTipoRol() {
    var listar = JSON.parse($('#hdfValoresTipoRolPopup').val());
    var objTipoRol = listar.find(m => m.ID == parseInt($('#ddlTipoRoloPopup').val()));
    if (objTipoRol.MostrarComboCanal) {
        $('#ddlCanalPopup').removeAttr("readonly");
        $('.chkOpcionesMenu').attr("disabled", "");
    } else {
        $('#ddlCanalPopup').attr("readonly", "readonly");
        $('.chkOpcionesMenu').removeAttr("disabled");
    }
}

/**
 * CONSULTA USUARIOS
 */
function ConsultaUsuarios() {
    $.ajax({
        url: "/Seguridad/ConsultDetalleUsuarios",
        type: "POST",
        data: $('#fmrConsultaUsuarios').serialize(),
        dataType: "html",
        success: function (data) {
            $('#DetalleUsuarios').html(data);
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * CONSULTA USUARIO SI EXISTE SI NO LLENA LA ESTRUCTURA CON INFORMACIÓN BÁSICA
 * @param {any} xIDUsuario
 */
function CreaModificaUsuario(xIDUsuario) {
    $.ajax({
        url: "/Seguridad/AdministrarUsuario",
        type: "POST",
        data: "xIDUsuario=" + xIDUsuario,
        dataType: "html",
        success: function (data) {
            $('#cuerpoAdministraUsuario').html(data);
            $('#popupAdministrarUsuario').modal("show");
        },
        error: function (jqXHR, textStatus, error) {
            mostrarMensaje(2, error);
        }
    });
}

/**
 * GUARDA CAMBIOS SOBRE ROL
 */
function guardarUsuario() {

    /**
     * Carga los roles para el usuario
     */
    var listaMenu = $('.chkOpcionesUsuario');
    var strCadenaMenu = "";
    for (var i = 0; i < listaMenu.length; i++) {
        if ($(listaMenu[i]).prop('checked') == true) {
            strCadenaMenu += "," + $(listaMenu[i]).val();
        }
    }
    strCadenaMenu += ",";
    $('#hdfSeleccionMenuPopup').val(strCadenaMenu);


    $.ajax({
        url: "/Seguridad/guardarUsuario",
        type: "POST",
        data: $('#fmrGuardarUsuario').serialize(),
        dataType: "json",
        success: function (data) {
            if (data.ResultadoProceso == true) {
                $('#popupAdministrarUsuario').modal("hide");
                ConsultaUsuarios();
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