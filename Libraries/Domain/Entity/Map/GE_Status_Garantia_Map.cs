using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Map
{
    public class GE_Status_Garantia_Map : CRMEntityTypeConfiguration<GE_Status_Garantia>, IMapping
    {

        public GE_Status_Garantia_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GE_STATUS_GARANTIA", schema);

            //Definimos a propriedade como chave primária
            //this.HasKey(x => new {x.cod_pessoa, x.cod_nota});
            this.HasKey(x => x.Cod_Status);

            this.Property(x => x.Cod_Status).HasColumnName("COD_STATUS").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsRequired();
            this.Property(x => x.ind_finalizacao).HasColumnName("IND_FINALIZACAO").IsRequired();


        }


    }
}
