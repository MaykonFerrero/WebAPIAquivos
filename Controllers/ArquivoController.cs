using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador_Arquivos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArquivoController : ControllerBase
    {
        private static string diretorio;
        private SHA256 Sha256 = SHA256.Create();

        public ArquivoController()
        {
            diretorio = Path.Combine(Directory.GetCurrentDirectory(), "arquivos");

            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }
        }

        [HttpGet]
        public string Get()
        {
            string resposta = "Gerenciador de Arquivos em execução: " + DateTime.Now.ToLongTimeString();
            resposta += $"\n Diretorio de arquivos: \"{diretorio}\"";
            return resposta;
        }

        [HttpPost("upload")]
        public async Task<string> EnviaArquivo([FromForm] IFormFile arquivo)
        {
            if (arquivo.Length > 0)
            {
                try
                {
                    using (FileStream filestream = System.IO.File.Create(Path.Combine(diretorio, arquivo.FileName)))
                    {
                        await arquivo.CopyToAsync(filestream);
                        filestream.Flush();

                        byte[] hash = Sha256.ComputeHash(arquivo.OpenReadStream());
                        string strHash = BytesToString(hash);

                        string resposta = "Arquivo recebido.";
                        resposta += $"\nNome: {arquivo.FileName}";
                        resposta += $"\nTamanho: {arquivo.Length} bytes";
                        resposta += $"\nHash: {strHash}";
                        return resposta;
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Ocorreu uma falha no envio do arquivo...";
            }
        }

        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes)
            {
                result += b.ToString("x2");
            }
            return result;
        }
    }
}
