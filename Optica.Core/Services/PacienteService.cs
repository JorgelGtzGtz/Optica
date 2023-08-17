using dbconnection;
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
    public interface IPacienteService
    {
        Paciente GetPaciente(int id);
        List<Paciente> GetPacientes();
        List<Paciente> GetPacientesFiltro(string nombre = null);
        bool InsertUpdatePaciente(Paciente cliente, out string Message);
        bool EliminarPaciente(int id, out string Message);
    }

    public class PacienteService : IPacienteService
    {
        private readonly IPacientesRepository _pacientesRepository;

        public PacienteService(IPacientesRepository pacientesRepository) {
            _pacientesRepository = pacientesRepository;
        }


        public Paciente GetPaciente(int id) {
            return _pacientesRepository.Get(id);
        }

        public List<Paciente> GetPacientes() {
            return _pacientesRepository.GetAll("Pacientes").ToList();
        }

        public List<Paciente> GetPacientesFiltro(string nombre = null)
        {
            string filter = " Where ";

            if (!string.IsNullOrEmpty(nombre))
            {
                filter += string.Format("Nombre like '%{0}%' ", nombre);
            }

            Sql query = new Sql(@"select * from Pacientes " + (!string.IsNullOrEmpty(nombre) ? filter : ""));
            return _pacientesRepository.GetByFilter(query);
        }

        public bool InsertUpdatePaciente(Paciente cliente, out string Message) {

            Message = string.Empty;
            bool result = false;
            try
            {
                if (cliente.FechaCreacion == null)
                    cliente.FechaCreacion = DateTime.Now;

                if (cliente.ID == 0)
                {
                   var id = _pacientesRepository.InsertOrUpdate<int>(cliente);
                }
                else
                {
                    _pacientesRepository.InsertOrUpdate<int>(cliente);
                }

                Message = "Paciente guardado " + cliente.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Paciente No pudo ser guardado Error: " + ex.Message;
            }

            return result;
        }

        public bool EliminarPaciente(int id, out string Message)
        {
            Message = string.Empty;
            bool result = false;
            try
            {
                var cliente = _pacientesRepository.Get(id);

                //Eliminar cliente
                _pacientesRepository.Remove(cliente);

                Message = "Paciente eliminado " + cliente.Nombre + "con exito";
                result = true;
            }
            catch (Exception ex)
            {

                Message = "Paciente no pudo ser eliminado Error: " + ex.Message;
            }
            return result;
        }

    }
}
