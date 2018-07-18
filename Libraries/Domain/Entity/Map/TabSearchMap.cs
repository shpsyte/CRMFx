using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Map
{




    public class vw_pessoasMap : CRMEntityTypeConfiguration<vw_pessoas>, IMapping
    {
        public vw_pessoasMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("VW_PESSOAS", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_pessoa });

            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
            this.Property(x => x.des_pessoa).HasColumnName("DES_PESSOA").IsOptional();
            this.Property(x => x.des_email).HasColumnName("DES_EMAIL").IsOptional();
            //this.HasRequired(t => t.FormaPgto).WithMany().HasForeignKey(d => new { d.formapgtoid });
        }
    }

    public class w_garantiaMap : CRMEntityTypeConfiguration<w_garantia>, IMapping
    {
        public w_garantiaMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("W_GARANTIA", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.garantiaid });

            this.Property(x => x.garantiaid).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.cod_cliente).HasColumnName("COD_CLIENTE").IsOptional();
            this.Property(x => x.cod_representante).HasColumnName("COD_REPRESENTANTE").IsOptional();
            this.Property(x => x.des_pessoa).HasColumnName("DES_PESSOA").IsOptional();
            this.Property(x => x.num_nf_cliente).HasColumnName("NUM_NF_CLIENTE").IsOptional();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();
            //this.HasRequired(t => t.FormaPgto).WithMany().HasForeignKey(d => new { d.formapgtoid });
        }
    }
    public class vw_SacMap : CRMEntityTypeConfiguration<vw_Sac>, IMapping
    {
        public vw_SacMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("VW_PS_SAC", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_sac });

            this.Property(x => x.cod_sac).HasColumnName("COD_SAC").IsRequired();
            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
            this.Property(x => x.des_assunto).HasColumnName("DES_ASSUNTO").IsOptional();
            this.Property(x => x.des_estagio).HasColumnName("DES_ESTAGIO").IsOptional();
            this.Property(x => x.des_pessoa).HasColumnName("DES_PESSOA").IsOptional();


            //this.HasRequired(t => t.FormaPgto).WithMany().HasForeignKey(d => new { d.formapgtoid });

        }
    }
    public class vw_ContatosMap : CRMEntityTypeConfiguration<vw_Contatos>, IMapping
    {
        public vw_ContatosMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("VW_CONTATOS", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_contato });

            this.Property(x => x.cod_contato).HasColumnName("COD_CONTATO").IsRequired();
            this.Property(x => x.cod_empresa).HasColumnName("COD_EMPRESA").IsRequired();
            this.Property(x => x.des_empresa).HasColumnName("DES_EMPRESA").IsOptional();
            this.Property(x => x.cnpj).HasColumnName("CNPJ").IsOptional();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
            this.Property(x => x.des_email).HasColumnName("DES_EMAIL").IsOptional();
            this.Property(x => x.des_fone).HasColumnName("DES_FONE").IsOptional();


            //this.HasRequired(t => t.FormaPgto).WithMany().HasForeignKey(d => new { d.formapgtoid });

        }
    }

    public class vw_Ps_LeadsMap : CRMEntityTypeConfiguration<vw_Ps_Leads>, IMapping
    {
        public vw_Ps_LeadsMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("VW_PS_LEADS", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_lead });

            this.Property(x => x.cod_lead).HasColumnName("COD_LEAD").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsOptional();
            this.Property(x => x.des_email).HasColumnName("DES_EMAIL").IsOptional();
            this.Property(x => x.des_fone).HasColumnName("DES_FONE").IsOptional();
            this.Property(x => x.des_documento).HasColumnName("DES_DOCUMENTO").IsOptional();

            //this.HasRequired(t => t.FormaPgto).WithMany().HasForeignKey(d => new { d.formapgtoid });

        }
    }


}
