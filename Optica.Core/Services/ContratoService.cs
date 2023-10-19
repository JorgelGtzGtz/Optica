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
    public interface IContratoService
    {
        public Contrato GetLastContrato();
        public Producto GetProductoSelected(int id);

        public Paciente GetPacienteSelected(int id);
    }

    public class ContratoService : IContratoService
    {
        private readonly IContratosRepository _ContratosRepository;
        private readonly IProductosRepository _ProductosRepository;
        private readonly IPacientesRepository _PacientesRepository;

        public ContratoService(IPacientesRepository pacientesRepository, IContratosRepository ContratosRepository, IProductosRepository productosRepository)
        {
            _ContratosRepository = ContratosRepository;
            _ProductosRepository = productosRepository;
            _PacientesRepository = pacientesRepository;
        }

        public Contrato GetLastContrato()
        {
            var result = new Contrato();
            var a = _ContratosRepository.GetAll("contratos").ToList();
            result = (a.Count > 0) ? a[a.Count - 1] : new Contrato();
            return result;
        }

        public Producto GetProductoSelected(int id)
        {
            return _ProductosRepository.Get(id);
        }

        public Paciente GetPacienteSelected(int id)
        {
            return _PacientesRepository.Get(id);
        }

    }
}
