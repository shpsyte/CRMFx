using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class PsLeadsMap : CRMEntityTypeConfiguration<Ps_Leads>
    {
        public PsLeadsMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_LEADS", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_lead });

            this.Property(x => x.cod_lead).HasColumnName("COD_LEAD").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsRequired();
            this.Property(x => x.des_sobrenome).HasColumnName("DES_SOBRENOME").IsOptional();
            this.Property(x => x.cod_tipo).HasColumnName("COD_TIPO").IsOptional();
            this.Property(x => x.cod_origem).HasColumnName("COD_ORIGEM").IsOptional();
            this.Property(x => x.des_empresa).HasColumnName("DES_EMPRESA").IsOptional();
            this.Property(x => x.des_fone).HasColumnName("DES_FONE").IsOptional();
            this.Property(x => x.des_email).HasColumnName("DES_EMAIL").IsOptional();
            this.Property(x => x.des_documento).HasColumnName("DES_DOCUMENTO").IsOptional();
            this.Property(x => x.cod_interesse).HasColumnName("COD_INTERESSE").IsOptional();
            this.Property(x => x.des_info).HasColumnName("DES_INFO").IsOptional();
            this.Property(x => x.cod_situacao).HasColumnName("COD_SITUACAO").IsOptional();
            this.Property(x => x.dta_criado).HasColumnName("DTA_CRIADO").IsOptional();
            this.Property(x => x.des_interesse).HasColumnName("DES_INTERESSE").IsRequired();
            





            this.HasRequired(t => t.Ps_Interesse_Lead).WithMany().HasForeignKey(d => d.cod_interesse);
            this.HasRequired(t => t.Ps_Origem_Lead).WithMany().HasForeignKey(d => d.cod_origem);
            this.HasRequired(t => t.Ps_Tipo_Lead).WithMany().HasForeignKey(d => d.cod_tipo);
            this.HasRequired(t => t.Ps_Situacao_Lead).WithMany().HasForeignKey(d => d.cod_situacao);


        }
    }



    public class PsTipoLeadMap : CRMEntityTypeConfiguration<Ps_Tipo_Lead>
    {
        public PsTipoLeadMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_TIPO_LEAD", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_tipo });

            this.Property(x => x.cod_tipo).HasColumnName("COD_TIPO").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
        }
    }

    public class PsOrigemLeadMap : CRMEntityTypeConfiguration<Ps_Origem_Lead>
    {
        public PsOrigemLeadMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_ORIGEM_LEAD", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_origem });

            this.Property(x => x.cod_origem).HasColumnName("COD_ORIGEM").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
        }
    }

    public class PsInteresseLeadMap : CRMEntityTypeConfiguration<Ps_Interesse_Lead>
    {
        public PsInteresseLeadMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_INTERESSE_LEAD", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_interesse });

            this.Property(x => x.cod_interesse).HasColumnName("COD_INTERESSE").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();

        }
    }


    public class PsSituacaoLeadMap : CRMEntityTypeConfiguration<Ps_Situacao_Lead>
    {
        public PsSituacaoLeadMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_SITUACAO_LEAD", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_situacao });

            this.Property(x => x.cod_situacao).HasColumnName("COD_SITUACAO").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();

        }
    }


}
