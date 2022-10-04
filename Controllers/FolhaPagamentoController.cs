using System.Collections.Generic;
using System.Linq;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/folhas")]
    public class FolhaPagamentoController : ControllerBase
    {
        private readonly DataContext _context;

        public FolhaPagamentoController(DataContext context) =>
            _context = context;
        

        // GET: /api/folha/listar
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar() => Ok(_context.Folhas.Include(a => a.funcionario).ToList());

        // POST: /api/folhas/cadastrar
        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] FolhaPagamento folhas)
        {
            folhas.SalarioBruto = folhas.QuantidadeHoras * folhas.Hora;

            if(folhas.SalarioBruto < 1903.98){
                folhas.ImpostoRenda = 0;
            }else if(folhas.SalarioBruto > 1903.99 && folhas.SalarioBruto < 2826.65){
                folhas.ImpostoRenda = 142.80;
            }else if(folhas.SalarioBruto > 2826.66 && folhas.SalarioBruto < 3751.05){
                folhas.ImpostoRenda = 354.80;
            }else if(folhas.SalarioBruto > 3751.06 && folhas.SalarioBruto < 4664.68){
                folhas.ImpostoRenda = 636.13;
            }else{
                folhas.ImpostoRenda = 869.36;
            }

            if(folhas.SalarioBruto < 1693.72){
                folhas.ImpostoInss = folhas.SalarioBruto % 8;
            }else if(folhas.SalarioBruto > 1693.73 && folhas.SalarioBruto < 2822.90){
                folhas.ImpostoInss = folhas.SalarioBruto % 9;
            }else if(folhas.SalarioBruto > 2822.91  && folhas.SalarioBruto < 5645.80){
                folhas.ImpostoInss = folhas.SalarioBruto % 11;
            }else{
                folhas.ImpostoInss = folhas.SalarioBruto - 621.03;
            }

            folhas.ImpostoFgts = folhas.SalarioBruto % 8;

            folhas.SalarioLiquido = folhas.SalarioBruto - folhas.ImpostoInss - folhas.ImpostoRenda;

            if(folhas != null && _context.Funcionarios.Any(a => a.funcionarioId == folhas.FuncionarioId))
            {
                _context.Folhas.Add(folhas);
                _context.SaveChanges();
                return Created("", folhas);
            }
            else
                return NotFound("Funcionario não cadastrado no sistema de folha de pagamento!");
        }

        // GET: /api/folhas/buscar/{cpf}, {nascimento}
        [HttpGet]
        [Route("buscar/{cpf}/{nascimento}")]
        public IActionResult Buscar([FromRoute] string cpf, DateTime nascimento)
        {
            FolhaPagamento folha = _context.Folhas.Include(a => a.funcionario).FirstOrDefault(a => a.funcionario.Cpf.Equals(cpf) && a.funcionario.Nascimento.Equals(nascimento));
            return folha != null ? Ok(folha) : NotFound();
        }

        [HttpGet]
        [Route("filtrar/{nascimento}")]
        public IActionResult Filtrar([FromRoute] DateTime nascimento)
        {
            FolhaPagamento folha = _context.Folhas.Include(a => a.funcionario).FirstOrDefault(a => a.funcionario.Nascimento.Equals(nascimento));
            return folha != null ? Ok(folha) : NotFound();
        }
    }
}
