using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{



    public class ListaOcorrenciaMap : CRMEntityTypeConfiguration<ListaOcorrencia>
    {
        public ListaOcorrenciaMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("LISTA_OCORRENCIA", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_sac });

            this.Property(x => x.cod_sac).HasColumnName("COD_SAC").IsRequired();
            this.Property(x => x.cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
            this.Property(x => x.dta_abertura).HasColumnName("DTA_ABERTURA").IsRequired();
            this.Property(x => x.status).HasColumnName("STATUS").IsRequired();


        }


    }



    public class TituloMap : CRMEntityTypeConfiguration<ListaTitulos>
    {
        public TituloMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("LISTA_TITULOS", schema);

            //Definimos a propriedade como chave primária
             this.HasKey(x => new {x.cod_pessoa, x.num_titulo, x.num_parcela, x.cod_compl});

             this.Property(x => x. cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
             this.Property(x => x. num_titulo).HasColumnName("NUM_TITULO").IsRequired();
             this.Property(x => x. num_parcela).HasColumnName("NUM_PARCELA").IsRequired();
             this.Property(x => x. cod_compl).HasColumnName("COD_COMPL").IsRequired();
             this.Property(x => x. dta_emissao).HasColumnName("DTA_EMISSAO").IsRequired();
             this.Property(x => x. dta_vencimento).HasColumnName("DTA_VENCIMENTO").IsRequired();
             this.Property(x => x. diasatraso).HasColumnName("DIASATRASO").IsOptional();
             this.Property(x => x. valortitulo).HasColumnName("VALORTITULO").IsOptional();


        }


    }


}
