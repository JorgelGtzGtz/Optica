export class ContratoDetalle{
    constructor(){}
    public Clave: string;
    public Descripcion: string;
    public Paciente: string;
    public PContado: number | null;
    public PCredito: number | null;
    public ID_Paciente: number;
    public ID_Diagnostico: number;
    public Producto: number;
    public Contrato: number;
    public Cantidad: number;
    public Costo: number;
    public CostoTotal: number;
    public Precio: number;
    public VentaDetalle: number = 0;

}