export class Contrato{
    constructor(){}
    public ID: number;
    public Fecha: Date | null;
    public ID_Alterno: string;
    public Status: string;
    public ID_Sucursal: number;
    public ID_Cliente: number;
    public Anticipo: number | null;
    public Contraentrega: number | null;
    public Total: number | null;
    public Importe: number | null;
    public Periodos: string;
    public ImportePago: number | null;
    public Frecuencia: number | null;
    public FechaInicial: Date | null;
    public DiaSemana: string;

}