using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain.Entity.Map
{


    public class CampanhaMarketingPgtoMap : CRMEntityTypeConfiguration<CampanhaMarketingPgto>, IMapping
    {
        public CampanhaMarketingPgtoMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("CAMPANHAMARKETINGPGTO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.campanhamarketingpgtoid, x.campanhaid });

            this.Property(x => x.campanhamarketingpgtoid).HasColumnName("CAMPANHAMARKETINGPGTOID").IsRequired();
            this.Property(x => x.campanhaid).HasColumnName("CAMPANHAID").IsRequired();
            this.Property(x => x.formapgtoid).HasColumnName("FORMAPGTOID").IsOptional();
            this.Property(x => x.num_pedido).HasColumnName("NUM_PEDIDO").IsOptional();
            this.Property(x => x.cod_compl).HasColumnName("COD_COMPL").IsOptional();
            this.Property(x => x.num_titulo).HasColumnName("NUM_TITULO").IsOptional();
            this.Property(x => x.num_parcela).HasColumnName("NUM_PARCELA").IsOptional();
            this.Property(x => x.des_documento).HasColumnName("DES_DOCUMENTO").IsOptional();
            this.Property(x => x.des_agencia).HasColumnName("DES_AGENCIA").IsOptional();
            this.Property(x => x.des_banco).HasColumnName("DES_BANCO").IsOptional();
            this.Property(x => x.des_conta).HasColumnName("DES_CONTA").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsOptional();
            this.Property(x => x.dta_inclusao).HasColumnName("DTA_INCLUSAO").IsOptional();
            this.Property(x => x.vlr_pgto).HasColumnName("VLR_PGTO").IsOptional();
            this.Property(x => x.des_imagem).HasColumnName("DES_IMAGEM").IsOptional();
            this.Property(x => x.des_observacao).HasColumnName("DES_OBSERVACAO").IsOptional();
            this.Property(x => x.ind_total).HasColumnName("IND_TOTAL").IsOptional();
            this.Property(x => x.des_contentype).HasColumnName("DES_CONTENTYPE").IsOptional(); 


            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => new { d.cod_usuario });
            this.HasRequired(t => t.CampanhaMarketing).WithMany().HasForeignKey(d => new { d.campanhaid });
            this.HasRequired(t => t.FormaPgto).WithMany().HasForeignKey(d => new { d.formapgtoid });

        }
    }



    public class GeUnidadesMap : CRMEntityTypeConfiguration<GeUnidades>, IMapping
    {
        public GeUnidadesMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GE_UNIDADES", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_unidade });

            this.Property(x => x.cod_unidade).HasColumnName("COD_UNIDADE").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsRequired();
        }
    }
    public class CampanhaOrcamentoRegionalMap : CRMEntityTypeConfiguration<CampanhaOrcamentoRegional>, IMapping
    {
        public CampanhaOrcamentoRegionalMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("CAMPANHAORCAMENTOREGIONAL", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.campanhaorcamentoregionalid, x.campanhaorcamentoid });

            this.Property(x => x.campanhaorcamentoregionalid).HasColumnName("CAMPANHAORCAMENTOREGIONALID").IsRequired();
            this.Property(x => x.campanhaorcamentoid).HasColumnName("CAMPANHAORCAMENTOID").IsRequired();
            this.Property(x => x.cod_regional).HasColumnName("COD_REGIONAL").IsRequired();
            this.Property(x => x.vlr_orcamento).HasColumnName("VLR_ORCAMENTO").IsRequired();
            this.HasRequired(t => t.CampanhaOrcamento).WithMany().HasForeignKey(d => new { d.campanhaorcamentoid });
            this.HasRequired(t => t.GeUnidades).WithMany().HasForeignKey(d => new { d.cod_regional });
        }
    }


    public class CampanhaOrcamentoMap : CRMEntityTypeConfiguration<CampanhaOrcamento>, IMapping
    {
        public CampanhaOrcamentoMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("CAMPANHAORCAMENTO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.campanhaorcamentoid });

            this.Property(x => x.campanhaorcamentoid).HasColumnName("CAMPANHAORCAMENTOID").IsRequired();
            this.Property(x => x.ano).HasColumnName("ANO").IsRequired();
            this.Property(x => x.vlr_orcamento).HasColumnName("VLR_ORCAMENTO").IsRequired();

        }
    }
    


    public class EstagioUsuarioMap : CRMEntityTypeConfiguration<EstagioUsuario>, IMapping
    {
        public EstagioUsuarioMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("ESTAGIOUSUARIO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.estagiousuarioId });
            this.Property(x => x.estagioId).HasColumnName("ESTAGIOID").IsRequired();
            this.Property(x => x.estagiousuarioId).HasColumnName("ESTAGIOUSUARIOID").IsRequired();
            this.Property(x => x.cd_usuario).HasColumnName("CD_USUARIO").IsRequired();
            this.HasRequired(t => t.Estagio).WithMany().HasForeignKey(d => new { d.estagioId });
            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => new { d.cd_usuario });
        }
    }
    public class EstagioMap : CRMEntityTypeConfiguration<Estagio>, IMapping
    {
        public EstagioMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("ESTAGIO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.estagioId });
            this.Property(x => x.estagioId).HasColumnName("ESTAGIOID").IsRequired();
            this.Property(x => x.descricao).HasColumnName("DESCRICAO").IsRequired();
            this.Property(x => x.ind_inicio).HasColumnName("IND_INICIO").IsRequired();
            this.Property(x => x.estagioid_proximo).HasColumnName("ESTAGIOID_PROXIMO").IsRequired();
            this.Property(x => x.estagioid_anterior).HasColumnName("ESTAGIOID_ANTERIOR").IsRequired();
            this.Property(x => x.statusid).HasColumnName("STATUSID").IsRequired();
            this.Property(x => x.ind_liberado).HasColumnName("IND_LIBERADO").IsRequired();
            this.HasRequired(t => t.Status).WithMany().HasForeignKey(d => new { d.statusid });
            this.HasRequired(t => t.estagioAnterior).WithMany().HasForeignKey(d => new { d.estagioid_anterior });
            this.HasRequired(t => t.estagioProximo).WithMany().HasForeignKey(d => new { d.estagioid_proximo });
        }
    }
    public class StatusMap : CRMEntityTypeConfiguration<Status>, IMapping
    {
        public StatusMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("STATUS", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.statusId });
            this.Property(x => x.statusId).HasColumnName("STATUSID").IsRequired();
            this.Property(x => x.descricao).HasColumnName("DESCRICAO").IsRequired();
        }
    }
    public class FormaPgtoMap : CRMEntityTypeConfiguration<FormaPgto>, IMapping
    {
        public FormaPgtoMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("FORMAPGTO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.formapgtoid });
            this.Property(x => x.formapgtoid).HasColumnName("FORMAPGTOID").IsRequired();
            this.Property(x => x.des_forma).HasColumnName("DES_FORMA").IsRequired();
        }
    }
    public class SegmentosMap : CRMEntityTypeConfiguration<Segmentos>, IMapping
    {
        public SegmentosMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("SEGMENTOS", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.segmentoid });
            this.Property(x => x.segmentoid).HasColumnName("SEGMENTOID").IsRequired();
            this.Property(x => x.des_segmento).HasColumnName("DES_SEGMENTO").IsRequired();
        }
    }
    public class TipoAcaoMap : CRMEntityTypeConfiguration<TipoAcao>, IMapping
    {
        public TipoAcaoMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("TIPOACAO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.tipoacaoid, x.segmentoid });
            this.Property(x => x.tipoacaoid).HasColumnName("TIPOACAOID").IsRequired();
            this.Property(x => x.segmentoid).HasColumnName("SEGMENTOID").IsRequired();
            this.Property(x => x.des_acao).HasColumnName("DES_ACAO").IsRequired();
        }
    }
    public class CampanhaMarketingDocMap : CRMEntityTypeConfiguration<CampanhaMarketingDoc>, IMapping
    {
        public CampanhaMarketingDocMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("CAMPANHAMARKETINGDOC", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.CampanhaMarketingDocId, x.campanhaID });
            this.Property(x => x.campanhaID).HasColumnName("CAMPANHAID").IsRequired();
            this.Property(x => x.CampanhaMarketingDocId).HasColumnName("CAMPANHAMARKETINGDOCID").IsRequired();
            this.Property(x => x.Caminho).HasColumnName("CAMINHO").IsOptional();
            this.Property(x => x.des_imagem).HasColumnName("DES_IMAGEM").IsOptional();
            this.Property(x => x.des_contenttype).HasColumnName("DES_CONTENTTYPE").IsOptional();
            this.Property(x => x.nome_arquivo).HasColumnName("NOME_ARQUIVO").IsOptional();

        }
    }
    public class CampanhaMarketingMap : CRMEntityTypeConfiguration<CampanhaMarketing>, IMapping
    {
        public CampanhaMarketingMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("CAMPANHAMARKETING", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.campanhaID});
            this.Property(x => x.campanhaID).HasColumnName("CAMPANHAID").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsRequired(); 
            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsOptional();
            this.Property(x => x.cod_pessoa_string).HasColumnName("COD_PESSOA_STRING").IsOptional(); 
            this.Property(x => x.cod_representante).HasColumnName("COD_REPRESENTANTE").IsOptional();
            this.Property(x => x.des_email).HasColumnName("DES_EMAIL").IsOptional();
            this.Property(x => x.segmentoid).HasColumnName("SEGMENTOID").IsOptional();
            this.Property(x => x.tipoacaoid).HasColumnName("TIPOACAOID").IsOptional();
            this.Property(x => x.dta_inicial).HasColumnName("DTA_INICIAL").IsOptional();
            this.Property(x => x.dta_final).HasColumnName("DTA_FINAL").IsOptional();
            this.Property(x => x.ind_renova).HasColumnName("IND_RENOVA").IsOptional();
            this.Property(x => x.vlr_meta).HasColumnName("VLR_META").IsOptional();
            this.Property(x => x.vlr_contrato).HasColumnName("VLR_CONTRATO").IsOptional();
            this.Property(x => x.vlr_custo_medio).HasColumnName("VLR_CUSTO_MEDIO").IsOptional();
            this.Property(x => x.per_contrato).HasColumnName("PER_CONTRATO").IsOptional();
            this.Property(x => x.tip_apuracao).HasColumnName("TIP_APURACAO").IsOptional();
            this.Property(x => x.tip_pgto_premiacao).HasColumnName("TIP_PGTO_PREMIACAO").IsOptional();
            


            this.Property(x => x.formapgtoid).HasColumnName("FORMAPGTOID").IsOptional();
            this.Property(x => x.des_agencia).HasColumnName("DES_AGENCIA").IsOptional();
            this.Property(x => x.des_banco).HasColumnName("DES_BANCO").IsOptional();
            this.Property(x => x.des_conta).HasColumnName("DES_CONTA").IsOptional();
            this.Property(x => x.des_observacao).HasColumnName("DES_OBSERVACAO").IsOptional();
            this.Property(x => x.situacao).HasColumnName("SITUACAO").IsOptional();
            this.Property(x => x.statusId).HasColumnName("STATUSID").IsOptional();
            this.Property(x => x.estagioId).HasColumnName("ESTAGIOID").IsOptional();
            this.Property(x => x.cod_usuario_alteracao).HasColumnName("COD_USUARIO_ALTERACAO").IsOptional();
            this.Property(x => x.dta_alteracao).HasColumnName("DTA_ALTERACAO").IsOptional();
            this.Property(x => x.des_ult_obs).HasColumnName("DES_ULT_OBS").IsOptional();
            this.Property(x => x.dta_inclusao).HasColumnName("DTA_INCLUSAO").IsOptional();
            this.Property(x => x.cod_regional).HasColumnName("COD_REGIONAL").IsOptional();
            this.Property(x => x.cod_diretoria).HasColumnName("COD_DIRETORIA").IsOptional();
            this.Property(x => x.des_segmento).HasColumnName("DES_SEGMENTO").IsOptional();
            this.HasRequired(t => t.Segmentos).WithMany().HasForeignKey(d => d.segmentoid);
            this.HasRequired(t => t.TipoAcao).WithMany().HasForeignKey(d => new { d.tipoacaoid, d.segmentoid });
            this.HasRequired(t => t.FormaPgto).WithMany().HasForeignKey(d => d.formapgtoid);
            this.HasRequired(t => t.Ps_Pessoas).WithMany().HasForeignKey(d => d.cod_pessoa);
            this.HasRequired(t => t.Ps_Representantes).WithMany().HasForeignKey(d => d.cod_representante);
            this.HasRequired(t => t.Status).WithMany().HasForeignKey(d => d.statusId);
            this.HasRequired(t => t.Estagio).WithMany().HasForeignKey(d => d.estagioId);
            this.HasRequired(t => t.UsuarioAlteracao).WithMany().HasForeignKey(d => d.cod_usuario_alteracao);
            this.HasRequired(t => t.DadosDoCrm).WithMany().HasForeignKey(d => d.cod_pessoa_string);
            
        }
    }
    public class CampanhaMarketingEstagiosMap : CRMEntityTypeConfiguration<CampanhaMarketingEstagios>, IMapping
    {
        public CampanhaMarketingEstagiosMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("CAMPANHAMARKETINGESTAGIOS", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.num_seq, x.campanhaId });
            this.Property(x => x.num_seq).HasColumnName("NUM_SEQ").IsRequired();
            this.Property(x => x.campanhaId).HasColumnName("CAMPANHAID").IsRequired();
            this.Property(x => x.dta_troca).HasColumnName("DTA_TROCA").IsOptional();
            this.Property(x => x.estagioidentrada).HasColumnName("ESTAGIOIDENTRADA").IsOptional();
            this.Property(x => x.dta_entrada).HasColumnName("DTA_ENTRADA").IsOptional();
            this.Property(x => x.dta_saida).HasColumnName("DTA_SAIDA").IsOptional();
            this.Property(x => x.estagioidanterior).HasColumnName("ESTAGIOIDANTERIOR").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsOptional();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();
            this.HasRequired(t => t.CampanhaMarketing).WithMany().HasForeignKey(d => d.campanhaId);
            this.HasRequired(t => t.EstagioEntrada).WithMany().HasForeignKey(d => d.estagioidentrada);
            this.HasRequired(t => t.EstagioAnterior).WithMany().HasForeignKey(d => d.estagioidanterior);
            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => d.cod_usuario);
        }
    }
}
