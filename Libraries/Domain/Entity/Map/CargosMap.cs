using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class CargosMap : CRMEntityTypeConfiguration<Cargos>
    {



        public CargosMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_CARGOS_CRM", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => x.cod_cargo);

            this.Property(x => x.cod_cargo).HasColumnName("COD_CARGO").IsRequired();
            this.Property(x => x.des_cargo).HasColumnName("DES_CARGO").IsRequired();

        }


    }


}
