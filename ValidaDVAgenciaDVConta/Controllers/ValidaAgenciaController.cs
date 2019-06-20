using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ValidaDVAgenciaDVConta.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ValidaDVAgenciaDVConta.Controllers
{
    public class ValidaAgenciaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GerarDV(ContaBancaria contaBancaria)
        {
            if (ModelState.IsValid)
            {
                ValidarDvAgenciaConta(contaBancaria);
            }
            return View(contaBancaria);
        }

        private void ValidarDvAgenciaConta(ContaBancaria conta)
        {
            switch (conta.Banco)
            {
                case "104":
                    {

                        break;
                    }
                case "001":
                    {
                        ValidarBancoBrasil(conta);
                        break;
                    }
                case "033":
                    {
                        ValidarSantander(conta);
                        break;
                    }
                case "341":
                    {
                        ValidarItau(conta);
                        break;
                    }
            }
        }

        private void ValidarSantander(ContaBancaria conta)
        {
            try
            {
                if (conta.Agencia.Length > 4)
                {
                    throw new Exception("Agencia inválida.");
                }

                if (conta.Conta.Length > 8)
                {
                    throw new Exception("Conta inválida.");
                }
                List<int> pesoConta = new List<int>() { 9, 7, 3, 1, 0, 0, 9, 7, 1, 3, 1, 9, 7, 3 };

                var agenciaConta = string.Concat(conta.Agencia, "00", conta.Conta);

                List<int> algarismosAgenciaConta = new List<int>();

                for (int i = 0; i < agenciaConta.Length; i++)
                {
                    algarismosAgenciaConta.Add(Convert.ToInt32(agenciaConta[i].ToString()));
                }

                int soma = 0;

                for (int i = 0; i < pesoConta.Count(); i++)
                {
                    int num = algarismosAgenciaConta[i] * (pesoConta[i]);

                    soma += num;
                }

                if (soma > 9)
                {
                    soma = soma % 10;
                }

                int digitoCalculado = 10 - soma;

                if (conta.DvConta != digitoCalculado)
                {
                    throw new Exception("Digito da conta é invalido.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ValidarItau(ContaBancaria conta)
        {
            try
            {
                if (conta.Agencia.Length > 4)
                {
                    throw new Exception("Agencia inválida.");
                }

                if (conta.Conta.Length > 5)
                {
                    throw new Exception("Conta inválida.");
                }

                List<int> pesoConta = new List<int>() { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

                var agenciaConta = string.Concat(conta.Agencia, conta.Conta);

                List<int> algarismosAgenciaConta = new List<int>();

                for (int i = 0; i < agenciaConta.Length; i++)
                {
                    algarismosAgenciaConta.Add(Convert.ToInt32(agenciaConta[i].ToString()));
                }

                int soma = 0;

                for (int i = 0; i < pesoConta.Count(); i++)
                {
                    int num = algarismosAgenciaConta[i] * (pesoConta[i]);

                    if (num > 9)
                    {
                        int aux = num;
                        List<int> algarismosnum = new List<int>();
                        while (aux > 0)
                        {
                            algarismosnum.Add(aux % 10);
                            aux = aux / 10;
                        }

                        num = algarismosnum.Sum();
                    }

                    soma += num;
                }
                int digitocalc = 10 - (soma % 10);

                if (conta.DvConta != (digitocalc))
                {
                    throw new Exception("Digito da conta é invalido.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ValidarBancoBrasil(ContaBancaria conta)
        {
            try
            {
                if (conta.Agencia.Length > 4)
                {
                    throw new Exception("Agencia inválida.");
                }

                if (conta.Conta.Length > 8)
                {
                    throw new Exception("Conta inválida.");
                }

                List<int> algarismoAgencia = new List<int>();
                for (int i = 0; i < conta.Agencia.Length; i++)
                {
                    algarismoAgencia.Add(Convert.ToInt32(conta.Agencia[i].ToString()));
                }

                int soma = 0;
                int pesoAgencia = 5;

                foreach (var algarismo in algarismoAgencia)
                {
                    soma += algarismo * (pesoAgencia);
                    pesoAgencia--;
                }

                conta.DvAgencia = 11 - (soma % 11);

                soma = 0;
                int pesoConta = 9;

                List<int> algarismosConta = new List<int>();
                for (int i = 0; i < conta.Conta.Length; i++)
                {
                    algarismosConta.Add(Convert.ToInt32(conta.Conta[i].ToString()));
                }


                foreach (var algarismo in algarismosConta)
                {
                    soma += algarismo * (pesoConta);
                    pesoConta--;
                }

                var digitoconta = 11 - (soma % 11);

                if (digitoconta != Convert.ToInt32(conta.DvConta))
                {
                    throw new Exception("Digito da conta é invalido.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}