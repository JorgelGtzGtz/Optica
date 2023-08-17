using dbconnection;
using Optica.Core.Config;
using Optica.Core.Entities;
using Optica.Core.Repository;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optica.Core.Services
{
    public interface IDiagnosticosService
    {
        Diagnostico GetDiagnostico(int id);
        List<Diagnostico> GetDiagnosticos();
        List<dynamic> GetDiagnosticosFiltro(string from, string to, int? idpaciente, int? idoptometrista, string folio = null);
        bool InsertUpdateDiagnostico(Diagnostico model, int idUsuario, out string Message);
    }

    public class DiagnosticosService : IDiagnosticosService
    {
        private readonly IDiagnosticoRepository _diagnosticoRepository;
        private readonly ILogRepository _logRepository;
        

        public DiagnosticosService(IDiagnosticoRepository diagnosticoRepository, ILogRepository logRepository) {
            _diagnosticoRepository = diagnosticoRepository;
            _logRepository = logRepository;
        }

        public Diagnostico GetDiagnostico(int id) {
            return _diagnosticoRepository.Get(id);
        }

        public List<Diagnostico> GetDiagnosticos() {
            return _diagnosticoRepository.GetAll("Diagnosticos").ToList();
        }

        public List<dynamic> GetDiagnosticosFiltro(string from, string to, int? idpaciente, int? idoptometrista, string folio = null)
        {
            string filter = string.Empty;

            if (!string.IsNullOrEmpty(folio) || !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) || idpaciente != null || idoptometrista != null )
            {
                filter = " Where ";
            }

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                filter += string.Format("C.Fecha between '{0} 00:00:00' and '{1} 23:59:59' ", from, to);
            }
            if (idpaciente != null)
            {
                filter += string.Format("{1} C.ID_Paciente = '{0}' ", idpaciente, (filter.Length > 0 ? "and" : ""));
            }
            if (idoptometrista != null)
            {
                filter += string.Format("{1} C.ID_Optometrista = '{0}' ", idoptometrista, (filter.Length > 0 ? "and" : ""));
            }
            if (!string.IsNullOrEmpty(folio))
            {
                filter += string.Format("{1} C.ID like '%{0}%' ", folio, (filter.Length > 0 ? "and" : ""));
            }
            Sql query = new Sql(@"select C.*, S.Nombre as Optometrista, A.Nombre as Paciente from Diagnosticos C 
                                    inner join Pacientes A on A.ID = C.ID_Paciente
                                    inner join Usuarios S on S.ID = C.ID_Optometrista" + (filter.Length > 0 ? filter : ""));
            return _diagnosticoRepository.GetByDynamicFilter(query);
        }

        public bool InsertUpdateDiagnostico(Diagnostico model, int idUsuario,  out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {

                int id = _diagnosticoRepository.InsertOrUpdate<int>(model);

                Log log = new Log();
                log.Tipo = "Diagnostico";
                log.ID_Referencia = id;
                log.ID_Usuario = idUsuario;
                log.Fecha = DateTime.Now;
                if (model.ID <= 0)
                {
                    log.Accion = "Diagnostico Nueva Guardada";
                }
                else
                {
                    log.Accion = "Diagnostico Actualizada";
                }
                _logRepository.InsertOrUpdate<int>(log);

                Message = "Diagnostico guardado con éxito";
                result = true;
            }
            catch (Exception ex)
            {
                Message = "Diagnostico No pudo ser guardado Error: " + ex.Message;
            }

            return result;
        }
    }
}
