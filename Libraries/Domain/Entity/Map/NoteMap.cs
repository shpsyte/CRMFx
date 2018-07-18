using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class NoteMap : CRMEntityTypeConfiguration<Note>
    {



        public NoteMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("NOTE", schema);

            //Definimos a propriedade como chave primária
            //this.HasKey(x => new {x.cod_pessoa, x.cod_nota});
            this.HasKey(x => x.cod_nota);

             this.Property(x => x. cod_interno).HasColumnName("COD_INTERNO").IsRequired();
             this.Property(x => x. tipo_nota).HasColumnName("TIPO_NOTA").IsRequired();
             this.Property(x => x. cod_nota).HasColumnName("COD_NOTA").IsRequired();
             this.Property(x => x. cod_usuario).HasColumnName("COD_USUARIO").IsRequired();
             this.Property(x => x. dta_inclusao).HasColumnName("DTA_INCLUSAO").IsOptional();
             this.Property(x => x. msg).HasColumnName("MSG").IsOptional();

              // Relationships
            this.HasRequired(t => t.Usuario)
                .WithMany()
                .HasForeignKey(d => d.cod_usuario);

        }


    }


}
