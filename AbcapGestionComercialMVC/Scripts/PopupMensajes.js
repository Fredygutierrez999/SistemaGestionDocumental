var _tipoMensaje = 0;
var _rediccion = "";
/*
@Metodo encargado de mostrar mensaje de validación
@ 1=Datos requeridos, 2= Error, 3= Advertencia, 4 = Exitoso
*/
function mostrarMensaje(tipoMensaje,mensajeString) {
	_tipoMensaje = parseInt(tipoMensaje);
	var _contenidoHTML = "";
	var _tituloVentana = "";
	var _tipoAlerta = "";
	var _tipoButtom = "";

	if (_tipoMensaje == 1) { /*Datos requeridos*/
		_tituloVentana = "Datos requeridos";
		_tipoAlerta = "bg-warning";
		_tipoButtom = "btn-warning";
	}
	if (_tipoMensaje == 5) { /*Datos requeridos*/
	    _tituloVentana = "Exito";
	    _tipoAlerta = "bg-success";
	    _tipoButtom = "btn-success";
	}
	if (_tipoMensaje == 2) { /*Error*/
		_tituloVentana = "Error";
		_tipoAlerta = "bg-danger";
		_tipoButtom = "btn-danger";
	}
	if (_tipoMensaje == 3) { /*Advertencia*/
		_tituloVentana = "Advertencía";
		_tipoAlerta = "bg-warning";
		_tipoButtom = "btn-warning";
	}
	if (_tipoMensaje == 4) { /*Exito*/
		_tituloVentana = "Exito";
		_tipoAlerta = "bg-success";
		_tipoButtom = "btn-success";
	}

	/*Genera HTML*/
	_contenidoHTML = "<ul>";
	if (Array.isArray(mensajeString)) {
		for (var i = 0; i < mensajeString.length; i++) {
			_contenidoHTML += "<li>" + mensajeString[i] + "</li>";
		}
	} else {
		_contenidoHTML += "<li>" + mensajeString + "</li>";
	}
	_contenidoHTML += "</ul>";
	$('#contenidoPopupMensajeGenerico').html(_contenidoHTML);
	$('#TituloPopupMensajeGenerico').html(_tituloVentana);
	$('#popupHeaderGenerico').attr("class", "modal-header " + _tipoAlerta);
	$('#btnAceptarSiMensajeGenerico').attr("class", "btn " + _tipoButtom + " " + _tipoAlerta);
	$('#btnCancelarNoSiMensajeGenerico').attr("class", "btn btn-secondary");
    $('#popupMuestra').modal("show");
    setTimeout(function () { $('#btnAceptarSiMensajeGenerico').focus(); }, 400);
}


/*
@Metodo encargado de mostrar mensaje de validación
@ 1=Datos requeridos, 2= Error, 3= Advertencia, 4 = Exitoso
*/
function mostrarMensajeUrl(tipoMensaje, mensajeString, url) {
    _rediccion = url;
    mostrarMensaje(tipoMensaje, mensajeString);
}

/*
@Evento que se ejecuta al pultar el boton aceptar o si
*/
function eventoAceptarSi() {
	if (_tipoMensaje == 1) { /*Datos requeridos*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 2) { /*Error*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 3) { /*Advertencia*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 4) { /*Exito*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 5) { /*Exito*/
	    location.href = _rediccion;
	}
}

/*
@Evento que se ejecuta al pultar el boton aceptar o si
*/
function eventoCancelarNoSi() {
	if (_tipoMensaje == 1) { /*Datos requeridos*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 2) { /*Error*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 3) { /*Advertencia*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 4) { /*Exito*/
		$('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 5) { /*Exito*/
	    $('#popupMuestra').modal("hide");
	}
	if (_tipoMensaje == 0) { /*Exito*/
		$('#popupMuestra').modal("hide");
	}
}