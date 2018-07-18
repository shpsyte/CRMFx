using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class ListaPedidosMap : CRMEntityTypeConfiguration<ListaPedidos>
    {



        public ListaPedidosMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("LISTA_PEDIDOS", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new {x.num_pedido, x.cod_compl});


            this.Property(x => x.cod_cliente).HasColumnName("COD_CLIENTE").IsRequired();
            this.Property(x => x.num_pedido).HasColumnName("NUM_PEDIDO").IsRequired();
            this.Property(x => x.cod_compl).HasColumnName("COD_COMPL").IsRequired();
            this.Property(x => x.dta_emissao).HasColumnName("DTA_EMISSAO").IsRequired();
            this.Property(x => x.des_situacao).HasColumnName("DES_SITUACAO").IsRequired();
            this.Property(x => x.valorpedido).HasColumnName("VALORPEDIDO").IsOptional();


        }


    }


}
