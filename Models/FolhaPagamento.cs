using System.Reflection;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Models;
using System.Collections.Generic;

namespace API.Models
{
    public class FolhaPagamento
    {
        public FolhaPagamento () => CriadoEm = DateTime.Now;
        public int FolhaPagamentoId { get; set; }
        public int Hora { get; set;}
        public int QuantidadeHoras { get; set; }
        public int SalarioBruto { get; set; }
        public Double ImpostoRenda { get; set; }
        public Double ImpostoInss { get; set; }
        public Double ImpostoFgts { get; set; }
        public Double SalarioLiquido { get; set; }
        public int FuncionarioId { get; set; }
        public  Funcionario funcionario { get; set; }
        public DateTime  CriadoEm { get; set; }
    }
}