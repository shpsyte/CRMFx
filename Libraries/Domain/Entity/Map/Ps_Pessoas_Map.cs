using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Map
{
    public class Ps_Pessoas_Map : CRMEntityTypeConfiguration<Ps_Pessoas>
    {

        public Ps_Pessoas_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_PESSOAS", schema);

            //Definimos a propriedade como chave primária
            //this.HasKey(x => new {x.cod_pessoa, x.cod_nota});
            this.HasKey(x => x.cod_pessoa);

            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
            this.Property(x => x.des_pessoa).HasColumnName("DES_PESSOA").IsRequired();
            this.Property(x => x.des_fantasia).HasColumnName("DES_FANTASIA").IsRequired();
            
            this.Property(x => x.des_email).HasColumnName("DES_EMAIL").IsOptional();

            this.Property(x => x.des_endereco).HasColumnName("DES_ENDERECO").IsOptional();
            this.Property(x => x.num_cep).HasColumnName("NUM_CEP").IsOptional();
            this.Property(x => x.des_bairro).HasColumnName("DES_BAIRRO").IsOptional();
            this.Property(x => x.cod_cidade).HasColumnName("COD_CIDADE").IsOptional();
            this.Property(x => x.dta_cadastro).HasColumnName("DTA_CADASTRO").IsOptional();
            

            this.HasRequired(t => t.Cidade).WithMany().HasForeignKey(d => d.cod_cidade);

        }


    }

    public class G1_CidadesMap : CRMEntityTypeConfiguration<G1_Cidades>
    {

        public G1_CidadesMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("G1_CIDADES", schema);

            //Definimos a propriedade como chave primária
            //this.HasKey(x => new {x.cod_pessoa, x.cod_nota});
            this.HasKey(x => x.cod_cidade);

            this.Property(x => x.cod_cidade).HasColumnName("COD_CIDADE").IsRequired();
            this.Property(x => x.cod_uf).HasColumnName("COD_UF").IsRequired();
            this.Property(x => x.des_cidade).HasColumnName("DES_CIDADE").IsOptional();

        }


    }


    public class Ps_Representantes_Map : CRMEntityTypeConfiguration<Ps_Representantes>
    {

        public Ps_Representantes_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("REPRESENTANTES", schema);

            //Definimos a propriedade como chave primária
            //this.HasKey(x => new {x.cod_pessoa, x.cod_nota});
            this.HasKey(x => x.cod_pessoa);

            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
            this.Property(x => x.des_pessoa).HasColumnName("DES_PESSOA").IsRequired();
            this.Property(x => x.des_fantasia).HasColumnName("DES_FANTASIA").IsRequired();

            this.Property(x => x.des_endereco).HasColumnName("DES_ENDERECO").IsRequired();
            this.Property(x => x.des_cidade).HasColumnName("DES_CIDADE").IsRequired();
            this.Property(x => x.uf).HasColumnName("UF").IsRequired();
            this.Property(x => x.bairro).HasColumnName("BAIRRO").IsRequired();
            this.Property(x => x.ind_inativo).HasColumnName("IND_INATIVO").IsRequired();
            


        }


    }
}
