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
        private SHA256 Sha256 = SHA256.Create(); //cria uma instância para implementar o SHA256

        public ArquivoController()
        {
            diretorio = Path.Combine(Directory.GetCurrentDirectory(), "arquivos"); //guarda em diretorio o caminho para a pasta chamada de "arquivos" 

            if (!Directory.Exists(diretorio)) // caso não haja o diretório "arquivos" 
            {
                Directory.CreateDirectory(diretorio); //cria-se o diretório "arquivos" no endereço da variável diretorio
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
        public async Task<string> EnviaArquivo([FromForm] IFormFile arquivo) // espera de maneira assíncrona até que um arquivo seja enviado
        {
            if (arquivo.Length > 0) //quando recebe um arquivo de tamanho > 0 
            {
                try // é necessário ter um método try catch pois podem ocorrer excessões
                {
                    using (FileStream filestream = System.IO.File.Create(Path.Combine(diretorio, arquivo.FileName)))
                    {
                        await arquivo.CopyToAsync(filestream);
                        filestream.Flush();

                        byte[] hash = Sha256.ComputeHash(arquivo.OpenReadStream());
                        string strHash = BytesToString(hash);

                        string resposta = "Arquivo recebido.";     // a variável resposta contém a sinalização de que o arquivo foi recebido
                        resposta += $"\nNome: {arquivo.FileName}";
                        resposta += $"\nTamanho: {arquivo.Length} bytes";
                        resposta += $"\nHash: {strHash}";
                        return resposta; // além da mensagem, a variável carrega as informações do nome , tamanho e HASH do arquivo
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Ocorreu uma falha no envio do arquivo..."; // caso ocorra erro de upload a aplicação retorna esta mensagem
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
