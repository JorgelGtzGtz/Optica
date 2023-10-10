export class Contrato{
    constructor(){}
    public ID: number;
    public Clave: string;
    public ClaveContrato: string;
    public Fecha: Date | null;
    public Plazo: number | null;
    public DiaCobro: number | null;
    public TipoCobro: string;
    public Total: number | null;
    public Restante: number | null;
    public Abonos: number | null;
    public Descuento: number | null;
    public Estatus: boolean | null;
    public Observaciones: string;
    public ID_Cliente: number;
    public ID_UsuarioCobrador: number;
    public ID_UsuarioCreacion: number;
    public ID_Sucursal: number;
}