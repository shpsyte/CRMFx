using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Map
{
    public class IE_Itens_Map : CRMEntityTypeConfiguration<IE_Itens>
    {

        public IE_Itens_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("V_IE_ITENS", schema);

            //Definimos a propriedade como chave primária
            //this.HasKey(x => new {x.cod_pessoa, x.cod_nota});
            this.HasKey(x => x.cod_item);

            this.Property(x => x.cod_item).HasColumnName("COD_ITEM").IsRequired();
            this.Property(x => x.cod_foxlux).HasColumnName("COD_FOXLUX").IsRequired();
            this.Property(x => x.des_item).HasColumnName("DES_ITEM").IsRequired();
            this.Property(x => x.ncm).HasColumnName("NCM").IsOptional();


        }


    }
    

    public class IE_Itens_Vendas_Map : CRMEntityTypeConfiguration<IE_Itens_Vendas>
    {

        public IE_Itens_Vendas_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("IE_ITENS_VENDAS", schema);

            //Definimos a propriedade como chave primária
            //this.HasKey(x => new {x.cod_pessoa, x.cod_nota});
            this.HasKey(x => new { x.num_seq});

            this.Property(x => x.num_seq).HasColumnName("NUM_SEQ").IsRequired();
            this.Property(x => x.cod_cliente).HasColumnName("COD_CLIENTE").IsOptional();
            this.Property(x => x.des_pessoa).HasColumnName("DES_PESSOA").IsRequired();
            this.Property(x => x.cod_item).HasColumnName("COD_ITEM").IsOptional();
            this.Property(x => x.cod_foxlux).HasColumnName("COD_FOXLUX").IsOptional();
            this.Property(x => x.des_item).HasColumnName("DES_ITEM").IsRequired();
            this.Property(x => x.dta_lancamento).HasColumnName("DTA_LANCAMENTO").IsRequired();
            this.Property(x => x.dias).HasColumnName("DIAS").IsOptional();
            this.Property(x => x.qtd_lancamento).HasColumnName("QTD_LANCAMENTO").IsRequired();
            this.Property(x => x.num_documento).HasColumnName("NUM_DOCUMENTO").IsRequired();
            this.Property(x => x.vlr_total).HasColumnName("VLR_TOTAL").IsOptional();
            this.Property(x => x.vlr_unitario).HasColumnName("VLR_UNITARIO").IsOptional();
            this.Property(x => x.vlr_ipi).HasColumnName("VLR_IPI").IsOptional();
            this.Property(x => x.vlr_icms_subs).HasColumnName("VLR_ICMS_SUBS").IsOptional();
            this.HasRequired(t => t.Ps_Pessoas).WithMany().HasForeignKey(d => new { d.cod_cliente });


        }


    }
}
