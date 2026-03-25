using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Requests
{
    public class CreateInscriptionDto
    {
        public int UsuarioId { get; set; }

        public int CursoId { get; set; }
    }
}
