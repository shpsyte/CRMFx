using System.Data.Entity.ModelConfiguration;

namespace Domain.Entity.Map
{

    public class DadosDoCrmMap : CRMEntityTypeConfiguration<DadosDoCrm>
    {



        public DadosDoCrmMap(string schema)
            {
                /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
                this.ToTable("DADOS_CRM_VIEW", schema);

                 //Definimos a propriedade como chave primária
                 this.HasKey(x => x.cod_pessoa);
                 
                 this.Property(x => x. cod_grupo).HasColumnName("COD_GRUPO").IsOptional();
                 this.Property(x => x. des_grupo).HasColumnName("DES_GRUPO").IsOptional();
                 this.Property(x => x. cod_pessoa).HasColumnName("COD_PESSOA").IsRequired();
                 this.Property(x => x. des_pessoa).HasColumnName("DES_PESSOA").IsOptional();
                 this.Property(x => x. des_fantasia).HasColumnName("DES_FANTASIA").IsOptional();
                 this.Property(x => x. cod_diretoria).HasColumnName("COD_DIRETORIA").IsOptional();
                 this.Property(x => x. des_diretoria).HasColumnName("DES_DIRETORIA").IsOptional();
                 this.Property(x => x. cod_representante_cli).HasColumnName("COD_REPRESENTANTE_CLI").IsOptional();
                 this.Property(x => x. des_represetante).HasColumnName("DES_REPRESETANTE").IsOptional();
                 this.Property(x => x. cod_gerente).HasColumnName("COD_GERENTE").IsOptional();
                 this.Property(x => x. des_gerente).HasColumnName("DES_GERENTE").IsOptional();
                 this.Property(x => x. cep).HasColumnName("CEP").IsOptional();
                 this.Property(x => x. bairro).HasColumnName("BAIRRO").IsOptional();
                 this.Property(x => x. cod_cidade).HasColumnName("COD_CIDADE").IsOptional();
                 this.Property(x => x. des_cidade).HasColumnName("DES_CIDADE").IsOptional();
                 this.Property(x => x. uf).HasColumnName("UF").IsOptional();
                 this.Property(x => x. des_estado).HasColumnName("DES_ESTADO").IsOptional();
                 this.Property(x => x. regiao_geografica).HasColumnName("REGIAO_GEOGRAFICA").IsOptional();
                 this.Property(x => x. atividade).HasColumnName("ATIVIDADE").IsOptional();
                 this.Property(x => x. regiao_cliente).HasColumnName("REGIAO_CLIENTE").IsOptional();
                 this.Property(x => x. empresa_coligada).HasColumnName("EMPRESA_COLIGADA").IsOptional();
                 this.Property(x => x. segmento).HasColumnName("SEGMENTO").IsOptional();
                 this.Property(x => x. cgc_cpf).HasColumnName("CGC_CPF").IsOptional();
                 this.Property(x => x. tel_contato).HasColumnName("TEL_CONTATO").IsOptional();
                 this.Property(x => x. des_endereco).HasColumnName("DES_ENDERECO").IsOptional();
                 this.Property(x => x. lim_credito).HasColumnName("LIM_CREDITO").IsOptional();
                 this.Property(x => x. ie).HasColumnName("IE").IsOptional();
                 this.Property(x => x. ind_consumidor).HasColumnName("IND_CONSUMIDOR").IsOptional();
                 this.Property(x => x. cod_oper).HasColumnName("COD_OPER").IsOptional();
                 this.Property(x => x. dta_ult_compra).HasColumnName("DTA_ULT_COMPRA").IsOptional();
                 this.Property(x => x. vlr_maior_compra).HasColumnName("VLR_MAIOR_COMPRA").IsOptional();
                 this.Property(x => x. vlr_fat_ant).HasColumnName("VLR_FAT_ANT").IsOptional();
                 this.Property(x => x. per_rent_ant).HasColumnName("PER_RENT_ANT").IsOptional();
                 this.Property(x => x. vlr_faturamento).HasColumnName("VLR_FATURAMENTO").IsOptional();
                 this.Property(x => x. per_rentabilidade).HasColumnName("PER_RENTABILIDADE").IsOptional();
                 this.Property(x => x. vlr_dev_ant).HasColumnName("VLR_DEV_ANT").IsOptional();
                 this.Property(x => x. vlr_devolucao).HasColumnName("VLR_DEVOLUCAO").IsOptional();
                 this.Property(x => x. des_email_cli).HasColumnName("DES_EMAIL_CLI").IsOptional();
                 this.Property(x => x. des_email_rep).HasColumnName("DES_EMAIL_REP").IsOptional();
                 this.Property(x => x. vlr_investimento).HasColumnName("VLR_INVESTIMENTO").IsOptional();
                 this.Property(x => x. vlr_bonus).HasColumnName("VLR_BONUS").IsOptional();
                 this.Property(x => x. dta_ult_alteracao).HasColumnName("DTA_ULT_ALTERACAO").IsOptional();
                 this.Property(x => x. dta_cadastro).HasColumnName("DTA_CADASTRO").IsOptional();




            }


    }


}
