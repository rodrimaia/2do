﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MongoDB.Bson.Serialization.Attributes;
using _2do.Data.Interfaces;

namespace _2do.Models
{
    public class Projeto  : AbstractModel , IPossuiGuidId
    {
        private DateTime? _dataInicio;
        [BsonElement("Tarefas")]
        private IList<Tarefa> _tarefas; 

        public Guid Id { get; set; }
        public string Nome { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataEntrega { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataInicio
        {
            get { return _dataInicio; }
            set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException("Data Inicio nao pode ser menor que data Atual", "DataInicio");
                _dataInicio = value;
            }
        }

        public IEnumerable<Tarefa> Tarefas { get { return _tarefas; } }

        public ColaboradorInfo Responsavel { get; private set; }

        public void AdicionarTarefa(Tarefa item)
        {
            if(_tarefas == null)
                _tarefas = new List<Tarefa>();
            _tarefas.Add(item);
        }

        public void AdicionarTarefa(IEnumerable<Tarefa> tarefas)
        {
            foreach (var t in tarefas) 
                AdicionarTarefa(t);
        }

        public void AdicionarColaborador(Colaborador colaborador)
        {
            Responsavel = new ColaboradorInfo {Id = colaborador.Id, Nome = colaborador.Nome};
        }
    }

    public class ProjetoValidator : AbstractValidator<Projeto>
    {
        public ProjetoValidator()
        {
            RuleFor(p => p.DataInicio).NotNull().GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(p => p.Tarefas).SetCollectionValidator(new TarefaValidator());
        }
    }

   
}