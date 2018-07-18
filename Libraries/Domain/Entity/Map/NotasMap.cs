using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class NotasMap : CRMEntityTypeConfiguration<ListaNotas>
    {



        public NotasMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("LISTA_NOTA", schema);

            //Definimos a propriedade como chave primária
             this.HasKey(x => new {x.num_nota});

             this.Property(x => x. num_nota).HasColumnName("NUM_NOTA").IsRequired();
             this.Property(x => x. cod_cliente).HasColumnName("COD_CLIENTE").IsOptional();
             this.Property(x => x. cod_oper).HasColumnName("COD_OPER").IsOptional();
             this.Property(x => x. des_oper).HasColumnName("DES_OPER").IsRequired();
             this.Property(x => x. dta_emissao).HasColumnName("DTA_EMISSAO").IsRequired();
             this.Property(x => x. vlr_total_liquido).HasColumnName("VLR_TOTAL_LIQUIDO").IsOptional();
             this.Property(x => x. vlr_total_bruto).HasColumnName("VLR_TOTAL_BRUTO").IsOptional();

            

        }


    }


}
