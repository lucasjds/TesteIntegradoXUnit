using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Moq;
using Alura.CoisasAFazer.Core.Models;

namespace CoisasAFazerTestes
{
  public class CadastraTarefaHandlerExecute
  {
    [Fact]
    public void DadaTarefaComInformacoesValidasDeveInclurNoBd()
    {
      var comando = new CadastraTarefa("Estudar XUnit", 
                                      new Alura.CoisasAFazer.Core.Models.Categoria("Estudo"), 
                                      new DateTime(2019,12,31));

      var options = new DbContextOptionsBuilder<DbTarefasContext>().UseInMemoryDatabase("DbTarefasContext").Options;
      var contexto = new DbTarefasContext(options);

      var repo = new RepositorioTarefa(contexto);

      var handler = new CadastraTarefaHandler(repo);

      handler.Execute(comando);

      var tarefa = repo.ObtemTarefas(x => x.Titulo == "Estudar XUnit").FirstOrDefault();
      Assert.NotNull(tarefa);
    }

    [Fact]
    public void QuandoExceptionForLancadaResultadoIsSuccessDeveSerFalse()
    {
      var comando = new CadastraTarefa("Estudar XUnit",
                                      new Alura.CoisasAFazer.Core.Models.Categoria("Estudo"),
                                      new DateTime(2019, 12, 31));

      var mock = new Mock<IRepositorioTarefas>();
      mock.Setup(x => x.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(new Exception("Houve um erro"));

      var repo = mock.Object;

      var handler = new CadastraTarefaHandler(repo);

      CommandResult resultado = handler.Execute(comando);

      Assert.False(resultado.IsSucess); 
    }
  }
}
