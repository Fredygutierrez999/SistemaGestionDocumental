/*
	LIBRERIA ESCANER PARA CONECTAR APLICACION WEB CON APLICACION LOCAL Y SUBIR ARCHIVOS DESDE ESCANNER.
	Version: 1.0.0.1
	Fecha: 01/19/2017 (mm/dd/yyyy)
	Creador: Fredy Gutierrez
*/

this._conteoImagenes = 0;
this._nuevoImagenesAProcesar = 0;
this._imagenBD = true;
this._pdf = true;
this._bloquearVentana = true;
this._host = "127.0.0.1";
this._puesto = "18655";
this._ws;
this._ubicacionArchivoInstalacion = "../ProgramaEscanner/InstaladorEscanerJarandes.msi";
this._validaPluging = false;
this._Imagen;
this._textOCR;
this._ContadorItemn = 0;
///Socket
this._ws = null;
//Matriz enargada de almacenar objetos de iamgenes cargadas
this._MatrizImagenes = [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null];
//Ultima posicion libre
this._ConteoMatrizImagen = 0;


///Estructura para almacenar un objeto de imagen, con matriz de bis, largo y alto.
class ItemImagen {

    //Sobrecarga de constructor
    constructor(BitsImagen, Width, Heigth, xNombreArchivo) {
        this._BitsImagen = BitsImagen;
        this._Width = Width;
        this._Heigth = Heigth;
        this._partes = [];
        this._NombreArchivo = xNombreArchivo;
    }

    //Matriz de BITS de la imagen
    getBitsImagen() {
        return this._BitsImagen;
    }
    //Largo de la imagen
    getWidth() {
        return this._Width;
    }
    //Alto de la imagen
    getHeigth() {
        return this._Heigth;
    }
    //Nombre archivo
    getNombreArchivo() {
        return this._NombreArchivo;
    }
    adicionarParte(parte) {
        this._partes[this._partes.length] = parte;
        return false;
    }
}
////Se ejecuta cuando el puglin esta instalado
function cargarContenicoConPlugin() {
    $("#bodyEscanner").html("");
    document.getElementById("pnlEscannerPrincipal").style.display = "block";
    var contenidoVentanaEscanner = "<div id=\"EncabezadoConteImagen\">" +
        "<button class=\"btn btn-sm\" onclick=\"return cargarImage();\" style=\"margin-right: 3px;\"><img width=\"15px\" height=\"15px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADBElEQVR42oWTXUhTYRjHn7NN3ZorHc4V5FLXLLbpPmR+DJRBfsxYF7VVeGF0kReFN5riQggkozKii4XGRqCChYSgeRFZLbMW1oUyIdcGZrpN5nLaPtxkuq33fS+ki9ADh/dw3vP8zvP/P/+XggOuqakpTWZmpsTpdH6x2+3zvb29iX/3qf2KGxsbLxqNxnu5ubnCZDIJ8Xg8srOz8y0cDo+VlpaaDgS0trZ+7erqKsPPqVQKMASvqJNb9fX19/cFdHR0XCopKXmu1WrpNBpt7z2GtLS0PB4ZGWnbAzQ3N1MSieTqyspKPkVR8UQioZ6cnGxAeqni4mKg0+l7t8fjge7u7vPj4+NjBIDapOrq6gb6+vquzMzMAIvFAqQZmEwmqNVqwIDCwkLg8XiQlpYGs7OzMD09vb2+vn55eHj4FWUymXSVlZUT7e3t4HK5sFGwu7tLtOICNptNivPz80EqlUIoFAJkIjAYjOv9/f1Pqc7OzkcikaiNz+cD0gWBQIC0GQwGIRaLERjWjS8OhwNNTU2Qnp6OVCZu9PT0mDHgZXZ2tsHr9UI0GgVsmEAgIMVo9gSG/4qLa2pqQCaTkT2fz4f3qzDgNdKrNZvNgECkGLeelZUFKEBQXV0NRUVFRFpGRgZZ3W43LCwswNLS0jVKp9M9FIvFNwcHByndWS0cYdPA9dMB/kAcjvKPEw80Gg2gAJEpYNjy8jKgicHo6GgLpVKpNOij17VnqpjFhXnA4f6GRc8ELH0/AaHoFvg2/4Bc3gA5OTnEB4VCAQ6HA2w2W8RisZTjEaqUCqmlTXZBlr6YhAjy4o34HUTCIeDlseH9xyhUVNQSI1FGCACPEmXhzurq6m1qaGjAyOXE7rLmU7QdTgwO727DL/YWzAXcEPQy4IP1s0cul4eFQuHpgoICSqlUAvLtk9Vq1aKGoiSJer1eUKFWGLY21wylZYfK5+w/aIForvPtuO0JGusLv9+/gfSfQiaf43K5x1BeHqCcrP33LBgMeoFIdLLGYnk2htK2cdBx/wslZ1khgIbUqQAAAABJRU5ErkJggg==\" /> Escáner </button>" +
        "<button class=\"btn btn - sm\" onclick=\"return cargarMasivoImage();\"><img width=\"15px\" height=\"15px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADBElEQVR42oWTXUhTYRjHn7NN3ZorHc4V5FLXLLbpPmR+DJRBfsxYF7VVeGF0kReFN5riQggkozKii4XGRqCChYSgeRFZLbMW1oUyIdcGZrpN5nLaPtxkuq33fS+ki9ADh/dw3vP8zvP/P/+XggOuqakpTWZmpsTpdH6x2+3zvb29iX/3qf2KGxsbLxqNxnu5ubnCZDIJ8Xg8srOz8y0cDo+VlpaaDgS0trZ+7erqKsPPqVQKMASvqJNb9fX19/cFdHR0XCopKXmu1WrpNBpt7z2GtLS0PB4ZGWnbAzQ3N1MSieTqyspKPkVR8UQioZ6cnGxAeqni4mKg0+l7t8fjge7u7vPj4+NjBIDapOrq6gb6+vquzMzMAIvFAqQZmEwmqNVqwIDCwkLg8XiQlpYGs7OzMD09vb2+vn55eHj4FWUymXSVlZUT7e3t4HK5sFGwu7tLtOICNptNivPz80EqlUIoFAJkIjAYjOv9/f1Pqc7OzkcikaiNz+cD0gWBQIC0GQwGIRaLERjWjS8OhwNNTU2Qnp6OVCZu9PT0mDHgZXZ2tsHr9UI0GgVsmEAgIMVo9gSG/4qLa2pqQCaTkT2fz4f3qzDgNdKrNZvNgECkGLeelZUFKEBQXV0NRUVFRFpGRgZZ3W43LCwswNLS0jVKp9M9FIvFNwcHByndWS0cYdPA9dMB/kAcjvKPEw80Gg2gAJEpYNjy8jKgicHo6GgLpVKpNOij17VnqpjFhXnA4f6GRc8ELH0/AaHoFvg2/4Bc3gA5OTnEB4VCAQ6HA2w2W8RisZTjEaqUCqmlTXZBlr6YhAjy4o34HUTCIeDlseH9xyhUVNQSI1FGCACPEmXhzurq6m1qaGjAyOXE7rLmU7QdTgwO727DL/YWzAXcEPQy4IP1s0cul4eFQuHpgoICSqlUAvLtk9Vq1aKGoiSJer1eUKFWGLY21wylZYfK5+w/aIForvPtuO0JGusLv9+/gfSfQiaf43K5x1BeHqCcrP33LBgMeoFIdLLGYnk2htK2cdBx/wslZ1khgIbUqQAAAABJRU5ErkJggg==\" /> Escáner masivo </button> " +
        "<button class=\"btn btn - sm\" onclick=\"return adjuntarMasivoImage();\"><img width=\"15px\" height=\"15px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAACcklEQVR42n2TbUgTcRzHv3c3dd3SwxKTHihjvrCUggw1seVSEKc9WTkohKh8U4T0wkmsB0fIthdRvYiaREJvrkSt3AYRDBTMJCtfGJYYBRkoPahrzdv0f/a/LeYO4r7cF+53fH+f4//wY/BPHMc5zGZzE8/zadBQOByOBAIBDyHkilIz1Juoq91ut+fU6bMgsqzVD45l8eB+B1paWspo+VIB2Az7m5xHrSewZm2WKhyMhhBc9XylmWHRuqMBU2MTqLVYrPTToxjAaO91FmzLR/nmdMxJhAaBd9NhTPycxrjUBqRyCUhX1WXwn//AYqlZAay75nWm5mxFY0Empn4vxvzxV4QuZxbfJTugYxOA7uo2pEwGUVOTBMiosDrrG8+Ay9LjxdcBgOYZlsEyG0GUBGjBJACdle0gH+ZVAAP1Sa/Pd3f1dgHN/bbkfFxJ9S2TG6ExNUBRg9/vF7N2Crg6eFHzFNrKbuDH6H8APp9f3Fgk4ObrC5qA5t23MTUyr9rEGIAuQTQWC+gcPZcIZ+sJlpbpRTEsYmZBh7koB2vhHUwOz6uOMQbo6/OKRXszMPTJgYhMYEj5BkKb6YNFOR5TYBXGDrwZCKKurlYNePL0mVhiysX4l1d09ychMwt085RbKdP3KJbkEMJkBiVbHBjpn8WhgwfUgO6eXjG/PBuet+ehY9KQrtsAIXU9CjMrkSfsUmYl5kxBgNfnR/2Rw2qA+LhLrKrbh4fv70FOmofc9DyYcqrB0hlQbOB5dHX3wHr8mAqwp/WSfdBms9G/sNASITJcLhec7dcTwxQTy3KO4tLSJr1erznOkiRFhoeGPLIcH+e/fJ3wEegJBsUAAAAASUVORK5CYII=\" /> Adjuntar imagen</button> " +
        "<label id=\"lblResumen\" >Sin páginas</label>" +
        "</div><div id=\"CuerpoConteImagen\"></div>" +
        "<div ID=\"PieEscannerPrincipal\">";
    if (this._imagenBD == true) {
        contenidoVentanaEscanner += "<input id=\"rbImg\" type=\"radio\"  width=\"15px\" height=\"15px\"   name=\"gender\" checked=\"checked\" value=\"IMG\"><img width=\"15px\" height=\"15px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADSUlEQVR42lVTXWhcRRg938yd3Xv3bpLdLA1NSmJb20QFqVitfZJaNC+KIkitQmlf9KGliQ8R0dIXoYooRamgD0pp1UhRrKUopJr+IJTmqSJVQm3aKIlt/re79+79nTt+25CiAwOXuTPnO9855yP8d/3hdYrI3knGPEYmK4OWjzMDGKKqkeIS0ugENrv/rDyhlQ91wdtjpP2BcGWleUhieTdvGA2kzc1AaOhFYcKh7PHi0bsAarSxi/LOcUSARQFkjkBWE2AZP86YRajRSxrXIwe+sqBMsCfZXjhGhTFvdTIr/0ygio5sIMlLuI5ETi4z8LmqDmN8tk7i5S4bl5cSDP1ucG7W+FhrbyRxsjEkbOf93eXbGOi18auXYXDSIJ+3EBiD+60Yn3QTHqq4CJmNIwlhnGLHbxZOT9TfoJavFr87tLX0/P71/LfZo07w5riPw0s21tnA5z0aW9pyWBI5GG6Fu0M5D5ybM9j+w+Ipemlk+qfh/q4nqwlQ9xuoOIqrAK9fCzFWz/DtBgk/iOCzFSGreCMiJIbwzYyLH6eSUeo7cWtkYFNrf3tawyOrHKxpbwUxE9ukeOGKh1upQBkaHVnA7DIMz7uQhhW2WpDz6j+TGs5G4FK/0hrPdqTYXLIgKIGAj1NBHtepgHIa4uv7FNY4Fo5c9fDRBLukitBVBnhl9N0z7aXGUzJu8BMbi6YLk3Ir/pYPo5DjolGEtysBXuxuQ10btKgMu3/x8MVSCUU/G6XjF7u+L5bVc/5tVohVt4SBES5uiG24qPZigjbh0ZSZWT5iBmgkNs7OVdHReh5rgwun6b0zfW+1d6pD9QWOGjWFyhgnQ1F4MNLFZP5pXJHP4Gayke1z0R2fxTb1KfpWjWNhOjxIr37Y2VPZ4Fy1XZkHO0FE8BONSDfZZHDIgxASiaywyw4Kehrakqh6Kp6dCnvvZPXA8L37eh6wP07CjGeFY8tXqxzd2UZ8h7bFkSZ2JWMXtMUi5wQWJuKBk4M3j9wdpv1Hu19rW517xy1JhzLimSAE3M5ULcaclyBhIOJ8JDUdBjP6wPmD84f/N43N9cRgpe+eB+1dlqIt3Eub4sqKo1vn6M7U0tp8NR2buRZ9+dcxb3zlzb9K83Yg8ZYz7wAAAABJRU5ErkJggg==\" /> Imagen</input> ";
    }
    if (this._pdf == true) {
        contenidoVentanaEscanner += "<input id=\"rbPdf\" style=\"margin-left: 30px;\" type=\"radio\"  width=\"15px\" height=\"15px\"  name=\"gender\" checked=\"checked\" value=\"PDF\"><img width=\"15px\" height=\"15px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAACSElEQVR42n2TS2gTURiFzzzyrqCNqcRn3blRkQjSChVCa5tHY22NreBCCRZ0k10SRVwUkSRLV5IibsdKn5mZ2DRGrbG1FBci2p0SUFx0USsSE9OZeDORNJdCDvPD3Dv3/4ZzZn4G/8Vx3JjT6Rw1m80GNFGhUChls9mEoij3qmuG1CFSffF4PHE9cAOKqjbrB8eyePJ4HKFQ6CxZLlUBYZ/JFL3i96PVupc+zTA4HrmtvaVR71dX4PV4RsjtUw0wbrdHu4JBsCYTeIMBSrmM8vo6vqee41hShMVkpABLuRw8Hvc2QAKipwMBlPN56BwOcIqC37KM0sZP2NbWoNfrKcDiq5dwuxsA1wggMDiEI+1HSZgseI6r1Z7d4G/eAj5+BnfqRB2QfZGhABZSV0VJetTd3VO1XbNfdU4u9cMnMG02sPZ9dcDCQpoCVDUsy7Jwvrd3R+plMQ3ecRJMAyA9P78TIEmy4HL1Uc2VSgV/Yg9hjgSp/RQJtzFEDUAsCG6Xizr4d1oGSKD6AbLP8/V9OZWiPqMGSCZFwev1bDdnFsEe3E+8t0EhOei6OurPRFFCf7+XBszMzgk+AlC/5KF8zYO1toKxWVHZ2EQ59w5c+2HoznWCsZgxO5fEwAUfDZicmha8hhZUtra00LgDdtpO5jXUbz+g9/RgJvcGQ4MXaYAw8UwY9l9qOgfq5i8wu1owMTmFkct+CtAZuXP3bTgc1n6kZlIUFbFYDNEH9+vDpIllubEzHR2jRqOx6TgXi8XSyvJyQlVr4/wPtOvTEZocUVoAAAAASUVORK5CYII=\" /> PDF</input>";
    }
    contenidoVentanaEscanner += "<button class=\"btn btn-sm\" style=\"margin: 5px !important;\" id=\"btnCargaImagen\" type=\"Buttom\" width=\"15px\" height=\"10px\"  name=\"gender\" value=\"Cargar imagenes\" onclick=\"return generarArchivoFinal();\" ><img width=\"15px\" height=\"15px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAACP0lEQVR42oXSy2sTQRwH8O/ktdmY1ibWkK0VazA2WrCyKtgYwZNI9ShV6gsfaG+CB7EeVPoH2IO9eK0eqseKgiAoQm1NTEzaaluSQFJLXibBpOmmaZtdJ4Gm2STiF4ZlZnY+85udJU/u9XTdvcm/BnCANkIbov4Q/JM+bMa0dxdsJ4+iKhJtc89f+vpIwnvnrUql6K2ejfnD8E9NV/o7O0oAj9qIovSOpGYG3HRb2WyUAoGvM1VAG2yOeoCW4SGp6dtu1ACxwCIFZmVA54nDaBAPSfpuNQB+Iej8Uem37uEo0N0Y+O29UQfEA0sIun5WAWbstx9qDCS+X68HgkvIpnNIhSJopeVDLGLFYYVOIrBuqOVA3HOtDigIBTA6BulICsa2HYgJAoa5VWgpMLjcUn5WgJj7Sh1QmxF9FnOq9fJPcnyNwWVBvwVEv12i10j+CXxi8njF5mRjAyvN6F5n6DVKFHD1uxWMmU+Jp5FMpqFSq6HVakEIQZb5gxH2BQoQZUCzpMCjZQOaJIWHRJwX3Sp9N/9m0gSn0wmWZcFxXPnFhCkKg13CFEo/VeXcOCJo4Nhg0SVpPCTw+fzEtpZ9dleoB/PzC+XdjUZjuYJMJoNjZxR4vDYmq2AwZ4ClqMZqQfxCng3Z+3tPcaOMRqms3mUzYWUBQ/q0bOxhzoj2vLL4YSJ+tbyi75ylc3uTxtboI4q8zhI+yz5F1SF2v8/fFz5mxsfGgwsE/0n7BfNB64OO2a31RAoOh/nF0Yi31PsLK5/Vs63HSnEAAAAASUVORK5CYII=\" />  Adjuntar archivo(s)</button></div>";
    document.getElementById("pnlEscannerPrincipal").innerHTML = contenidoVentanaEscanner;
}
function string2Bin(str) {
    var result = [];
    for (var i = 0; i < str.length; i++) {
        result.push(str.charCodeAt(i).toString(2));
    }
    return result;
}
//Funcion de espera
function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}
///Realiza el envio y la compresion de las imagenes en PDF si se marca como PDF
function generarArchivoFinal() {
    $('#btnCargaImagen').attr('disabled', true);
    if (this._validaPluging) {
        if (this._MatrizImagenes.length > 0) {
            evaluaMatriz();
            if (this._ConteoMatrizImagen == -1 || this._ConteoMatrizImagen == 0) {
                $('#lblResumen').text("Debe escanear por  lo menos un archivo.");
            } else {
                var tipoGeneracion = $("input[name='gender']:checked").val();
                if (tipoGeneracion == "IMG") {

                    $('#lblResumen').text("Empaquetando y enviando imagen(es).");
                    var fd = new FormData();
                    for (i = 0; i < _MatrizImagenes.length; i++) {
                        if (_MatrizImagenes[i] != null) {
                            var Imagen = convertirStrinMatriz(this._MatrizImagenes[i].getBitsImagen(), 'image/jpeg');
                            Imagen.filename = "Imagen" + i;
                            fd.append("ImagenEscanner", Imagen);
                        }
                    }
                    sleep(1000);
                    $.ajax({
                        url: "../Radicacion/subirArchivos?xIDSession=0",
                        data: fd,
                        secureuri: false,
                        type: 'POST',
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: function (data, status) {
                            if (data.objResultado.length > 0) {

                                var objResultado = data.objResultado;
                                if (objResultado.ResultadoProceso) {
                                    for (var i = 0; i < objResultado.objResultado.length; i++) {
                                        lstSoportes.push(objResultado.objResultado[i]);
                                    }
                                    cargarListadoSoportes();
                                } else {
                                    mostrarMensaje(2, objResultado.MensajeProceso);
                                }
                                setTimeout(function () {
                                    $('#fileAbjunto').fileinput('reset');
                                    $('#fileAbjunto').fileinput('refresh');
                                }, 300);
                                $('#lblResumen').text("Imagen(es) subida(s).");

                            }
                        },
                        error: function (data, status, e) {
                            console.log("error al ejecutar proceso final." + e.message);
                        }
                    });
                }
                if (tipoGeneracion == "PDF") {
                    var NombreImagenes = "";
                    for (i = 0; i < _MatrizImagenes.length; i++) {
                        if (_MatrizImagenes[i] != null) {
                            NombreImagenes += this._MatrizImagenes[i].getNombreArchivo() + "+";
                        }
                    }
                    ProcesoLocal(18, null, 0, 0, NombreImagenes); // Inicia proceso de generacion de pdf
                }
            }
        } else {
            this.mostrarMensaje("Debe escanear por lo menos un archivo.", 1);
        }
    } else {
        this.mostrarMensaje("El complemento no se ha iniciado.", 0);
    }
    return false;
}
//solicitud de parte
function partes(imagen, parteSolicitada) {
    if (this._MatrizImagenes[imagen] != null) {
        var Numerocorte = 60000;
        var cantidadPartes = this._MatrizImagenes[imagen].getBitsImagen().length / parseInt(Numerocorte);
        if (cantidadPartes >= parteSolicitada || cantidadPartes > (parteSolicitada - 1)) {
            var inicia = parseInt(Numerocorte) * parteSolicitada;
            var parte = this._MatrizImagenes[imagen].getBitsImagen().substr(inicia, Numerocorte);
            if (parte.length > Numerocorte) {
                console.log("sobrepaso cantidad");
            }
            ProcesoLocal(12, parte, imagen, parteSolicitada, ""); // Procesa partes de la imagen
            console.log("Parte solicitada, enviada.");
        } else {
            sleep(1000);
            ProcesoLocal(11, null, imagen, 0, ""); // Finaliza Imagen para continuar con la siguiente imagen
        }
    } else {
        ProcesoLocal(18, null, 0, 0, "");
    }
}

///Evento encargado de enviar solicitud a complemento
function ProcesoLocal(codigoProceso, Imagen, PosicionImagen, Parte, nombreArchivo) {
    if (this._validaPluging == true) {
        $('#lblResumen').text("Procesando PDF...");
        var xmlSolicitud = "<?xml version=\"1.0\"?>" +
            "<Proceso xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
            "<CodigoProceso>" + codigoProceso + "</CodigoProceso>" +
            "<UbicacionImagen>c:/</UbicacionImagen>" +
            "<Imagen>" + Imagen + "</Imagen>" +
            "<PosicionImagen>" + PosicionImagen + "</PosicionImagen>" +
            "<Parte>" + Parte + "</Parte>" +
            "<NombreArchivo>" + nombreArchivo + "</NombreArchivo>" +
            "<CadenaError></CadenaError>" +
            "</Proceso>";
        if (this._ws) {
            this._ws.send(xmlSolicitud);
        } else {
            console.error("Socket no inicializado.");
        }
    } else {
        console.log("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.");
        this.mostrarMensaje("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.", 1);
    }
    return false;
}

///Calula el porcentaje de subida de la imagen
function progressBar(percent) {
    console.log(percent * 100);
}
///Metodo encargado de convertir una cadena string en un array de bytes
function convertirStrinMatriz(CadenaString, tipo) {
    var dataURL = 'data:' + tipo + ';base64,' + CadenaString;
    // Decodifica dataURL
    var binary = atob(dataURL.split(',')[1]);
    // Se transfiere a un array de 8-bit unsigned
    var array = [];
    var length = binary.length;
    for (var i = 0; i < length; i++) {
        array.push(binary.charCodeAt(i));
    }
    // Retorna el objeto Blob
    return new Blob([new Uint8Array(array)], { type: tipo })
}
////Se ejecuta cuando el puglin esta instalado
function ocultarContenicoConPlugin() {
    document.getElementById("pnlEscannerPrincipal").style.display = "none";
    $("#bodyEscanner").html("<div id= \"VentanaEscanner\">" +
        "<div class=\"Texto-Informativa\">" +
        "Su navegador no cuenta con el pluglin para realizar el escáner desde esta " +
        "página.</br> Por favor,  descárguelo e instálelo manualmente y pulse sobre el botón actualizar o recargue la página. " +
        "</div>" +
        "<a href=\"#\"><buttom class=\"btnDescargarEscanner\" onclick=\"InicializarSocket();\" style=\"right:105px;\">Actualizar</buttom></a>" +
        "<a href=\"" + this._ubicacionArchivoInstalacion + "\"><buttom class=\"btnDescargarEscanner\"  style=\"right:5px;\">Descargar</buttom></a>" +
        "<div id=\"CerrarEscannerRequisito\" onclick=\"document.getElementById('VentanaEscanner').style.display = 'None';\" >" +
        "X" +
        "</div>" +
        " </div>");
    if (this._bloquearVentana == true) {
        $("#bodyEscanner").html($("#bodyEscanner").html() + "<div id=\"bloqueaPantalla\" ></div>");
    }
}
///Carga imagenes a grid
function cargarImagenesEscaneadas() {
    document.getElementById("CuerpoConteImagen").innerHTML = "";
    for (i = 0; i < _MatrizImagenes.length; i++) {
        if (_MatrizImagenes[i] != null) {
            document.getElementById("CuerpoConteImagen").innerHTML += cargarItemImagen(_MatrizImagenes[i], i);
        }
    }
    contarDetalles();
}

function cargarItemImagen(objItemImagen, i) {
    var ItemImagen = "<div class=\"ItemImagen\" id=\"ItemImagen" + i + "\">" +
        "<div class=\"cabeceraItem\">" +
        "<div><label > Página " + (parseInt(i) + 1) + " </label></div>" +
        "<button class=\"btnEliminar\" onclick=\"eliminarItemSeleccionado('ItemImagen" + i + "'," + i + ");\" > <img width=\"20px\" height=\"20px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAC3klEQVR42m2TS0hUYRTH/+fzzoyP64zjI9+TMSVKpBAZYtSijVDQxghyYVAEBRERtAkKiqB1BRUh1s4WtatFbQsqCioNXxn5qGyycZw7D0dn7v36XzMR88K59zv3O+d3Ht/5BBs/vu2m2bHZNLdppfSEZY2OJJOvNJBdbyhrlTyg4GRR0fUTDQ2nyuvqfBIMQns8yGUy+BaJpO+Pj9/s//HjcnYNaBUQAEL3lHrRHA6HPE1NyK+vh7e0FLICyMzOwhobw7vp6bELk5N7E47zaxXAV9EDYChcUhLyNTYiQ8dwWxuMQACSnw/tOIgMDmJmaAieRAIfIpHh85FIq1vSMuAocKtHqTNGdTXeJJMoF8G+ri5sam+H8vmQmJ7Gx74+fI3FECTcpE2fZV17kk5fcgFmLxANGIbXX1ODD6kUAlqjyu/Hju5u+JjF5MOHsBYWMBWPo7aqCqmZGcyk0/PnLKtCQiIHzmv91G8YqCgvR1kohNHv31GoFIKEFLIEZduIptOobG6GNTGBn9yPU7+SyXRIE3D2MHDDT4dqOixDGOULjQzWXuj1Lje5trUVKTbyFwGzLCW2uIjebLZHthKwn4BiGlWy3sriYngYdTgaRZAn4MvLwyJBW+rq4OVpROfm8Nv9ZrPot+0eqRA5eEjrJzYBJczCS4cBbpaxkX6u2RvEqKdYRotpwpvLYY7rJULvar3HbWLguEjE1tqXpPKJ4mZjUjoLClBKyEvWG6ODu7+DWRXxG9c6dieXq1k+xp0it3dpfdri+v3KcOyno5uFQVGUF0tLWOD/NmakCHsucn3Ati/+m8TgEZH3ZVpvdqMsueVQDEIMHqliaQ51Dg48dJ4UGX5k221UU6ujzCiNPI1nVVo3uP1wRblCZ/X3ngB0nlJq5LHjdFKb+u8y8SltEblK9DEOk6lWfrqRYyLx1yK9Q45zhWpiw9u4FlQsspsXLOz6zwOfk1q/5Xp+veEfpeco1W8WPrMAAAAASUVORK5CYII=\" /></button>" +
        "<button class=\"btnMaximizar\" onclick=\"maximizarItemSeleccionado('ItemImagen" + i + "');\" > <img width=\"20px\" height=\"20px\"  src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAB9klEQVR42mNkIADKysrYPDw8ctnY2DSZmZn/v3r16v/jx4+vHjlyZNqKFSt+M+LTXFpayh4eHj5LSUlpi6Cg4G2g0B8g/vnixQvFzZs3B2/YsCEHrwH79u2rNDQ0vCcgIHAXyP3w7t27p8LCwt9BciUlJZocHBweeA04ffr0chMTk2VA5nsgvvH379+PLCwsv2HylZWVPXgNuHz58i4dHZ0NQCbIBa+B+AXQkDdAQ36B5AsLC/EbcPTo0T1WVlYngMxXQPwMiO8B8cNly5a9i46O/l9QUNCN14AlS5a0+fv7q/Lw8DyFar4OxLfu3bv3tKamRoqTkzMarwGHDx+O+PTp03Q7O7udQEMuAoWugAw5e/bsP2AUVmzZsqUApwH79+/v+fr1ay4wzue/efPmg7q6ui4wKj8+ffr0E9Brzw8dOtRz8+bNr1gN2L17d//3798L3r59e2bp0qUWe/bs+QsSl5OTY3306NFvZLUYBmzdurUfGNIFQNvPLl++3HXTpk3v8XkTxYB169b1MzExFfz48eMs0I+uwJSGVzOKAUAN/cC0XvDnz5+za9ascV27di1BzXADFi9e3A9MHAX///8/C7TVddWqVURpBhvQ399fJioq2snIyHgWmEFcgS4hWjPYgLCwMElgPPceO3YsG5jCSNIMAgBUROiruIykMQAAAABJRU5ErkJggg==\" /></button>" +
        "</div>" +
        "<div class=\"ItemImagenCuerpo\">" +
        "<img width=\"550px\" height=\"855px\" class=\"HojaEscanner\" src=\"" + "data:image/png;base64," + objItemImagen.getBitsImagen() + "\"> <hr>" +
        "</div>" +
        "</div>";
    return ItemImagen;
}
///contar detalle
function contarDetalles() {
    var cantidadPaginas = 0;
    for (i = 0; i < _MatrizImagenes.length; i++) {
        if (_MatrizImagenes[i] != null) {
            cantidadPaginas += 1;
        }
    }
    $("#listas li").each(function () {
        alert($(this).attr('id'));
    });
    $('#lblResumen').text("Número de páginas: " + cantidadPaginas);
}


//Realiza un recorrido sobre la matrix que contiene las imagenes para no dejar espacios vacios entre imagenes.
function evaluaMatriz() {
    var conservaValor = parseInt(this._ConteoMatrizImagen);
    this._ConteoMatrizImagen = -1;
    for (i = 0; i < _MatrizImagenes.length; i++) {
        if (_MatrizImagenes[i] == null) {
            for (j = i; j < _MatrizImagenes.length && _MatrizImagenes[i] == null; j++) {
                if (_MatrizImagenes[j] != null) {
                    _MatrizImagenes[i] = _MatrizImagenes[j];
                    _MatrizImagenes[j] = null;
                }
            }
            if (_MatrizImagenes[i] == null && _ConteoMatrizImagen == -1) {
                _ConteoMatrizImagen = i;
            }
            if (_MatrizImagenes[i] != null) {
                _ConteoMatrizImagen = i + 1;
            }
        } else {
            _ConteoMatrizImagen = i + 1;
        }
    }
}
///Procesa XML recibido pro el socket
function procesarXML(xml) {
    var result = $(xml).find("CodigoProceso").text();
    if (result != null) {

        if (result == 0) {
            $('#lblResumen').text("Complemento conectado.");
            this._validaPluging = true;
            $('#btnCargaImagen').attr('disabled', false);
        }

        //result = 5, significa que se presento un error en el servidor local o se cancelo la carga de la imagen.
        if (result == 5) { $('#lblResumen').text($(xml).find("CadenaError").text()); console.log($(xml).find("CadenaError").text()); $('#btnCargaImagen').attr('disabled', false); }

        //result = 2, Significa que el programa informa cuanta cantidad de imagenes escaneadas va a enviar.
        if (result == 2) {
            $('#lblResumen').text("Procesando " + $(xml).find("partes").text() + " imagenes."); this._Imagen = "";
            document.getElementById('btnCargaImagen').enabled = true;
            $('#btnCargaImagen').attr('disabled', true);
            this._nuevoImagenesAProcesar = $(xml).find("partes").text();
            this._conteoImagenes = 0;
        }

        //result = 3, Significa que la imagen se escaneo y se empezara a enviar al socket de la pagina.
        if (result == 3) { this._conteoImagenes = this._conteoImagenes + 1; this._Imagen = ""; document.getElementById('btnCargaImagen').enabled = true; $('#btnCargaImagen').attr('disabled', true); }

        //result = 6, Significa que se envia una parte de la imagen del servidor local al socket web.
        if (result == 6) { $('#lblResumen').text("Recibiendo imagen " + this._conteoImagenes + " de " + this._nuevoImagenesAProcesar + "."); console.log("Recibiendo paquete"); this._Imagen += $(xml).find("Imagen").text(); $('#btnCargaImagen').attr('disabled', true);; }

        //result = 7, Significa que el envio por partes finalizo y se debe procesar la imagen.
        if (result == 7) {
            // se evalua la matriz donde se almacenan las imagenes para determinar en que lugar de la matriz quedara 
            // la nueva imagen
            evaluaMatriz();
            if (_ConteoMatrizImagen != -1) {
                console.log("Procesando paquetes recibidos");

                $('#lblResumen').text($('#lblResumen').text() + " (Procesando imagen...)");
                var _width = $(xml).find("width").text();
                var _height = $(xml).find("height").text();
                var _nombreArchivo = $(xml).find("NombreArchivo").text();

                const objItemImagen = new ItemImagen(this._Imagen, _width, _height, _nombreArchivo);
                _MatrizImagenes[_ConteoMatrizImagen] = objItemImagen;
                document.getElementById("CuerpoConteImagen").innerHTML += cargarItemImagen(objItemImagen, _ConteoMatrizImagen);
                if (this._nuevoImagenesAProcesar == this._conteoImagenes || this._nuevoImagenesAProcesar == 0) {
                    contarDetalles();
                    this._nuevoImagenesAProcesar = 0;
                    this._nuevoImagenesAProcesar = 0;
                }
                this._Imagen = "";
                $('#btnCargaImagen').attr('disabled', false);

            } else {
                $('#lblResumen').text("Solo puede cargar 40 imagenes.")
                console.log("Solo puede cargar 20 imagenes.");
            }
        }

        //result = 38, Significa que se empieza a enviar texto OCR
        if (result == 38) { this._textOCR = ""; }

        //result = 39, Recepción de cadena OCR
        if (result == 39) { this._textOCR += $(xml).find("NombreArchivo").text(); }

        //result = 40, Finalización de cadena OCR
        if (result == 40) { mostrarMensaje(1, this._textOCR); }


        //result = 33, Significa que la imagen se escaneo y se empezara a enviar al socket de la pagina.
        if (result == 33) { $('#lblResumen').text("Inicia generación PDF."); console.log("Inicia nueva carga de pdf"); this._Imagen = ""; document.getElementById('btnCargaImagen').enabled = true; $('#btnCargaImagen').attr('disabled', true); }

        //result = 36, Significa que se envia una parte de la imagen del servidor local al socket web.
        if (result == 36) { $('#lblResumen').text("Recibiendo PDF..."); console.log("Recibiendo paquete PDF"); this._Imagen += $(xml).find("Imagen").text(); $('#btnCargaImagen').attr('disabled', true);; }

        //result = 7, Significa que el envio por partes finalizo y se debe procesar la imagen.
        if (result == 37) {
            // se evalua la matriz donde se almacenan las imagenes para determinar en que lugar de la matriz quedara 
            // la nueva imagen
            _ConteoMatrizImagen[0] = null;
            console.log("Subiendo...");
            $('#lblResumen').text("Subiendo...");
            const objItemImagen = new ItemImagen(this._Imagen, 0, 0);
            _MatrizImagenes[0] = objItemImagen;
            this._Imagen = "";

            var fd = new FormData();
            var Imagen = convertirStrinMatriz(this._MatrizImagenes[0].getBitsImagen(), 'image/pdf');
            //Imagen.filename = "Imagen" + i;
            fd.append("ImagenEscanner", Imagen);

            sleep(1000);
            $.ajax({
                url: "../Radicacion/subirArchivos?xIDSession=0",
                data: fd,
                secureuri: false,
                type: 'POST',
                processData: false,
                contentType: false,
                dataType: 'JSON',
                success: function (data, status) {
                    if (data.ResultadoProceso) {
                        var objResultado = data;
                        if (objResultado.ResultadoProceso) {
                            for (var i = 0; i < objResultado.objResultado.length; i++) {
                                lstSoportes.push(objResultado.objResultado[i]);
                            }
                            cargarListadoSoportes();
                        } else {
                            mostrarMensaje(2, objResultado.MensajeProceso);
                        }
                        setTimeout(function () {
                            $('#fileAbjunto').fileinput('reset');
                            $('#fileAbjunto').fileinput('refresh');
                        }, 300);
                        $('#lblResumen').text("Imagen(es) subida(s).");
                    } else {
                        mostrarMensaje(2, data.MensajeProceso);
                    }
                },
                error: function (data, status, e) {
                    console.log("error al ejecutar proceso final." + e.message);
                    $('#btnCargaImagen').attr('disabled', false);
                }
            });
        }
    }
}
///Realiza una primer conexion verificando la conectividad entre el socket y el servidor 
function coneccionInicial() {
    $('#lblResumen').text("Validando complemento.");
    this._ws.send("PING");
}
///Incinicializa Socket 
function InicializarSocket() {
    $('#btnCargaImagen').attr('disabled', true);
    if ("WebSocket" in window) {
        try {
            this._ws = new WebSocket("ws://" + this._host + ":" + this._puesto, ['soap', 'xmpp', 'json']);
            //Si la conexion es exitosa con el servidor local, se cargan controles por defecto sobre el DIV que va a mostrar toda la informacion de carga.
            this._ws.onopen = function () {
                cargarContenicoConPlugin();
                coneccionInicial();
            };
            //Este evento se ejecuta cada vez que se realiza una peticion y nos envia una respuesta, por parte del servidor local.
            this._ws.onmessage = function (evt) {
                var xml = evt.data;
                procesarXML(xml);
            };
            //Evento que se dispara si se ejecuta un error del socket
            this._ws.onerror = function (evt) {
                ocultarContenicoConPlugin();
                this._validaPluging = false;
                $('#pnlContenedorEscanner').html("<div style=\"padding: 5px !important;margin-bottom: 5px !important;\" class=\"alert alert-danger\">Error con el complemento.</div>");
            };
            //Evento que se ejecuta si el socket se cierra
            this._ws.onclose = function (evt) {
                console.error("Coneccion con servidor local cerrada");
                this._validaPluging = false;
                $('#pnlContenedorEscanner').html("<div style=\"padding: 5px !important;margin-bottom: 5px !important\" class=\"alert alert-danger\">Sin conexión con el complemento. <a href=\"../Instalador/InstaladorEscanerJarandes.msi\">Click para descargar.</a></div>");
            };
            return true;
        } catch (ex) {

            this._ws.close();
            $('#pnlContenedorEscanner').html("<div  style=\"padding: 5px !important;margin-bottom: 5px !important\" class=\"alert alert-danger\">Sin pluglin</div>");
            this._validaPluging = false;
            console.error("No posee pluging local instalado." + ex);
            this.mostrarMensaje("No posee pluging local instalado." + ex);
            return false;
        }
    } else {
        mostrarMensaje("Su navegador no soporta WebSocket, No podra utilizar el escaner desde el sitio web.", 1);
        console.log("Su navegador no soporta WebSocket, No podra utilizar el escaner desde el sitio web.");
        this._validaPluging = false;
        return false;
    }
}
/*
FInaliza el publing
*/
function finalizarPluging() {
    try {
        if (this._ws != null) {
            this._ws.close();
            this._validaPluging = false;
        }
    }
    catch (ex) {
        console.error("No posee pluging local instalado." + ex);
    }
}

///Metodo encargado de llamar el socket para que inicie el proceso de carge de la imagen
function cargarImage() {
    if (this._validaPluging == true) {
        $('#lblResumen').text("Escaneando...");
        var xmlSolicitud = "<?xml version=\"1.0\"?>" +
            "<Proceso xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
            "<CodigoProceso>1</CodigoProceso>" +
            "<UbicacionImagen>c:/</UbicacionImagen>" +
            "<CadenaError></CadenaError>" +
            "<Imagen></Imagen>" +
            "<PosicionImagen>0</PosicionImagen>" +
            "<Parte>0</Parte>" +
            "</Proceso>";
        if (this._ws) {
            $('#btnCargaImagen').attr('disabled', true);
            this._ws.send(xmlSolicitud);
        } else {
            $('#btnCargaImagen').attr('disabled', false);
            console.error("Socket no inicializado.");
        }
    } else {
        $('#btnCargaImagen').attr('disabled', false);
        console.log("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.");
        this.mostrarMensaje("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.", 1);
        document.getElementById('btnCargaImagen').enabled = true;
    }
    return false;
}
///Metodo encargado de llamar el socket para que inicie el proceso de carge de la imagen
function cargarMasivoImage() {
    if (this._validaPluging == true) {
        $('#lblResumen').text("Escaneando...");
        var xmlSolicitud = "<?xml version=\"1.0\"?>" +
            "<Proceso xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
            "<CodigoProceso>101</CodigoProceso>" +
            "<UbicacionImagen>c:/</UbicacionImagen>" +
            "<CadenaError></CadenaError>" +
            "<Imagen></Imagen>" +
            "<PosicionImagen>0</PosicionImagen>" +
            "<Parte>0</Parte>" +
            "</Proceso>";
        if (this._ws) {
            $('#btnCargaImagen').attr('disabled', true);
            this._ws.send(xmlSolicitud);
        } else {
            $('#btnCargaImagen').attr('disabled', false);
            console.error("Socket no inicializado.");
        }
    } else {
        $('#btnCargaImagen').attr('disabled', false);
        console.log("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.");
        this.mostrarMensaje("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.", 1);
        document.getElementById('btnCargaImagen').enabled = true;
    }
    return false;
}

///Metodo encargado de llamar el socket para que inicie el proceso de carge de la imagen
function adjuntarMasivoImage() {
    if (this._validaPluging == true) {
        $('#lblResumen').text("Abjuntando...");
        var xmlSolicitud = "<?xml version=\"1.0\"?>" +
            "<Proceso xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
            "<CodigoProceso>102</CodigoProceso>" +
            "<UbicacionImagen>c:/</UbicacionImagen>" +
            "<CadenaError></CadenaError>" +
            "<Imagen></Imagen>" +
            "<PosicionImagen>0</PosicionImagen>" +
            "<Parte>0</Parte>" +
            "</Proceso>";
        if (this._ws) {
            $('#btnCargaImagen').attr('disabled', true);
            this._ws.send(xmlSolicitud);
        } else {
            $('#btnCargaImagen').attr('disabled', false);
            console.error("Socket no inicializado.");
        }
    } else {
        $('#btnCargaImagen').attr('disabled', false);
        console.log("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.");
        this.mostrarMensaje("No cuenta con el plugin instalado o no inicializo este objeto con la propiedad connect.", 1);
        document.getElementById('btnCargaImagen').enabled = true;
    }
    return false;
}

///Metodo que se ejecuta para eliminar una imagen de la matriz y de diseño.
function eliminarItemSeleccionado(objeto, posicion) {
    document.getElementById(objeto).innerHTML = "";
    this._MatrizImagenes[parseInt(posicion)] = null;
    evaluaMatriz();
    contarDetalles();
}

/*Maximizar Imagen*/
function maximizarItemSeleccionado(objeto) {
    $("#bodyEscanner").html("");
}

//InicializarSocket();