using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Map
{
    //SELECT * FROM sp_table_class_map where table_name = 'PS_Tipo_Sac'



    public class EstagioUsuario_map : CRMEntityTypeConfiguration<Ps_Estagio_Usuario>
    {
        public EstagioUsuario_map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_ESTAGIO_USUARIO", schema);

            this.HasKey(x => new { x.idrow });

            this.Property(x => x.idrow).HasColumnName("IDROW").IsRequired();
            this.Property(x => x.cd_usuario).HasColumnName("CD_USUARIO").IsRequired();
            this.Property(x => x.cod_estagio).HasColumnName("COD_ESTAGIO").IsRequired();

            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => new { d.cd_usuario });



        }


    }

    public class SacProcedimento_map : CRMEntityTypeConfiguration<SacProcedimento>
    {
        public SacProcedimento_map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("SACPROCEDIMENTO", schema);

            this.HasKey(x => new { x.cod_sac, x.cod_procedimento });

            this.Property(x => x.cod_sac).HasColumnName("COD_SAC").IsRequired();
            this.Property(x => x.cod_procedimento).HasColumnName("COD_PROCEDIMENTO").IsRequired();



        }


    }


    public class Tp_Procedimento_Motivos_Map : CRMEntityTypeConfiguration<Tp_Procedimento_Motivos>
    {
        public Tp_Procedimento_Motivos_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("TP_PROCEDIMENTO_MOTIVOS", schema);

            this.HasKey(x => new { x.COD_TIPO, x.MOTIVOID });

            this.Property(x => x.COD_TIPO).HasColumnName("COD_TIPO").IsRequired();
            this.Property(x => x.MOTIVOID).HasColumnName("MOTIVOID").IsRequired();
            this.Property(x => x.DES_NOME).HasColumnName("DES_NOME").IsRequired();

        }


    }


    public class SacGarantia_Map : CRMEntityTypeConfiguration<SacGarantia>
    {
        public SacGarantia_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("SACGARANTIA", schema);

            this.HasKey(x => new { x.cod_sac, x.garantiaId });
            this.Property(x => x.cod_sac).HasColumnName("COD_SAC").IsRequired();
            this.Property(x => x.garantiaId).HasColumnName("GARANTIAID").IsRequired();

        }


    }


    public class sac_troca_estagio_Map : CRMEntityTypeConfiguration<sac_troca_estagio>
    {
        public sac_troca_estagio_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("SAC_TROCA_ESTAGIO", schema);

            this.HasKey(x => new { x.num_seq, x.cod_sac });

            this.Property(x => x.num_seq).HasColumnName("NUM_SEQ").IsRequired();
            this.Property(x => x.cod_sac).HasColumnName("COD_SAC").IsRequired();
            this.Property(x => x.dta_troca).HasColumnName("DTA_TROCA").IsOptional();
            this.Property(x => x.cod_estagio_entrada).HasColumnName("COD_ESTAGIO_ENTRADA").IsOptional();
            this.Property(x => x.dta_entrada).HasColumnName("DTA_ENTRADA").IsOptional();
            this.Property(x => x.dta_saida).HasColumnName("DTA_SAIDA").IsOptional();
            this.Property(x => x.cod_estagio_anterior).HasColumnName("COD_ESTAGIO_ANTERIOR").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsOptional();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();


            this.HasRequired(t => t.PS_Sac).WithMany().HasForeignKey(d => new { d.cod_sac });
            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => new { d.cod_usuario });
            this.HasRequired(t => t.EstagioEntrada).WithMany().HasForeignKey(d => new { d.cod_estagio_entrada });
            this.HasRequired(t => t.EstagioAnterior).WithMany().HasForeignKey(d => new { d.cod_estagio_anterior });


        }


    }

    public class Ps_Sac_Reabertura_Map : CRMEntityTypeConfiguration<Ps_Sac_Reabertura>
    {
        public Ps_Sac_Reabertura_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_SAC_REABERTURA", schema);

            this.HasKey(x => new { x.id });

            this.Property(x => x.id).HasColumnName("ID").IsRequired();
            this.Property(x => x.cod_sac).HasColumnName("COD_SAC").IsRequired();
            this.Property(x => x.dta_reabertura).HasColumnName("DTA_REABERTURA").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsRequired();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();

            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => d.cod_usuario);
            this.HasRequired(t => t.PS_Sac).WithMany().HasForeignKey(d => d.cod_sac);
        }
    }


    public class PS_Sac_Map : CRMEntityTypeConfiguration<PS_Sac>
    {
        public PS_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_SAC", schema);

            this.HasKey(x => new { x.cod_sac });

            this.Property(x => x.cod_sac).HasColumnName("COD_SAC").IsRequired();
            this.Property(x => x.dta_abertura).HasColumnName("DTA_ABERTURA").IsOptional();
            this.Property(x => x.dta_finalizacao).HasColumnName("DTA_FINALIZACAO").IsOptional();
            this.Property(x => x.des_assunto).HasColumnName("DES_ASSUNTO").IsOptional();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
            this.Property(x => x.des_descricao).HasColumnName("DES_DESCRICAO").IsOptional();
            this.Property(x => x.des_solucao).HasColumnName("DES_SOLUCAO").IsOptional();
            this.Property(x => x.tp_pessoa).HasColumnName("TP_PESSOA").IsOptional();
            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsOptional();
            this.Property(x => x.cod_tipo).HasColumnName("COD_TIPO").IsOptional();
            this.Property(x => x.cod_situacao).HasColumnName("COD_SITUACAO").IsOptional();
            this.Property(x => x.cod_origem).HasColumnName("COD_ORIGEM").IsOptional();
            this.Property(x => x.cod_classe).HasColumnName("COD_CLASSE").IsOptional();
            this.Property(x => x.cod_categoria).HasColumnName("COD_CATEGORIA").IsOptional();
            this.Property(x => x.cod_estagio).HasColumnName("COD_ESTAGIO").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsRequired();
            this.Property(x => x.cod_empresa).HasColumnName("COD_EMPRESA").IsRequired();
            this.Property(x => x.tag).HasColumnName("TAG").IsOptional();


            this.HasRequired(t => t.PS_Pessoas_Sac).WithMany().HasForeignKey(d => new { d.tp_pessoa, d.cod_pessoa });
            this.HasRequired(t => t.PS_Tipo_Sac).WithMany().HasForeignKey(d => d.cod_tipo);
            this.HasRequired(t => t.PS_Situacao_Sac).WithMany().HasForeignKey(d => d.cod_situacao);
            this.HasRequired(t => t.PS_Origem_Sac).WithMany().HasForeignKey(d => d.cod_origem);
            this.HasRequired(t => t.PS_Classe_Sac).WithMany().HasForeignKey(d => d.cod_classe);
            this.HasRequired(t => t.PS_Categorias_Sac).WithMany().HasForeignKey(d => new { d.cod_categoria, d.cod_classe });
            this.HasRequired(t => t.PS_Estagio_Sac).WithMany().HasForeignKey(d => d.cod_estagio);
            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => d.cod_usuario);


        }


    }



    public class Ps_Sac_Ps_Sac_Map : CRMEntityTypeConfiguration<Ps_Sac_Ps_Sac>
    {
        public Ps_Sac_Ps_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_SAC_PS_SAC", schema);

            this.HasKey(x => x.rec );

            this.Property(x => x.rec).HasColumnName("REC").IsRequired();
            this.Property(x => x.cod_sac_original).HasColumnName("COD_SAC_ORIGINAL").IsOptional();
            this.Property(x => x.cod_sac_novo).HasColumnName("COD_SAC_NOVO").IsOptional();
            this.Property(x => x.dta_reabertura).HasColumnName("DTA_REABERTURA").IsOptional();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsOptional();
           

            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => d.cod_usuario);

            this.HasRequired(t => t.SacOriginal).WithMany().HasForeignKey(d => d.cod_sac_original);
            this.HasRequired(t => t.SacNovo).WithMany().HasForeignKey(d => d.cod_sac_novo);


        }


    }


    public class SacArquivo_Map : CRMEntityTypeConfiguration<SacArquivo>
    {
        public SacArquivo_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("SACARQUIVO", schema);

            this.HasKey(x => new { x.id, x.sacid });

            this.Property(x => x.id).HasColumnName("ID").IsRequired();
            this.Property(x => x.sacid).HasColumnName("SACID").IsRequired();
            this.Property(x => x.caminho).HasColumnName("CAMINHO").IsOptional();
            this.Property(x => x.des_imagem).HasColumnName("DES_IMAGEM").IsOptional();
            this.Property(x => x.des_contenttype).HasColumnName("DES_CONTENTTYPE").IsOptional();
            this.Property(x => x.nome_arquivo).HasColumnName("NOME_ARQUIVO").IsOptional();

        }


    }



    public class PS_Pessoas_Sac_Map : CRMEntityTypeConfiguration<PS_Pessoas_Sac>
    {
        public PS_Pessoas_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_PESSOAS_SAC", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.tipo, x.cod_pessoa });
            this.Property(x => x.tipo).HasColumnName("TIPO").IsRequired();
            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
            this.Property(x => x.des_pessoa).HasColumnName("DES_PESSOA").IsOptional();
            this.Property(x => x.tipostring).HasColumnName("TIPOSTRING").IsOptional();
            this.Property(x => x.des_fantasia).HasColumnName("DES_FANTASIA").IsOptional();
            this.Property(x => x.des_empresa).HasColumnName("DES_EMPRESA").IsOptional();
            this.Property(x => x.cgc_cpf).HasColumnName("CGC_CPF").IsOptional();
            this.Property(x => x.des_email_cli).HasColumnName("DES_EMAIL_CLI").IsOptional();
            this.Property(x => x.tel_contato).HasColumnName("TEL_CONTATO").IsOptional();
            

        }


    }


    public class PS_Estagio_Sac_Map : CRMEntityTypeConfiguration<PS_Estagio_Sac>
    {
        public PS_Estagio_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_ESTAGIO_SAC", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_estagio });

            this.Property(x => x.cod_estagio).HasColumnName("COD_ESTAGIO").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsRequired();
            this.Property(x => x.ind_finalizacao).HasColumnName("IND_FINALIZACAO").IsOptional();
        }


    }


    public class PS_Categorias_Sac_Map : CRMEntityTypeConfiguration<PS_Categorias_Sac>
    {
        public PS_Categorias_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_CATEGORIAS_SAC", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_categoria, x.cod_classe });

            this.Property(x => x.cod_classe).HasColumnName("COD_CLASSE").IsRequired();
            this.Property(x => x.cod_categoria).HasColumnName("COD_CATEGORIA").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
        }

    }

    public class PS_Classe_Sac_Map : CRMEntityTypeConfiguration<PS_Classe_Sac>
    {
        public PS_Classe_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_CLASSE_SAC", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_classe });

            this.Property(x => x.cod_classe).HasColumnName("COD_CLASSE").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
        }

    }


    public class PS_origem_Sac_Map : CRMEntityTypeConfiguration<PS_Origem_Sac>
    {
        public PS_origem_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_ORIGEM_SAC", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_origem });

            this.Property(x => x.cod_origem).HasColumnName("COD_ORIGEM").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
        }

    }


    public class PS_Situacao_Sac_Map : CRMEntityTypeConfiguration<PS_Situacao_Sac>
    {
        public PS_Situacao_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_SITUACAO_SAC", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_situacao });

            this.Property(x => x.cod_situacao).HasColumnName("COD_SITUACAO").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
        }

    }



    public class PS_Tipo_Sac_Map : CRMEntityTypeConfiguration<PS_Tipo_Sac>
    {
        public PS_Tipo_Sac_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_TIPO_SAC", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_tipo });

            this.Property(x => x.cod_tipo).HasColumnName("COD_TIPO").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
            this.Property(x => x.tp_finalizacao).HasColumnName("TP_FINALIZACAO").IsOptional();

        }

    }





}
