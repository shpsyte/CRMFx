namespace Domain.Entity.Map
{
    public class W_TopProcedimentoByDeptoMap : CRMEntityTypeConfiguration<W_TopProcedimentoByDepto>
    {
        public W_TopProcedimentoByDeptoMap(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("W_TOPPROCEDIMENTOBYDEPTO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cd_departamento });
            this.Property(x => x.cd_departamento).HasColumnName("CD_DEPARTAMENTO").IsRequired();
            this.Property(x => x.departamento).HasColumnName("DEPARTAMENTO").IsRequired();
            this.Property(x => x.maiorproc).HasColumnName("MAIORPROC").IsOptional();
            this.Property(x => x.rk).HasColumnName("RK").IsOptional();
        }
    }


    public class W_QTDE_SAC_DIA_MAP : CRMEntityTypeConfiguration<W_QTDE_SAC_DIA>
    {
        public W_QTDE_SAC_DIA_MAP(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("W_QTDE_SAC_DIA", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.dia });
            this.Property(x => x.dia).HasColumnName("DIA").IsRequired();
            this.Property(x => x.qtde).HasColumnName("QTDE").IsRequired();
        }
    }

    public class w_sac_por_tipo_Map : CRMEntityTypeConfiguration<w_sac_por_tipo>
    {
        public w_sac_por_tipo_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("W_SAC_POR_TIPO", schema);
            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.Tipo_Ocorrencia });
            this.Property(x => x.Tipo_Ocorrencia).HasColumnName("TIPO_OCORRENCIA").IsRequired();
            this.Property(x => x.Qtde).HasColumnName("QTDE").IsRequired();
            this.Property(x => x.Total).HasColumnName("TOTAL").IsRequired();
            this.Property(x => x.Percentual).HasColumnName("PERCENTUAL").IsRequired();
        }
    }




}
