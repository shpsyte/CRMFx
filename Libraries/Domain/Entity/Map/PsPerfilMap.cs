using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class PsPerfilMap : CRMEntityTypeConfiguration<Ps_Perfil_Crm>
    {



        public PsPerfilMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_PERFIL_CRM", schema);

            //Definimos a propriedade como chave primária
             this.HasKey(x => new {x.cod_pessoa});

             this.Property(x => x. cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
             this.Property(x => x. dta_fundacao).HasColumnName("DTA_FUNDACAO").IsOptional();
             this.Property(x => x. des_evento).HasColumnName("DES_EVENTO").IsOptional();
             this.Property(x => x. des_clube).HasColumnName("DES_CLUBE").IsOptional();
             this.Property(x => x. des_acao).HasColumnName("DES_ACAO").IsOptional();
             this.Property(x => x. des_outras).HasColumnName("DES_OUTRAS").IsOptional();



        }


    }


}
