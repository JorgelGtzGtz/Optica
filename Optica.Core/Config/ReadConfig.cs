using Optica.Core.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Config
{
    public static class ReadConfig
    {
        #region propiedades
        public static int IDCompra;
        public static int IDVenta;
        public static int IDDevVenta;
        public static int IDDevCompra;
        public static int IDInvInicial;
        public static int IDRemision;
        public static int IDTraspasoE;
        public static int IDTraspasoS;
        public static int IDAjusteFisicoE;
        public static int IDAjusteFisicoS;
        public static int IDRetiro;
        public static int IDConsumoInterno;
        public static int IDCambModeloE;
        public static int IDCambModeloS;
        public static int IDMCAOfillamas;
        public static int IDConsignacionE;
        public static int IDConsignacionS;
        public static int IDEntrada;
        public static int IDSalida;
        
        public static int ValTipoEntrada;
        public static int ValTipoCompra;
        public static int ValTipoSalida;
        public static int ValTipoFisico;
        public static int ValTipoTraspaso;
        public static int ValTipoDescomposicion;
        public static int ValTipoCrearKit;
        public static int ValTipoOtro;
        public static int ValTipoPrestamo;

        public static string TimeZone;
        public static bool EnableTimeZone;
        #endregion

        public static void init()
        {
            //ValTipoEntrada = 101;
            //ValTipoSalida = 102;
            //ValTipoPrestamo = 103;

            ////ValTipoCompra = 100;
            ////ValTipoFisico = 103;
            ////ValTipoTraspaso = 104;
            ////ValTipoDescomposicion = 105;
            ////ValTipoCrearKit = 106;
            ////ValTipoOtro = 99;

            //IDCompra = ConfigurationManager.AppSettings["IDCompra"].ToString().ParseInt();
            //IDVenta = ConfigurationManager.AppSettings["IDVenta"].ToString().ParseInt();
            //IDDevVenta = ConfigurationManager.AppSettings["IDDevVenta"].ToString().ParseInt();
            //IDDevCompra = ConfigurationManager.AppSettings["IDDevCompra"].ToString().ParseInt();
            //IDInvInicial = ConfigurationManager.AppSettings["IDInvInicial"].ToString().ParseInt();
            //IDRemision = ConfigurationManager.AppSettings["IDRemision"].ToString().ParseInt();
            //IDTraspasoE = ConfigurationManager.AppSettings["IDTraspasoE"].ToString().ParseInt();
            //IDTraspasoS = ConfigurationManager.AppSettings["IDTraspasoS"].ToString().ParseInt();
            //IDAjusteFisicoE = ConfigurationManager.AppSettings["IDAjusteFisicoE"].ToString().ParseInt();
            //IDAjusteFisicoS = ConfigurationManager.AppSettings["IDAjusteFisicoS"].ToString().ParseInt();
            //IDConsumoInterno = ConfigurationManager.AppSettings["IDConsumoInterno"].ToString().ParseInt();
            //IDCambModeloE = ConfigurationManager.AppSettings["IDCambModeloE"].ToString().ParseInt();
            //IDCambModeloS = ConfigurationManager.AppSettings["IDCambModeloS"].ToString().ParseInt();
            //IDMCAOfillamas = ConfigurationManager.AppSettings["IDMCAOfillamas"].ToString().ParseInt();
            //IDConsignacionE = ConfigurationManager.AppSettings["IDConsignacionE"].ToString().ParseInt();
            //IDConsignacionS = ConfigurationManager.AppSettings["IDConsignacionS"].ToString().ParseInt();
            //IDEntrada = ConfigurationManager.AppSettings["IDEntrada"].ToString().ParseInt();
            //IDSalida = ConfigurationManager.AppSettings["IDSalida"].ToString().ParseInt();

            EnableTimeZone = bool.Parse(ConfigurationManager.AppSettings["EnableTimeZone"]);
            TimeZone = ConfigurationManager.AppSettings["TimeZone"];
        }

    }
}
