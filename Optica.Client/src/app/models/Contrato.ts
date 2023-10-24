export class Contrato{
    constructor(){}
    public ID: number;
    public Fecha: Date | null;
    public Clave: string;
    public ClaveContrato: string;
    public Status: string;
    public ID_Sucursal: number;
    public ID_Cliente: number;
    public ID_UsuarioCreacion: number;
    public Anticipo: number | null = 0;
    public Contraentrega: number | null = 0;
    public Total: number | null = 0;
    public ImportePrestamo: number | null = 0;
    public Plazo: number = 1;
    public ImportePago: number | null = 0;
    public Frecuencia: number | null;
    public FechaInicial: Date | null;
    public fechaEspEnt: Date | null;
    public DiaCobro: number;
    public Abonos: number = 0;
    public Descuento: number = 0;
    public Estatus: number = 1;
    public Restante: number;
    public ocupaAnticipo: boolean = false;
    public ocupaCE: boolean = false;
    public gastoCob: boolean = false;
}