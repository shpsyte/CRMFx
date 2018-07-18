using System.Data.Entity.ModelConfiguration;

namespace Domain
{
    
    public abstract class CRMEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected CRMEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}