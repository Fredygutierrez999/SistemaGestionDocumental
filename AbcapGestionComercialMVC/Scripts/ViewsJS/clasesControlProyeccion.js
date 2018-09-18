class movimiento {
    constructor(xOrden) {
        this._orden = xOrden;
        this._IDUniversal = parseInt(0);
        this._ID = parseInt(0);
        this._AnioSemana = "";
        this._Canal = "";
        this._CodigoPedido = "";
        this._IDClientes = 0;
        this._IDProductos = 0;
        this._IDPresentaciones = 0;
        this._IDTemporadabase = 0;
        this._IDPeriodo = 0;
        this._AnteriorValor = parseFloat('0');
        this._NuevoValor = parseFloat('0');
        this._SincronizadaBD = 0;
    }

    get ID() {
        return this._ID;
    }
    get IDUniversal() {
        return this._IDUniversal;
    }
    get Canal() {
        return this._Canal;
    }
    get CodigoPedido() {
        return this._CodigoPedido;
    }
    get IDcliente() {
        return this._IDClientes;
    }
    get IDProductos() {
        return this._IDProductos;
    }
    get IDPresentaciones() {
        return this._IDPresentaciones;
    }
    get IDTemporadaBase() {
        return this._IDTemporadabase;
    }
    get IDperiodo() {
        return this._IDPeriodo;
    }
    get AnioSemana() {
        return this._AnioSemana;
    }
    get AnteriorValor() {
        return this._AnteriorValor;
    }
    get NuevoValor() {
        return this._NuevoValor;
    }
    get Orden() {
        return this._orden;
    }

    get SincronizadaBD() {
        return this._SincronizadaBD;
    }


    set SetID(xValor) {
        this._ID = xValor;
    }
    set SetAnioSemana(xValor) {
        this._AnioSemana = xValor;
    }
    set SetAnteriorValor(xValor) {
        this._AnteriorValor = xValor;
    }
    set SetNuevoValor(xValor) {
        this._NuevoValor = xValor;
    }
    set SetIDUniversal(xValor) {
        this._IDUniversal = xValor;
    }
    set SetCanal(xValor) {
        this._Canal = xValor;
    }
    set SetCodigoPedido(xValor) {
        this._CodigoPedido = xValor;
    }
    set SetIDCliente(xValor) {
        this._IDClientes = xValor;
    }
    set SetIDProductos(xValor) {
        this._IDProductos = xValor;
    }
    set SetIDPresentaciones(xValor) {
        this._IDPresentaciones = xValor;
    }
    set SetIDTemporadaBase(xValor) {
        this._IDTemporadabase = xValor;
    }
    set SetIDPeriodo(xValor) {
        this._IDPeriodo = xValor;
    }
    set SetSinCronizadaBD(xValor) {
        this._SincronizadaBD = xValor;
    }

}