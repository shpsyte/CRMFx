using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class ContatosMap : CRMEntityTypeConfiguration<Contatos>
    {



        public ContatosMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("PS_CONTATOS_CRM", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => x.cod_contato);


            this.Property(x => x.cod_contato).HasColumnName("COD_CONTATO").IsRequired();
            this.Property(x => x.des_nome).HasColumnName("DES_NOME").IsRequired();
            this.Property(x => x.cod_conta).HasColumnName("COD_CONTA").IsRequired();


            this.Property(x => x.des_email).HasColumnName("DES_EMAIL").IsOptional();
            this.Property(x => x.des_fone).HasColumnName("DES_FONE").IsOptional();
            this.Property(x => x.des_celular).HasColumnName("DES_CELULAR").IsOptional();
            this.Property(x => x.cod_cargo).HasColumnName("COD_CARGO").IsOptional();
            this.Property(x => x.dta_nascimento).HasColumnName("DTA_NASCIMENTO").IsOptional();
            this.Property(x => x.des_filhos).HasColumnName("DES_FILHOS").IsOptional();
            this.Property(x => x.cod_estado_civil).HasColumnName("COD_ESTADO_CIVIL").IsOptional();
            this.Property(x => x.des_time).HasColumnName("DES_TIME").IsOptional();
            this.Property(x => x.des_hobby).HasColumnName("DES_HOBBY").IsOptional();
            this.Property(x => x.des_outras).HasColumnName("DES_OUTRAS").IsOptional();
                // Relationships
            this.HasRequired(t => t.DadosDoCrm).WithMany().HasForeignKey(d => d.cod_conta);
            this.HasRequired(T => T.Cargos).WithMany().HasForeignKey(d => d.cod_cargo);

        }


    }


}
