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
    [Route("api/[controller]")] // define que a rota ou os próximos passos são controllers de uma api
    [ApiController] // Informa que todos as ações são de Controlador
    public class ArquivoController : ControllerBase
    {
        private static string diretorio;
        private SHA256 Sha256 = SHA256.Create(); //cria uma instância para implementar o SHA256

        public ArquivoController() // cria um método público responsável por criar o diretório onde ficarão armazenados os arquivos
        {
            diretorio = Path.Combine(Directory.GetCurrentDirectory(), "arquivos"); //guarda em diretorio o caminho para a pasta chamada de "arquivos" 

            if (!Directory.Exists(diretorio)) // caso não haja o diretório "arquivos" 
            {
                Directory.CreateDirectory(diretorio); //cria-se o diretório "arquivos" no endereço da variável diretorio
            }
        }

        [HttpGet] // informa que as próximas ações são de HTTP GET
        public string Get()
        {
            string resposta = "Gerenciador de Arquivos em execução: " + DateTime.Now.ToLongTimeString();
            resposta += $"\n Diretorio de arquivos: \"{diretorio}\"";
            return resposta; // informa quando foi realizada a execução e em que local do diretório estão os arquivos que foram upados
        }

        [HttpPost("upload")] // Informa que a próxima ação é um HTTP POST
        public async Task<string> EnviaArquivo([FromForm] IFormFile arquivo) // espera de maneira assíncrona até que um arquivo seja enviado
        {
            if (arquivo.Length > 0) 
            {
                try // é necessário ter um método try catch pois podem ocorrer excessões
                {
                    using (FileStream filestream = System.IO.File.Create(Path.Combine(diretorio, arquivo.FileName)))
                    // faz um stream do arquivo possibilitando as operações de leitura
                    {
                        await arquivo.CopyToAsync(filestream); //Lê de forma assíncrona os bytes do fluxo atual(filestream) 
                        filestream.Flush(); //Limpa os buffers do fluxo atual e faz com que todos os dados armazenados em buffer sejam gravados no arquivo (filestream)

                        byte[] hash = Sha256.ComputeHash(arquivo.OpenReadStream()); //Calcula o Hash do arquivo e retorna um array de bytes 
                        string strHash = BytesToString(hash); //transforma esse array de byte em string

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

        public static string BytesToString(byte[] bytes) // método que retorna um array  com uma sequência de valores hexadecimais
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
