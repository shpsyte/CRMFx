using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Entity.Map;
using System.Reflection;
using Domain;
namespace Data.Context
{
    public class b2yweb_entities : DbContext
    {
        public b2yweb_entities()
            : base("B2yContext")
        {
        }
        public b2yweb_entities(String strEntity = "oracle")
            : base("name=" + strEntity + "_entities")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string shemma = "PATEND";
            base.Configuration.LazyLoadingEnabled = false;
            base.Configuration.AutoDetectChangesEnabled = false;
            //Aqui vamos remover a pluralização padrão do Etity Framework que é em inglês
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            /*Desabilitamos o delete em cascata em relacionamentos 1:N evitando
             ter registros filhos     sem registros pai*/
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //Basicamente a mesma configuração, porém em relacionamenos N:N
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //Aqui vamos remover a pluralização padrão do Etity Framework que é em inglês
            modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();
            modelBuilder.Configurations.Add(new DadosDoCrmMap(shemma));
            modelBuilder.Configurations.Add(new ListaPedidosMap(shemma));
            modelBuilder.Configurations.Add(new NoteMap(shemma));
            modelBuilder.Configurations.Add(new NotasMap(shemma));
            modelBuilder.Configurations.Add(new TituloMap(shemma));
            modelBuilder.Configurations.Add(new PsPerfilMap(shemma));
            modelBuilder.Configurations.Add(new CargosMap(shemma));
            modelBuilder.Configurations.Add(new W_TopProcedimentoByDeptoMap(shemma));
            modelBuilder.Configurations.Add(new W_QTDE_SAC_DIA_MAP(shemma));
            modelBuilder.Configurations.Add(new w_sac_por_tipo_Map(shemma));
            modelBuilder.Configurations.Add(new ContatosMap(shemma));
            modelBuilder.Configurations.Add(new PsLeadsMap(shemma));
            modelBuilder.Configurations.Add(new PsTipoLeadMap(shemma));
            modelBuilder.Configurations.Add(new PsOrigemLeadMap(shemma));
            modelBuilder.Configurations.Add(new PsInteresseLeadMap(shemma));
            modelBuilder.Configurations.Add(new PsSituacaoLeadMap(shemma));
            modelBuilder.Configurations.Add(new PS_Estagio_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new PS_Categorias_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new PS_Classe_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new PS_origem_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new PS_Situacao_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new PS_Tipo_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new PS_Pessoas_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new PS_Sac_Map(shemma));
            modelBuilder.Configurations.Add(new Ps_Sac_Ps_Sac_Map(shemma));
            
            modelBuilder.Configurations.Add(new SacArquivo_Map(shemma));
            modelBuilder.Configurations.Add(new Ps_Sac_Reabertura_Map(shemma));
            modelBuilder.Configurations.Add(new sac_troca_estagio_Map(shemma));
            modelBuilder.Configurations.Add(new SacProcedimento_map(shemma));
            modelBuilder.Configurations.Add(new EstagioUsuario_map(shemma));
            modelBuilder.Configurations.Add(new Ps_Pessoas_Map(shemma));
            modelBuilder.Configurations.Add(new IE_Itens_Map(shemma));
            modelBuilder.Configurations.Add(new GE_Status_Garantia_Map(shemma));
            modelBuilder.Configurations.Add(new Garantia_Map(shemma));
            modelBuilder.Configurations.Add(new GarantiaItem_Map(shemma));
            modelBuilder.Configurations.Add(new Ps_Representantes_Map(shemma));
            modelBuilder.Configurations.Add(new Garantia_Troca_Status_Map(shemma));
            modelBuilder.Configurations.Add(new IE_Itens_Vendas_Map(shemma));
            modelBuilder.Configurations.Add(new CartItem_Map(shemma));
            modelBuilder.Configurations.Add(new CartItemPrint_Map(shemma));
            modelBuilder.Configurations.Add(new GarantiaProcedimento_Map(shemma));
            modelBuilder.Configurations.Add(new GarantiaArq_Map(shemma));
            modelBuilder.Configurations.Add(new Tmp_Garantia_Impressao_Map(shemma));
            
            modelBuilder.Configurations.Add(new GarantiaImagens_Map(shemma));
            modelBuilder.Configurations.Add(new OperacaoFiscal_Map(shemma));
            modelBuilder.Configurations.Add(new NotaCliente_Map(shemma));
            
            modelBuilder.Configurations.Add(new NotaItemCliente_Map(shemma));
            modelBuilder.Configurations.Add(new SacGarantia_Map(shemma));
            modelBuilder.Configurations.Add(new ListaOcorrenciaMap(shemma));
            modelBuilder.Configurations.Add(new Tp_Procedimento_Motivos_Map(shemma));
            modelBuilder.Configurations.Add(new FormaPgtoMap(shemma));
            modelBuilder.Configurations.Add(new SegmentosMap(shemma));
            modelBuilder.Configurations.Add(new TipoAcaoMap(shemma));
            modelBuilder.Configurations.Add(new CampanhaMarketingMap(shemma));
            modelBuilder.Configurations.Add(new CampanhaMarketingDocMap(shemma));
            modelBuilder.Configurations.Add(new StatusMap(shemma));
            modelBuilder.Configurations.Add(new EstagioMap(shemma));
            modelBuilder.Configurations.Add(new EstagioUsuarioMap(shemma));
            modelBuilder.Configurations.Add(new CampanhaMarketingEstagiosMap(shemma));
            modelBuilder.Configurations.Add(new G1_CidadesMap(shemma));
            modelBuilder.Configurations.Add(new CampanhaOrcamentoMap(shemma));
            modelBuilder.Configurations.Add(new CampanhaOrcamentoRegionalMap(shemma));
            modelBuilder.Configurations.Add(new GeUnidadesMap(shemma));
            modelBuilder.Configurations.Add(new CampanhaMarketingPgtoMap(shemma));

            modelBuilder.Configurations.Add(new vw_Ps_LeadsMap(shemma));
            modelBuilder.Configurations.Add(new vw_ContatosMap(shemma));

            modelBuilder.Configurations.Add(new vw_SacMap(shemma));
            modelBuilder.Configurations.Add(new w_garantiaMap(shemma));
            modelBuilder.Configurations.Add(new vw_pessoasMap(shemma));



            //Fazendo o mapeamento com o banco de dados
            //Pega todas as classes que estão implementando a interface IMapping
            ////Assim o Entity Framework é capaz de carregar os mapeamentos
            //var typesToMapping = (from x in Assembly.GetExecutingAssembly().GetTypes()
            //                      where x.IsClass && typeof(IMapping).IsAssignableFrom(x)
            //                      select x).ToList();
            //// Varrendo todos os tipos que são mapeamento 
            //// Com ajuda do Reflection criamos as instancias 
            //// e adicionamos no Entity Framework
            //foreach (var mapping in typesToMapping)
            //{
            //    dynamic mappingClass = Activator.CreateInstance(mapping);
            //    modelBuilder.Configurations.Add(mappingClass);
            //}
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();
            //modelBuilder.Entity<DadosDoCrm>().ToTable("DADOS_CRM_VIEW", shemma);
            //modelBuilder.Entity<DadosDoCrm>().HasKey(s => new { s.cod_pessoa });
            modelBuilder.Entity<Usuario>().ToTable("USUARIO", shemma);
            modelBuilder.Entity<Usuario>().HasKey(s => new { s.CD_USUARIO });
            modelBuilder.Entity<GUsuario>().ToTable("GUSUARIO", shemma);
            modelBuilder.Entity<GUsuario>().HasKey(s => new { s.CD_GUSUARIO });
            modelBuilder.Entity<tp_procedimento>().ToTable("TP_PROCEDIMENTO", shemma);
            modelBuilder.Entity<tp_procedimento>().HasKey(s => new { s.CD_TIPO });
            modelBuilder.Entity<Combo>().ToTable("COMBO", shemma);
            modelBuilder.Entity<Combo>().HasKey(s => new { s.ID });
            modelBuilder.Entity<DEPARTAMENTO>().ToTable("DEPARTAMENTO", shemma);
            modelBuilder.Entity<DEPARTAMENTO>().HasKey(s => new { s.CD_DEPARTAMENTO });
            modelBuilder.Entity<DepartamentoUsuario>().ToTable("DEPARTAMENTOUSUARIO", shemma);
            modelBuilder.Entity<DepartamentoUsuario>().HasKey(s => new { s.ID });
            modelBuilder.Entity<UsuarioRegional>().ToTable("USUARIOREGIONAL", shemma);
            modelBuilder.Entity<UsuarioRegional>().HasKey(s => new { s.ID });
            modelBuilder.Entity<Regional>().ToTable("REGIONAL", shemma);
            modelBuilder.Entity<Regional>().HasKey(s => new { s.CD_REGIONAL });
            modelBuilder.Entity<Clientes>().ToTable("CLIENTES", shemma);
            modelBuilder.Entity<Clientes>().HasKey(s => new { s.CD_CADASTRO });
            modelBuilder.Entity<Permissoes>().ToTable("PERMISSOES", shemma);
            modelBuilder.Entity<Permissoes>().HasKey(s => new { s.ID_INSERT });
            modelBuilder.Entity<ProcedimentoAdm>().ToTable("PROCEDIMENTO", shemma);
            modelBuilder.Entity<ProcedimentoAdm>().HasKey(s => new { s.CD_PROCEDIMENTO });
            modelBuilder.Entity<wProcedimento>().ToTable("WPROCEDIMENTO", shemma);
            modelBuilder.Entity<wProcedimento>().HasKey(s => new { s.CD_PROCEDIMENTO });

            modelBuilder.Entity<ProcedimentoAdmArq>().ToTable("PROCEDIMENTOARQ", shemma);
            modelBuilder.Entity<ProcedimentoAdmArq>().HasKey(s => new { s.ID_ARQ });
            modelBuilder.Entity<ProcedimentoAdmArq>().Property(x => x.ID_ARQ).HasColumnName("ID_ARQ").IsRequired();
            modelBuilder.Entity<ProcedimentoAdmArq>().Property(x => x.CD_PROCEDIMENTO).HasColumnName("CD_PROCEDIMENTO").IsOptional();
            modelBuilder.Entity<ProcedimentoAdmArq>().Property(x => x.CAMINHO).HasColumnName("CAMINHO").IsOptional();
            modelBuilder.Entity<ProcedimentoAdmArq>().Property(x => x.DES_IMAGEM).HasColumnName("DES_IMAGEM").IsOptional();
            modelBuilder.Entity<ProcedimentoAdmArq>().Property(x => x.DES_CONTENTTYPE).HasColumnName("DES_CONTENTTYPE").IsOptional();
            modelBuilder.Entity<ProcedimentoAdmArq>().Property(x => x.NOME_ARQUIVO).HasColumnName("NOME_ARQUIVO").IsOptional();




            modelBuilder.Entity<eNota>().ToTable("ENOTA", shemma);
            modelBuilder.Entity<eNota>().HasKey(s => new { s.NR_NOTA });
            modelBuilder.Entity<Grafico1>().ToTable("GRAFICO1", shemma);
            modelBuilder.Entity<Grafico1>().HasKey(s => new { s.CD_DEPARTAMENTO });
            modelBuilder.Entity<Grafico2>().ToTable("GRAFICO2", shemma);
            modelBuilder.Entity<Grafico2>().HasKey(s => new { s.ID_SITUACAO });
            modelBuilder.Entity<Situacao>().ToTable("SITUACOESPROCEDIMENTO", shemma);
            modelBuilder.Entity<Situacao>().HasKey(s => new { s.ID_SITUACAO });
            modelBuilder.Entity<Modulos>().ToTable("MODULOS", shemma);
            modelBuilder.Entity<Modulos>().HasKey(s => new { s.CD_MODULO });
            modelBuilder.Entity<pa_troca_departamentos>().ToTable("PA_TROCA_DEPARTAMENTOS", shemma);
            modelBuilder.Entity<pa_troca_departamentos>().HasKey(s => new { s.NUM_SEQ });
            modelBuilder.Entity<pa_troca_departamentos>().HasRequired(a => a.DEPANT).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_ANT);
            modelBuilder.Entity<pa_troca_departamentos>().HasRequired(a => a.DEPNOVA).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_NOVA);
            modelBuilder.Entity<wpa_troca_departamentos>().ToTable("WPA_TROCA_DEPARTAMENTOS", shemma);
            modelBuilder.Entity<wpa_troca_departamentos>().HasKey(s => new { s.NUM_SEQ });
            modelBuilder.Entity<TRANSPORTADOR>().ToTable("TRANSPORTADOR", shemma);
            modelBuilder.Entity<TRANSPORTADOR>().HasKey(s => new { s.COD_TRANSPORTADOR });
            modelBuilder.Entity<wpa_troca_departamentos>().HasRequired(a => a.DEPANT).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_ANT);
            modelBuilder.Entity<wpa_troca_departamentos>().HasRequired(a => a.DEPNOVA).WithMany().HasForeignKey(u => u.CD_DEPARTAMENTO_NOVA);
            /* foreign keys */
            /* modelBuilder.Entity<DepartamentoUsuario>().HasRequired(a => a.CD_USUARIO).WithMany().HasForeignKey(u => u.CD_USUARIO);
               modelBuilder.Entity<DepartamentoUsuario>().HasRequired(a => a.CD_DEPARTAMENTO).WithMany().HasForeignKey(u => new { u.CD_DEPARTAMENTO }); */

            modelBuilder.Entity<AI_NE_NOTAS>().ToTable("AI_NE_NOTAS", shemma);
            modelBuilder.Entity<AI_NE_NOTAS_ITENS>().ToTable("AI_NE_NOTAS_ITENS", shemma);
            modelBuilder.Entity<AI_NE_CONHECIMENTOS>().ToTable("AI_NE_CONHECIMENTOS", shemma);
            modelBuilder.Entity<AI_NE_NOTAS_ICMS>().ToTable("AI_NE_NOTAS_ICMS", shemma);
            modelBuilder.Entity<AI_NE_NOTAS_OPERACOES>().ToTable("AI_NE_NOTAS_OPERACOES", shemma);
            modelBuilder.Entity<AI_NE_OBSERVACOES>().ToTable("AI_NE_OBSERVACOES", shemma);
            modelBuilder.Entity<AI_NE_TITULOS>().ToTable("AI_NE_TITULOS", shemma);
            modelBuilder.Entity<AI_NE_CONTROLE>().ToTable("AI_NE_CONTROLE", shemma);
            modelBuilder.Entity<NE_NOTA_MENSAGEM>().ToTable("NE_NOTA_MENSAGEM", shemma);
            modelBuilder.Entity<Ne_Nota_Chave>().ToTable("NE_NOTA_CHAVE", shemma);
            modelBuilder.Entity<t4_conds_pgto>().ToTable("T4_CONDS_PGTO", shemma);
            





        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<GUsuario> GUsuario { get; set; }
        public DbSet<Combo> Combo { get; set; }
        public DbSet<tp_procedimento> TP_PROCEDIMENTO { get; set; }
        public DbSet<DEPARTAMENTO> DEPARTAMENTOes { get; set; }
        public DbSet<DepartamentoUsuario> DepartamentoUsuario { get; set; }
        public DbSet<UsuarioRegional> UsuarioRegional { get; set; }
        public DbSet<Regional> Regional { get; set; }
        public DbSet<vw_Sac> vw_Sac { get; set; }
        public DbSet<w_garantia> w_garantia { get; set; }
        public DbSet<vw_pessoas> vw_pessoas { get; set; }
        

        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<vw_Contatos> vw_Contatos { get; set; }
        
        public DbSet<TRANSPORTADOR> TRANSPORTADOR { get; set; }
        public DbSet<ProcedimentoAdm> ProcedimentoAdm { get; set; }
        public DbSet<eNota> eNota { get; set; }
        public DbSet<ProcedimentoAdmArq> ProcedimentoAdmArq { get; set; }
        public DbSet<Situacao> Situacao { get; set; }
        public DbSet<pa_troca_departamentos> pa_troca_departamentos { get; set; }
        public DbSet<wpa_troca_departamentos> wpa_troca_departamentos { get; set; }
        public DbSet<Grafico1> Grafico1 { get; set; }
        public DbSet<Grafico2> Grafico2 { get; set; }
        public DbSet<Permissoes> Permissoes { get; set; }
        public DbSet<Modulos> Modulos { get; set; }
        public DbSet<wProcedimento> wProcedimento { get; set; }
        public DbSet<DadosDoCrm> Dados_crm { get; set; }
        public DbSet<ListaPedidos> Lista_Pedidos { get; set; }
        public DbSet<ListaNotas> Lista_Notas { get; set; }
        public DbSet<ListaOcorrencia> Lista_Ocorrencia { get; set; }
        public DbSet<Note> ListaComentarios { get; set; }
        public DbSet<ListaTitulos> Lista_Titulos { get; set; }
        public DbSet<Ps_Perfil_Crm> Ps_Perfil_Crm { get; set; }
        public DbSet<W_TopProcedimentoByDepto> W_TopProcedimentoByDepto { get; set; }
        public DbSet<W_QTDE_SAC_DIA> W_QTDE_SAC_DIA  { get; set; }
        public DbSet<w_sac_por_tipo> w_sac_por_tipo { get; set; }
        public DbSet<Cargos> Cargos { get; set; }
        public DbSet<Contatos> Contatos { get; set; }
        public DbSet<Ps_Leads> Ps_Leads { get; set; }
        public DbSet<Ps_LeadsInteresse> Ps_LeadsInteresse { get; set; }
        public DbSet<Ps_Interesse_Lead> Ps_Interesse_Lead { get; set; }
        public DbSet<Ps_Origem_Lead> Ps_Origem_Lead { get; set; }
        public DbSet<Ps_Tipo_Lead> Ps_Tipo_Lead { get; set; }
        public DbSet<Ps_Situacao_Lead> Ps_Situacao_Lead { get; set; }
        public DbSet<PS_Estagio_Sac> PS_Estagio_Sac { get; set; }
        public DbSet<PS_Categorias_Sac> PS_Categorias_Sac { get; set; }
        public DbSet<PS_Classe_Sac> PS_Classe_Sac { get; set; }
        public DbSet<PS_Origem_Sac> PS_Origem_Sac { get; set; }
        public DbSet<PS_Situacao_Sac> PS_Situacao_Sac { get; set; }
        public DbSet<PS_Tipo_Sac> PS_Tipo_Sac { get; set; }
        public DbSet<PS_Sac> PS_Sac { get; set; }
        public DbSet<SacArquivo> SacArquivo { get; set; }
        public DbSet<Ps_Sac_Reabertura> Ps_Sac_Reabertura { get; set; }
        public DbSet<Ps_Sac_Ps_Sac> Ps_Sac_Ps_Sac { get; set; }
        public DbSet<PS_Pessoas_Sac> PS_Pessoas_Sac { get; set; }
        public DbSet<G1_Cidades> G1_Cidades { get; set; }
        public DbSet<sac_troca_estagio> sac_troca_estagio { get; set; }
        public DbSet<SacProcedimento> SacProcedimento { get; set; }
        public DbSet<Ps_Estagio_Usuario> Ps_Estagio_Usuario { get; set; }
        public DbSet<Ps_Pessoas> Ps_Pessoas { get; set; }
        public DbSet<IE_Itens> IE_Itens { get; set; }
        public DbSet<IE_Itens_Vendas> IE_Itens_Vendas { get; set; }
        public DbSet<GE_Status_Garantia> GE_Status_Garantia { get; set; }
        public DbSet<Garantia> Garantia { get; set; }
        public DbSet<GarantiaItem> GarantiaItem { get; set; }
        public DbSet<Ps_Representantes> Ps_Representantes { get; set; }
        public DbSet<Garantia_Troca_Status> Garantia_Troca_Status { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<CartItemPrint> CartItemPrint { get; set; }
        public DbSet<GarantiaProcedimento> GarantiaProcedimento { get; set; }
        public DbSet<GarantiaImagens> GarantiaImagens { get; set; }
        public DbSet<NotaCliente> NotaCliente { get; set; }
        public DbSet<vWOperacaoFiscal> OperacaoFiscal { get; set; }
        public DbSet<NotaItemCliente> NotaItemCliente { get; set; }
        public DbSet<GarantiaArq> GarantiaArq { get; set; }
        public DbSet<Tmp_Garantia_Impressao> Tmp_Garantia_Impressao  { get; set; }
        public DbSet<SacGarantia> SacGarantia { get; set; }
        public DbSet<Tp_Procedimento_Motivos> Tp_Procedimento_Motivos { get; set; }
        public DbSet<FormaPgto> FormaPgto { get; set; }
        public DbSet<Segmentos> Segmentos { get; set; }
        public DbSet<TipoAcao> TipoAcao { get; set; }
        public DbSet<CampanhaMarketing> CampanhaMarketing { get; set; }
        public DbSet<CampanhaMarketingDoc> CampanhaMarketingDoc { get; set; }
        public DbSet<Status> Status{ get; set; }
        public DbSet<vw_Ps_Leads> vw_Ps_Leads { get; set; }
        public DbSet<Estagio> Estagio { get; set; }
        public DbSet<EstagioUsuario> EstagioUsuario { get; set; }
        public  DbSet<CampanhaMarketingEstagios> CampanhaMarketingEstagios { get; set; }
        public DbSet<CampanhaOrcamento> CampanhaOrcamento { get; set; }
        public DbSet<CampanhaOrcamentoRegional> CampanhaOrcamentoRegional { get; set; }
        public DbSet<GeUnidades> GeUnidades { get; set; }
        public DbSet<CampanhaMarketingPgto> CampanhaMarketingPgto { get; set; }

        public DbSet<t4_conds_pgto> t4_conds_pgto { get; set; }

        
            public DbSet<Ne_Nota_Chave> Ne_Nota_Chave { get; set; }
        public DbSet<AI_NE_NOTAS> AI_NE_NOTAS { get; set; }
        public DbSet<AI_NE_NOTAS_ITENS> AI_NE_NOTAS_ITENS { get; set; }
        public DbSet<AI_NE_CONHECIMENTOS> AI_NE_CONHECIMENTOS { get; set; }
        public DbSet<AI_NE_NOTAS_ICMS> AI_NE_NOTAS_ICMS { get; set; }
        public DbSet<AI_NE_NOTAS_OPERACOES> AI_NE_NOTAS_OPERACOES { get; set; }
        public DbSet<AI_NE_OBSERVACOES> AI_NE_OBSERVACOES { get; set; }
        public DbSet<AI_NE_TITULOS> AI_NE_TITULOS { get; set; }

        public DbSet<AI_NE_CONTROLE> AI_NE_CONTROLE { get; set; }
        public DbSet<NE_NOTA_MENSAGEM> NE_NOTA_MENSAGEM { get; set; }
        



    }
}
